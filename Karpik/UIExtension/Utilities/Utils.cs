using System;
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
            TypeConverter<UI, Source> converter1,
            TypeConverter<Source, UI> converter2,
            BindingMode bindingMode,
            bool native = true)
            where UI : class
            where Source : class
        {
            if (native)
            {
                var binding = new DataBinding()
                {
                    dataSourcePath = PropertyPath.FromName(bindingPath),
                    bindingMode = bindingMode
                };
            
                binding.sourceToUiConverters.AddConverter(converter1);
                binding.uiToSourceConverters.AddConverter(converter2);
            
                element.SetBinding(uiValue, binding);
                return;
            }

            throw new NotImplementedException("Non native binding is not supported. Use native instead.");

            element.RegisterCallback<ChangeEvent<UI>>(e =>
            {
                var source = element.dataSource as Source;
                var value = e.newValue;
                source = converter1?.Invoke(ref value);
                
            });
        }
        
        public static bool HasWorld(this VisualElement element, Vector2 worldPosition)
        {
            return element.ContainsPoint(element.WorldToLocal(worldPosition));
        }
    }
}
