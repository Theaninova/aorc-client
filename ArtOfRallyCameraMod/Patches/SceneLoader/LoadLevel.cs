using HarmonyLib;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ArtOfRallyChampionshipMod.Patches.SceneLoader
{
    [HarmonyPatch(typeof(global::SceneLoader), nameof(global::SceneLoader.LoadLevel))]
    public class LoadLevel
    {
        public static void Prefix(int __0)
        {
            Main.Client.EmitAsync("load_level", __0);
        }
    }
}