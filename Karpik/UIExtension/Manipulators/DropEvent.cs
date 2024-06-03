using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    //https://gist.github.com/shanecelis/b6fb3fe8ed5356be1a3aeeb9e7d2c145
    public class DropEvent : EventBase<DropEvent>
    {
        public DragManipulator Dragger { get; private set; }
        public VisualElement Droppable { get; private set; }

        public static DropEvent GetPooled(DragManipulator dragger, VisualElement droppable)
        {
            DropEvent pooled = EventBase<DropEvent>.GetPooled();
            pooled.Dragger = dragger;
            pooled.Droppable = droppable;
            return pooled;
        }

        public DropEvent() => LocalInit();

        protected override void Init()
        {
            base.Init();
            LocalInit();
        }

        private void LocalInit()
        {
            bubbles = true;
            tricklesDown = true;
        }
    }
}
