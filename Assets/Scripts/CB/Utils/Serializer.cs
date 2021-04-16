using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MessagePack;
using Cysharp.Threading.Tasks;

namespace CB.Utils
{
    /// <summary>
    /// Generic serializer for MessagePack types
    /// </summary>
    public static class Serializer
    {
        public static bool Load<T>(string path, out T result)
        {
            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                try
                {
                    result = MessagePackSerializer.Deserialize<T>(stream);
                }
                catch (MessagePackSerializationException)
                {
                    Debug.LogError("Error! Failed to load, file may be corrupted! | Path: \"" + path + "\" | Type: \"" + typeof(T) + "\"");
                    result = default;
                }
                stream.Close();

                if (result != null) return true;
            }
            result = default;
            return false;
        }

        public static void Save<T>(string path, T entry)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            FileStream stream = new FileStream(path, FileMode.Create);
            try
            {
                MessagePackSerializer.Serialize<T>(stream, entry);
            }
            catch (MessagePackSerializationException)
            {
                Debug.LogError("Error! Failed to save, type not compatible with MessagePack! | Path: \"" + path + "\" | Type: \"" + typeof(T) + "\"");
            }
            stream.Close();
        }

        public static async UniTask<(bool, T)> LoadAsync<T>(string path)
        {
            T result = default;
            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                try
                {
                    result = await MessagePackSerializer.DeserializeAsync<T>(stream);
                }
                catch (MessagePackSerializationException)
                {
                    Debug.LogError("Error! Failed to load, file may be corrupted! | Path: \"" + path + "\" | Type: \"" + typeof(T) + "\"");
                }
                stream.Close();

                if (result != null) return (true, result);
            }
            return (false, result);
        }

        public static async UniTaskVoid SaveAsync<T>(string path, T entry)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            FileStream stream = new FileStream(path, FileMode.Create);
            try
            {
                await MessagePackSerializer.SerializeAsync<T>(stream, entry);
            }
            catch (MessagePackSerializationException)
            {
                Debug.LogError("Error! Failed to save, type not compatible with MessagePack! | Path: \"" + path + "\" | Type: \"" + typeof(T) + "\"");
            }
            stream.Close();
        }
    }
}
