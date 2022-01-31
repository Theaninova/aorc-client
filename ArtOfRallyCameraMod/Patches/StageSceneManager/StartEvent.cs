// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using HarmonyLib;

namespace ArtOfRallyChampionshipMod.Patches.StageSceneManager
{
    [HarmonyPatch(typeof(global::StageSceneManager), nameof(global::StageSceneManager.OnEventOver))]
    public class StartEvent
    {
        public static void Prefix(global::StageSceneManager __instance)
        {
        }
    }
}