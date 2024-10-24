using System;
using Karpik.UIExtension.Load;
using UnityEngine;
using UnityEngine.UIElements;

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
        }
        
        public void SetValueWithoutNotify(Vector2 newValue)
        {
            style.left = new StyleLength(newValue.x);
            style.top = new StyleLength(newValue.y);
        }
        
        public void SetId(string id)
        {
            Id = id;
        }
    }
}
