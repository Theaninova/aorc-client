using System.Linq;
using HarmonyLib;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ArtOfRallyChampionshipMod.Patches.ReplayManager
{
    [HarmonyPatch(typeof(global::ReplayManager), nameof(global::ReplayManager.NotifyEventEnded))]
    public class RecordKeyframe
    {
        public static ReplayKey_Car CurrentCar;
        
        // ReSharper disable once InconsistentNaming
        public static void Postfix(ReplayInstance ___currentRecording)
        {
            CurrentCar = ___currentRecording.data.keys.Last();
        }
    }
}