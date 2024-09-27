using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public class TooltipManipulator : PointerManipulator
    {
        private static TooltipElement _tooltip = new();

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
        private Mode _followMode = Mode.FollowCursor;

        private Func<string> _getTitle;
        private Func<string> _getDescription;
        private Func<VisualElement> _tooltipContainer;

        public TooltipManipulator(
            Func<string> getTitle,
            Func<string> getDescription,
            Func<VisualElement> tooltipContainer,
            Mode mode = Mode.FollowCursor)
        {
            _getTitle = getTitle;
            _getDescription = getDescription;
            _tooltipContainer = tooltipContainer;
            _followMode = mode;
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
                SetPosition(e);
            }
        }

        private void OnPointerEnter(PointerEnterEvent e)
        {
            _container = _tooltipContainer?.Invoke();
            _tooltip.Title = _getTitle?.Invoke();
            _tooltip.Description = _getDescription?.Invoke();
            target.CapturePointer(e.pointerId);
            _tooltip.Show();
            if (_container.hierarchy.Children().Contains(_tooltip)) return;
            
            _container.hierarchy.Add(_tooltip);
        }

        private void OnPointerLeave(PointerLeaveEvent e)
        {
            if (target.HasPointerCapture(e.pointerId))
            {
                target.ReleasePointer(e.pointerId);
            }
            
            _tooltip.Hide();
            
            if (!_container.Children().Contains(_tooltip)) return;
            
            _container.hierarchy.Remove(_tooltip);
        }

        private void SetPosition(PointerMoveEvent e)
        {
            Vector2 offset = AdditionalOffset?.Invoke() ?? Vector2.zero;
            
            switch (_followMode)
            {
                case Mode.FollowCursor:
                    _tooltip.transform.position =
                        _container.WorldToLocal(e.position) + Vector2.one + new Vector2(10, 10) + offset;
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
            var container = _tooltipContainer?.Invoke();
            if (container.FullyContains(_tooltip))
            {
                return;
            }
            VisualElementExtensions.ClampChildWithinParent(_tooltip, container);
        }
        
        public enum Mode
        {
            FollowCursor,
            Centralized
        }
    }
}