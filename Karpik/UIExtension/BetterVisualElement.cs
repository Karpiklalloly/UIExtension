using System;
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
        private EnvironmentType _environmentType;
        private int _zIndex = 0;
        
        public BetterVisualElement()
        {
            InitContentContainer();
            this.AddManipulator(_contextMenuManipulator);
            RegisterCallback<DetachFromPanelEvent>(x => Dispose());
        }

        public new virtual void Add(VisualElement element)
        {
            VisualElement parent;
            
            if (contentContainer == this)
            {
                base.Add(element);
                parent = this;
            }
            else
            {
                contentContainer.Add(element);
                parent = contentContainer;
            }

            if (parent == this)
            {
                Sort();
            }
            OnChildAdded(element);
            ChildAdded?.Invoke(element);

            
            if (contentContainer is BetterVisualElement betterParent)
            {
                if (contentContainer == parent)
                {
                    betterParent.Sort();
                }
                
                betterParent.OnChildAdded(element);
                betterParent.ChildAdded?.Invoke(element);
            }
            

            if (element is BetterVisualElement better)
            {
                better.OnAddTo();
                better.AddedTo?.Invoke(parent);
            }
        }

        public new virtual void Remove(VisualElement element)
        {
            if (contentContainer == this)
            {
                base.Remove(element);
            }
            else
            {
                contentContainer.Remove(element);
            }
            
            ChildRemoved?.Invoke(element);
            if (element is not BetterVisualElement better) return;
            better.OnRemoveFrom(this);
            better.RemovedFrom?.Invoke(element, this);
        }

        public new virtual void RemoveAt(int index)
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
            
            ChildRemoved?.Invoke(element);
            if (element is not BetterVisualElement better) return;
            better.OnRemoveFrom(this);
            better.RemovedFrom?.Invoke(element, this);

        }
        
        public void AddContextMenu(string path, Action<ContextMenuManipulatorEvent> action)
        {
            _contextMenuManipulator.Add(path, action);
        }

        public void Dispose()
        {
            _contextMenuManipulator.Enabled = false;
            OnDispose();
        }

        protected virtual void OnAddTo()
        {
            
        }

        protected virtual void InitContentContainer()
        {
            
        }

        protected virtual void OnRemoveFrom(BetterVisualElement parent)
        {
            
        }

        protected virtual void OnDispose()
        {
            
        }

        protected virtual void OnChildAdded(VisualElement element)
        {
            
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
