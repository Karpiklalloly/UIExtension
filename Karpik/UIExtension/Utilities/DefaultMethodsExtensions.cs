using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static class DefaultMethodsExtensions
    {
        public static void AddChild(this VisualElement element, VisualElement child)
        {
            if (element is ExtendedVisualElement better)
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
            if (element is ExtendedVisualElement better)
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
            if (element is ExtendedVisualElement better)
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
            if (element is ExtendedVisualElement better)
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
            if (element is ExtendedVisualElement better)
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