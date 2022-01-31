using HarmonyLib;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ArtOfRallyChampionshipMod.Patches.ReplayManager
{
    [HarmonyPatch(typeof(global::ReplayManager), nameof(global::ReplayManager.NotifyEventEnded))]
    [HarmonyPatch(typeof(global::ReplayManager), nameof(global::ReplayManager.ResetReplay))]
    [HarmonyPatch(typeof(global::ReplayManager), nameof(global::ReplayManager.StartGhostPlayback))]
    [HarmonyPatch(typeof(global::ReplayManager), nameof(global::ReplayManager.StopPlayback))]
    public class CurrentReplayChange
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(ReplayInstance ___currentReplay)
        {
        }
    }
}