﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stellamod.Gores;
using Stellamod.Helpers;
using Stellamod.Trails;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Stellamod.Projectiles.GunHolster
{
    internal class BasterPartyProj : ModProjectile,
        IPixelPrimitiveDrawer
    {
        private int _color;
        private ref float Timer => ref Projectile.ai[0];
        public override string Texture => TextureRegistry.EmptyTexture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            Timer++;
            if (Timer == 1)
            {
                _color = Main.rand.Next(4);
            }

            Projectile.velocity.Y += 0.2f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public float WidthFunction(float completionRatio)
        {
            return 1;
        }

        public Color ColorFunction(float completionRatio)
        {
            switch (_color)
            {
                default:
                case 0:
                    return new Color(0, 219, 255);
                case 1:
                    return new Color(255, 145, 0);
                case 2:
                    return new Color(242, 255, 51);
                case 3:
                    return Color.White;
            }
        }

        public override void OnKill(int timeLeft)
        {
            int goreType;
            switch (_color)
            {
                default:
                case 0:
                    goreType = ModContent.GoreType<RibbonBlue>();
                    break;
                case 1:
                    goreType = ModContent.GoreType<RibbonPink>();
                    break;
                case 2:
                    goreType = ModContent.GoreType<RibbonYellow>();
                    break;
                case 3:
                    goreType = ModContent.GoreType<RibbonWhite>();
                    break;
            }
            for (int i = 0; i < 1; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4, 4);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, velocity,
                  goreType);
            }
        }

        internal PrimitiveTrail BeamDrawer;
        internal Vector2[] TrailPos;
        public void DrawPixelPrimitives(SpriteBatch spriteBatch)
        {
            BeamDrawer ??= new PrimitiveTrail(WidthFunction, ColorFunction, null, true, TrailRegistry.LaserShader);
            BeamDrawer.SpecialShader = null;

            if (TrailPos == null)
            {
                TrailPos = new Vector2[Projectile.oldPos.Length];
                for (int i = 0; i < TrailPos.Length; i++)
                {
                    Projectile.oldPos[i] = Projectile.position;
                }
            }

            for (int i = 0; i < TrailPos.Length; i++)
            {
                float distance = 32;
                TrailPos[i] = Projectile.oldPos[i];
                TrailPos[i] += new Vector2(VectorHelper.Osc(0, 16, offset: i));
            }
            BeamDrawer.DrawPixelated(TrailPos, -Main.screenPosition, 32);
            Main.spriteBatch.ExitShaderRegion();
        }
    }
}
