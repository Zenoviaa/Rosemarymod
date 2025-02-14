﻿using Microsoft.Xna.Framework;
using Stellamod.Helpers;
using Stellamod.Items.Weapons.Melee;
using Stellamod.NPCs.Bosses.GothiviaTheSun.GOS;
using Stellamod.NPCs.Town;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Stellamod
{
    public static class StellaMultiplayer
    {
        private struct Wait
        {
            public Func<bool> Condition { get; set; }
            public Action Result { get; set; }
        }

        private static List<Wait> _waits = new List<Wait>();

        public static bool IsHost => Main.netMode != NetmodeID.MultiplayerClient;
        public static void Load() => Main.OnTickForInternalCodeOnly += OnTick;

        public static void Unload()
        {
            Main.OnTickForInternalCodeOnly -= OnTick;
            _waits = null;
        }

        public static void OnTick()
        {
            if (_waits == null) return;

            for (int i = 0; i < _waits.Count; i++)
            {
                Wait wait = _waits[i];
                if (wait.Condition.Invoke())
                {
                    wait.Result?.Invoke();
                    _waits.RemoveAt(i--);
                }
            }
        }

        public static void WaitUntil(Func<bool> condition, Action whenTrue) => _waits.Add(new Wait() { Condition = condition, Result = whenTrue });
        public static void WriteItemList(this BinaryWriter writer, List<Item> arr)
        {
            writer.Write(arr.Count);
            for (int i = 0; i < arr.Count; i++)
            {
                writer.Write(arr[i].type);
            }
        }
        public static List<Item> ReadItemList(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            List<Item> itemList = new List<Item>();
            for (int i = 0; i < length; i++)
            {
                itemList.Add(new Item(reader.ReadInt32()));
            }
            return itemList;
        }

        public static void WriteItemArray(this BinaryWriter writer, Item[] arr)
        {
            writer.Write(arr.Length);
            for (int i = 0; i < arr.Length; i++)
            {
                writer.Write(arr[i].type);
            }
        }
        public static Item[] ReadItemArray(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            Item[] array = new Item[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = new Item(reader.ReadInt32());
            }
            return array;
        }

        public static ModPacket WriteToPacket(ModPacket packet, byte msg, params object[] param)
        {
            packet.Write(msg);

            for (int m = 0; m < param.Length; m++)
            {
                object obj = param[m];
                if (obj is bool) packet.Write((bool)obj);
                else if (obj is byte) packet.Write((byte)obj);
                else if (obj is int) packet.Write((int)obj);
                else if (obj is float) packet.Write((float)obj);
                else if (obj is double) packet.Write((double)obj);
                else if (obj is short) packet.Write((short)obj);
                else if (obj is ushort) packet.Write((ushort)obj);
                else if (obj is sbyte) packet.Write((sbyte)obj);
                else if (obj is uint) packet.Write((uint)obj);
                else if (obj is decimal) packet.Write((decimal)obj);
                else if (obj is long) packet.Write((long)obj);
                else if (obj is string) packet.Write((string)obj);
            }
            return packet;
        }

        public static ModPacket WriteToPacket(int capacity, MessageType type)
        {
            ModPacket packet = Stellamod.Instance.GetPacket(capacity);
            packet.Write((byte)type);
            return packet;
        }

        public static ModPacket WriteToPacket(int capacity, MessageType type, Action<ModPacket> action)
        {
            ModPacket packet = Stellamod.Instance.GetPacket(capacity);
            packet.Write((byte)type);
            action?.Invoke(packet);
            return packet;
        }

        public static ModPacket WriteToPacketAndSend(int capacity, MessageType type, Action<ModPacket> beforeSend)
        {
            var packet = WriteToPacket(capacity, type, beforeSend);
            packet.Send();
            return packet;
        }

        public static void HandlePacket(BinaryReader reader, int whoAmI)
        {
            var id = (MessageType)reader.ReadByte();
            byte player;
            switch (id)
            {
                case MessageType.Dodge:
                    VixylPlayer.HandleExampleDodgeMessage(reader, whoAmI);
                    break;
                case MessageType.BossSpawnFromClient:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        player = reader.ReadByte();
                        int bossType = reader.ReadInt32();
                        int TileCordsX = reader.ReadInt32();
                        int TileCordsY = reader.ReadInt32();
                        int npcCenterX = reader.ReadInt32();
                        int npcCenterY = reader.ReadInt32();

                        if (NPC.AnyNPCs(bossType))
                            return;

                        int npcID = NPC.NewNPC(new EntitySource_TileBreak(TileCordsX, TileCordsY), TileCordsX, TileCordsY, bossType);
                        Main.npc[npcID].netUpdate2 = true;

                    }
                    break;
                case MessageType.CompleteMerenaQuest:
                    break;

                case MessageType.CompleteZuiQuest:
                    ZuiQuestSystem.QuestsCompleted++;
                    break;

                case MessageType.CreatePortal:
                    break;


                case MessageType.StartBossFromDialogue:

                    break;

                case MessageType.StartDialogue:

                    break;

                case MessageType.STARBLOCK:
                    //EventWorld.Aurorean = false;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetworkText auroeanStarfallEnded = NetworkText.FromLiteral("The Aurorean Starfall has been blocked! :(");
                        ChatHelper.BroadcastChatMessage(auroeanStarfallEnded, new Color(234, 96, 114));
                    }

                    break;
            }
        }

  
     
        public static void SpawnBossFromClient(byte whoAmI, int type, int x, int y) =>
            Stellamod.WriteToPacket(Stellamod.Instance.GetPacket(), (byte)MessageType.BossSpawnFromClient, whoAmI, type, x, y).Send(-1);
    }
}