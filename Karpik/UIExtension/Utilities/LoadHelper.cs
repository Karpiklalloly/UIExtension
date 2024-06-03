using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static partial class LoadHelper
    {
        public static Dictionary<string, Texture> Images
        {
            get
            {
                var textures = Resources.LoadAll<Texture>("Images");
                var dict = new Dictionary<string, Texture>();
                foreach (var texture in textures)
                {
                    dict.Add(texture.name, texture);
                }
                return dict;
            }
        }
        
        public static VisualTreeAsset Grid => Resources.Load<VisualTreeAsset>("Elements/Grid");
    }
}
