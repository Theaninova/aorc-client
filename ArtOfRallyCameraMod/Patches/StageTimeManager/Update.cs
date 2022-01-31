// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using HarmonyLib;

namespace ArtOfRallyChampionshipMod.Patches.StageTimeManager
{
    [HarmonyPatch(typeof(StageTimerManager), "Update")]
    public class Update
    {
        public static void Prefix(StageTimerManager __instance)
        {
            Main.Client.EmitAsync("timerUpdate", __instance.GetStageTimeMS());
        }
    }
}