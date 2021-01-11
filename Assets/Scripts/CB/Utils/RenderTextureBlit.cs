using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.Utils
{
    /// <summary>
    /// Copies render texture and applies post processing material to another render texture
    /// </summary>
    public class UIRenderTextureMaterial : MonoBehaviour
    {
        public RenderTexture Source;
        public RenderTexture Destination;
        public Material Post;

        void Start()
        {
            Graphics.Blit(Source, Destination, Post);
        }
    }
}