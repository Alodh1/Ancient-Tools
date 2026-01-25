using AncientTools.Config;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace AncientTools.Utility
{
    class ModConfig
    {
        public AncientToolsConfig ATConfig { get; set; }

        public void ReadConfig(ICoreAPI api, AncientToolsConfig updatedConfig)
        {
            api.World.Config.SetInt("MortarOutputModifier", updatedConfig.MortarOutputModifier);
            api.World.Config.SetFloat("MortarGrindTime", updatedConfig.MortarGrindTime);
            api.World.Config.SetBool("AllowMortarGrindablePlaceableWithSneak", updatedConfig.AllowMortarGrindablePlaceableWithSneak);
            api.World.Config.SetInt("BarkPerLog", updatedConfig.BarkPerLog);
            api.World.Config.SetDouble("BaseBarkStrippingSpeed", updatedConfig.BaseBarkStrippingSpeed);
            api.World.Config.SetDouble("BasePrimitiveBarrelCarvingSpeed", updatedConfig.BasePrimitiveBarrelCarvingSpeed);
            api.World.Config.SetDouble("SalveMixTime", updatedConfig.SalveMixTime);
            api.World.Config.SetBool("BarkBreadEnabled", updatedConfig.BarkBreadEnabled);
            api.World.Config.SetBool("DisableVanillaHideCrafting", updatedConfig.DisableVanillaHideCrafting);
            api.World.Config.SetBool("DisableVanillaHideCraftingRecipeOnly", updatedConfig.DisableVanillaHideCraftingRecipeOnly);
            api.World.Config.SetBool("DisableVanillaGlue", updatedConfig.DisableVanillaGlue);
            api.World.Config.SetBool("SalveEnabled", updatedConfig.SalveEnabled);
            api.World.Config.SetBool("AllowCarvingForResin", updatedConfig.AllowCarvingForResin);
            api.World.Config.SetFloat("SkinningTime", updatedConfig.SkinningTime);
            api.World.Config.SetDouble("WaterSackConversionHours", updatedConfig.WaterSackConversionHours);
            api.World.Config.SetBool("DisableBrainDrops", updatedConfig.DisableBrainDrops);
            api.World.Config.SetInt("BrainsPerBrainingSolutionCraft", updatedConfig.BrainsPerBrainingSolutionCraft);
            api.World.Config.SetFloat("BrainedHideSealHours", updatedConfig.BrainedHideSealHours);
            api.World.Config.SetFloat("BrainedHideSmokingSeconds", updatedConfig.BrainedHideSmokingSeconds);
            api.World.Config.SetBool("InWorldBeamCraftingOnly", updatedConfig.InWorldBeamCraftingOnly);
            api.World.Config.SetBool("DisableWedgePickupWireframe", updatedConfig.DisableWedgePickupWireframe);
            api.World.Config.SetBool("AdzeStrippingOnly", updatedConfig.AdzeStrippingOnly);
            api.World.Config.SetInt("CandleChamberstickLightLevel", updatedConfig.CandleChamberstickLightLevel);
            api.World.Config.SetInt("PitchChamberstickLightLevel", updatedConfig.PitchChamberstickLightLevel);
        }
        public void ReadConfigServer(ICoreServerAPI api)
        {
            try
            {
                ATConfig = LoadConfig(api);

                if (ATConfig == null)
                {
                    GenerateConfig(api);
                    ATConfig = LoadConfig(api);
                }
                else
                {
                    GenerateConfig(api, ATConfig);
                }
            }
            catch
            {
                GenerateConfig(api);
                ATConfig = LoadConfig(api);
            }

            if (ATConfig == null && api.ModLoader.IsModEnabled("autoconfiglib"))
            {
                api.Logger.Warning("AutoConfigLib does not play nicely with the Ancient Tools config system. An error was captured, but configuration should still function through the ConfigLib system.");
                return;
            }

            ReadConfig(api, ATConfig);

        }
        private AncientToolsConfig LoadConfig(ICoreAPI api)
        {
            return api.LoadModConfig<AncientToolsConfig>("AncientToolsConfig.json");
        }
        private void GenerateConfig(ICoreAPI api)
        {
            api.StoreModConfig<AncientToolsConfig>(new AncientToolsConfig(), "AncientToolsConfig.json");
        }
        private void GenerateConfig(ICoreAPI api, AncientToolsConfig previousConfig)
        {
            api.StoreModConfig<AncientToolsConfig>(new AncientToolsConfig(previousConfig), "AncientToolsConfig.json");
        }
    }
}
