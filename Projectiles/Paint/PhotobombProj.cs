﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stellamod.UI.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Stellamod.Projectiles.Paint
{
	public class PhotobombProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;//number of frames the animation has
		}

		public float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void SetDefaults()
		{
			Projectile.damage = 0;
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = 595;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.timeLeft = 101;
		}

		public override bool? CanDamage()
		{
			return false;
		}


		public float beens = 0;
		public override void AI()
		{
			Timer++;
			beens++;
			if (Timer > 155)
			{
				// Our timer has finished, do something here:
				// Main.PlaySound, Dust.NewDust, Projectile.NewProjectile, etc. Up to you.		
				SoundEngine.PlaySound(new SoundStyle($"Stellamod/Assets/Sounds/MorrowSalfi"));
				Timer = 0;
			}

			Player player = Main.player[Projectile.owner];
			if (player.noItems || player.CCed || player.dead || !player.active)
				Projectile.Kill();

			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, true);
			float swordRotation = 0f;
			if (Main.myPlayer == Projectile.owner)
			{
				player.ChangeDir(Projectile.direction);
				swordRotation = (Main.MouseWorld - player.Center).ToRotation();
				if (!player.channel)
					Projectile.Kill();
			}

			Projectile.velocity = swordRotation.ToRotationVector2();
			Projectile.spriteDirection = player.direction;
			if (Projectile.spriteDirection == 1)
				Projectile.rotation = Projectile.velocity.ToRotation();
			else
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;


			if (beens == 5)
            {
				float speedX = Projectile.velocity.X * 10;
				float speedY = Projectile.velocity.Y * 7;

				Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, AmmoID.Bullet), Projectile.Center, Projectile.velocity * 12f, ProjectileID.PainterPaintball, Projectile.damage * 1, Projectile.knockBack, player.whoAmI);

				beens = 0;
            }


			if (Timer == 20)
			{
				float speedX = Projectile.velocity.X * 10;
				float speedY = Projectile.velocity.Y * 7;

				Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, AmmoID.Bullet), Projectile.Center, Projectile.velocity * 12f, ModContent.ProjectileType<PhotobombShot>(), Projectile.damage * 1, Projectile.knockBack, player.whoAmI);
				
				ShakeModSystem.Shake = 4;
				SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, Projectile.Center);
			}

			if (Timer == 60)
			{
				ShakeModSystem.Shake = 4;
				Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, AmmoID.Bullet), Projectile.Center, Projectile.velocity * 10f, ModContent.ProjectileType<PhotobombShot>(), Projectile.damage * 1, Projectile.knockBack, player.whoAmI);
				SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, Projectile.Center);
			}

			if (Timer == 100)
			{
				ShakeModSystem.Shake = 4;
				Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, AmmoID.Bullet), Projectile.Center, Projectile.velocity * 10f, ModContent.ProjectileType<PhotobombShot>(), Projectile.damage * 1, Projectile.knockBack, player.whoAmI);
				SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, Projectile.Center);
			}

			Projectile.Center = playerCenter + Projectile.velocity * 1f;// customization of the hitbox position

			player.heldProj = Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * Projectile.direction, Projectile.velocity.X * Projectile.direction);

			if (++Projectile.frameCounter >= 1)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 1)
				{
					Projectile.frame = 0;
				}
			}
		}
		private void UpdatePlayerVisuals(Player player, Vector2 playerhandpos)
		{
			Projectile.Center = playerhandpos;
			Projectile.spriteDirection = Projectile.direction;

			// Constantly resetting player.itemTime and player.itemAnimation prevents the player from switching items or doing anything else.
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 3;
			player.itemAnimation = 3;

			player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
		}
		public override bool PreDraw(ref Color lightColor)
		{
			//Player player = Main.player[Projectile.owner];
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			int startY = frameHeight * Projectile.frame;
			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size() / 2f;
			origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - 30 : 30; // Customization of the sprite position

			Color drawColor = Projectile.GetAlpha(lightColor);
			Main.EntitySpriteDraw((Texture2D)TextureAssets.Projectile[Projectile.type], Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
			return false;
		}
	}
}