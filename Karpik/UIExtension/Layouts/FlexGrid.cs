using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class FlexGrid : ExtendedVisualElement
    {
        public override VisualElement contentContainer
        {
            get
            {
                if (_stacks.Count == 0)
                {
                    Stack stack = new();
                    _stacks.Add(stack);
                    _verticalStack.Add(stack);
                }

                var s = _stacks.Last();
                if (s.Children().Count() > _countPerLine)
                {
                    Stack stack = new();
                    _stacks.Add(stack);
                    _verticalStack.Add(stack);
                    return stack;
                }
                return s;
            }
        }

        [UxmlAttribute][Min(1)]
        public float LineHeight
        {
            get => _lineHeight;
            set
            {
                _lineHeight = value;
                foreach (var stack in _stacks)
                {
                    stack.style.maxHeight = _lineHeight;
                    stack.style.minHeight = _lineHeight;
                }
            }
        }

        [UxmlAttribute][Min(1)]
        public int CountPerLine
        {
            get => _countPerLine;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException();
                
                _countPerLine = value;
                Update();
            }
        }
        
        private float _lineHeight = 128;
        private int _countPerLine = 1;
        
        private Stack _verticalStack;
        private List<Stack> _stacks = new();

        public FlexGrid()
        {
            if (!ClassListContains(Selectors.FlexGrid))
            {
                AddToClassList(Selectors.FlexGrid);
            }
            Init();
        }

        private void Init()
        {
            if (_verticalStack == null)
            {
                var children = hierarchy.Children().Where(x => x is Stack);
                if (!children.Any())
                {
                    _verticalStack = new Stack();
                }
                else
                {
                    _verticalStack = children.Cast<Stack>().First();
                }
            }
            hierarchy.Add(_verticalStack);
            _verticalStack.Direction = Selectors.Direction.Vertical;
            _verticalStack.style.height = _lineHeight;
            
            _stacks = _verticalStack.Children().Where(x => x is Stack).Cast<Stack>().ToList();
            if (_stacks.Count == 0)
            {
                AddHorizontalStack();
            }
        }

        protected override void OnChildAdded(VisualElement element)
        {
            base.OnChildAdded(element);
            Update();
        }

        private IEnumerable<VisualElement> Elements()
        {
            return _stacks.SelectMany(stack => stack.Children()).ToList();
        }

        private void Update()
        {
            var elements = Elements();
            foreach (var item in _stacks)
            {
                item.Clear();
            }
            _stacks.Clear();
            _verticalStack.Clear();
            
            int i = 0;
            foreach (var child in elements)
            {
                Debug.Log("for");
                if (i == 0)
                {
                    Debug.Log("Add");
                    AddHorizontalStack();
                }
                _stacks.Last().Add(child);
                i++;
                i %= _countPerLine;
            }
        }

        private Stack AddHorizontalStack()
        {
            Stack stack = new();
            stack.Direction = Selectors.Direction.Horizontal;
            stack.style.minHeight = _lineHeight;
            stack.style.maxHeight = _lineHeight;
            _stacks.Add(stack);
            _verticalStack.Add(stack);
            return stack;
        }
    }
}
