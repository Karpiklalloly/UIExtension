using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class Line : ExtendedVisualElement
    {
        [UxmlAttribute]
        public Color StartColor
        {
            get => _painter.StartColor;
            set => _painter.StartColor = value;
        }
        
        [UxmlAttribute]
        public Color EndColor
        {
            get => _painter.EndColor;
            set => _painter.EndColor = value;
        }
        
        [UxmlAttribute]
        public float Width
        {
            get => _painter.Width;
            set
            {
                _painter.Width = value;
                style.height = value;
            }
        }

        [UxmlAttribute]
        public Vector2 Start { get; set; }

        [UxmlAttribute]
        public Vector2 End { get; set; }
        
        public VisualElement StartElement => _start as VisualElement;
        public VisualElement EndElement => _end as VisualElement;

        private Painter _painter;
        
        private IPositionNotify _start = null;
        private IPositionNotify _end = null;

        private Vector2 _startOffset = Vector2.zero;
        private Vector2 _endOffset = Vector2.zero;

        public Line()
        {
            StartColor = Color.white;
            EndColor = Color.white;
            Width = 5;
            
            style.height = Width;
            _painter.Start = Vector2.zero;
            generateVisualContent += OnGenerateVisualContent;
            transform.position = new Vector3(transform.position.x, transform.position.y, -100);
        }


        public void SetStart(IPositionNotify value, Vector2 offset)
        {
            _start?.UnregisterValueChangedCallback(OnStartChanged);
            value.RegisterValueChangedCallback(OnStartChanged);
            _startOffset = offset;
            Start = value.value;
            _start = value;
            Update();
        }
        
        public void SetEnd(IPositionNotify value, Vector2 offset)
        {
            _end?.UnregisterValueChangedCallback(OnEndChanged);
            value.RegisterValueChangedCallback(OnEndChanged);
            _endOffset = offset;
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
            var start = Start + _startOffset;
            var end = End + _endOffset;
            var rotation = 90 - Mathf.Atan2(end.x - start.x, end.y - start.y) * Mathf.Rad2Deg;
            
            transform.position = Vector2.Lerp(start, end, 0.5f) -
                                 new Vector2(Vector2.Distance(start, end) / 2, 0);
            style.width = Vector2.Distance(start, end);
            _painter.End = new Vector2(style.width.value.value, 0);
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            _painter.Draw(ctx);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            _startOffset = Vector2.zero;
            _endOffset = Vector2.zero;
            
            Start = Vector2.zero;
            End = Vector2.zero;
            
            UnRegister();
        }

        protected override void OnRemoveFrom()
        {
            UnRegister();
        }

        private void UnRegister()
        {
            _start.UnregisterValueChangedCallback(OnStartChanged);
            _end.UnregisterValueChangedCallback(OnEndChanged);
            
            _start = null;
            _end = null;
        }
    }
}
