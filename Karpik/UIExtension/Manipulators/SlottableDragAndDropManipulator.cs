using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public class SlottableDragAndDropManipulator : PointerManipulator
    {
        public const string Slot = "slot";
        public const string Item = "item";

        public bool Enabled { get; set; } = true;
        public delegate void DropEvent(VisualElement slot, VisualElement item);
        
        private Vector3 _startPosition;
        private Vector3 _startMousePosition;
        private bool _startedDragging;
        private VisualElement _child = new VisualElement();
        private Vector2 _size = Vector2.zero;
        private VisualElement _startElement;

        private string _name;
        private VisualElement _root;
        private readonly DropEvent _positionOnDrop;
        private readonly Action _onDrag;
        private readonly Action<Vector2> _onMove;
        private readonly Action _onDragEnd;
        private readonly Func<bool> _additionalRequirements;
        private StyleLength _height;
        private StyleLength _width;

        public SlottableDragAndDropManipulator(string name,
            VisualElement root,
            DropEvent positionOnDrop,
            Action onDrag = null,
            Action<Vector2> onMove = null,
            Action onDragEnd = null,
            Func<bool> additionalRequirements = null)
        {
            _name = name;
            _root = root;
            _positionOnDrop = positionOnDrop;
            _onDrag = onDrag;
            _onMove = onMove;
            _onDragEnd = onDragEnd;
            _additionalRequirements = additionalRequirements;
            
            _child.StretchToParentSize();
        }
        
        public static string SlotName(string name) => Slot + "--" + name;
        public static string ItemName(string name) => Item + "--" + name;
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(DragBegin);
            target.RegisterCallback<PointerUpEvent>(DragEnd);
            target.RegisterCallback<PointerMoveEvent>(PointerMove);
            target.AddToClassList(ItemName(_name));
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(DragBegin);
            target.UnregisterCallback<PointerUpEvent>(DragEnd);
            target.UnregisterCallback<PointerMoveEvent>(PointerMove);
            target.RemoveFromClassList(ItemName(_name));
        }

        private void DragBegin(PointerDownEvent e)
        {
            if (!Enabled) return;
            
            _startMousePosition = e.position;
            target.CapturePointer(e.pointerId);
            _size = target.layout.size;
            _startElement = target.parent;
            _child.Add(target);
            target.transform.position = _child.WorldToLocal(target.worldBound.position);
            
            _startPosition = target.transform.position;
            _width = target.style.width;
            _height =  target.style.height;
            target.style.width = _size.x;
            target.style.height = _size.y;
            _root.Add(_child);
            _startedDragging = true;
            _onDrag?.Invoke();
        }

        private void DragEnd(PointerUpEvent e)
        {
            if (!Enabled) return;
            if (!_startedDragging) return;
            
            target.ReleasePointer(e.pointerId);
            _startedDragging = false;
            _root.Remove(_child);
            target.transform.position = Vector3.zero;
            target.style.width = _width;
            target.style.height = _height;
            
            var elements = _root
                .DeepQs<VisualElement>(className: SlotName(_name))
                .Where(OverlapsTarget);
            
            VisualElement closestSlot = FindClosestSlot(elements, e.position);

            if (closestSlot == null || _additionalRequirements?.Invoke() == false)
            {
                _startElement.Add(target);
                return;
            }

            _positionOnDrop?.Invoke(closestSlot, target);
            
            _onDragEnd?.Invoke();
        }

        private void PointerMove(PointerMoveEvent e)
        {
            if (!Enabled) return;
            if (!_startedDragging) return;

            var pointerDelta = e.position - _startMousePosition;
            target.MoveTo(_startPosition + pointerDelta);
            target.style.width = _size.x;
            target.style.height = _size.y;
            _onMove?.Invoke(target.transform.position);
        }
        
        private bool OverlapsTarget(VisualElement slot)
        {
            return target.worldBound.Overlaps(slot.worldBound);
        }
        
        private VisualElement FindClosestSlot(IEnumerable<VisualElement> slots, Vector3 position)
        {
            var bestDistanceSq = float.MaxValue;
            VisualElement closest = null;
            foreach (var slot in slots)
            {
                var displacement = RootSpaceOfSlot(slot) - position;
                var distanceSq = displacement.sqrMagnitude;
                if (distanceSq >= bestDistanceSq) continue;
                
                bestDistanceSq = distanceSq;
                closest = slot;
            }
            return closest;
        }
        
        private Vector3 RootSpaceOfSlot(VisualElement slot)
        {
            return slot.worldBound.center;
        }
    }

    public static class DnDExtensions
    {
        public static void ToSlot(this VisualElement slot, string name)
        {
            slot.AddToClassList(SlottableDragAndDropManipulator.SlotName(name));
        }

        public static void ToSlottableItem(this VisualElement item, string name,
            VisualElement root,
            SlottableDragAndDropManipulator.DropEvent positionOnDrop,
            Action onDrag = null,
            Action<Vector2> onMove = null,
            Action onDragEnd = null,
            Func<bool> additionalRequirements = null)
        {
            item.AddManipulator(new SlottableDragAndDropManipulator(
                name,
                root,
                positionOnDrop,
                onDrag,
                onMove,
                onDragEnd,
                additionalRequirements));
        }
    }
}