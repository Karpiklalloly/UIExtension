using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public struct Painter
    {
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public float Width { get; set; }
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        public void Draw(MeshGenerationContext ctx)
        {
            var start = Start + new Vector2(0, Width / 2);
            var end = End + new Vector2(0, Width / 2);
            
            var painter = ctx.painter2D;
            painter.lineWidth = Width;
            painter.strokeGradient = new Gradient()
            {
                colorKeys = new GradientColorKey[]
                {
                    new() { color = StartColor, time = 0 },
                    new() { color = EndColor, time = 1 }
                }
            };
            
            painter.lineJoin = LineJoin.Round;
            painter.lineCap = LineCap.Round;
            
            painter.BeginPath();
            
            painter.MoveTo(start);
            painter.LineTo(end);
            
            painter.Stroke();
            painter.ClosePath();
        }
    }
}