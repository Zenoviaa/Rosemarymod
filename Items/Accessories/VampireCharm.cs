﻿using Microsoft.Xna.Framework;
using Stellamod.Brooches;
using Stellamod.Items.Materials;
using Stellamod.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Stellamod.Items.Accessories
{
    public class VampireCritPlayer : ModPlayer
    {
        public bool hasVampireCharm;
        public override void ResetEffects()
        {
            base.ResetEffects();
            hasVampireCharm = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (hit.Crit && hasVampireCharm)
            {
                if (Main.rand.NextBool(4))
                {
                    //Life steal for 5% of the damage
                    float healFactor = damageDone * 0.08f;
                    int healthToHeal = (int)healFactor;
                    healthToHeal = Math.Clamp(healthToHeal, 1, 20);
                    Player.Heal(healthToHeal);

                    int count = 8;
                    float degreesPer = 360 / (float)count;
                    for (int k = 0; k < count; k++)
                    {
                        float degrees = k * degreesPer;
                        Vector2 direction = Vector2.One.RotatedBy(MathHelper.ToRadians(degrees));
                        Vector2 vel = direction * 2;
                        Dust.NewDust(target.Center, 0, 0, DustID.BloodWater, vel.X, vel.Y);
                    }

                    Dust.QuickDustLine(Player.Center, target.Center, 100f, Color.Red);
                    SoundEngine.PlaySound(SoundID.NPCHit18);
                }

                int projectilSpawnCount = Main.rand.Next(2, 5);
                for (int d = 0; d < projectilSpawnCount; d++)
                {
                    float speedX = Main.rand.Next(-15, 15);
                    float speedY = Main.rand.Next(-15, 15);
                    Vector2 speed = new Vector2(speedX, speedY);
                    Projectile.NewProjectile(Player.GetSource_OnHit(target), (int)target.Center.X, (int)target.Center.Y, speed.X, speed.Y, ModContent.ProjectileType<BloodWaterProj>(), 60, 1f, Player.whoAmI);
                }
            }
        }
    }

    public class VampireCharm : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.value = 2500;
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            //Increased Crit Chance
            player.GetCritChance(DamageClass.Generic) += 8f;
            player.GetModPlayer<VampireCritPlayer>().hasVampireCharm = true;
        }

        public override void AddRecipes()
        {
            base.AddRecipes();
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PutridScent, 1);
            recipe.AddIngredient(ModContent.ItemType<TerrorFragments>(), 30);
            recipe.AddIngredient(ItemID.CursedFlame, 15);
            recipe.AddIngredient(ItemID.SoulofNight, 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}