using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MessagePack;

namespace CB.Calculator
{
    /// <summary>
    /// Represents a generic library of video urls
    /// </summary>
    [MessagePackObject]
    public class VideoUrlLibrary
    {
        [Key(0)]
        public List<string> Urls = new List<string>();

        public static bool Load(string path, out VideoUrlLibrary result)
        {
            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                try
                {
                    result = MessagePackSerializer.Deserialize<VideoUrlLibrary>(stream);
                }
                catch (MessagePackSerializationException)
                {
                    Debug.Log("Error! Failed to load, file may be corrupted! Path: " + path + " \"Load()\" @VideoUrlLibrary");
                    result = null;
                }
                stream.Close();

                if (result != null) return true;
            }
            result = null;
            return false;
        }

        public static void Save(string path, VideoUrlLibrary entry)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            FileStream stream = new FileStream(path, FileMode.Create);
            MessagePackSerializer.Serialize<VideoUrlLibrary>(stream, entry);
            stream.Close();
        }
    }
}
