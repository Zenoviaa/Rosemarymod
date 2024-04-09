﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Stellamod.Projectiles.IgniterExplosions
{
    internal class SiriusBoom : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 30;
        }

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.width = 129;
            Projectile.height = 129;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Projectile.rotation -= 0.01f;
            Vector3 RGB = new(0.89f, 2.53f, 2.55f);
            // The multiplication here wasn't doing anything
            Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);
        }

        public override bool PreAI()
        {
            Projectile.tileCollide = false;
            if (++Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 30)
                {
                    Projectile.frame = 0;
                }
            }
            return true;


        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * (1f - Projectile.alpha / 50f);
        }
    }
}