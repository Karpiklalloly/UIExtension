using Karpik.UIExtension.Load;
using UnityEngine;

namespace Karpik.UIExtension
{
    public static class PlaceHolder
    {
        public const string PlaceHolderSprite = "Placeholder";
        private static TextureInfo _placeHolder;

        public static TextureInfo TextureInfo => _placeHolder ??= TextureLoader.Instance[PlaceHolderSprite];
        
        public static Texture Texture => TextureInfo.Texture;
        
        public static Texture Icon => Texture;
    }
}
