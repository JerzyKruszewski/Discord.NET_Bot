using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace DiscordBOT.Storage
{
    public class JsonStorage
    {
        public static T RestoreObject<T>(string filepath)
        {
            var json = File.ReadAllText($"{filepath}.json");
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static void StoreObject(object obj, string filepath)
        {
            var file = $"{filepath}.json";

            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);

            File.WriteAllText(file, json);
        }

        public static bool FileExist(string filepath)
        {
            return File.Exists($"{filepath}.json");
        }
    }
}
