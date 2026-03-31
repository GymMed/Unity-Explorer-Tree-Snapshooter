using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using System.Reflection;

namespace UnityExplorerTreeSnapShooter
{
    [BepInDependency(UnityExplorer.ExplorerCore.GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class UnityExplorerTreeSnapShooter : BaseUnityPlugin
    {
        public const string GUID = "gymmed.unity_tree_snap_shooter";
        public const string NAME = "Unity Tree Snap Shooter";
        public const string VERSION = "0.0.1";

        public static readonly string Prefix = "[Unity-Tree-Snap-Shooter]";
        public const string EVENTS_LISTENER_GUID = GUID + "_*";

        internal static ManualLogSource Log;

        internal void Awake()
        {
            Log = this.Logger;
            LogMessage($"Hello world from {NAME} {VERSION}!");

            new Harmony(GUID).PatchAll();
        }

        internal void Update()
        {
        }

        public static void LogMessage(string message)
        {
            Log.LogMessage($"{Prefix} {message}");
        }

        public static string GetProjectLocation()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
