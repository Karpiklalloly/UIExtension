using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public class TwoElementsChooserManipulator : PointerManipulator
    {
        public bool Enabled { get; set; }

        private Type _secondElementType;
        private bool _dragging;
        
        private Action<TwoElementsChooserManipulatorEvent> _onSuccess;
        private Action<TwoElementsChooserManipulatorEvent> _onMove;
        private VisualElement _selectedElement;
        private Vector2 _startPosition;

        public TwoElementsChooserManipulator(
            Type secondElementType,
            Action<TwoElementsChooserManipulatorEvent> onSuccess,
            Action<TwoElementsChooserManipulatorEvent> onMove)
        {
            _secondElementType = secondElementType;
            _onSuccess = onSuccess;
            _onMove = onMove;

            activators.Add(new ManipulatorActivationFilter()
            {
                button = MouseButton.LeftMouse,
                clickCount = 2
            });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<ClickEvent>(OnClick);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<ClickEvent>(OnClick);
        }

        private void OnClickDown(IPointerEvent e)
        {
            if (!Enabled) return;
            if (!CanStartManipulation(e)) return;

            _startPosition = e.position;
            Debug.Log("Set pos");
        }
        
        private void OnClick(IPointerEvent e)
        {
            if (!Enabled) return;
            if (!CanStartManipulation(e)) return;
            var tar = target.Children().FirstOrDefault(x => x.HasWorld(e.position));

            if (tar == null)
            {
                _dragging = false;
                return;
            }
            
            if (_selectedElement == tar)
            {
                _selectedElement = null;
                _dragging = false;
                return;
            }

            if (_selectedElement == null)
            {
                _selectedElement = tar;
                _dragging = true;
                return;
            }

            if (_dragging && !_secondElementType.IsAssignableFrom(tar.GetType()))
            {
                _selectedElement = null;
                _dragging = false;
                return;
            }
            
            _onSuccess?.Invoke(new TwoElementsChooserManipulatorEvent(e.position)
            {
                FirstElement = _selectedElement,
                SecondElement = tar,
            });
            _dragging = false;
            
            _selectedElement = null;
        }
        
        private void OnPointerMove(PointerMoveEvent e)
        {
            if (!_dragging)
                return;
            
            var pos = target.WorldToLocal(e.position);
            _onMove?.Invoke(new TwoElementsChooserManipulatorEvent(pos)
            {
                FirstElement = _selectedElement,
            });
        }
    }
    
    public class TwoElementsChooserManipulatorEvent : EventBase<ContextMenuManipulatorEvent>
    {
        public Vector2 Position { get; }
        
        public VisualElement FirstElement { get; set; }
        
        public VisualElement SecondElement { get; set; }

        public TwoElementsChooserManipulatorEvent() : this(Vector2.zero)
        {
            
        }

        public TwoElementsChooserManipulatorEvent(Vector2 position)
        {
            Position = position;
        }
    }
}