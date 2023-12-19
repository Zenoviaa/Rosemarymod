﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using ReLogic.Content;
using Stellamod.Particles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Stellamod.NPCs.Bosses.Sylia.Projectiles
{
    internal class VoidVortex : ModProjectile
    {
		private static Asset<Texture2D> VorTexture;
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 18;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 2;
			Projectile.scale = 1.3f;
			Projectile.usesLocalNPCImmunity = true;

			//5 ticks local npc immunity
			Projectile.localNPCHitCooldown = 16;
		}

		public override void AI()
		{
			Projectile.rotation += 0.5f;
			Projectile.velocity.X *= 0.0f;
			Projectile.velocity.Y *= 0.01f;

			float suckingStrength = 0.21f;
			float suckingDistance = 2048;
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player npc = Main.player[i];
				if (npc.active)
				{
					float distance = Vector2.Distance(Projectile.Center, npc.Center);
					if (distance <= suckingDistance)
					{
						Vector2 direction = npc.Center - Projectile.Center;
						direction.Normalize();
						npc.velocity -= direction * suckingStrength;
					}
				}
			}

			VoidDustVisuals();
			VoidSuckVisuals();
			Visuals();
		}

		private void VoidSuckVisuals()
        {
			float distance = 128;
			float particleSpeed = 4;
			Vector2 position = Projectile.Center + Main.rand.NextVector2CircularEdge(distance, distance);
			Vector2 speed = (Projectile.Center - position).SafeNormalize(Vector2.Zero) * particleSpeed;
			ParticleManager.NewParticle(position, speed, ParticleManager.NewInstance<VoidParticle>(), default(Color), 1f);
		}

		private void VoidDustVisuals()
        {
			float distance = 128;
			float particleSpeed = 4;
			Vector2 position = Projectile.Center + Main.rand.NextVector2CircularEdge(distance, distance);
			Vector2 speed = (Projectile.Center - position).SafeNormalize(Vector2.Zero) * particleSpeed;
			Dust.NewDustPerfect(position, DustID.GemAmethyst, speed);
		}

		private void Visuals()
		{
			Lighting.AddLight(Projectile.position, 1.5f, 0.7f, 2.5f);
			Lighting.Brightness(2, 2);
		}

		public override bool PreAI()
		{
			Projectile.tileCollide = false;
			if (++Projectile.frameCounter >= 1)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 18)
				{
					Projectile.frame = 0;
				}
			}

			return true;
		}

		public override void Load()
		{ // This is called once on mod (re)load when this piece of content is being loaded.
		  // This is the path to the texture that we'll use for the hook's chain. Make sure to update it.
			VorTexture = ModContent.Request<Texture2D>("Stellamod/Assets/Effects/VoxTexture");
		}

		public override void Unload()
		{ // This is called once on mod reload when this piece of content is being unloaded.
		  // It's currently pretty important to unload your static fields like this, to avoid having parts of your mod remain in memory when it's been unloaded.
			VorTexture = null;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			Color drawColor = new(60, 0, 255, 0);
			Main.EntitySpriteDraw(VorTexture.Value, drawPosition,
						  VorTexture.Value.Bounds, drawColor, Projectile.rotation,
						  VorTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);

			Color drawColor2 = new(60, 0, 255, 0);
			Main.EntitySpriteDraw(VorTexture.Value, drawPosition,
						  VorTexture.Value.Bounds, drawColor2, -Projectile.rotation,
						  VorTexture.Size() * 0.5f, 2f, SpriteEffects.None, 0);

			Color drawColor3 = new(60, 0, 255, 0);
			Main.EntitySpriteDraw(VorTexture.Value, drawPosition,
						  VorTexture.Value.Bounds, drawColor3, Projectile.rotation,
						  VorTexture.Size() * 0.5f, 3f, SpriteEffects.None, 0);

			Color drawColor4 = new(60, 0, 255, 0);
			Main.EntitySpriteDraw(VorTexture.Value, drawPosition,
						  VorTexture.Value.Bounds, drawColor4, -Projectile.rotation,
						  VorTexture.Size() * 0.5f, 4f, SpriteEffects.None, 0);
			return true;
		}
	}
}