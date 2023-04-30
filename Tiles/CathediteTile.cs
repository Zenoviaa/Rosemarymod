﻿
using Microsoft.Xna.Framework;
using Stellamod.Items.Placeable.Cathedral;
using Terraria;
using Terraria.ModLoader;

namespace Stellamod.Tiles
{
	public class CathediteTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;

			DustType = Main.rand.Next(110, 113);
			ItemDrop = ModContent.ItemType<CathediteGrassBlock>();
			MineResist = 2f;
			MinPick = 225;

			AddMapEntry(new Color(2, 14, 26));

			// TODO: implement
			// SetModTree(new Trees.ExampleTree());
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		// TODO: implement
		// public override void ChangeWaterfallStyle(ref int style) {
		// 	style = mod.GetWaterfallStyleSlot("ExampleWaterfallStyle");
		// }
		//
		// public override int SaplingGrowthType(ref int style) {
		// 	style = 0;
		// 	return TileType<ExampleSapling>();
		// }
	}
}