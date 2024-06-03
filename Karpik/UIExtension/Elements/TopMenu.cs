using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class TopMenu : BetterVisualElement
    {
        private List<VisualElement> _buttons = new();
        
        public TopMenu()
        {
            InitStyles();
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

        public void SetButtonColor(Color color)
        {
            foreach (var button in _buttons)
            {
                button.style.backgroundColor = color;
            }
        }

        private void InitStyles()
        {
            this.ToSideBar(side: Selectors.Side.Top);
        }
    }
}
