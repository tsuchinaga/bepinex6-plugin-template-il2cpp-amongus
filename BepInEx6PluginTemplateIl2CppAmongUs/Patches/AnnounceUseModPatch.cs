using BepInEx6PluginTemplateIl2CppAmongUs.Helpers;
using HarmonyLib;

namespace BepInEx6PluginTemplateIl2CppAmongUs.Patches
{
    public class AnnounceUseModPatch
    {
        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoSpawnPlayer))]
        [HarmonyPostfix]
        public static void PostfixCoSpawnPlayer(PlayerPhysics __instance)
        {
            if (__instance.myPlayer.PlayerId != PlayerControl.LocalPlayer.PlayerId) return;

            switch (AmongUsClient.Instance.GameMode)
            {
                case GameModes.LocalGame when Plugin.EnableLocalGame.Value:
                case GameModes.OnlineGame when Plugin.EnableOnlineGame.Value:
                    __instance.myPlayer.RpcSendChat($"{Text.GetText("announce_text.json", "useModAnnounce", (int)SaveManager.LastLanguage)}: {PluginVersion.Name}");
                    break;
            }
        }
    }
}
