using System;
using System.Linq;
using ArtOfRallyChampionshipMod.Protocol;
using HarmonyLib;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ArtOfRallyChampionshipMod.Patches.ReplayManager
{
    [HarmonyPatch(typeof(StageTimerManager), "Update")]
    public class RecordKeyframe
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(StageTimerManager __instance)
        {
            // if (GameEntryPoint.EventManager.status != EventStatusEnums.EventStatus.UNDERWAY) return;

            try
            {
                var recording = global::ReplayManager.Instance().CurrentReplayDataRecording();
                var dataExists = recording != null && recording.keys.Count != 0;

                Main.Client.EmitAsync("stageUpdate", new StageUpdateData
                {
                    time = __instance.GetStageTimeMS(),
                    carData = dataExists
                        ? CarData.FromReplayKey(recording!.keys[recording.keys.Count - 1])
                        : (CarData?)null,
                });
            }
            catch (Exception e)
            {
                Main.Logger.Critical(e.Message);
            }
        }
    }
}