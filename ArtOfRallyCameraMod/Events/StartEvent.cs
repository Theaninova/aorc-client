// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using HarmonyLib;

namespace ArtOfRallyChampionshipMod.Events
{
    [HarmonyPatch(typeof(global::StageSceneManager), nameof(global::StageSceneManager.StartEvent))]
    public class StartEvent
    {
        public static void Postfix(global::StageSceneManager __instance)
        {
        }
    }
}