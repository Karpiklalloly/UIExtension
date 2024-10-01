using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static class DefaultMethodsExtensions
    {
        public static void AddChild(this VisualElement element, VisualElement child)
        {
            if (element is BetterVisualElement better)
            {
                better.Add(child);
            }
            else
            {
                element.Add(child);
            }
        }

        public static void RemoveChild(this VisualElement element, VisualElement child)
        {
            if (element is BetterVisualElement better)
            {
                better.Remove(child);
            }
            else
            {
                element.Remove(child);
            }
        }
        
        public static void RemoveChildAt(this VisualElement element, int index)
        {
            if (element is BetterVisualElement better)
            {
                better.RemoveAt(index);
            }
            else
            {
                element.RemoveAt(index);
            }
        }

        public static void ManipulatorAdd(this VisualElement element, IManipulator manipulator)
        {
            if (element is BetterVisualElement better)
            {
                better.AddManipulator(manipulator);
            }
            else
            {
                element.AddManipulator(manipulator);
            }
        }
        
        public static void ManipulatorRemove(this VisualElement element, IManipulator manipulator)
        {
            if (element is BetterVisualElement better)
            {
                better.RemoveManipulator(manipulator);
            }
            else
            {
                element.RemoveManipulator(manipulator);
            }
        }
    }
}