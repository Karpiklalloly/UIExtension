using System;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class Grid : BetterVisualElement
    {
        public override VisualElement contentContainer => _root?.Q("Grid");

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
        private int _countPerLine = 1;
        
        private VisualElement _root;
        
        protected override void InitContentContainer()
        {
            base.InitContentContainer();
            _root = LoadHelper.Grid.CloneTree();
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
        }

        private void ApplyFlex(VisualElement element)
        {
            element.style.flexBasis = new StyleLength(new Length(100f / _countPerLine, LengthUnit.Percent));
        }

        private void ApplyPadding(VisualElement element)
        {
            element.style.paddingTop = _padding / 2;
            element.style.paddingRight = _padding / 2;
            element.style.paddingBottom = _padding / 2;
            element.style.paddingLeft = _padding / 2;
        }

        private void ApplyMargin(VisualElement element)
        {
            element.style.marginTop = _padding / 2;
            element.style.marginRight = _padding / 2;
            element.style.marginBottom = _padding / 2;
            element.style.marginLeft = _padding / 2;
        }
    }
}
