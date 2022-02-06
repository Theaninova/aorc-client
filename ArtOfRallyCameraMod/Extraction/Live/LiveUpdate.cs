using System;
using System.Threading.Tasks;
using ArtOfRallyChampionshipMod.Protocol;
using HarmonyLib;
using UnityEngine;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace ArtOfRallyChampionshipMod.Extraction.Live
{
    public static class LiveDataManager
    {
        public static Task? Task;
        public static float handbrake = 0f;
        public static CarData? last;
    }

    [HarmonyPatch(typeof(CarController), "FixedUpdate")]
    public class FixedUpdate
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(CarController? __instance, Drivetrain? ___drivetrain, Rigidbody? ___body, CarDynamics? ___cardynamics)
        {
            if (LiveDataManager.Task is { IsCompleted: false }) return;

            var stageSceneManager = GameEntryPoint.EventManager;
            var time = stageSceneManager is StageSceneManager
                ? stageSceneManager.stageTimerManager != null ? stageSceneManager.stageTimerManager.GetStageTimeMS() :
                0.0f
                : 0.0f;

            var data = CarData.FromCarController(__instance, ___drivetrain, ___body, ___cardynamics,
                LiveDataManager.last);
            LiveDataManager.last = data;
            try
            {
                LiveDataManager.Task = Main.Client.EmitAsync("stageUpdate", new StageUpdateData
                {
                    time = time,
                    carData = data
                });
            }
            catch (Exception e)
            {
                Main.Logger.Critical(e.ToString());
            }
        }
    }
}