using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class ModalWindow : BetterVisualElement
    {
        public event Action Closed;
        public override VisualElement contentContainer => _body;

        public override bool canGrabFocus => true;

        [UxmlAttribute]
        public Color BackgroundColor
        {
            get => _background.style.backgroundColor.value;
            set => _background.style.backgroundColor = value;
        }

        [UxmlAttribute]
        public Color WindowHeadColor
        {
            get => _head.style.backgroundColor.value;
            set => _head.style.backgroundColor = value;
        }
        
        [UxmlAttribute]
        public Color WindowColor
        {
            get => _window.style.backgroundColor.value;
            set => _window.style.backgroundColor = value;
        }
        
        [UxmlAttribute]
        public Color ContentColor
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

        private VisualElement _background;
        private VisualElement _window;
        private VisualElement _head;
        private VisualElement _body;
        
        public ModalWindow()
        {
            this.StretchToParentSize();
        }

        protected override void InitContentContainer()
        {
            base.InitContentContainer();
            InitBack();
            InitWindow();
            
            hierarchy.Add(_background);
            hierarchy.Add(_window);
        }

        public void Open()
        {
            style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }

        public void Close()
        {
            style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            Closed?.Invoke();
        }

        protected override void OnRemoveFrom(BetterVisualElement parent)
        {
            style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            
            _background = null;
            _window = null;
            _head = null;
            _body = null;
        }

        private void InitBack()
        {
            _background = new VisualElement();
            _background.style.backgroundColor = new Color(0, 0, 0, 0.3f);
            _background.StretchToParentSize();
            _background.RegisterCallback<MouseDownEvent>(e => Close());
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
            
            _window.RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.Escape) Close();
            });
            
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
            _head.RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.Escape) Close();
            });
            _window.Add(_head);
        }

        private void InitContent()
        {
            _body = new ScrollView();
            _body.StretchToParentSize();
            _body.style.top = new StyleLength(new Length(_head.style.height.value.value));
            _body.contentContainer.StretchToParentSize();
            
            _body.style.paddingTop = new StyleLength(new Length(10, LengthUnit.Pixel));
            _body.RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.Escape) Close();
            });
            _window.Add(_body);
        }
    }
}
