using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class ModalWindow : ExtendedVisualElement, IModalWindow
    {
        public event Action Opened;
        public event Action Closed;

        [UxmlAttribute]
        public Color WindowColor
        {
            get => _window.style.backgroundColor.value;
            set => _window.style.backgroundColor = value;
        }
        
        [UxmlAttribute]
        public Color BodyColor
        {
            get => _body.style.backgroundColor.value;
            set => _body.style.backgroundColor = value;
        }

        [UxmlAttribute]
        public string Title
        {
            get => _head.Q<Label>().text;
            set => _head.Q<Label>().text = value;
        }

        [UxmlAttribute]
        public Color TitleTextColor
        {
            get => _head.Q<Label>().style.color.value;
            set => _head.Q<Label>().style.color = value;
        }
        
        [UxmlAttribute]
        public Color TitleColor
        {
            get => _head.Q<Label>().style.backgroundColor.value;
            set => _head.Q<Label>().style.backgroundColor = value;
        }
        
        public override VisualElement contentContainer => _body;

        public override bool canGrabFocus => true;
        
        private VisualElement _window;
        private VisualElement _head;
        private VisualElement _body;
        
        public ModalWindow()
        {
            this.ToCenter();
            InitWindow();
            
            hierarchy.Add(_window);
        }

        public void Open()
        {
            style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            Opened?.Invoke();
        }

        public void Close()
        {
            if (style.display.value == DisplayStyle.None) return;
            style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            Closed?.Invoke();
        }

        protected override void OnRemoveFrom()
        {
            style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            
            _window = null;
            _head = null;
            _body = null;
        }

        private void InitWindow()
        {
            _window = new VisualElement();
            
            _window.ToFloatWindow();
            _window.ToCenter();
            _window.style.backgroundColor = Color.gray;
            _window.style.maxWidth = new StyleLength(new Length(300, LengthUnit.Pixel));
            _window.style.minHeight = new StyleLength(new Length(450, LengthUnit.Pixel));
            _window.style.minWidth = new StyleLength(new Length(300, LengthUnit.Pixel));
            
            InitHead();
            InitContent();
        }

        private void InitHead()
        {
            _head = new VisualElement();
            _head.style.height = new StyleLength(new Length(20, LengthUnit.Pixel));
            _head.style.borderBottomColor = Color.black;
            _head.style.borderBottomWidth = new StyleFloat(2);
            
            var label = new Label();
            label.text = "Window";
            label.style.color = Color.black;

            var closeButton = new Button();
            closeButton.clicked += Close;
            closeButton.style.position = new StyleEnum<Position>(Position.Absolute);
            closeButton.text = "X";
            closeButton.style.right = 0;
            closeButton.style.top = 0;
            closeButton.style.bottom = 0;
            
            _head.Add(label);
            _head.Add(closeButton);
            _window.Add(_head);
        }

        private void InitContent()
        {
            _body = new ScrollView();
            _body.StretchToParentSize();
            _body.style.top = new StyleLength(new Length(_head.style.height.value.value));
            _body.contentContainer.StretchToParentSize();
            
            _body.style.paddingTop = new StyleLength(new Length(10, LengthUnit.Pixel));
            _window.Add(_body);
        }
    }
}
