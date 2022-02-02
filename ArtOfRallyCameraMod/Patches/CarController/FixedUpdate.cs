using System;
using System.Threading.Tasks;
using ArtOfRallyChampionshipMod.Protocol;
using HarmonyLib;
using TinyJson;
using UnityEngine;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace ArtOfRallyChampionshipMod.Patches.CarController
{
    public static class LiveDataManager
    {
        public static Task? Task;
    }

    [HarmonyPatch(typeof(global::CarController), "FixedUpdate")]
    public class FixedUpdate
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(global::CarController? __instance, Drivetrain? ___drivetrain, Rigidbody? ___body, CarDynamics? ___cardynamics)
        {
            if (LiveDataManager.Task is { IsCompleted: false }) return;

            var stageSceneManager = GameEntryPoint.EventManager;
            var time = stageSceneManager is global::StageSceneManager
                ? stageSceneManager.stageTimerManager != null ? stageSceneManager.stageTimerManager.GetStageTimeMS() :
                0.0f
                : 0.0f;

            try
            {
                LiveDataManager.Task = Main.Client.EmitAsync("stageUpdate", new StageUpdateData
                {
                    time = time,
                    carData = CarData.FromCarController(__instance, ___drivetrain, ___body, ___cardynamics)
                });
            }
            catch (Exception e)
            {
                Main.Logger.Critical(e.ToString());
            }
        }
    }
}