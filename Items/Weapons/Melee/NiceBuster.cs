﻿using Microsoft.Xna.Framework;
using Stellamod.Items.Harvesting;
using Stellamod.Items.Materials;
using Stellamod.Projectiles.Magic;
using Stellamod.Projectiles.Slashers.NiceBuster;
using Stellamod.Projectiles.Swords;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Stellamod.Items.Weapons.Melee
{
    class NiceBusterr : ClassSwapItem
    {

        public int dir;

        public override DamageClass AlternateClass => DamageClass.Magic;

        public override void SetClassSwappedDefaults()
        {
            Item.damage = 80;
            Item.mana = 12;
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cinder Braker");
        }

        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7;
            Item.value = Item.sellPrice(0, 10, 20, 14);
            Item.rare = ItemRarityID.Blue;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<NiceBusterProj>();
            Item.shootSpeed = 8f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Using the shoot function, we override the swing projectile to set ai[0] (which attack it is)
            int Sound = Main.rand.Next(1, 3);

            float numberProjectiles = 2;
            float rotation = MathHelper.ToRadians(20);
            position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f; // This defines the projectile roatation and speed. .4f == projectile speed
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<NiceBuster2>(), damage, Item.knockBack, player.whoAmI);
            }

            if (dir == -1)
            {
                dir = 1;
            }
            else if (dir == 1)
            {
                dir = -1;
            }
            else
            {
                dir = 1;
            }

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, dir);
            return false; // return false to prevent original projectile from being shot
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<AlcaricMush>(), 25);
            recipe.AddIngredient(ItemType<IshtarCandle>(), 1);
            recipe.AddIngredient(ItemType<Curlistine>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}