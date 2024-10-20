using UnityEngine;

namespace Karpik.UIExtension.Load
{
    public class ResourcesLoader : ITextureLoader
    {
        public TextureInfo Load(string key)
        {
            var texture = Resources.Load<Texture>(key);
            TextureInfo info = null;
            if (texture != null)
            {
                info = new TextureInfo(texture, key);
            }
            else
            {
                Debug.LogError($"Failed to load {key} texture");
            }
            return info;
        }
    }
}