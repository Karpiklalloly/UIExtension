using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class TopMenu : ExtendedVisualElement
    {
        public float Height
        {
            get => style.height.value.value;
            set
            {
                style.height = value;
            }
        }
        
        private List<VisualElement> _buttons = new();

        public new MenuElement this[int index] => new(_buttons[index]);

        public TopMenu()
        {
            this.ToSideBar(side: Selectors.Side.Top);
        }

        public void AddButton(string text, Action onClick, bool toggle = false)
        {
            VisualElement element;
            
            if (toggle)
            {
                var button = new Toggle
                {
                    text = text
                };
                button.RegisterValueChangedCallback(e => onClick());
                element = button;
            }
            else
            {
                var button = new Button()
                {
                    text = text
                };
                button.clicked += onClick;
                element = button;
            }
            
            element.styleSheets.Add(StyleSheets.ContainerItems);
            element.AddToClassList(Selectors.XBarElement);
            element.style.color = Color.black;
            element.ToCenter(Selectors.CenterPosition.Vertical);
            Add(element);
            _buttons.Add(element);
        }

        public void SetMenuColor(Color color)
        {
            style.backgroundColor = color;
        }

        public struct MenuElement
        {
            private VisualElement _element;

            public MenuElement(VisualElement element)
            {
                _element = element;
            }
            
            public void SetBackgroundColor(Color color)
            {
                _element.style.backgroundColor = color;
            }
            
            public void SetTextColor(Color color)
            {
                _element.style.color = color;
            }
        }
    }
}
