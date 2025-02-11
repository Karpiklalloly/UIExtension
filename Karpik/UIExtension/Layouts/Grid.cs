using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class Grid : ExtendedVisualElement
    {
        public override VisualElement contentContainer => this;

        [UxmlAttribute]
        public float Margin
        {
            get => _margin;
            set
            {
                _margin = value;
                foreach (var child in Children())
                {
                    ApplyModification(child);
                }
            }
        }

        [UxmlAttribute][Min(1)]
        public float LineHeight
        {
            get => _lineHeight;
            set
            {
                _lineHeight = value;
                foreach (var child in Children())
                {
                    ApplyModification(child);
                }
            }
        }

        /// <summary>
        /// There may be bug with non 2^x count 
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [UxmlAttribute][Min(1)]
        public int CountPerLine
        {
            get => _countPerLine;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException();
                
                _countPerLine = value;

                foreach (var child in Children())
                {
                    ApplyModification(child);
                }
            }
        }
        
        private float _margin = 0;
        private float _lineHeight = 128;
        private int _countPerLine = 1;

        public Grid()
        {
            if (!ClassListContains(Selectors.Grid))
            {
                AddToClassList(Selectors.Grid);
            }
            Init();
        }
        
        private void Init()
        {
            foreach (var child in Children())
            {
                ApplyFlex(child);
            }
        }

        protected override void OnChildAdded(VisualElement child)
        {
            base.OnChildAdded(child);
            ApplyModification(child);
        }

        private void ApplyModification(VisualElement element)
        {
            ApplyFlex(element);
            ApplyMargin(element);
            ApplyHeight(element);
        }

        private void ApplyFlex(VisualElement element)
        {
            var width = resolvedStyle.width;
            var efficientWidth = width - _countPerLine * _margin;
            element.style.flexBasis = new StyleLength(new Length(efficientWidth / _countPerLine, LengthUnit.Pixel));
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
