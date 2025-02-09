using Microsoft.Xna.Framework;
using Stellamod.Items.Materials.Molds;
using Stellamod.Items.Ores;
using Stellamod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Stellamod.Items.Weapons.Mage
{
    public class SwarmerStaff : ClassSwapItem
    {

        public override DamageClass AlternateClass => DamageClass.Ranged;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gelatal Slaff");
            // Tooltip.SetDefault("Summons an Jelly boi to fight for you");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetClassSwappedDefaults()
        {
            base.SetClassSwappedDefaults();
            Item.damage = 7;
        }

        public override void SetDefaults()
        {
            Item.staff[Item.type] = true;
            Item.damage = 15;
            Item.knockBack = 3f;
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item43;
            Item.value = Item.sellPrice(0, 0, 33, 0);
            Item.rare = ItemRarityID.Green;

            // These below are needed for a minion weapon
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.shoot = ModContent.ProjectileType<SwarmerStaffProj>();
            Item.shootSpeed = 12;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numProjectiles = 3;
            for (int i = 0; i < numProjectiles; i++)
            {
                Vector2 shootVelocity = velocity.RotatedByRandom(MathHelper.PiOver4 / 2);
                Projectile.NewProjectile(source, position, shootVelocity, type, damage, knockback, player.whoAmI);
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void AddRecipes()
        {
            base.AddRecipes();
            this.RegisterBrew(mold: ModContent.ItemType<BlankStaff>(), material: ModContent.ItemType<GintzlMetal>());
        }
    }
}