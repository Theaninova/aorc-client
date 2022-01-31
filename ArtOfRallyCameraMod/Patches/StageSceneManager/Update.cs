// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System;
using ArtOfRallyChampionshipMod.Patches.ReplayManager;
using HarmonyLib;

namespace ArtOfRallyChampionshipMod.Patches.StageSceneManager
{
    [Serializable]
    public class StageUpdateData
    {
        public float time;
        public ReplayKey_Car carData;
    }
    
    [HarmonyPatch(typeof(global::StageSceneManager), nameof(global::StageSceneManager.Update))]
    public class Update
    {
        public static void Prefix(global::StageSceneManager __instance)
        {
            Main.Client.EmitAsync("stageUpdate", new StageUpdateData {
                time = __instance.stageTimerManager.GetStageTimeMS(),
                carData = RecordKeyframe.CurrentCar
            });
        }
    }
}