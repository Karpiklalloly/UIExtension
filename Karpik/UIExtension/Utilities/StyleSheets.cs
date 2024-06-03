using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public static class StyleSheets
    {
        public static StyleSheet Containers => Resources.Load<StyleSheet>("Styles/Containers");
        public static StyleSheet ContainerItems => Resources.Load<StyleSheet>("Styles/ContainerItems");
        
        public static StyleSheet Positions => Resources.Load<StyleSheet>("Styles/Positions");
    }
}
