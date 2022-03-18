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
            bot.transform.position = PlayerControl.LocalPlayer.transform.position;
            bot.NetTransform.enabled = true;
            AmongUsClient.Instance.Spawn(bot, -2, SpawnFlags.None);
            GameData.Instance.RpcSetTasks(bot.PlayerId, new byte[0]);
            
            //new LateTask(() => {
                bot.RpcSetColor(1);
                bot.RpcSetName("BOT");
                bot.RpcSetPet(PlayerControl.LocalPlayer.CurrentOutfit.PetId);
                bot.RpcSetSkin(PlayerControl.LocalPlayer.CurrentOutfit.SkinId);
                bot.RpcSetNamePlate(PlayerControl.LocalPlayer.CurrentOutfit.NamePlateId);
            //}, 2f, "Set Bot's Cosmetics Task");

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