using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public class GraphNode : Miniature, IGraphNode
    {
        public Vector2 value
        {
            get => Position;
            set => Position = value;
        }

        public Vector2 Center
        {
            get => Position + new Vector2(Size.x / 2, Size.y / 2);
            set => Position = value - new Vector2(Size.x / 2, Size.y / 2);
        }
        
        public string Id { get; private set; }
        
        public Vector2 Position
        {
            get => new(style.left.value.value, style.top.value.value);
            set
            {
                using var e = ChangeEvent<Vector2>.GetPooled(Position, value);
                e.target = this;
                SetValueWithoutNotify(value);
                SendEvent(e);
            }
        }
        
        public new Vector2 Size
        {
            get => new(style.width.value.value, style.height.value.value);
            set
            {
                style.width = value.x;
                style.height = value.y;
            }
        }

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
