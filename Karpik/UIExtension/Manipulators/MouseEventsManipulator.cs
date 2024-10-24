using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public class MouseEventsManipulator : MouseManipulator
    {
        public event EventCallback<MouseDownEvent> MouseDowned;
        public event EventCallback<MouseUpEvent> MouseUpped;
        public event EventCallback<ClickEvent> Clicked;
        public event EventCallback<MouseMoveEvent> MouseMoved;
        public event EventCallback<MouseEnterEvent> MouseEntered;
        public event EventCallback<MouseLeaveEvent> MouseLeaved;

        public MouseEventsManipulator(
            EventCallback<MouseDownEvent> mouseDowned = null,
            EventCallback<MouseUpEvent> mouseUpped = null,
            EventCallback<ClickEvent> clicked = null,
            EventCallback<MouseMoveEvent> mouseMoved = null,
            EventCallback<MouseEnterEvent> mouseEntered = null,
            EventCallback<MouseLeaveEvent> mouseLeaved = null)
        {
            MouseDowned += mouseDowned;
            MouseUpped += mouseUpped;
            Clicked += clicked;
            MouseMoved += mouseMoved;
            MouseEntered += mouseEntered;
            MouseLeaved += mouseLeaved;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDowned);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
            target.RegisterCallback<ClickEvent>(OnClicked);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMoved);
            target.RegisterCallback<MouseEnterEvent>(OnMouseEntered);
            target.RegisterCallback<MouseLeaveEvent>(OnMouseLeaved);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDowned);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
            target.UnregisterCallback<ClickEvent>(OnClicked);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMoved);
            target.UnregisterCallback<MouseEnterEvent>(OnMouseEntered);
            target.UnregisterCallback<MouseLeaveEvent>(OnMouseLeaved);
        }
        
        private void OnMouseDowned(MouseDownEvent evt) => MouseDowned?.Invoke(evt);
        private void OnMouseUp(MouseUpEvent evt) => MouseUpped?.Invoke(evt);
        private void OnClicked(ClickEvent evt) => Clicked?.Invoke(evt);
        private void OnMouseMoved(MouseMoveEvent evt) => MouseMoved?.Invoke(evt);
        private void OnMouseEntered(MouseEnterEvent evt) => MouseEntered?.Invoke(evt);
        private void OnMouseLeaved(MouseLeaveEvent evt) => MouseLeaved?.Invoke(evt);
    }
}