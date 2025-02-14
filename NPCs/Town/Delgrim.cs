﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Stellamod.Common;
using Stellamod.Helpers;
using Stellamod.Items.Accessories.Brooches;
using Stellamod.Items.Materials.Molds;
using Stellamod.Items.Weapons.Melee.Greatswords;
using Stellamod.Items.Weapons.Ranged;
using Stellamod.Items.Weapons.Ranged.GunSwapping;

using Stellamod.UI.CellConverterSystem;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Stellamod.NPCs.Town
{
    // [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
    //[AutoloadHead]
    [AutoloadBossHead]
    public class Delgrim : VeilTownNPC
    {
        public int NumberOfTimesTalkedTo = 0;
        public const string ShopName = "Shop";
        public const string ShopName2 = "New Shop";
        public override void SetStaticDefaults()
        {
            // DisplayName automatically assigned from localization files, but the commented line below is the normal approach.
            // DisplayName.SetDefault("Example Person");
            Main.npcFrameCount[Type] = 11; // The amount of frames the NPC has

            NPCID.Sets.ActsLikeTownNPC[Type] = true;

            //To reiterate, since this NPC isn't technically a town NPC, we need to tell the game that we still want this NPC to have a custom/randomized name when they spawn.
            //In order to do this, we simply make this hook return true, which will make the game call the TownNPCName method when spawning the NPC to determine the NPC's name.
            NPCID.Sets.SpawnsWithCustomName[Type] = true;

            NPCID.Sets.NoTownNPCHappiness[Type] = true;
            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            // Sets NPC to be a Town NPC
            NPC.friendly = true; // NPC Will not attack player
            NPC.width = 92;
            NPC.height = 84;
            NPC.aiStyle = -1;
            NPC.damage = 90;
            NPC.defense = 42;
            NPC.lifeMax = 1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            NPC.dontTakeDamage = true;
            NPC.BossBar = Main.BigBossProgressBar.NeverValid;
            SpawnAtPoint = true;
            HasTownDialogue = true;
        }


        //This prevents the NPC from despawning
        public override bool CheckActive()
        {
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.20f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }



        public override ITownNPCProfile TownNPCProfile()
        {
            return new DelgrimPersonProfile();
        }

        public class DelgrimPersonProfile : ITownNPCProfile
        {
            public int RollVariation() => 0;
            public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

            public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
            {
                if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
                    return ModContent.Request<Texture2D>("Stellamod/NPCs/Town/Delgrim");

                if (npc.altTexture == 1)
                    return ModContent.Request<Texture2D>("Stellamod/NPCs/Town/Delgrim_Head");

                return ModContent.Request<Texture2D>("Stellamod/NPCs/Town/Delgrim");
            }

            public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("Stellamod/NPCs/Town/Delgrim_Head");
        }

        public override bool CanChat()
        {
            return true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement(LangText.Bestiary(this, "A magical engineer huh?")),

				// You can add multiple elements if you really wanted to
				// You can also use localization keys (see Localization/en-US.lang)
				new FlavorTextBestiaryInfoElement(LangText.Bestiary(this, "Delgrim the eternal engineer.", "2"))
            });
        }

        // The PreDraw hook is useful for drawing things before our sprite is drawn or running code before the sprite is drawn
        // Returning false will allow you to manually draw your NPC
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // This code slowly rotates the NPC in the bestiary
            // (simply checking NPC.IsABestiaryIconDummy and incrementing NPC.Rotation won't work here as it gets overridden by drawModifiers.Rotation each tick)
            if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(Type, out NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers))
            {


                // Replace the existing NPCBestiaryDrawModifiers with our new one with an adjusted rotation
                NPCID.Sets.NPCBestiaryDrawOffset.Remove(Type);
                NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            }

            return true;

        }
        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();

            int partyGirl = NPC.FindFirstNPC(NPCID.Steampunker);

            // These are things that the NPC has a chance of telling you when you talk to it.
            chat.Add(LangText.Chat(this, "Basic1"));
            chat.Add(LangText.Chat(this, "Basic2"));
            chat.Add(LangText.Chat(this, "Basic3"));
            chat.Add(LangText.Chat(this, "Basic4"), 1.0);
            chat.Add(LangText.Chat(this, "Basic5"), 1.0);


            NumberOfTimesTalkedTo++;
            if (NumberOfTimesTalkedTo >= 40)
            {
                //This counter is linked to a single instance of the NPC, so if ExamplePerson is killed, the counter will reset.
                chat.Add("...");
            }

            return chat; // chat is implicitly cast to a string.
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Magical Engineer Delgrim"
            };
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            // What the chat buttons are when you open up the chat UI
            button2 = Language.GetTextValue("LegacyInterface.28");
            button = LangText.Chat(this, "Button");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (!firstButton)
            {
                shop = ShopName;
            }
        }

        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName)
            //.Add(new Item(ItemID.WaterBolt) { shopCustomPrice = Item.buyPrice(gold: 40) })
            .Add<BlankCard>()
            .Add<BlankAccessory>()
            .Add<BlankBag>()
            .Add<BlankBow>()
            .Add<BlankSword>()
            .Add<BlankGun>()
            .Add<BlankJuggler>()
            .Add<BlankStaff>()
            .Add<BlankStein>()
            .Add<BlankSafunai>()
            .Add<BlankRune>()
            .Add<BlankShield>()
            .Add<BlankBrooch>()
            .Add<BlankOrb>()
            .Add<GunHolster>()
            .Add<Pulsing>()

            .Add<Hitme>()
            .Add<VillagersBroochA>()
            .Add<CogBomber>(Condition.Hardmode)
            .Add<TheTingler>(Condition.Hardmode)
            .Add<GearGutter>(Condition.Hardmode)
            .Add<DelgrimsHammer>(Condition.Hardmode)
            .Add(new Item(ItemID.Wire) { shopCustomPrice = Item.buyPrice(copper: 5) })
            ;
            npcShop.Register(); // Name of this shop tab		
        }


        public override void OpenTownDialogue(ref string text, ref string portrait, ref float timeBetweenTexts, ref SoundStyle? talkingSound, List<Tuple<string, Action>> buttons)
        {
            base.OpenTownDialogue(ref text, ref portrait, ref timeBetweenTexts, ref talkingSound, buttons);
            //Set buttons
            buttons.Add(new Tuple<string, Action>("Shop", OpenShop));
            buttons.Add(new Tuple<string, Action>("CellConverter", OpenCellConverter));

            //Delgrim Portrait
            text = "TestDialogue";
            portrait = "DelgrimPortrait";
            timeBetweenTexts = 0.015f;
            talkingSound = SoundID.Item1;
        }

        private void OpenCellConverter()
        {
            Main.CloseNPCChatOrSign();
            Main.playerInventory = true;
            CellConverterUISystem uiSystem = ModContent.GetInstance<CellConverterUISystem>();
            uiSystem.CellConverterPos = NPC.Center;
            uiSystem.OpenUI();

        }
    }
}