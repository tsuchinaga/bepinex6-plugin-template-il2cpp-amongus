using HarmonyLib;

namespace BepInEx6PluginTemplateIl2CppAmongUs.Patches
{
    public class ShowModStampPatch
    {
        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Awake))]
        [HarmonyPostfix]
        public static void PostfixAwake()
        {
            ModManager.Instance.ShowModStamp();
        }
    }
}
