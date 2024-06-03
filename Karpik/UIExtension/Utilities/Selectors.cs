using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static class Selectors
    {
        public static string Grid => "grid";
        public static string HorizontalContainer => "horizontal-container";
        public static string XBar => "x-bar";
        public static string FloatWindow => "float-window";

        public static string ContainerElement => "col";
        public static string XBarElement => "x-bar-item";

        public static string Center => "center";
        public static string HorizontalCenter => "horizontal-center";
        public static string VerticalCenter => "vertical-center";
        

        public static void ToGrid(this VisualElement element)
        {
            element.styleSheets.Add(StyleSheets.Containers);
            element.AddToClassList(Grid);
        }

        public static void ToSideBar(this VisualElement element,
            Side side = Side.Top,
            float offset = 0f,
            float height = -1f,
            float width = -1f)
        {
            if (height < 0) height = 20;
            if (width < 0) width = 20;
            
            element.styleSheets.Add(StyleSheets.Containers);
            element.AddToClassList(XBar);
            
            element.style.backgroundColor = new StyleColor(Color.grey);
            switch (side)
            {
                case Side.Top:
                    element.style.top = offset;
                    element.style.height = height;
                    element.style.left = 0;
                    element.style.right = 0;
                    return;
                case Side.Bottom:
                    element.style.bottom = offset;
                    element.style.height = height;
                    element.style.left = 0;
                    element.style.right = 0;
                    break;
                case Side.Right:
                    element.style.right = offset;
                    element.style.width = width;
                    element.style.top = 0;
                    element.style.bottom = 0;
                    break;
                case Side.Left:
                    element.style.left = offset;
                    element.style.width = width;
                    element.style.top = 0;
                    element.style.bottom = 0;
                    break;
            }
        }

        public static void ToFloatWindow(this VisualElement element)
        {
            element.styleSheets.Add(StyleSheets.Containers);
            element.AddToClassList(FloatWindow);
        }

        public static void ToCenter(this VisualElement element, CenterPosition center = CenterPosition.Both)
        {
            element.styleSheets.Add(StyleSheets.Positions);
            switch (center)
            {
                case CenterPosition.Horizontal:
                    element.AddToClassList(HorizontalCenter);
                    break;
                case CenterPosition.Vertical:
                    element.AddToClassList(VerticalCenter);
                    break;
                case CenterPosition.Both:
                    element.AddToClassList(Center);
                    break;
            }
        }

        public static void ToContainable(this VisualElement element)
        {
            element.styleSheets.Add(StyleSheets.ContainerItems);
            element.AddToClassList(ContainerElement);
        }

        public enum Side
        {
            Top,
            Bottom,
            Right,
            Left
            
        }

        public enum CenterPosition
        {
            Horizontal,
            Vertical,
            Both
        }
    }
}
