using System;
using System.Threading.Tasks;
using ArtOfRallyChampionshipMod.Protocol;
using HarmonyLib;
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
    
    [HarmonyPatch(typeof(CarDynamics), "FixedUpdate")]
    public class FixedUpdate
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(CarDynamics __instance, Drivetrain ___drivetrain, Rigidbody ___body)
        {
            if (LiveDataManager.Task is { IsCompleted: false }) return;

            var stageSceneManager = GameEntryPoint.EventManager;
            if (!(stageSceneManager is global::StageSceneManager)) return;

            try
            {
                LiveDataManager.Task = Main.Client.EmitAsync("stageUpdate", new StageUpdateData
                {
                    time = stageSceneManager.stageTimerManager.GetStageTimeMS(),
                    carData = CarData.FromCarController(__instance, ___drivetrain, ___body)
                });
            }
            catch (Exception e)
            {
                Main.Logger.Critical(e.Message);
            }
        }
    }
}