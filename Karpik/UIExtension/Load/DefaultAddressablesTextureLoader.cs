using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Karpik.UIExtension.Load
{
    public class DefaultAddressablesTextureLoader : ITextureLoader
    {
        public TextureInfo Load(string key)
        {
            TextureInfo info = null;
            
            try
            {
                var handle = Addressables.LoadAssetAsync<Texture>(key);
                handle.WaitForCompletion();
                if (handle.IsDone && handle.Result != null) 
                {
                    info = new TextureInfo(handle.Result, key);
                    handle.Release();
                }
                else
                {
                    info = new ResourcesLoader().Load(key);
                }
            }
            catch (InvalidKeyException)
            {
                info = new ResourcesLoader().Load(key);
            }

            return info;
        }
    }
}