using System;
using UnityEngine;

namespace Karpik.UIExtension
{
    public interface IModalWindow
    {
        public event Action Opened;
        public event Action Closed;

        public string Title { get; set; }
        public Color TitleTextColor { get; set; }
        public Color TitleColor { get; set; }
        
        public Color BodyColor { get; set; }
        public Color WindowColor { get; set; }

        public void Open();
        public void Close();
    }
}