using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public class ChildElementMoverManipulator : PointerManipulator
    {
        private IEnumerable<VisualElement> _childs;
        private bool _dragging = false;
        private Vector3 _offset;
        
        public ChildElementMoverManipulator()
        {
            activators.Add(new ManipulatorActivationFilter()
            {
                button = MouseButton.MiddleMouse
            });
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
            target.RegisterCallback<PointerOutEvent>(OnPointerOut);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            target.UnregisterCallback<PointerOutEvent>(OnPointerOut);
        }

        private void OnPointerDown(PointerDownEvent e)
        {
            if (!CanStartManipulation(e))
                return;
            _childs = target.hierarchy.Children();
            _offset = e.localPosition;
            _dragging = true;
        }

        private void OnPointerMove(PointerMoveEvent e)
        {
            if (!_dragging)
                return;

            foreach (var child in _childs)
            { 
                child.Move(e.deltaPosition);
            }
        }

        private void OnPointerOut(PointerOutEvent e)
        {
            OnPointerUp(e);
        }

        private void OnPointerUp(IPointerEvent e)
        {
            _childs = null;
            _dragging = false;
        }
    }
}
