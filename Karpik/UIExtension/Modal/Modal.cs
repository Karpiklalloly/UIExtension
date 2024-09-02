using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static class Modal
    {
        private static Stack<VisualElement> _contexts = new();

        public static void PushContext(VisualElement element)
        {
            _contexts.Push(element);
        }

        public static void PopContext()
        {
            _contexts.Pop();
        }
        
        public static ModalPart<T> Start<T>(string title = "My title") where T : VisualElement, IModalWindow, new()
        {
            return Start<T>(_contexts.Peek(), title);
        }
        
        public static ModalPart<T> Start<T>(VisualElement overElement, string title = "My title") where T : VisualElement, IModalWindow, new()
        {
            return new ModalPart<T>(overElement, title);
        }

        public class ModalPart<T> where T : VisualElement, IModalWindow,  new()
        {
            private readonly T _window;
            
            private VisualElement _parent;
            
            public ModalPart(VisualElement element, string title)
            {
                _parent = element.parent;
                
                _window = new T();
                _window.Title = title;
                _window.Closed += () => _parent.hierarchy.Remove(_window);
            }
            
            public ModalPart<T> TitleColor(Color color)
            {
                _window.TitleColor = color;
                return this;
            }
            
            public ModalPart<T> WindowColor(Color color)
            {
                _window.WindowColor = color;
                return this;
            }
            
            public ModalPart<T> BodyColor(Color color)
            {
                _window.BodyColor = color;
                return this;
            }

            public ModalPart<T> OnShow(Action action)
            {
                _window.Opened += action;
                return this;
            }

            public ModalPart<T> OnClose(Action action)
            {
                _window.Closed += action;
                return this;
            }

            public ModalPart<T> Add(VisualElement element)
            {
                _window.Add(element);
                return this;
            }

            public T Show()
            {
                _parent.hierarchy.Add(_window);
                return _window;
            }
        }
    }
}
