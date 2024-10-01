using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public class BetterVisualElement : VisualElement, IDisposable
    {
        public event Action<VisualElement> ChildAdded;
        public event Action<VisualElement> ChildRemoved;
        public event Action<VisualElement> AddedTo;
        public event Action<VisualElement, BetterVisualElement> RemovedFrom;

        public bool EnableContextMenu
        {
            get => _contextMenuManipulator.Enabled;
            set => _contextMenuManipulator.Enabled = value;
        }
        public int ZIndex
        {
            get => _zIndex;
            set
            {
                _zIndex = value;
                parent?.Sort(SortCondition);
            }
        }

        private ContextMenuManipulator _contextMenuManipulator = new();
        private int _zIndex = 0;
        private HashSet<IManipulator> _manipulators = new();
        
        public BetterVisualElement()
        {
            InitContentContainer();
            AddManipulator(_contextMenuManipulator);
            RegisterCallback<DetachFromPanelEvent>(x => Dispose());
        }

        public new void Add(VisualElement element)
        {
            VisualElement localParent;
            
            if (contentContainer == this)
            {
                base.Add(element);
                localParent = this;
            }
            else
            {
                contentContainer.AddChild(element);
                localParent = contentContainer;
            }

            if (localParent == this)
            {
                Sort();
            }
            OnChildAdded(element);
            ChildAdded?.Invoke(element);
            
            if (element is BetterVisualElement better)
            {
                better.OnAddTo();
                better.AddedTo?.Invoke(element.parent);
            }
        }

        public new void Remove(VisualElement element)
        {
            if (contentContainer == this)
            {
                base.Remove(element);
            }
            else
            {
                contentContainer.RemoveChild(element);
            }
            
            OnChildRemoved(element);
            ChildRemoved?.Invoke(element);
            if (element is BetterVisualElement better)
            {
                better.OnRemoveFrom();
                better.RemovedFrom?.Invoke(element, this);
            }
        }

        public new void RemoveAt(int index)
        {
            var element = ElementAt(index);
            if (contentContainer == this)
            {
                base.RemoveAt(index);
            }
            else
            {
                contentContainer.RemoveAt(index);
            }
            
            OnChildRemoved(element);
            ChildRemoved?.Invoke(element);
            if (element is BetterVisualElement better)
            {
                better.OnRemoveFrom();
                better.RemovedFrom?.Invoke(element, this);
            }
        }

        public void AddManipulator(IManipulator manipulator)
        {
            manipulator.target = this;
            _manipulators.Add(manipulator);
        }

        public void RemoveManipulator(IManipulator manipulator)
        {
            _manipulators.Remove(manipulator);
            manipulator.target = null;
        }

        public T GetManipulator<T>() where T : IManipulator
        {
            return (T)GetManipulator(typeof(T));
        }

        public IManipulator GetManipulator(Type manipulatorType)
        {
            return _manipulators.First(x => x.GetType() == manipulatorType);
        }
        
        public void AddContextMenu(string path, Action<ContextMenuManipulatorEvent> action, Func<bool> enable = null)
        {
            _contextMenuManipulator.Add(path, action, enable);
        }

        public void Dispose()
        {
            _contextMenuManipulator.Enabled = false;
            OnDispose();
        }
        
        protected virtual void InitContentContainer()
        {
            
        }

        protected virtual void OnAddTo()
        {
            
        }
        
        protected virtual void OnChildAdded(VisualElement element)
        {
            
        }

        protected virtual void OnRemoveFrom()
        {
            
        }

        protected virtual void OnChildRemoved(VisualElement element)
        {
            
        }

        protected virtual void OnDispose()
        {
            foreach (var manipulator in _manipulators)
            {
                manipulator.target = null;
            }
            _manipulators.Clear();
        }

        private void Sort()
        {
            base.Sort(SortCondition);
        }

        private int SortCondition(VisualElement e1, VisualElement e2)
        {
            var z1 = 0;
            var z2 = 0;
            if (e1 is BetterVisualElement better) z1 = better.ZIndex;
            if (e2 is BetterVisualElement better2) z2 = better2.ZIndex;

            if (z1 > z2) return 1;
            if (z1 < z2) return -1;

            return 0;
        }
    }
}
