using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static class Utils
    {
        public static void Move(this VisualElement element, Vector2 delta)
        {
            if (element is IPositionNotify n)
            {
                n.value += delta;
            }
            else
            {
                element.transform.position += new Vector3(delta.x, delta.y, element.transform.position.z);
            }
        }

        public static void MoveTo(this VisualElement element, Vector2 position)
        {
            if (element is IPositionNotify n)
            {
                n.value = position;
            }
            else
            {
                element.transform.position = new Vector3(position.x, position.y, element.transform.position.z);
            }
        }

        public static void RegisterBinding<UI, Source>(this VisualElement element,
                string bindingPath,
                string uiValue,
                TypeConverter<UI, Source> uiToSource = null,
                TypeConverter<Source, UI> sourceToUI = null,
                BindingMode bindingMode = BindingMode.TwoWay)
        {
            var binding = new DataBinding()
            {
                dataSourcePath = PropertyPath.FromName(bindingPath),
                bindingMode = bindingMode
            };

            if (sourceToUI != null)
            {
                binding.sourceToUiConverters.AddConverter(sourceToUI);
            }

            if (uiToSource != null)
            {
                binding.uiToSourceConverters.AddConverter(uiToSource);
            }
                
            element.SetBinding(uiValue, binding);
        }

        public static bool HasWorld(this VisualElement element, Vector2 worldPosition)
        {
            return element.ContainsPoint(element.WorldToLocal(worldPosition));
        }
    }
}
