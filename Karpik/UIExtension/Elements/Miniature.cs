using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class Miniature : ExtendedVisualElement
    {
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
    }
}
