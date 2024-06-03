using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static class Modal
    {
        private static Stack<VisualElement> _contexts = new();

        public static void AddContext(VisualElement element)
        {
            _contexts.Push(element);
        }

        public static void PopContext()
        {
            _contexts.Pop();
        }
        
        public static ModalPart<T> Start<T>(VisualElement overElement, string title = "My title") where T : ModalWindow, new()
        {
            return new ModalPart<T>(overElement, title);
        }
        
        public static ModalPart<T> Start<T>(string title = "My title") where T : ModalWindow, new()
        {
            return new ModalPart<T>(_contexts.Peek(), title);
        }

        public class ModalPart<T> where T : ModalWindow, new()
        {
            private readonly T _window;
            
            private VisualElement _parent;
            private string _title;
            
            public ModalPart(VisualElement element, string title)
            {
                _parent = element.parent;
                _title = title;
                
                _window = new T();
                _window.Title = _title;
                _window.Closed += () => _parent.hierarchy.Remove(_window);
                _window.Closed += () => Debug.Log("Closed");
            }

            public ModalPart<T> BackColor(Color color)
            {
                _window.BackgroundColor = color;
                return this;
            }
            
            public ModalPart<T> HeadColor(Color color)
            {
                _window.WindowHeadColor = color;
                return this;
            }
            
            public ModalPart<T> WindowColor(Color color)
            {
                _window.WindowColor = color;
                return this;
            }
            
            public ModalPart<T> ContentBackColor(Color color)
            {
                _window.ContentColor = color;
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
