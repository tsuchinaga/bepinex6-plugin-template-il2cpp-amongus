using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using BepInEx6PluginTemplateIl2CppAmongUs.Helpers;
using BepInEx6PluginTemplateIl2CppAmongUs.Patches;
using HarmonyLib;

namespace BepInEx6PluginTemplateIl2CppAmongUs
{
    [BepInPlugin(PluginVersion.Guid, PluginVersion.Name, PluginVersion.Version)]
    [BepInProcess("Among Us.exe")]
    public class Plugin : BasePlugin
    {
        public static ManualLogSource Logger;

        public static ConfigEntry<bool> EnableLocalGame;
        public static ConfigEntry<bool> EnableOnlineGame;

        public override void Load()
        {
            Logger = Log;

            Logger.LogInfo($"Plugin {PluginVersion.Guid} {PluginVersion.Version} is loaded!");
            Text.LoadAllTextJson();
            Image.LoadAllPngImage();

            EnableLocalGame = Config.Bind("LocalGame", "Enable", false);
            EnableOnlineGame = Config.Bind("OnlineGame", "Enable", false);
            Harmony.CreateAndPatchAll(typeof(AnnounceUseModPatch));

            Harmony.CreateAndPatchAll(typeof(ShowModStampPatch));
        }
    }
}
