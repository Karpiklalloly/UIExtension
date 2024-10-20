using System;
using Karpik.UIExtension.Load;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class Grid : ExtendedVisualElement
    {
        public override VisualElement contentContainer => _root.Q("Grid");

        [UxmlAttribute]
        public float Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                foreach (var child in Children())
                {
                    ApplyPadding(child);
                }
            }
        }

        [UxmlAttribute]
        public float Margin
        {
            get => _margin;
            set
            {
                _margin = value;
                foreach (var child in Children())
                {
                    ApplyMargin(child);
                }
            }
        }

        public float LineHeight
        {
            get => _lineHeight;
            set
            {
                _lineHeight = value;
                foreach (var child in Children())
                {
                    ApplyHeight(child);
                }
            }
        }

        [UxmlAttribute("CountPerLine")]
        public int CountPerLine
        {
            get => _countPerLine;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException();
                
                _countPerLine = value;

                foreach (var child in Children())
                {
                    ApplyFlex(child);
                }
            }
        }
        
        private float _margin = 0;
        private float _padding = 0;
        private float _lineHeight = 128;
        private int _countPerLine = 1;
        
        private VisualElement _root = new VisualElement();

        public Grid()
        {
            _root.ToGrid();
            hierarchy.Add(_root);
        }

        protected override void OnChildAdded(VisualElement child)
        {
            base.OnChildAdded(child);
            ApplyModification(child);
        }

        private void ApplyModification(VisualElement element)
        {
            ApplyFlex(element);
            ApplyPadding(element);
            ApplyMargin(element);
            ApplyHeight(element);
        }

        private void ApplyFlex(VisualElement element)
        {
            element.style.flexBasis = new StyleLength(new Length(100f / _countPerLine, LengthUnit.Percent));
        }

        private void ApplyPadding(VisualElement element)
        {
            element.style.Padding(_padding / 2);
        }

        private void ApplyMargin(VisualElement element)
        {
            element.style.Margin(_margin / 2);
        }

        private void ApplyHeight(VisualElement element)
        {
            element.style.height = _lineHeight;
        }
    }
}
