﻿// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using HarmonyLib;

namespace ArtOfRallyChampionshipMod.Patches.StageSceneManager
{
    [HarmonyPatch(typeof(global::StageSceneManager), nameof(global::StageSceneManager.OnEventOver))]
    public class OnEventOver
    {
        public static void Postfix(global::StageSceneManager __instance)
        {
        }
    }
}