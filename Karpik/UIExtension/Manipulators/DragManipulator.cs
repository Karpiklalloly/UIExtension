using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    //Modified https://gist.github.com/shanecelis/b6fb3fe8ed5356be1a3aeeb9e7d2c145
    public sealed class DragManipulator : IManipulator
    {
        public VisualElement target
        {
            get => _target;
            set
            {
                if (_target == value) return;
                if (_target != null)
                {
                    _target.UnregisterCallback<PointerDownEvent>(DragBegin);
                    _target.UnregisterCallback<PointerUpEvent>(DragEnd);
                    _target.UnregisterCallback<PointerMoveEvent>(PointerMove);
                    _target.RemoveFromClassList(DropItemClass);
                    _lastDroppable?.RemoveFromClassList(DropContainerClass + CanDropClass);
                    _lastDroppable = null;
                }

                _target = value;
                if (_target == null) return;
                _target.RegisterCallback<PointerDownEvent>(DragBegin);
                _target.RegisterCallback<PointerUpEvent>(DragEnd);
                _target.RegisterCallback<PointerMoveEvent>(PointerMove);
                _target.AddToClassList(DropItemClass);
            }
        }

        public bool Enabled { get; set; } = true;
        
        public event Action<VisualElement> DragStarted;
        public event Action<VisualElement> Dragging; 
        public event Action<VisualElement> DragEnded;
        
        public const string DropContainerClass = "droppable";
        public const string DropItemClass = "draggable";
        public const string DraggingClass = "--dragging";
        public const string CanDropClass = "--can-drop";

        private VisualElement _target;
        private VisualElement _lastDroppable;
        
        private Vector3 _offset;
        private Vector3 _startPosition;
        private bool _isDragging;
        private PickingMode _lastPickingMode;
        
        public static IVisualElementScheduledItem ChangeParent(VisualElement target,
            VisualElement newParent) {
            var positionParent = target.ChangeCoordinatesTo(newParent, Vector2.zero);
            target.RemoveFromHierarchy();
            target.MoveTo(Vector2.zero);
            newParent.Add(target);
            return target.schedule.Execute(() => {
                var newPosition = positionParent - target.ChangeCoordinatesTo(newParent,
                    Vector2.zero);
                target.RemoveFromHierarchy();
                target.MoveTo(newPosition);
                newParent.Add(target);
            });
        }
        
        private void DragBegin(PointerDownEvent e)
        {
            if (!Enabled) return;
            if (e.button != 0) return;
            
            target.AddToClassList(DropItemClass + DraggingClass);

            _lastPickingMode = target.pickingMode;
            target.pickingMode = PickingMode.Ignore;
            _isDragging = true;
            _offset = e.localPosition;
            _startPosition = target.transform.position;
            target.CapturePointer(e.pointerId);
            DragStarted?.Invoke(target);
        }

        private void DragEnd(IPointerEvent e)
        {
            if (!_isDragging) return;
            VisualElement droppable;
            bool canDrop = CanDrop(e.position, out droppable);
            if (canDrop)
            {
                droppable.RemoveFromClassList(DropContainerClass + CanDropClass);
            }
            
            target.RemoveFromClassList(DropItemClass + DraggingClass);
            target.RemoveFromClassList(DropItemClass + CanDropClass);
            
            _lastDroppable?.RemoveFromClassList(DropContainerClass + CanDropClass);
            _lastDroppable = null;
            
            target.ReleasePointer(e.pointerId);
            target.pickingMode = _lastPickingMode;

            _isDragging = false;
            if (canDrop) Drop(droppable);
            else target.MoveTo(_startPosition);
            DragEnded?.Invoke(target);
        }

        private void Drop(VisualElement element)
        {
            var e = DropEvent.GetPooled(this, element);
            e.target = target;
            target.schedule.Execute(() => e.target.SendEvent(e));
        }

        private bool CanDrop(Vector3 position, out VisualElement droppable)
        {
            droppable = target.panel.Pick(position);
            var element = droppable;
            while (element != null && !element.ClassListContains(DropContainerClass))
            {
                element = element.parent;
            }

            if (element == null) return false;
            droppable = element;
            return true;
        }

        private void PointerMove(PointerMoveEvent e)
        {
            if (!Enabled) return;
            if (!_isDragging) return;

            Vector3 delta = e.localPosition - _offset;
            target.Move(delta);

            var canDrop = CanDrop(e.position, out var droppable);
            
            if (canDrop)
            {
                target.AddToClassList(DropItemClass + CanDropClass);
                droppable.AddToClassList(DropContainerClass + CanDropClass);

                if (_lastDroppable != droppable)
                {
                    _lastDroppable?.RemoveFromClassList(DropContainerClass + CanDropClass);
                }

                _lastDroppable = droppable;
            }
            else
            {
                target.RemoveFromClassList(DropItemClass + CanDropClass);
                _lastDroppable?.RemoveFromClassList(DropContainerClass + CanDropClass);
                _lastDroppable = null;
            }
            
            Dragging?.Invoke(target);
        }
    }
}
