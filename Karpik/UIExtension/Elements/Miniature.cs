using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class Miniature : ExtendedVisualElement
    {
        public Vector2 value
        {
            get => new(style.left.value.value, style.top.value.value);
            set
            {
                using var e = ChangeEvent<Vector2>.GetPooled(this.value, value);
                e.target = this;
                SetValueWithoutNotify(value);
                SendEvent(e);
            }
        }

        public Vector2 Center
        {
            get => value + new Vector2(Size.x / 2, Size.y / 2);
            set => this.value = value - new Vector2(Size.x / 2, Size.y / 2);
        }
        
        [UxmlAttribute]
        public Texture Texture
        {
            get => _icon.image;
            set => _icon.image = value;
        }
        
        [UxmlAttribute]
        public Vector2 Size
        {
            get => new(style.width.value.value, style.height.value.value);
            set
            {
                style.width = value.x;
                style.height = value.y;
            }
        }
        
        private Image _icon;
        private Vector2 _position;
        
        public Miniature()
        {
            _icon = new Image
            {
                image = PlaceHolder.Icon
            };
            
            _icon.ToCenter();
            hierarchy.Add(_icon);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            Remove(_icon);
            _icon = null;
        }

        public void SetValueWithoutNotify(Vector2 newValue)
        {
            style.left = new StyleLength(newValue.x);
            style.top = new StyleLength(newValue.y);
        }
    }
}
