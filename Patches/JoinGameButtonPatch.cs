using HarmonyLib;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System;

namespace TownOfHost
{
    [HarmonyPatch(typeof(JoinGameButton), nameof(JoinGameButton.OnClick))]
    class JoinGameButtonPatch
    {
        public static void Prefix(JoinGameButton __instance)
        {
            if (__instance.GameIdText == null) return;
            if (Regex.IsMatch(GUIUtility.systemCopyBuffer, @"[A-Z]{6}"))
            {
                Logger.info($"{GUIUtility.systemCopyBuffer}");
                __instance.GameIdText.SetText(GUIUtility.systemCopyBuffer);
            }
        }
    }
    [HarmonyPatch(typeof(MMOnlineManager), nameof(MMOnlineManager.Start))]
    class MMOStartPatch
    {
        public static void Prefix(MMOnlineManager __instance)
        {
            var leftTime = main.BanTimestamp.Value + 60 * 30 - (int)((DateTime.UtcNow.Ticks - DateTime.Parse("1970-01-01 00:00:00").Ticks) / 10000000);
            if (leftTime > 0 && main.BanTimestamp.Value != -1)
            {
                if (SceneManager.GetActiveScene().name == "MMOnline") SceneChanger.ChangeScene("MainMenu");
                Logger.info($"BAN解除まで{leftTime}秒");
            }
        }
    }
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.LateUpdate))]
    class MainMenuPatch
    {
        public static GameObject obj;
        public static void Prefix(MainMenuManager __instance)
        {
            if (obj == null) obj = GameObject.Find("PlayOnlineButton");
            var leftTime = main.BanTimestamp.Value + 60 * 30 - (int)((DateTime.UtcNow.Ticks - DateTime.Parse("1970-01-01 00:00:00").Ticks) / 10000000);
            if (leftTime > 0 && main.BanTimestamp.Value != -1)
            {
                obj.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = $"BAN解除まで{leftTime}秒";
                obj.transform.GetComponent<PassiveButton>().enabled = false;
            }
            else
            {
                obj.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = TranslationController.Instance.GetString(StringNames.OnlineButton);
                obj.transform.GetComponent<PassiveButton>().enabled = true;
            }
        }
    }
    [HarmonyPatch(typeof(AbilityButton), nameof(AbilityButton.DoClick))]

    class AbilityButtonDoClickPatch
    {

        static void Prefix(AbilityButton __instance)
        {
            if (true)//押した人の役職がxxだった場合～
            {
                Logger.SendInGame("変身のメニュー画面開く時にログ流れる");

            }
            else
            {
                Logger.SendInGame("それ以外");
            }
        }
        public static void Postfix(AbilityButton __instance)
        {
            if (CustomRoles.MineHunter.isEnable())
            {
                //変身する人を選ぶ画面のキャンセル
                //変身クールダウンのリセット[メモ：opt.RoleOptions.ShapeshifterCooldown = 0;]
                //地雷の設置。
                //非同期で爆弾の座標にいるクルーをキルする処理
            }
        }
    }
}