using System.Collections;
using System.Collections.Generic;
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
    }
}
