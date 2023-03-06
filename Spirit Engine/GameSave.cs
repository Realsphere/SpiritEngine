using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Realsphere.Spirit
{
    /// <summary>
    /// A Game Save can store data, write it to a file, and read it from a file.
    /// </summary>
    public class GameSave
    {
        private Dictionary<string, string> data = new Dictionary<string, string>();

        public string GetData(string key) => data[key];
        public void SetData(string key, string data)
        {
            if(this.data.ContainsKey(key))
                this.data[key] = data;
            else
                this.data.Add(key, data);
        }

        public void ClearData()
            => this.data.Clear();

        byte[] getBits()
        {
            JsonObject obj = new();
            foreach (var pair in data)
                obj.Add(pair.Key, pair.Value);
            return Encoding.UTF8.GetBytes(obj.ToJsonString(new()
            {
                WriteIndented = false,
            }));
        }

        static KeyValuePair<string, string>[] GetJsonKeyValuePairs(string jsonString)
        {
            List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>();

            // Parse the JSON document
            JsonDocument jsonDocument = JsonDocument.Parse(jsonString);

            // Traverse the JSON document and extract the key-value pairs
            foreach (JsonProperty jsonProperty in jsonDocument.RootElement.EnumerateObject())
            {
                string key = jsonProperty.Name;
                string value = jsonProperty.Value.ToString();
                keyValues.Add(new KeyValuePair<string, string>(key, value));
            }

            return keyValues.ToArray();
        }

        public static GameSave Load(string path)
        {
            GameSave res = new();
            try
            {
                byte[] decrypted = Encryption.Decrypt(File.ReadAllBytes(path), File.GetLastWriteTime(path).Ticks + "");
                var values = GetJsonKeyValuePairs(Encoding.UTF8.GetString(decrypted));
                foreach (var item in values)
                    res.SetData(item.Key, item.Value);
            }
            catch(Exception)
            {
                Game.Throw(new InvalidDataException("Game Save is corrupted or has been edited."));
            }
            return res;
        }

        public void SaveToFile(string path)
        {
            if (File.Exists(path)) File.Delete(path);
            File.Create(path).Close();
            var writeTime = File.GetLastWriteTime(path);

            File.WriteAllBytes(path, Encryption.Encrypt(getBits(), "" + writeTime.Ticks));

            File.SetLastWriteTime(path, writeTime);
        }
    }
}
