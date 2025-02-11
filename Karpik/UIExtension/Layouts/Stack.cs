using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class Stack : ExtendedVisualElement
    {
        public override VisualElement contentContainer => this;

        [UxmlAttribute]
        public Selectors.Direction Direction
        {
            get => _direction;
            set
            {
                if (_direction == value) return;

                _direction = value;
                
                
                Update();
            }
        }
        
        [UxmlAttribute]
        public bool IsRevert
        {
            get => _isRevert;
            set
            {
                if (_isRevert == value) return;

                _isRevert = value;
                Update();
            }
        }

        private Selectors.Direction _direction;
        private bool _isRevert;

        public Stack()
        {
            if (!ClassListContains(Selectors.Stack))
            {
                AddToClassList(Selectors.Stack);
            }
            Update();
        }
        
        protected override void OnChildAdded(VisualElement child)
        {
            base.OnChildAdded(child);
            if (!child.ClassListContains(Selectors.StackElement))
            {
                child.AddToClassList(Selectors.StackElement);
            }
        }

        protected override void OnChildRemoved(VisualElement element)
        {
            base.OnChildRemoved(element);
            
            element.RemoveFromClassList(Selectors.StackElement);
        }

        private void Update()
        {
            if (_isRevert)
            {
                style.flexDirection = _direction switch
                {
                    Selectors.Direction.Horizontal => new StyleEnum<FlexDirection>(FlexDirection.RowReverse),
                    Selectors.Direction.Vertical => new StyleEnum<FlexDirection>(FlexDirection.ColumnReverse),
                    _ => style.flexDirection
                };
            }
            else
            {
                style.flexDirection = _direction switch
                {
                    Selectors.Direction.Horizontal => new StyleEnum<FlexDirection>(FlexDirection.Row),
                    Selectors.Direction.Vertical => new StyleEnum<FlexDirection>(FlexDirection.Column),
                    _ => style.flexDirection
                };
            }
        }
    }
}