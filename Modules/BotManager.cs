using System;
using System.Collections.Generic;
using System.Linq;
using InnerNet;
using TownOfHost;

namespace TownOfHost
{
    class BotManager
    {
        public static BotManager Instance;
        public List<PlayerControl> CrewmateBots;
        public List<PlayerControl> ImpostorBots;
        public static void Begin()
        {
            Instance = new BotManager();
        }
        public void SpawnBots()
        {
            GameOptionsData opt = main.RealOptionsData;
            DespawnBots();

            int CrewBotNum = opt.NumImpostors + 1;
        }
        public PlayerControl SpawnBot(byte playerId)
        {
            var bot = UnityEngine.Object.Instantiate(AmongUsClient.Instance.PlayerPrefab);
            bot.PlayerId = playerId;
            GameData.Instance.AddPlayer(bot);
            AmongUsClient.Instance.Spawn(bot, -2, SpawnFlags.None);
            bot.transform.position = PlayerControl.LocalPlayer.transform.position;
            bot.NetTransform.enabled = true;
            GameData.Instance.RpcSetTasks(bot.PlayerId, new byte[0]);
            
            bot.RpcSetColor((byte)PlayerControl.LocalPlayer.CurrentOutfit.ColorId);
            bot.RpcSetName(PlayerControl.LocalPlayer.name);
            bot.RpcSetPet(PlayerControl.LocalPlayer.CurrentOutfit.PetId);
            bot.RpcSetSkin(PlayerControl.LocalPlayer.CurrentOutfit.SkinId);
            bot.RpcSetNamePlate(PlayerControl.LocalPlayer.CurrentOutfit.NamePlateId);
            
            return bot;
        }
        public void DespawnBots()
        {
        }
        public void DespawnBot(PlayerControl bot)
        {
            GameData.Instance.RemovePlayer(bot.PlayerId);
            AmongUsClient.Instance.Despawn(bot);
        }
        public BotManager()
        {
            this.CrewmateBots = new List<PlayerControl>();
            this.ImpostorBots = new List<PlayerControl>();
        }
    }
}