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
        public static void LoadAllPlates() {
            var plateDir = new DirectoryInfo("NamePlates");
            var jsonFiles = plateDir.GetFiles("*.json");
            foreach(var file in jsonFiles) {
            }
        }
    }
    [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetUnlockedNamePlates))]
    class GetUnlockedNamePlatesPatch {
        static bool isAdded = false;
        public static void Postfix(HatManager __instance, ref Il2CppReferenceArray<NamePlateData> __result) {
            if(isAdded) return;
            var AllPlates = __result.ToList();
            var plate = ScriptableObject.CreateInstance<NamePlateData>();
            plate.name = "CustomPlate";
            plate.Order = 99;
            plate.ChipOffset = new Vector2(0f, 0.2f);
            plate.Free = true;
            plate.Image = TextureManager.loadSprite("Resources\\screenshot.png");
            AllPlates.Add(plate);
            __result = AllPlates.ToArray();
            __instance.AllNamePlates.Add(plate);
            isAdded = true;
        }
    }
}