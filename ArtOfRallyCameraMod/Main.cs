using System.Reflection;
using HarmonyLib;
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
                Transport = TransportProtocol.WebSocket
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
            Client.OnConnected += (sender, args) => Logger.Log("Connected to server!");
            Client.OnReconnectAttempt += (sender, i) => Logger.Log($"Trying to reconnect {i}x");
            Logger.Log("Trying to connect...");
            await Client.ConnectAsync();
        }
    }
}