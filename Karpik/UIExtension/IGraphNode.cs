using Karpik.UIExtension.Load;
using UnityEngine;

namespace Karpik.UIExtension
{
    public interface IGraphNode : IPositionNotify
    {
        public string Id { get; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public TextureInfo Texture { get; set; }
    }
}