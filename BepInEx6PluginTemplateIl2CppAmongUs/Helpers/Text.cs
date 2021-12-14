using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BepInEx6PluginTemplateIl2CppAmongUs.Helpers
{
    public class Text
    {
        private static readonly Dictionary<string, Dictionary<string, Dictionary<int, string>>> _text = new();

        public static string GetText(string path, string keyword, int lang = 0)
        {
            if (!_text.ContainsKey(path))
            {
                LoadTextJson(path);
            }


            if (!_text.ContainsKey(path) || !_text[path].ContainsKey(keyword)) return "";
            if (_text[path][keyword].ContainsKey(lang)) return _text[path][keyword][lang];
            if (_text[path][keyword].ContainsKey(0)) return _text[path][keyword][0];
            return "";
        }

        public static void LoadTextJson(string path)
        {
            var fullPath = $"{PluginVersion.Name}.Resources.{path}";
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(fullPath);
            if (stream == null)
            {
                Plugin.Logger.LogWarning($"not found {fullPath}");
                return;
            }

            _text[path] = new Dictionary<string, Dictionary<int, string>>();
            var data = new byte[stream.Length];
            _ = stream.Read(data, 0, (int)stream.Length);
            var strData = Encoding.UTF8.GetString(data);

            var jsonData = JObject.Parse(strData);
            var children = jsonData.ChildrenTokens;

            for (var i = 0; i < jsonData.Count; i++)
            {
                var token = children[i].TryCast<JProperty>();
                if (token == null) continue;

                var group = token.Name;
                _text[path][group] = new Dictionary<int, string>();

                if (token.HasValues)
                {
                    var languages = token.Value.TryCast<JObject>();
                    var langChildren = languages.ChildrenTokens;

                    for (var j = 0; j < languages.Count; j++)
                    {
                        var langToken = langChildren[j].TryCast<JProperty>();
                        if (langToken == null) continue;

                        var langKey = langToken.Name;
                        var text = langToken.Value.ToString();

                        if (text != null && text.Length > 0)
                        {
                            _text[path][group][int.Parse(langKey)] = text;
                        }
                    }
                }
            }

            Plugin.Logger.LogInfo($"{fullPath} loaded");
        }

        public static void LoadAllTextJson()
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var name in assembly.GetManifestResourceNames())
            {
                if (name.EndsWith("text.json"))
                {
                    var paths = name.Split(".");
                    if (paths.Length < 2) continue;
                    LoadTextJson($"{paths[^2]}.{paths[^1]}");
                }
            }
        }
    }
}
