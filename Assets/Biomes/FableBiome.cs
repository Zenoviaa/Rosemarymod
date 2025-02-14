﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace Stellamod.Assets.Biomes
{
    // Shows setting up two basic biomes. For a more complicated example, please request.
    public class FableBiome : ModBiome
    {
        public bool IsPrimaryBiome = true; // Allows this biome to impact NPC prices


        // Select all the scenery
        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("Stellamod/IshtarWaterStyle"); // Sets a water style for when inside this biome
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("Stellamod/FabledBackgroundStyle");
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        // Select Music



        public override int Music
        {
            get
            {
                //Put your if statement here


                //Normal music
                if (Main.dayTime)
                {
                    return MusicLoader.GetMusicSlot(Mod, "Assets/Music/OggFabledWilds");
                }
                else
                {
                    return MusicLoader.GetMusicSlot(Mod, "Assets/Music/morrowunderground");
                }
            }
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;

        // Populate the Bestiary Filter
        public override string BestiaryIcon => base.BestiaryIcon;
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;

        // Use SetStaticDefaults to assign the display name
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cathedral of the Moon");
        }

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player) => BiomeTileCounts.InFable;
        public override void OnEnter(Player player) => player.GetModPlayer<MyPlayer>().ZoneFable = true;
        public override void OnLeave(Player player) => player.GetModPlayer<MyPlayer>().ZoneFable = false;
    }
}
