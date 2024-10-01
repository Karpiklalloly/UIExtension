using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static class VisualElementExtensions
    {
        public static T DeepQ<T>(this VisualElement element, string name = null, string className = null) where T : VisualElement
        {
            var elements = new Queue<VisualElement>();
            elements.Enqueue(element);

            while (elements.Count > 0)
            {
                var element2 = elements.Dequeue();
                var p = element2.Q<T>(name, className);
                if (p != null)
                {
                    return p;
                }

                foreach (var child in element2.Children())
                {
                    elements.Enqueue(child);
                }
            }
            return null;
        }

        public static IEnumerable<T> DeepQs<T>(this VisualElement element, string name = null, string className = null)
            where T : VisualElement
        {
            List<T> t = new();
            var elements = new Queue<VisualElement>();
            elements.Enqueue(element);

            while (elements.Count > 0)
            {
                var element2 = elements.Dequeue();
                var p = element2.Q<T>(name, className);
                if (p != null)
                {
                    t.Add(p);
                }

                foreach (var child in element2.Children())
                {
                    elements.Enqueue(child);
                }
            }
            return t;
        }
        
        public static bool FullyContains(this VisualElement container, VisualElement element)
        {
            return container.worldBound.FullyContains(element.worldBound);
        }
        
        public static bool FullyContains(this Rect container, Rect element)
        {
            return container.xMin <= element.xMin && container.yMin <= element.yMin && container.xMax >= element.xMax && container.yMax >= element.yMax;
        }
        
        public static void ToBounds(this VisualElement child, VisualElement parent)
        {
            var parentWidth = parent.resolvedStyle.width;
            var parentHeight = parent.resolvedStyle.height;

            var childWidth = child.resolvedStyle.width;
            var childHeight = child.resolvedStyle.height;

            var childTransform = child.transform;

            var childOffsetX = childTransform.position.x;
            var childOffsetY = childTransform.position.y;

            if (childOffsetX < 0)
            {
                childOffsetX = 0;
            }
            else if (childOffsetX + childWidth > parentWidth)
            {
                childOffsetX = parentWidth - childWidth;
            }

            if (childOffsetY < 0)
            {
                childOffsetY = 0;
            }
            else if (childOffsetY + childHeight > parentHeight)
            {
                childOffsetY = parentHeight - childHeight;
            }

            child.transform.position = new Vector3(childOffsetX, childOffsetY, 0);
        }
    }
}