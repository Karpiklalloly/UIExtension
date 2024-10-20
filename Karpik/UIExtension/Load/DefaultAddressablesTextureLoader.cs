using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Karpik.UIExtension.Load
{
    public class DefaultAddressablesTextureLoader : ITextureLoader
    {
        public TextureInfo Load(string key)
        {
            var handle = Addressables.LoadAssetAsync<Texture>(key);
            handle.WaitForCompletion();
            TextureInfo info = null;
            if (handle.IsDone)
            {
                info = handle.Result == null
                    ? new ResourcesLoader().Load(key)
                    : new TextureInfo(handle.Result, key);
            }
            else
            {
                Debug.LogError($"Failed to load {key} texture");
            }
            handle.Release();
            return info;
        }
    }
}