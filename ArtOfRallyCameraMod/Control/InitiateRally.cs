using System.IO;
using System.Xml.Serialization;
using HarmonyLib;

namespace ArtOfRallyChampionshipMod.Control
{
    public static class InitiateRally
    {
        public const string MultiplayerChangeMap = "multiplayerChangeMap";
        public const string MultiplayerChangeRally = "multiplayerChangeRally";
        public const string MultiplayerLoadMap = "multiplayerLoadMap";
        
        public static void StartRallyRemotely(LoadMapData mapData)
        {
            GameModeManager.RallyManager.LoadNextStage(mapData.IsLoadingFromMainMenu, mapData.IncrementStageIndex);
        }

        public static void ChangeMode(GameModeManager.GAME_MODES mode)
        {
            GameModeManager.SetGameMode(mode);
        }

        public static void ChangeRallyData(string dataRaw)
        {
            var serializer = new XmlSerializer(typeof(RallyData));
            var reader = new StringReader(dataRaw);
            var data = (RallyData)serializer.Deserialize(reader);
            GameModeManager.RallyManager.RallyData = data;
        }
    }

    public struct LoadMapData
    {
        public bool IsLoadingFromMainMenu;
        public bool IncrementStageIndex;
    }

    [HarmonyPatch(typeof(GameModeManager), nameof(GameModeManager.SetGameMode))]
    public class SetGameMode
    {
        public static void Postfix(GameModeManager.GAME_MODES __0)
        {
            if (!Main.Settings.EnableMultiplayer || !Main.Settings.MultiplayerServer) return;
            
            Main.Client.EmitAsync(InitiateRally.MultiplayerChangeMap, __0);
        }
    }

    [HarmonyPatch(typeof(RallyManager), nameof(RallyManager.RallyData), MethodType.Setter)]
    public class SetRallyData
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(RallyManager __instance)
        {
            if (!Main.Settings.EnableMultiplayer || !Main.Settings.MultiplayerServer) return;
            
            var serializer = new XmlSerializer(typeof(RallyData));
            var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, __instance.RallyData);
            Main.Client.EmitAsync(InitiateRally.MultiplayerChangeRally, stringWriter.ToString());
        }
    }
    
    
    [HarmonyPatch(typeof(RallyManager), nameof(RallyManager.LoadNextStage))]
    public class LoadNextStage
    {
        public static void Postfix(bool __0, bool __1)
        {
            if (!Main.Settings.EnableMultiplayer || !Main.Settings.MultiplayerServer) return;

            Main.Client.EmitAsync(InitiateRally.MultiplayerLoadMap, new LoadMapData
            {
                IsLoadingFromMainMenu = __0,
                IncrementStageIndex = __1
            });
        }
    }
}