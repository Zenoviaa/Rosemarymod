﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Stellamod.Helpers
{
    public static class DrawHelper
    {
		/// <summary>
		/// Oscillates between two colors
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="speed"></param>
		/// <returns></returns>
		public static Color Oscillate(Color from, Color to, float speed)
        {
			float t = VectorHelper.Osc(0, 1, speed);
			return Color.Lerp(from, to, t);
        }


		/// <summary>
		/// Oscillates between two colors, but Huntrian
		/// <br>See Firefly Staff for example usage</br>
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="speed"></param>
		/// <param name="colorOffset"></param>
		/// <returns></returns>
		public static Vector3 HuntrianColorOscillate(Vector3 from, Vector3 to, Vector3 speed, float colorOffset)
		{
			Vector3 xyz;
			xyz.X = VectorHelper.Osc(from.X, to.X, speed.X, colorOffset);
			xyz.Y = VectorHelper.Osc(from.Y, to.Y, speed.Y, colorOffset);
			xyz.Z = VectorHelper.Osc(from.Z, to.Z, speed.Z, colorOffset);
			return xyz;
		}

		/// <summary>
		/// Simple draw function for animated projectiles
		/// <br>Call this inside of PreDraw and return false</br>
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="lightColor"></param>
		public static void PreDrawAnimatedProjectile(ModProjectile projectile, ref Color lightColor)
        {
			// Center It
			SpriteEffects spriteEffects = SpriteEffects.None;

			if (projectile.Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(projectile.Texture);
			int frameHeight = texture.Height / Main.projFrames[projectile.Projectile.type];
			int startY = frameHeight * projectile.Projectile.frame;

			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size() / 2f;
			float offsetX = 20f;
			origin.X = projectile.Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;
			Color drawColor = projectile.Projectile.GetAlpha(lightColor);
			Main.EntitySpriteDraw(texture,
				projectile.Projectile.Center - Main.screenPosition + new Vector2(0f, projectile.Projectile.gfxOffY),
				sourceRectangle, drawColor, projectile.Projectile.rotation, origin, projectile.Projectile.scale, spriteEffects, 0);
		}

		/// <summary>
		/// Draws an after image for the projectile, this should be called in PreDraw
		/// <br>DOES NOT WORK WITH ANIMATED TEXTURES</br>
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="startColor"></param>
		/// <param name="endColor"></param>
		/// <param name="lightColor"></param>
		public static void PreDrawAdditiveAfterImage(Projectile projectile, Color startColor, Color endColor, ref Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
			int projFrames = Main.projFrames[projectile.type];
			int frameHeight = texture.Height / projFrames;
			int startY = frameHeight * projectile.frame;

			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 drawOrigin = sourceRectangle.Size() / 2f;
			float offsetX = 20f;
			drawOrigin.X = projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(Color.Lerp(startColor, endColor, 1f / projectile.oldPos.Length * k) * (1f - 1f / projectile.oldPos.Length * k));
				Main.spriteBatch.Draw(texture, drawPos, sourceRectangle, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}

		/// <summary>
		/// Draws a dim light effect, this should be called in PostDraw
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="dimLightX"></param>
		/// <param name="dimLightY"></param>
		/// <param name="dimLightZ"></param>
		public static void PostDrawDimLight(Projectile projectile, float dimLightX, float dimLightY, float dimLightZ, Color worldLightingColor, Color lightColor)
        {
			Texture2D texture = ModContent.Request<Texture2D>("Stellamod/Effects/Masks/DimLight").Value;
			for (int i = 0; i < 4; i++)
			{
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, new Color((int)(dimLightX * 1), (int)(dimLightY * 1), (int)(dimLightZ * 1), 0), projectile.rotation, new Vector2(32, 32), 0.17f * (7 + 0.6f), SpriteEffects.None, 0f);
			}

			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, new Color((int)(dimLightX * 1), (int)(dimLightY * 1), (int)(dimLightZ * 1), 0), projectile.rotation, new Vector2(32, 32), 0.07f * (7 + 0.6f), SpriteEffects.None, 0f);
			Lighting.AddLight(projectile.Center, worldLightingColor.ToVector3() * 1.0f * Main.essScale);
		}

		/// <summary>
		/// Animates the projectile from top to bottom
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="frameSpeed"></param>
        public static void AnimateTopToBottom(Projectile projectile, int frameSpeed)
		{           
			// This is a simple "loop through all frames from top to bottom" animation
			projectile.frameCounter++;
			if (projectile.frameCounter >= frameSpeed)
			{
				projectile.frameCounter = 0;
				projectile.frame++;

				if (projectile.frame >= Main.projFrames[projectile.type])
				{
					projectile.frame = 0;
				}
			}
		}
    }
}