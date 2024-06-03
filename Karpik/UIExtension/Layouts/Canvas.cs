using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class Canvas : BetterVisualElement
    {
        public override VisualElement contentContainer => Root;
        
        protected List<DragManipulator> DragManipulators = new();
        private VisualElement Root;
        
        public Canvas()
        {
            this.StretchToParentSize();

            style.flexWrap = new StyleEnum<Wrap>(Wrap.NoWrap);
        }

        protected override void InitContentContainer()
        {
            base.InitContentContainer();

            Root = new VisualElement
            {
                name = "Root"
            };
            Root.StretchToParentSize();
            Root.AddManipulator(new ChildElementMoverManipulator());
            Root.AddToClassList("droppable");
            hierarchy.Add(Root);
        }

        public override void Add(VisualElement element)
        {
            base.Add(element);
            
            element.style.position = new StyleEnum<Position>(Position.Absolute);
            var manipulator = new DragManipulator()
            {
                Enabled = true
            };
            DragManipulators.Add(manipulator);
            element.AddManipulator(manipulator);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            Root?.Clear();
            ;
            Root = null;
            foreach (var manipulator in DragManipulators)
            {
                manipulator.Enabled = false;
            }
            DragManipulators.Clear();
        }
    }
}
