﻿using Microsoft.Xna.Framework;
using Stellamod.Items.Materials.Molds;
using Stellamod.Items.Ores;
using Stellamod.Projectiles.Paint;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Stellamod.Items.Weapons.Mage
{
    public class PieceOfArt : ClassSwapItem
    {

        public override DamageClass AlternateClass => DamageClass.Ranged;

        public override void SetClassSwappedDefaults()
        {
            Item.damage = 6;
            Item.mana = 0;
        }
        public int AttackCounter = 1;
        public int combowombo = 1;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Doorlauncher"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            // Tooltip.SetDefault("Electrical nail, thats weird");
        }


        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Magic;
            Item.width = 0;
            Item.height = 0;
            Item.useTime = 100;
            Item.useAnimation = 100;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2;
            Item.value = 10000;
            Item.noMelee = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<POAProj>();
            Item.shootSpeed = 20f;
            Item.noUseGraphic = true;
            Item.crit = 12;
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            int dir = AttackCounter;

            Projectile.NewProjectile(source, position, velocity, type, damage + player.GetModPlayer<MyPlayer>().PPPaintDMG2, knockback, player.whoAmI, 1, dir);
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<POAProj2>(), (damage + player.GetModPlayer<MyPlayer>().PPPaintDMG2) * 2, knockback, player.whoAmI, 1, dir);
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<POAProj3>(), (damage + player.GetModPlayer<MyPlayer>().PPPaintDMG2), knockback, player.whoAmI, 1, dir);
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<POAProj4>(), (damage + player.GetModPlayer<MyPlayer>().PPPaintDMG2) * 2, knockback, player.whoAmI, 1, dir);
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<POAProj5>(), (damage + player.GetModPlayer<MyPlayer>().PPPaintDMG2) * 3, knockback, player.whoAmI, 1, dir);
            return false;
        }
        public override void AddRecipes()
        {
            base.AddRecipes();
            this.RegisterBrew(mold: ModContent.ItemType<BlankStaff>(), material: ModContent.ItemType<KaleidoscopicInk>());
        }
    }
}