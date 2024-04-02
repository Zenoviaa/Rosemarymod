﻿using Stellamod.Helpers;
using Stellamod.UI.Dialogue;
using Stellamod.UI.Scripture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Stellamod.Assets.Biomes;
using Stellamod.Dusts;
using Stellamod.Items.Accessories;
using Stellamod.Items.Accessories.Brooches;
using Stellamod.Items.Armors.Vanity.Gia;
using Stellamod.Items.Consumables;
using Stellamod.Items.Harvesting;
using Stellamod.Items.Materials;
using Stellamod.Items.Materials.Tech;
using Stellamod.Items.Ores;
using Stellamod.Items.Placeable;
using Stellamod.Items.Quest.BORDOC;
using Stellamod.Items.Quest.Merena;
using Stellamod.Items.Weapons.Igniters;
using Stellamod.Items.Weapons.Mage;
using Stellamod.Items.Weapons.Mage.Stein;
using Stellamod.Items.Weapons.Melee;
using Stellamod.Items.Weapons.Melee.Greatswords;
using Stellamod.Items.Weapons.Melee.Safunais;
using Stellamod.Items.Weapons.PowdersItem;
using Stellamod.Items.Weapons.Ranged;
using Stellamod.Items.Weapons.Summon;
using Stellamod.Items.Weapons.Thrown;
using Stellamod.Items.Weapons.Whips;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.Localization;
using Terraria.ModLoader.Utilities;
using Terraria.Utilities;

namespace Stellamod.Items.Special.Sirestias
{
    internal class SiresMail : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ModContent.RarityType<SirestiasSpecialRarity>();
        }


        public override bool? UseItem(Player player)
        {
            DialogueSystem dialogueSystem = ModContent.GetInstance<DialogueSystem>();
            if (!DownedBossSystem.downedGintzlBoss)
            {
                switch (Main.rand.Next(3))
                {


                    case 0:
                            CallDialogue1 exampleDialogue = new CallDialogue1();

                       
                             dialogueSystem.StartDialogue(exampleDialogue);
                        break;

                    case 1:
                        CallDialogue1 exampleDialogue2 = new CallDialogue1();


                        dialogueSystem.StartDialogue(exampleDialogue2);
                        break;


                    case 2:
                        CallDialogue1 exampleDialogue3 = new CallDialogue1();


                        dialogueSystem.StartDialogue(exampleDialogue3);
                        break;



                }
                      



            }

            if (DownedBossSystem.downedGintzlBoss)
            {

                if (player.GetModPlayer<MyPlayer>().ZoneFable)
                switch (Main.rand.Next(1))
                {


                    case 0:
                        CallDialogue2 exampleDialogue = new CallDialogue2();


                        dialogueSystem.StartDialogue(exampleDialogue);
                        break;

                   



                }

                if (!player.GetModPlayer<MyPlayer>().ZoneFable)
                    switch (Main.rand.Next(1))
                    {


                        case 0:
                            CallDialogue3 exampleDialogue = new CallDialogue3();


                            dialogueSystem.StartDialogue(exampleDialogue);
                            break;





                    }




            }
            //1. Get the dialogue system


            //2. Create a new instance of your dialogue

            return true;
        }
    }
}