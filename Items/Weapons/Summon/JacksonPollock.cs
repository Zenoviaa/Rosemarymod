﻿using Microsoft.Xna.Framework;
using Stellamod.Buffs.Minions;
using Stellamod.Items.Materials.Molds;
using Stellamod.Items.Ores;
using Stellamod.Projectiles.Summons.Minions;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Stellamod.Items.Weapons.Summon
{
    internal class JacksonPollock : ClassSwapItem
    {

        public override DamageClass AlternateClass => DamageClass.Magic;

        public override void SetClassSwappedDefaults()
        {
            Item.damage = 25;
            Item.mana = 10;
        }
        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.knockBack = 3f;
            Item.mana = 20;
            Item.width = 76;
            Item.height = 80;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 33, 0);
            Item.rare = ItemRarityID.LightPurple;

            // These below are needed for a minion weapon
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;

            // No buffTime because otherwise the item tooltip would say something like "1 minute duration"
            Item.buffType = ModContent.BuffType<JacksonPollockMinionBuff>();
            Item.shoot = ModContent.ProjectileType<JacksonPollockMinionProj>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Spawn at the mouse cursor position
            position = Main.MouseWorld;
            player.AddBuff(Item.buffType, 2);
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            player.UpdateMaxTurrets();
            return false;
        }

        public override void AddRecipes()
        {
            base.AddRecipes();
            this.RegisterBrew(mold: ModContent.ItemType<BlankStaff>(), material: ModContent.ItemType<KaleidoscopicInk>());
        }
    }
}
