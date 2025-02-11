using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public class TooltipManipulator : PointerManipulator
    {
        

        public Mode FollowMode
        {
            get => _followMode;
            set
            {
                _followMode = value;
            }
        }
        
        public Func<Vector2> AdditionalOffset;
        
        private VisualElement _container;
        private Mode _followMode;

        private TooltipElement _tooltip;
        private Func<VisualElement> _getContainer;
        private Func<string> _getTitle;
        private Func<string> _getDescription;

        public TooltipManipulator(
            Func<VisualElement> getContainer,
            Func<string> getTitle,
            Func<string> getDescription,
            Mode mode = Mode.FollowCursor)
        {
            _getContainer = getContainer;
            _getTitle = getTitle;
            _getDescription = getDescription;
            _followMode = mode;
            _tooltip = new TooltipElement();
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
            target.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);
            target.UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
        }

        private void OnPointerMove(PointerMoveEvent e)
        {
            if (_container != null)
            {
                SetPosition(e.position);
            }
        }

        private void OnPointerEnter(PointerEnterEvent e)
        {
            if (_getContainer != null)
            {
                _container = _getContainer?.Invoke();
            }

            if (_container == null)
            {
                _container = target.panel.visualTree.Children().Last();
            }
            
            _tooltip.Title = _getTitle?.Invoke();
            _tooltip.Description = _getDescription?.Invoke();
            _tooltip.Show();
            if (_container.hierarchy.Children().Contains(_tooltip)) return;
            
            _container.hierarchy.Add(_tooltip);
        }

        private void OnPointerLeave(PointerLeaveEvent e)
        {
            _tooltip.Hide();
        }

        private void SetPosition(Vector2 worldPosition)
        {
            Vector2 offset = AdditionalOffset?.Invoke() ?? Vector2.zero;
            
            switch (_followMode)
            {
                case Mode.FollowCursor:
                    _tooltip.transform.position =
                        _container.WorldToLocal(worldPosition) + Vector2.one + new Vector2(10, 10) + offset;
                    ToContainerBounds();
                    break;
                case Mode.Centralized:
                    _tooltip.transform.position = _container.WorldToLocal(target.LocalToWorld(Vector2.zero)) + offset;
                    _tooltip.transform.position += new Vector3(
                        target.style.width.value.value / 2,
                        target.style.height.value.value / 2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ToContainerBounds()
        {
            var container = target.panel.visualTree;
            if (container.FullyContains(_tooltip))
            {
                return;
            }
            _tooltip.ToBounds(container);
        }
        
        public enum Mode
        {
            FollowCursor,
            Centralized
        }
    }
}