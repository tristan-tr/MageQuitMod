using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace MageQuitMod.HarmonyPatches
{
    public partial class HarmonyPatches
    {
        [HarmonyPatch(typeof(AvailableElements))]
        [HarmonyPatch(nameof(AvailableElements.GetRandomAvailable))]
        // Allows the user to be able to enable/disable elements by choice instead of having to start with 'ice'
        class BetterElementSelection
        {
            public static bool Prefix(ref Element[] __result, OnlineLobby.Match match = null)
            {
                Debug.LogError("Custom element selection logic started");

                List<Element> elementsAvailable = SRDebugWrapper.ElementsAvailableOptions.Instance.GetElements();
                if (elementsAvailable.Count < 4)
                {
                    Debug.LogError("Less than 4 elements enabled, skipping custom element selection");
                    return true; // Run original
                }

                List<Element> elementsIncluded = SRDebugWrapper.ElementsIncludedOptions.Instance.GetElementsIncluded();


                // Randomly leave 4 elements in list
                while (elementsAvailable.Count > 4)
                {
                    elementsAvailable.RemoveAt(UnityEngine.Random.Range(0, elementsAvailable.Count - 1));
                }



                __result = elementsAvailable.ToArray();

                Debug.LogError("Done!");

                return false; // Skip original
            }
        }
    }
}
