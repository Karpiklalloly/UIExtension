using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public class ContextMenuManipulator : PointerManipulator
    {
        private static bool _showed = false;
        
        public IEnumerable<string> Paths => _elements.Keys;
        public bool Enabled { get; set; } = false;
        
        private Action<ContextualMenuPopulateEvent> _menuBuilder;
        private ContextMenuManipulatorEvent _event;
        
        private readonly Dictionary<string, Data> _elements = new();

        public ContextMenuManipulator(Dictionary<string, Action<ContextMenuManipulatorEvent>> elements = null)
        {
            elements ??= new Dictionary<string, Action<ContextMenuManipulatorEvent>>();
            foreach (var pair in elements)
            {
                Add(pair.Key, pair.Value);
            }
            
            activators.Add(new ManipulatorActivationFilter()
            {
                button = MouseButton.RightMouse
            });
        }

        public void Add(string path, Action<ContextMenuManipulatorEvent> onPick, Func<bool> enable = null)
        {
            enable ??= Enable;

            _elements.Add(path, new Data()
            {
                Event = (e) =>
                {
                    onPick(e);
                    _showed = false;
                },
                Enable = enable
            });
        }

        public void Remove(string path)
        {
            _elements.Remove(path);
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        }

        private void OnPointerDown(PointerDownEvent e)
        {
            if (!Enabled) return;
            if (!CanStartManipulation(e)) return;
            if (_showed) return;
            DoDisplayMenu(e);
        }

        private void DoDisplayMenu(IPointerEvent evt)
        {
            var pos = evt.position;
            _event = new ContextMenuManipulatorEvent(pos);
            
            var offset = new Vector3(0, -300, 0);
            pos += offset;
            var menu = new GenericDropdownMenu();
            
            bool has = false;
            foreach (var pair in _elements)
            {
                var enabled = pair.Value.Enable?.Invoke() ?? false;
                if (enabled)
                {
                    has = true;
                    menu.AddItem(pair.Key, false, () => pair.Value.Event?.Invoke(_event));
                }
            }

            if (!has) return;
            menu.DropDown(new Rect(pos, new Vector2(200, 300)), target, true);
            menu.contentContainer.RegisterCallbackOnce<DetachFromPanelEvent>((e) => _showed = false);
            _showed = true;
        }

        private bool Enable() => true;
        
        private struct Data
        {
            public Action<ContextMenuManipulatorEvent> Event;
            public Func<bool> Enable;
        }
    }

    public class ContextMenuManipulatorEvent : EventBase<ContextMenuManipulatorEvent>
    {
        public Vector2 Position { get; }

        public ContextMenuManipulatorEvent() : this(Vector2.zero)
        {
            
        }

        public ContextMenuManipulatorEvent(Vector2 position)
        {
            Position = position;
        }
    }

    
}
