using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BepInEx6PluginTemplateIl2CppAmongUs.Helpers
{
    public class Image
    {
        private static readonly Dictionary<string, Sprite> _images = new();

        public static Sprite GetSprite(string path)
        {
            if (!_images.ContainsKey(path))
            {
                LoadImage(path);
            }

            if (_images.ContainsKey(path))
            {
                return _images[path];
            }

            return null;
        }

        public static void LoadImage(string path)
        {
            var fullPath = $"{PluginVersion.Name}.Resources.{path}";
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(fullPath);
            if (stream == null)
            {
                Plugin.Logger.LogWarning($"not found {fullPath}");
                return;
            }

            var data = new byte[stream.Length];
            _ = stream.Read(data, 0, (int)stream.Length);

            var t2d = new Texture2D(1, 1);
            _images[path] = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));

            Plugin.Logger.LogInfo($"{fullPath} loaded");
        }

        public static void LoadAllPngImage()
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var name in assembly.GetManifestResourceNames())
            {
                if (name.EndsWith(".png"))
                {
                    var paths = name.Split(".");
                    if (paths.Length < 2) continue;
                    LoadImage($"{paths[^2]}.{paths[^1]}");
                }
            }
        }
    }
}
