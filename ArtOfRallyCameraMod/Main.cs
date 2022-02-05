using System.Reflection;
using ArtOfRallyChampionshipMod.Patches.ReplayManager;
using ArtOfRallyChampionshipMod.Protocol;
using HarmonyLib;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using SocketIOClient.Transport;
using UnityEngine;
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
            var labelRect = new Rect(70, 40, 200, 200);
            modEntry.OnFixedGUI = entry =>
            {
                if (MultiplayerConnectionManager.CurrentCar != null)
                {
                    var pos = MultiplayerConnectionManager.CurrentCar.Value.position;
                    GUI.Label(labelRect, $"{pos.x} {pos.y} {pos.z}");
                }
                else
                {
                    GUI.Label(labelRect, "No data received");
                }
            };
            
            Connect();
            
            return true;
        }

        private static async void Connect()
        {
            Client.JsonSerializer = new NewtonsoftJsonSerializer();
            Client.OnError += (sender, error) => Logger.Error(error);
            Client.OnConnected += (sender, args) => Logger.Log("Connected to server!");
            Client.OnDisconnected += (sender, s) => Logger.Warning("Got disconnected");
            Client.OnReconnectAttempt += (sender, i) => Logger.Log($"Trying to reconnect {i}x");
            
            Client.On("replayReceived", response =>
            {
                var data = response.GetValue<MultiplayerCar>();
                MultiplayerConnectionManager.LastCar = MultiplayerConnectionManager.CurrentCar;
                MultiplayerConnectionManager.CurrentCar = data.ToNative();
            });
            await Client.ConnectAsync();
        }
    }
}