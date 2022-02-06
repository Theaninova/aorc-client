using HarmonyLib;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ArtOfRallyChampionshipMod.Events
{
    [HarmonyPatch(typeof(SceneLoader), nameof(SceneLoader.LoadLevel))]
    public class LoadLevel
    {
        public static void Prefix(int __0)
        {
            Main.Client.EmitAsync("loadLevel", __0);
        }
    }
}