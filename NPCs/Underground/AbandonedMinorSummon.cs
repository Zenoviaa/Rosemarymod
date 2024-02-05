﻿using Stellamod.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Stellamod.NPCs.Underground
{
    internal class AbandonedMinorSummon : ModNPC
    {
        private bool _spawn;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 16;
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.aiStyle = -1;
            NPC.damage = 42;
            NPC.defense = 8;
            NPC.lifeMax = 170;
            NPC.HitSound = SoundID.NPCHit48;
            NPC.DeathSound = SoundID.DD2_SkeletonDeath;
            NPC.value = 563f;
            NPC.knockBackResist = .45f;
        }

        public override void AI()
        {
            if (!_spawn && StellaMultiplayer.IsHost)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X + 30, (int)NPC.Center.Y, 
                    ModContent.NPCType<AbandonedMinor>());

                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X - 30, (int)NPC.Center.Y, 
                    ModContent.NPCType<AbandonedMinor>());

                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X - 45, (int)NPC.Center.Y, 
                    ModContent.NPCType<AbandonedMinor>());
                _spawn = true;
                NPC.Kill();
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //You can't be in the surface and underground at the same time so this should work
            //0.05f should make it 20 less common than normal spawns.
            return (SpawnCondition.Cavern.Chance * 0.5f);
        }
    }
}