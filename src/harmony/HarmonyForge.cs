using AncientTools.BlockEntityBehaviors;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace AncientTools
{
    [HarmonyPatch(typeof(BlockEntityForge), "OnPlayerInteract", MethodType.Normal)]
    public class HarmonyForgeInteract
    {
        static void Prefix(BlockEntityForge __instance, IPlayer byPlayer)
        {
            try
            {
                BlockEntityBehaviorFireproofFuel fireproofFuelBehavior = __instance.GetBehavior<BlockEntityBehaviorFireproofFuel>();
                ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;

                if (byPlayer.Entity.Controls.Sneak && fireproofFuelBehavior != null && slot.Itemstack != null)
                {
                    CombustibleProperties combustProps = slot.Itemstack.Collectible.GetCombustibleProperties(__instance.Api.World, slot.Itemstack, null);

                    if (combustProps != null && combustProps.BurnTemperature > 1000)
                    {
                        bool isWaterproofFuel = slot.Itemstack.Collectible.Attributes?["waterproofFuel"].AsBool() == true;
                        fireproofFuelBehavior.SetFedFireproofFuel(isWaterproofFuel);
                    }
                }
            }
            catch
            {

            }
        }
    }
}
