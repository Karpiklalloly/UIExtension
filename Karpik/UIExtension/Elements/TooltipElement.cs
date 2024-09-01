using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public class TooltipElement : BetterVisualElement
    {
        public string Title
        {
            get => _title.text;
            set => _title.text = value;
        }

        public string Description
        {
            get => _description.text;
            set => _description.text = value;
        }

        private Label _title;
        private Label _description;

        public TooltipElement()
        {
            _title = new Label();
            _title.name = "Title";
            _description = new Label();
            _description.name = "Title";
            
            hierarchy.Add(_title);
            hierarchy.Add(_description);
            
            style.BorderRadius(5f);
            style.Border(2f, Color.yellow);
            style.backgroundColor = Color.black;
            style.opacity = new StyleFloat(0.75f);
            style.position = new StyleEnum<Position>(Position.Absolute);
            style.justifyContent = new StyleEnum<Justify>(Justify.FlexStart);
            style.alignContent = new StyleEnum<Align>(Align.Stretch);
            //style.minHeight = new StyleLength(20);
            style.minWidth = new StyleLength(60);
            style.maxWidth = new StyleLength(300);
            style.color = Color.white;
        }

        public void Show()
        {
            style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
        }

        public void Hide()
        {
            style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
        }
    }
}