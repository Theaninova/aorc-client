// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using HarmonyLib;

namespace ArtOfRallyChampionshipMod.Events
{
    [HarmonyPatch(typeof(StageSceneManager), nameof(StageSceneManager.OnEventOver))]
    public class OnEventOver
    {
        public static void Postfix(StageSceneManager __instance)
        {
        }
    }
}