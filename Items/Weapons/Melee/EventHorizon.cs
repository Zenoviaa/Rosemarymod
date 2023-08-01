﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Net;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.Creative;
using Stellamod.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stellamod.Projectiles.Weapons.Bow;
using Terraria.DataStructures;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;
using Stellamod.Projectiles.Weapons.Swords;
using Stellamod.Projectiles.Weapons.Magic;
using Stellamod.Items.Accessories.Runes;

using Stellamod.Projectiles.Weapons.Spears;
using Terraria.Audio;
using Stellamod.Projectiles.Weapons.Gun;

namespace Stellamod.Items.Weapons.Swords
{
    public class EventHorizon : ModItem
    {
        public int WinterboundArrow;
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Damage reduces the farther you are away from the target");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.width = 50;
            Item.height = 50;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(0, 0, 16, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<HorizonBolt>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Melee;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(2))
            {
                // Emit dusts when the sword is swung
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Snow);
            }
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            WinterboundArrow += 1;
            if (WinterboundArrow >= 4)
            {
                SoundEngine.PlaySound(new SoundStyle("Stellamod/Sounds/Custom/Item/FrostBringer"), player.position);
                WinterboundArrow = 0;
                type = ModContent.ProjectileType<HorizonBomb>();
            }


        }
    }
}