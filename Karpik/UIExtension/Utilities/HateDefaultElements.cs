using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static class HateDefaultElements
    {
        public static void ForceUpdate(this ScrollView scroll)
        {
            scroll.style.height = scroll.resolvedStyle.height;
            scroll.schedule.Execute(() =>
            {
                var fakeOldRect = Rect.zero;
                var fakeNewRect = scroll.layout;

                using var evt = GeometryChangedEvent.GetPooled(fakeOldRect, fakeNewRect);
                evt.target = scroll.contentContainer;
                scroll.contentContainer.SendEvent(evt);
            });
        }
    }
}