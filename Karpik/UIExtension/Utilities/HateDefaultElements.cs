using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static class HateDefaultElements
    {
        public static void ForceUpdate(this ScrollView scroll)
        {
            if (scroll.childCount > 0)
            {
                var child = scroll.Children().First();
                child.schedule.Execute(() =>
                {
                    var fakeOldRect = new Rect(child.layout.position + Vector2.one, child.layout.size + Vector2.one);
                    var fakeNewRect = child.layout;

                    using var evt = GeometryChangedEvent.GetPooled(fakeOldRect, fakeNewRect);
                    evt.target = child.contentContainer;
                    child.contentContainer.SendEvent(evt);
                });
            }

            var v = scroll.verticalScroller.value;
            var h = scroll.horizontalScroller.value;
            scroll.schedule.Execute(() =>
            {
                var fakeOldRect = new Rect(scroll.layout.position + Vector2.one, scroll.layout.size + Vector2.one);
                var fakeNewRect = scroll.layout;

                using var evt = GeometryChangedEvent.GetPooled(fakeOldRect, fakeNewRect);
                evt.target = scroll.contentContainer;
                scroll.contentContainer.SendEvent(evt);
            });
            
            scroll.schedule.Execute(() =>
            {
                scroll.verticalScroller.value = v > scroll.verticalScroller.highValue ? scroll.verticalScroller.highValue : v;
                scroll.horizontalScroller.value = h > scroll.horizontalScroller.highValue ? scroll.horizontalScroller.highValue : h;
            }).StartingIn(1);
        }
    }
}