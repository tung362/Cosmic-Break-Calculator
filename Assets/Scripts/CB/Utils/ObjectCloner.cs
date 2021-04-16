using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace CB.Utils
{
    /// <summary>
    /// Generic cloner for MessagePack types
    /// </summary>
    public static class ObjectCloner<T>
    {
        public static T Clone(T objectToClone)
        {
            T result = default;
            try
            {
                byte[] bytes = MessagePackSerializer.Serialize<T>(objectToClone);
                result = MessagePackSerializer.Deserialize<T>(bytes);
            }
            catch (MessagePackSerializationException)
            {
                Debug.LogError("Error! Failed to clone, type not compatible with MessagePack! | Type: \"" + typeof(T) + "\"");
            }
            return result;
        }
    }
}
