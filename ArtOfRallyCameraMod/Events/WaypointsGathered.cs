
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace ArtOfRallyChampionshipMod.Events
{
    [HarmonyPatch(typeof(global::OutOfBoundsManager), nameof(global::OutOfBoundsManager.Start))]
    public class Start
    {
        public static void Postfix(List<Vector3> ___WaypointList)
        {
            var waypoints = ___WaypointList.ConvertAll(it => new float[] { it.x, it.y, it.z });

            Main.Client.EmitAsync("waypointsGathered", waypoints);
        }
    }
}