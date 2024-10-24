using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static class StyleExtensions
    {
        public static void Margin(this IStyle style, 
        float top,
        float right,
        float bottom,
        float left,
        LengthUnit unit)
        {
            style.marginBottom = new StyleLength(new Length(bottom, unit));
            style.marginTop = new StyleLength(new Length(top, unit));
            style.marginLeft = new StyleLength(new Length(left, unit));
            style.marginRight = new StyleLength(new Length(right, unit));
        }
        
        public static void Margin(this IStyle style, 
            float top, LengthUnit unitTop,
            float right, LengthUnit unitRight,
            float bottom, LengthUnit unitBottom,
            float left, LengthUnit unitLeft)
        {
            style.marginBottom = new StyleLength(new Length(bottom, unitBottom));
            style.marginTop = new StyleLength(new Length(top, unitTop));
            style.marginLeft = new StyleLength(new Length(left, unitLeft));
            style.marginRight = new StyleLength(new Length(right, unitRight));
        }
        
        public static void Margin(this IStyle style, 
            float margin,
            LengthUnit unit)
        {
            style.marginBottom = new StyleLength(new Length(margin, unit));
            style.marginTop = new StyleLength(new Length(margin, unit));
            style.marginLeft = new StyleLength(new Length(margin, unit));
            style.marginRight = new StyleLength(new Length(margin, unit));
        }
        
        public static void Margin(this IStyle style, 
            float margin)
        {
            style.marginBottom = margin;
            style.marginTop = margin;
            style.marginLeft = margin;
            style.marginRight = margin;
        }
        
        public static void Padding(this IStyle style, 
            float top,
            float right,
            float bottom,
            float left,
            LengthUnit unit)
        {
            style.paddingBottom = new StyleLength(new Length(bottom, unit));
            style.paddingTop = new StyleLength(new Length(top, unit));
            style.paddingLeft = new StyleLength(new Length(left, unit));
            style.paddingRight = new StyleLength(new Length(right, unit));
        }
        
        public static void Padding(this IStyle style, 
            float top, LengthUnit unitTop,
            float right, LengthUnit unitRight,
            float bottom, LengthUnit unitBottom,
            float left, LengthUnit unitLeft)
        {
            style.paddingBottom = new StyleLength(new Length(bottom, unitBottom));
            style.paddingTop = new StyleLength(new Length(top, unitTop));
            style.paddingLeft = new StyleLength(new Length(left, unitLeft));
            style.paddingRight = new StyleLength(new Length(right, unitRight));
        }
        
        public static void Padding(this IStyle style, 
            float padding,
            LengthUnit unit)
        {
            style.paddingBottom = new StyleLength(new Length(padding, unit));
            style.paddingTop = new StyleLength(new Length(padding, unit));
            style.paddingLeft = new StyleLength(new Length(padding, unit));
            style.paddingRight = new StyleLength(new Length(padding, unit));
        }
        
        public static void Padding(this IStyle style, 
            float padding)
        {
            style.paddingBottom = padding;
            style.paddingTop = padding;
            style.paddingLeft = padding;
            style.paddingRight = padding;
        }
        
        public static void Border(this IStyle style, 
            float top,
            float right,
            float bottom,
            float left,
            Color color)
        {
            style.borderBottomWidth = new StyleFloat(bottom);
            style.borderBottomColor = new StyleColor(color);
            
            style.borderTopWidth = new StyleFloat(top);
            style.borderTopColor = new StyleColor(color);
            
            style.borderLeftWidth = new StyleFloat(left);
            style.borderLeftColor = new StyleColor(color);
            
            style.borderRightWidth = new StyleFloat(right);
            style.borderRightColor = new StyleColor(color);
        }
        
        public static void Border(this IStyle style, 
            float top, Color colorTop,
            float right, Color colorRight,
            float bottom, Color colorBottom,
            float left, Color colorLeft)
        {
            style.borderBottomWidth = new StyleFloat(bottom);
            style.borderBottomColor = new StyleColor(colorBottom);
            
            style.borderTopWidth = new StyleFloat(top);
            style.borderTopColor = new StyleColor(colorTop);
            
            style.borderLeftWidth = new StyleFloat(left);
            style.borderLeftColor = new StyleColor(colorLeft);
            
            style.borderRightWidth = new StyleFloat(right);
            style.borderRightColor = new StyleColor(colorRight);
        }

        public static void BorderColor(this IStyle style,
            Color color)
        {
            style.borderBottomColor = new StyleColor(color);
            style.borderTopColor = new StyleColor(color);
            style.borderLeftColor = new StyleColor(color);
            style.borderRightColor = new StyleColor(color);
        }

        public static Color BorderColor(this IStyle style) => style.borderBottomColor.value;

        public static void BorderWidth(this IStyle style,
            float width)
        {
            style.borderBottomWidth = new StyleFloat(width);
            style.borderTopWidth = new StyleFloat(width);
            style.borderLeftWidth = new StyleFloat(width);
            style.borderRightWidth = new StyleFloat(width);
        }
        
        public static float BorderWidth(this IStyle style) => style.borderBottomWidth.value;

        public static void BorderRadius(this IStyle style, 
            float topLeft,
            float topRight,
            float bottomLeft,
            float bottomRight)
        {
            style.borderTopLeftRadius = new StyleLength(topLeft);
            style.borderTopRightRadius = new StyleLength(topRight);
            style.borderBottomLeftRadius = new StyleLength(bottomLeft);
            style.borderBottomRightRadius = new StyleLength(bottomRight);
        }
        
        public static void BorderRadius(this IStyle style, 
            float radius)
        {
            style.borderTopLeftRadius = new StyleLength(radius);
            style.borderTopRightRadius = new StyleLength(radius);
            style.borderBottomLeftRadius = new StyleLength(radius);
            style.borderBottomRightRadius = new StyleLength(radius);
        }
        
        public static float BorderRadius(this IStyle style) => style.borderTopLeftRadius.value.value;
    }
}