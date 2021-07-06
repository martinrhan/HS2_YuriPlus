using BepInEx;
using BepInEx.Configuration;
using BepInEx.Harmony;
using BepInEx.Logging;
using HarmonyLib;
using HS2;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace HS2_YuriPlus {
    [BepInPlugin("HS2_YuriPlus", "Yuri Plus Plugin", "0.8")]
    class Main : BaseUnityPlugin {
        static internal Main Instance;
        void Start() {
            GameObject MainGameObject = new GameObject("HS2_YuriPlus");
            MainGameObject.AddComponent<Monobehaviour_HS2_YuriPlus>();
            Instance = this;
            DontDestroyOnLoad(MainGameObject);
            HarmonyLib.Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "HS2_YuriPlus");
            Logger.LogMessage("HS2_YuriPlus Pathching Complete");
            InitConfig();

        }

        void InitConfig() {
            ConfigDef_NoDick = new ConfigDefinition("General", "No Dick");
            this.Config.Bind<bool>(ConfigDef_NoDick, true,
                new ConfigDescription("When enabled, all H motions with dick will be removed from the list."));
            ConfigDef_NoMotionLimit = new ConfigDefinition("General", "No Motion Limit");
            this.Config.Bind<bool>(ConfigDef_NoMotionLimit, true,
                new ConfigDescription("When enabled, H motions will be loaded into the list without limit (except the no dick setting)."));
        }

        ConfigDefinition ConfigDef_NoDick;
        public bool ConfigValue_NoDick { get { return (bool)(this.Config[this.ConfigDef_NoDick].BoxedValue); } }
        ConfigDefinition ConfigDef_NoMotionLimit;
        public bool ConfigValue_NoMotionLimit { get { return (bool)(this.Config[this.ConfigDef_NoMotionLimit].BoxedValue); } }

        public static BepInEx.Logging.ManualLogSource GetLogger { get { return Instance.Logger; } }
    }

    public class Monobehaviour_HS2_YuriPlus : MonoBehaviour {
        public static List<HScene.AnimationListInfo>[] lstAnimInfo;

        internal static void Set() {
            lstAnimInfo = HSceneManager.HResourceTables.lstAnimInfo;
        }
    }

    internal static class CommonMethods {
        internal static bool CheckHasDick(in HScene.AnimationListInfo lstAnimInfo) {
            string[] splitedStrings = lstAnimInfo.assetpathFemale.Split('/');
            string lastString = splitedStrings[splitedStrings.Length - 1];
            return lastString == "houshi.unity3d" || lastString == "sonyu.unity3d" || lastString == "3p.unity3d";
        }
    }
}
