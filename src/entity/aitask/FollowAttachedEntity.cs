using AncientTools.Utility;
using System;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace AncientTools.Entities.Tasks
{
    class AITaskFollowAttachedEntity : AiTaskBase
    {
        EntityMobileStorage mobileStorageEntity = null;

        private const float FOLLOW_DISTANCE = 0.7f;
        private const float PARKED_DISTANCE = 1.0f;
        private const float PULL_RESPONSE_PER_SECOND = 4.0f;
        private const double SNAP_DISTANCE_SQ = 9.0;
        private const double MIN_MOVE_DISTANCE_SQ = 0.0004;

        private Vec3d BehindVector { get; set; }
        private bool SettlingToParkedDistance { get; set; } = false;

        public AITaskFollowAttachedEntity(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
        {
            if (entity is EntityMobileStorage)
                mobileStorageEntity = entity as EntityMobileStorage;
        }
        public override bool ShouldExecute()
        {
            if (mobileStorageEntity.AttachedEntity != null)
            {
                return true;
            }

            return false;
        }
        public override void StartExecute()
        {
            base.StartExecute();

            SetBehindVector(PARKED_DISTANCE, 1.0f, true);
            SettlingToParkedDistance = false;
        }

        public override bool ContinueExecute(float dt)
        {
            if (mobileStorageEntity.AttachedEntity == null)
            {
                return false;
            }
            if (mobileStorageEntity.AttachedEntity.Controls.TriesToMove)
            {
                SettlingToParkedDistance = false;
                SetBehindVector(FOLLOW_DISTANCE, dt);
            }
            else
            {
                if (!SettlingToParkedDistance)
                {
                    BehindVector = GetBehindVector(PARKED_DISTANCE);
                    SettlingToParkedDistance = true;
                }

                if (!MoveTowardBehindVector(dt))
                    SettlingToParkedDistance = false;
            }

            return base.ContinueExecute(dt);
        }
        private void SetBehindVector(float distance, float dt, bool snap = false)
        {
            BehindVector = GetBehindVector(distance);

            MoveTowardBehindVector(dt, snap);
        }
        private Vec3d GetBehindVector(float distance)
        {
            Vec3d targetPosition = mobileStorageEntity.AttachedEntity.Pos.BehindCopy(distance).XYZ;
            targetPosition.Y = mobileStorageEntity.AttachedEntity.Pos.Y;

            return targetPosition;
        }
        private bool MoveTowardBehindVector(float dt, bool snap = false)
        {
            Vec3d currentPosition = mobileStorageEntity.EntityTransform.XYZ;
            double dx = BehindVector.X - currentPosition.X;
            double dy = BehindVector.Y - currentPosition.Y;
            double dz = BehindVector.Z - currentPosition.Z;
            double distanceSq = dx * dx + dy * dy + dz * dz;

            if (snap || distanceSq > SNAP_DISTANCE_SQ)
            {
                mobileStorageEntity.SetEntityPosition(BehindVector);
                return true;
            }

            if (distanceSq < MIN_MOVE_DISTANCE_SQ)
                return false;

            double response = Math.Min(1.0, Math.Max(0.0, dt * PULL_RESPONSE_PER_SECOND));
            Vec3d nextPosition = new Vec3d(
                currentPosition.X + dx * response,
                BehindVector.Y,
                currentPosition.Z + dz * response
            );

            mobileStorageEntity.SetEntityPosition(nextPosition);
            return true;
        }
    }
}
