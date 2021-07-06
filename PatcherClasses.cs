using BepInEx.Configuration;
using HarmonyLib;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HS2_YuriPlus {
    [HarmonyPatch]
    static class Patcher_GameCharaFileInfoAssist {
        [HarmonyPatch(typeof(GameLoadCharaFileSystem.GameCharaFileInfoAssist), "CreateCharaFileInfoList")]
        [HarmonyPrefix]
        static bool Prefix_CreateCharaFileInfoList(ref bool useFemaleFutanariOnly) {
            ////Main.GetLogger.LogDebug("Prefix_CreateCharaFileInfoList Called");
            useFemaleFutanariOnly = false;
            return true;
        }

    }


    [HarmonyPatch]
    static class Patcher_HSceneSprite {
        [HarmonyPatch(typeof(HSceneSprite), "CheckMotionLimit")]
        [HarmonyPrefix]
        static bool Prefix_CheckMotionLimit(ref bool __result, in HScene.AnimationListInfo lstAnimInfo) {
            //Main.GetLogger.LogDebug("111");
            if (Main.Instance.ConfigValue_NoDick && CommonMethods.CheckHasDick(lstAnimInfo)) {
                //Main.GetLogger.LogDebug("222");
                __result = false;
                return false;
            }
            if (Main.Instance.ConfigValue_NoMotionLimit) {
                __result = true;
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(HSceneSprite), "CheckMotionLimitAppend3P")]
        [HarmonyPrefix]
        static bool Prefix_CheckMotionLimitAppend3P(ref bool __result, in HScene.AnimationListInfo lstAnimInfo) {
            //Main.GetLogger.LogDebug("a");
            if (Main.Instance.ConfigValue_NoDick && CommonMethods.CheckHasDick(lstAnimInfo)) {
                //Main.GetLogger.LogDebug("b");
                __result = false;
                return false;
            }
            if (Main.Instance.ConfigValue_NoMotionLimit) {
                __result = true;
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(HSceneSprite), "CheckPlace", new Type[] { typeof(HScene.AnimationListInfo) })]
        [HarmonyPrefix]
        static bool Prefix_CheckPlace(ref bool __result, in HScene.AnimationListInfo info) {
            //Main.GetLogger.LogDebug("aa");
            if (Main.Instance.ConfigValue_NoDick && CommonMethods.CheckHasDick(info)) {
                //Main.GetLogger.LogDebug("bb");
                __result = false;
                return false;
            }
            return true;
        }

    }

    [HarmonyPatch]
    static class Pather_HSceneManager_HSceneTables {
        [HarmonyPatch(typeof(HSceneManager.HSceneTables), "LoadAnimationFileName")]
        [HarmonyPostfix]
        static void Postfix_LoadAnimationFileName() {
            Monobehaviour_HS2_YuriPlus.Set();
        }

    }

    [HarmonyPatch]
    static class Patcher_HScene {
        private static Manager.HSceneManager hSceneManager;
        [HarmonyPatch(typeof(HScene),"Start")]
        [HarmonyPrefix]
        static bool Prefix_Start() {
            hSceneManager = Singleton<HSceneManager>.Instance;
            Main.GetLogger.LogDebug(hSceneManager.player);
            if (!hSceneManager.bFutanari) {
                hSceneManager.bFutanari = true;
                //IsModified = true;
            }
            return true;
        }
        /*
        private static bool IsModified = false;

        [HarmonyPatch(typeof(HScene),"Start")]
        [HarmonyPostfix]
        static void Postfix_Start() {
            if (IsModified) {
                hSceneManager.bFutanari = true;
                IsModified = false;
            }
        }*/
    }

}
