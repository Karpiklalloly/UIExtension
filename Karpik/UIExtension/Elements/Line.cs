using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class Line : BetterVisualElement
    {
        [UxmlAttribute]
        public Color StartColor { get; set; } = Color.green;
        [UxmlAttribute]

        public Color EndColor { get; set; } = Color.green;
        
        [UxmlAttribute]
        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                style.height = _width;
            }
        }
        [UxmlAttribute]
        public Vector2 Start { get; set; } = Vector2.zero;
        [UxmlAttribute]
        public Vector2 End { get; set; } = Vector2.zero;
        [UxmlAttribute]
        public Vector2 StartOffset { get; set; } = Vector2.zero;
        [UxmlAttribute]
        public Vector2 EndOffset { get; set; } = Vector2.zero;
        
        public Action<Line> OnClick { get; set; } = null;
        
        public VisualElement StartElement => _start as VisualElement;
        public VisualElement EndElement => _end as VisualElement;

        private float _width = 5f;
        
        private IPositionNotify _start = null;
        private IPositionNotify _end = null;

        public Line()
        {
            style.height = _width;
            this.RegisterCallback<ClickEvent>(Clicked);
            generateVisualContent += OnGenerateVisualContent;
            transform.position = new Vector3(transform.position.x, transform.position.y, -100);
        }


        public void SetStart<T>(T value, Vector2 offset) where T : IPositionNotify
        {
            _start?.UnregisterValueChangedCallback(OnStartChanged);
            value.RegisterValueChangedCallback(OnStartChanged);
            StartOffset = offset;
            Start = value.value;
            _start = value;
            Update();
        }
        
        public void SetEnd<T>(T value, Vector2 offset) where T : IPositionNotify
        {
            _end?.UnregisterValueChangedCallback(OnEndChanged);
            value.RegisterValueChangedCallback(OnEndChanged);
            EndOffset = offset;
            End = value.value;
            _end = value;
            Update();
        }

        private void OnStartChanged(ChangeEvent<Vector2> e)
        {
            Start = e.newValue;
            Update();
            MarkDirtyRepaint();
        }

        private void OnEndChanged(ChangeEvent<Vector2> e)
        {
            End = e.newValue;
            Update();
            MarkDirtyRepaint();
        }

        private void Update()
        {
            var start = Start + StartOffset;
            var end = End + EndOffset;
            var rotation = 90 - Mathf.Atan2(end.x - start.x, end.y - start.y) * Mathf.Rad2Deg;
            
            transform.position = Vector2.Lerp(start, end, 0.5f) -
                                 new Vector2(Vector2.Distance(start, end) / 2, 0);
            style.width = Vector2.Distance(start, end);
            transform.rotation = Quaternion.Euler(
                0, 0, rotation);
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
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
            
            painter.MoveTo(new Vector2(0, Width / 2));
            painter.LineTo(new Vector2(style.width.value.value, Width / 2));
            
            painter.ClosePath();
            
            painter.Stroke();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            StartOffset = Vector2.zero;
            EndOffset = Vector2.zero;
            
            Start = Vector2.zero;
            End = Vector2.zero;
            
            _start.UnregisterValueChangedCallback(OnStartChanged);
            _end.UnregisterValueChangedCallback(OnEndChanged);

            _start = null;
            _end = null;

            OnClick = null;
        }

        protected override void OnRemoveFrom(BetterVisualElement parent)
        {
            _start.UnregisterValueChangedCallback(OnStartChanged);
            _end.UnregisterValueChangedCallback(OnEndChanged);
            
            _start = null;
            _end = null;
        }
        
        private void Clicked(ClickEvent evt)
        {
            OnClick?.Invoke(this);
        }

    }
}
