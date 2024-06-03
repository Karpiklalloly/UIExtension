using UnityEngine;

namespace Karpik.UIExtension
{
    public static class PlaceHolder
    {
        public static Texture Texture => Resources.Load<Texture>("Images/Placeholder");
        public static Texture Icon => Texture;
    }
}
