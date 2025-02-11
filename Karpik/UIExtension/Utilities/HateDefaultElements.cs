using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static class HateDefaultElements
    {
        private static MethodInfo _incrementVersion =
            typeof(VisualElement).GetMethod("IncrementVersion", BindingFlags.NonPublic | BindingFlags.Instance);

        private static PropertyInfo _pseudoState =
            typeof(VisualElement).GetProperty("pseudoStates", BindingFlags.NonPublic | BindingFlags.Instance);
        
        public static void ForceUpdate(this VisualElement element)
        {
            element.style.visibility = Visibility.Hidden;
            element.schedule.Execute(() =>
            {
                var fakeOldRect = new Rect(element.layout.position + Vector2.one, element.layout.size + Vector2.one);
                var fakeNewRect = element.layout;
                element.style.visibility = Visibility.Visible;
            
                using var evt = GeometryChangedEvent.GetPooled(fakeOldRect, fakeNewRect);
                evt.target = element.contentContainer;
                element.contentContainer.SendEvent(evt);
            });
        }
    }
}