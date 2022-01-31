using System;
using ArtOfRallyChampionshipMod.Protocol;
using HarmonyLib;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ArtOfRallyChampionshipMod.Patches.StageTimerManager
{
    [HarmonyPatch(typeof(global::StageTimerManager), "Update")]
    public class Update
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(global::StageTimerManager __instance)
        {
            var playerManager = GameEntryPoint.EventManager.playerManager;

            try
            {
                var recording = ReplayManager.Instance().CurrentReplayDataRecording();
                var dataExists = recording != null && recording.keys.Count != 0;

                Main.Client.EmitAsync("stageUpdate", new StageUpdateData
                {
                    time = __instance.GetStageTimeMS(),
                    carData = dataExists
                        ? CarData.FromReplayKey(
                            recording!.keys[recording.keys.Count - 1],
                            playerManager.drivetrain
                        )
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