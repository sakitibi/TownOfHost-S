using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using System;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnhollowerBaseLib;
using Hazel;
using System.Linq;

namespace TownOfHost
{
    static class CustomNamePlatesManager {
        public static List<NamePlateData> CustomPlates;
        public static bool isAdded = false;
        public static void LoadAllPlates() {
            var plateDir = new DirectoryInfo("NamePlates");
            var pngFiles = plateDir.GetFiles("*.png");
            CustomPlates = new List<NamePlateData>();
            foreach(var file in pngFiles) {
                try {
                    var plate = ScriptableObject.CreateInstance<NamePlateData>();
                    plate.name = file.Name.Substring(0, file.Name.Length - 5);
                    plate.ProductId = "TOH_" + file.Name;
                    plate.Order = 99;
                    plate.ChipOffset = new Vector2(0f, 0.2f);
                    plate.Free = true;
                    plate.Image = TextureManager.loadSprite("NamePlates\\" + file.Name);
                    CustomPlates.Add(plate);
                    Logger.info("プレート読み込み完了:" + file.Name);
                }
                catch {
                    Logger.error("エラー:CustomNamePlateの読み込みに失敗しました:" + file.FullName);
                }
            }
            Logger.info("プレート読み込み処理終了");
            isAdded = false;
        }
    }
    [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetUnlockedNamePlates))]
    class GetUnlockedNamePlatesPatch {
        public static void Postfix(HatManager __instance, ref Il2CppReferenceArray<NamePlateData> __result) {
            if(CustomNamePlatesManager.isAdded) return;
            var AllPlates = __result.ToList();
            foreach(var plate in CustomNamePlatesManager.CustomPlates) {
                AllPlates.Add(plate);
                __instance.AllNamePlates.Add(plate);
            }

            __result = AllPlates.ToArray();
            CustomNamePlatesManager.isAdded = true;
        }
    }
}