﻿using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public class TooltipElement : ExtendedVisualElement
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
        
        public float MinWidth
        {
            get => style.minWidth.value.value;
            set => style.minWidth = value;
        }

        public float MaxWidth
        {
            get => style.maxWidth.value.value;
            set => style.maxWidth = value;
        }

        public float BorderWidth
        {
            get => style.BorderWidth();
            set => style.BorderWidth(value);
        }

        public float BorderRadius
        {
            get => style.BorderRadius();
            set => style.BorderRadius(value);
        }

        public Color TitleColor
        {
            get => _title.style.color.value;
            set => _title.style.color = value;
        }
        
        public Color DescriptionColor
        {
            get => _description.style.color.value;
            set => _description.style.color = value;
        }

        public Color BackgroundColor
        {
            get => style.backgroundColor.value;
            set => style.backgroundColor = new Color(value.r, value.g, value.b, 1);
        }

        public Color BorderColor
        {
            get => style.BorderColor();
            set => style.BorderColor(value);
        }

        private string _descriptionRaw;

        private Label _title;
        private Label _description;

        public TooltipElement()
        {
            _title = new Label();
            _title.name = "Title";
            _description = new Label();
            _description.name = "Description";
            
            hierarchy.Add(_title);
            hierarchy.Add(_description);

            style.opacity = new StyleFloat(0.75f);
            style.position = new StyleEnum<Position>(Position.Absolute);
            style.justifyContent = new StyleEnum<Justify>(Justify.FlexStart);
            style.alignContent = new StyleEnum<Align>(Align.Stretch);
            
            MinWidth = 60;
            MaxWidth = 300;
            TitleColor = Color.white;
            DescriptionColor = Color.white;
            BackgroundColor = Color.black;
            BorderColor = Color.yellow;
            BorderWidth = 2;
            BorderRadius = 5;
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