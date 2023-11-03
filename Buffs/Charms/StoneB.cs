﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Stellamod.Buffs.Charms
{
	public class StoneB : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Charm Buff!");
			// Description.SetDefault("Icy Frileness!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			Lighting.AddLight(player.Center, Color.LightYellow.ToVector3() * 2.75f * Main.essScale);
			player.statDefense += 1;
			player.pickSpeed += 20;
			player.noFallDmg = true;
			player.GetDamage(DamageClass.Generic) *= 0.85f;
		}
	}
}