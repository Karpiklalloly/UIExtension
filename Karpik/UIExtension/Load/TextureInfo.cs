using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Karpik.UIExtension.Load
{
    [Serializable]
    public class TextureInfo
    {
        [JsonIgnore]
        public Texture Texture;
        public string LoadPath;

        public TextureInfo(Texture texture, string loadPath)
        {
            Texture = texture;
            LoadPath = loadPath;
        }

        [JsonConstructor]
        private TextureInfo(string loadPath)
        {
            LoadPath = loadPath;
            Texture = TextureLoader.Instance[loadPath].Texture;
        }
    }
}