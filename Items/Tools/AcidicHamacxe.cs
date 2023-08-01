using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;
using Stellamod.Items.Materials;
using Stellamod.Items.Materials.Tech;

namespace Stellamod.Items.Tools
{
	public class AcidicHamacxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Irradia Hamaxe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
		}

		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.axe = 100;
			Item.hammer = 150;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 8);
            recipe.AddIngredient(ItemType<ToolDrive>(), 1);
            recipe.AddIngredient(ItemType<IrradiatedBar>(), 18);
			recipe.AddIngredient(ItemType<VirulentPlating>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

	}
}