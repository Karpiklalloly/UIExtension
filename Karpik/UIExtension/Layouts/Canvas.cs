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
        }

        public override void Remove(VisualElement element)
        {
            base.Remove(element);

            for (var i = 0; i < DragManipulators.Count; i++)
            {
                var manipulator = DragManipulators[i];
                if (manipulator.target != element) continue;
                DragManipulators.Remove(manipulator);
                element.RemoveManipulator(manipulator);
                break;
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            Root?.Clear();
            Root = null;
            foreach (var manipulator in DragManipulators)
            {
                manipulator.target = null;
                manipulator.Enabled = false;
            }
            DragManipulators.Clear();
        }
    }
}
