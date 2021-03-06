using System.Reflection;
using ArtOfRallyChampionshipMod.Control;
using ArtOfRallyChampionshipMod.Extraction.Live;
using ArtOfRallyChampionshipMod.Protocol;
using HarmonyLib;
using I2.Loc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using SocketIOClient.Transport;
using UnityModManagerNet;

namespace ArtOfRallyChampionshipMod
{
    public static class Main
    {
        public static Settings.Settings Settings = null!;
        public static UnityModManager.ModEntry.ModLogger Logger = null!;
        public static SocketIO Client = null!;

        // ReSharper disable once UnusedMember.Local
        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            Settings = UnityModManager.ModSettings.Load<Settings.Settings>(modEntry);
            Client = new SocketIO("http://localhost:4593/users", new SocketIOOptions
            {
                Transport = TransportProtocol.WebSocket,
                Reconnection = true
            });

            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            // modEntry.OnUpdate = CameraHandler.OnUpdate;
            // modEntry.OnFixedGUI = EditorGUI.OnFixedGUI;
            modEntry.OnGUI = entry => Settings.Draw(entry);
            modEntry.OnSaveGUI = entry => Settings.Save(entry);

            Connect();

            return true;
        }

        private static async void Connect()
        {
            Client.JsonSerializer = new NewtonsoftJsonSerializer();
            Client.OnError += (sender, error) => Logger.Error(error);
            Client.OnConnected += (sender, args) =>
            {
                Logger.Log("Connected to server!");
                Client.EmitAsync("carsInfo", JsonConvert.SerializeObject(
                    CarManager.AllCarsList, new StringEnumConverter()));
                Client.EmitAsync("stagesInfo", JsonConvert.SerializeObject(
                    AreaManager.AreaDictionary, new StringEnumConverter()));
                Client.EmitAsync("translationsGathered", JsonConvert.SerializeObject(
                    LocalizationManager.Sources, new StringEnumConverter()));
            };
            Client.OnDisconnected += (sender, s) => Logger.Warning("Got disconnected");
            Client.OnReconnectAttempt += (sender, i) => Logger.Log($"Trying to reconnect {i}x");

            Client.On("replayReceived", response =>
            {
                var data = response.GetValue<MultiplayerCar>();
                MultiplayerConnectionManager.LastCar = MultiplayerConnectionManager.CurrentCar;
                MultiplayerConnectionManager.CurrentCar = data.ToNative();
            });

            // Multiplayer
            Client.On(InitiateRally.MultiplayerLoadMap,
                response => InitiateRally.StartRallyRemotely(response.GetValue<LoadMapData>()));
            Client.On(InitiateRally.MultiplayerChangeMap,
                response => InitiateRally.ChangeMode(response.GetValue<GameModeManager.GAME_MODES>()));
            Client.On(InitiateRally.MultiplayerChangeRally,
                response => InitiateRally.ChangeRallyData(response.GetValue<string>()));
            await Client.ConnectAsync();
        }
    }
}