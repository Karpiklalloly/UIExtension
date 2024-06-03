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
                    _target.UnregisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
                    _target.RemoveFromClassList("draggable");
                    _lastDroppable?.RemoveFromClassList("droppable--can-drop");
                    _lastDroppable = null;
                }

                _target = value;
                
                _target.RegisterCallback<PointerDownEvent>(DragBegin);
                _target.RegisterCallback<PointerUpEvent>(DragEnd);
                _target.RegisterCallback<PointerMoveEvent>(PointerMove);
                _target.RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
                _target.AddToClassList("draggable");
            }
        }

        public bool Enabled { get; set; } = true;
        
        private static readonly CustomStyleProperty<bool> _draggableEnabledProperty = new("--draggable-enabled");
        private const string _droppableId = "droppable";

        private VisualElement _target;
        private VisualElement _lastDroppable;
        
        private Vector3 _offset;
        private Vector3 _startPosition;
        private bool _isDragging;
        private PickingMode _lastPickingMode;
        
        public static IVisualElementScheduledItem ChangeParent(VisualElement target,
            VisualElement newParent) {
            var position_parent = target.ChangeCoordinatesTo(newParent, Vector2.zero);
            target.RemoveFromHierarchy();
            target.MoveTo(Vector2.zero);
            newParent.Add(target);
            return target.schedule.Execute(() => {
                var newPosition = position_parent - target.ChangeCoordinatesTo(newParent,
                    Vector2.zero);
                target.RemoveFromHierarchy();
                target.MoveTo(newPosition);
                newParent.Add(target);
            });
        }
        
        public void ResetPosition()
        {
            target.MoveTo(Vector2.zero);
        }

        private void OnCustomStyleResolved(CustomStyleResolvedEvent e)
        {
            if (e.customStyle.TryGetValue(_draggableEnabledProperty, out var got)) Enabled = got;
        }

        private void DragBegin(PointerDownEvent e)
        {
            if (!Enabled) return;
            if (e.button != 0) return;
            
            target.AddToClassList("draggable--dragging");

            _lastPickingMode = target.pickingMode;
            target.pickingMode = PickingMode.Ignore;
            _isDragging = true;
            _offset = e.localPosition;
            _startPosition = target.transform.position;
            target.CapturePointer(e.pointerId);
        }

        private void DragEnd(IPointerEvent e)
        {
            if (!_isDragging) return;
            VisualElement droppable;
            bool canDrop = CanDrop(e.position, out droppable);
            if (canDrop)
            {
                droppable.RemoveFromClassList("droppable--can-drop");
            }
            
            target.RemoveFromClassList("draggable--dragging");
            target.RemoveFromClassList("draggable--can-drop");
            
            _lastDroppable?.RemoveFromClassList("droppable--can-drop");
            _lastDroppable = null;
            
            target.ReleasePointer(e.pointerId);
            target.pickingMode = _lastPickingMode;

            _isDragging = false;
            if (canDrop) Drop(droppable);
            else target.MoveTo(_startPosition);
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
            while (element != null && !element.ClassListContains(_droppableId))
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
                target.AddToClassList("draggable--can-drop");
                droppable.AddToClassList("droppable--can-drop");

                if (_lastDroppable != droppable)
                {
                    _lastDroppable?.RemoveFromClassList("droppable--can-drop");
                }

                _lastDroppable = droppable;
            }
            else
            {
                target.RemoveFromClassList("draggable--can-drop");
                _lastDroppable?.RemoveFromClassList("droppable--can-drop");
                _lastDroppable = null;
            }
        }
    }
}
