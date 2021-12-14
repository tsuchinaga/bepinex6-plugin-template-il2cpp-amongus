using System.Reflection;
using UnityEngine;

namespace BepInEx6PluginTemplateIl2CppAmongUs.Helpers
{
    public class Image
    {
        public static Sprite GetSprite(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream($"{PluginVersion.Name}.Resources.{path}");
            if (stream == null) return null;

            var data = new byte[stream.Length];
            _ = stream.Read(data, 0, (int)stream.Length);

            var t2d = new Texture2D(1, 1);
            return Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
        }
    }
}
