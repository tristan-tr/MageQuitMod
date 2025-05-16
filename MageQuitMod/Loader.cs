using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MageQuitMod
{
    public class Loader
    {
        static Harmony harmony;

        public static void Init()
        {
            Loader.Load = new GameObject();

            Loader.Load.AddComponent<MainClass>();

            UnityEngine.Object.DontDestroyOnLoad(Loader.Load);

            harmony = new Harmony("trist.magequit.mod");
            harmony.PatchAll();
        }

        public static void Unload()
        {
            harmony.UnpatchAll();

            UnityEngine.Object.Destroy(Load);
        }

        public static GameObject Load;
    }
}
