using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class Canvas : ExtendedVisualElement
    {
        public override VisualElement contentContainer => _root;
        
        protected List<DragManipulator> DragManipulators = new();
        private VisualElement _root;
        
        public Canvas()
        {
            _root = new ExtendedVisualElement
            {
                name = "Root"
            };
            _root.StretchToParentSize();
            _root.AddManipulator(new ChildElementMoverManipulator());
            _root.AddToClassList(DragManipulator.DropContainerClass);
            hierarchy.Add(_root);
            
            style.flexWrap = new StyleEnum<Wrap>(Wrap.NoWrap);
        }

        protected override void OnChildAdded(VisualElement element)
        {
            base.OnChildAdded(element);
            element.style.position = new StyleEnum<Position>(Position.Absolute);
            var manipulator = new DragManipulator()
            {
                Enabled = true
            };
            DragManipulators.Add(manipulator);
            
            element.ManipulatorAdd(manipulator);
        }

        protected override void OnChildRemoved(VisualElement element)
        {
            base.OnChildRemoved(element);
            for (var i = 0; i < DragManipulators.Count; i++)
            {
                var manipulator = DragManipulators[i];
                if (manipulator.target != element) continue;
                DragManipulators.Remove(manipulator);
                element.ManipulatorRemove(manipulator);
                break;
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            _root.Clear();
            _root = null;
            foreach (var manipulator in DragManipulators)
            {
                manipulator.target = null;
                manipulator.Enabled = false;
            }
            DragManipulators.Clear();
            DragManipulators = null;
        }

        protected DragManipulator GetDragManipulator(VisualElement element)
        {
            return DragManipulators.First(x => x.target == element);
        }
    }
}
