using System;
using Karpik.UIExtension.Load;
using UnityEngine;

namespace Karpik.UIExtension
{
    public class GraphNode : Miniature, IGraphNode
    {
        public string Id { get; private set; }
        
        public Vector2 Position
        {
            get => value;
            set => this.value = value;
        }

        public new TextureInfo Texture
        {
            get => _texture;
            set
            {
                _texture = value;
                base.Texture = value.Texture;
            }
        }

        private TextureInfo _texture;

        public GraphNode()
        {
            Size = new Vector2(100, 100);
            Id = Guid.NewGuid().ToString();
            Texture = PlaceHolder.TextureInfo;
        }
        
        public void SetId(string id)
        {
            Id = id;
        }
    }
}
