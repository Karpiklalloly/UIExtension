using System.Collections.Generic;
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
    }
}