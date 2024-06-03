using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class Graph : Canvas
    {
        public IReadOnlyList<IGraphNode> Nodes => _idToNode.Values.ToList();
        
        [SerializeField]
        private Dictionary<string, IGraphNode> _idToNode = new();
        [SerializeField]
        private TopMenu _menu = new();

        private TwoElementsChooserManipulator _elementsChooserManipulator;
        
        public Graph()
        {
            hierarchy.Add(_menu);
            SetButtons();
            
            _elementsChooserManipulator = new TwoElementsChooserManipulator(
                typeof(IGraphNode),
                AddLink,
                null)
            {
                Enabled = false
            };
            EnableContextMenu = false;
            contentContainer.AddManipulator(_elementsChooserManipulator);
        }

        public void AddNode<T>(T node, Action onClick) where T : BetterVisualElement, IGraphNode
        {
            node.EnableContextMenu = !EnableContextMenu;
            node.AddContextMenu("Open", e =>
            {
                onClick?.Invoke();
            });

            _idToNode.Add(node.Id, node);
            node.name = node.GetType().ToString() + _idToNode.Count.ToString();
            
            Add(node);
            foreach (var dragManipulator in DragManipulators)
            {
                dragManipulator.Enabled = EnableContextMenu;
            }
            Save();
        }
        
        public void RemoveNode(string id)
        {
            if (Nodes.FirstOrDefault(x => x.Id == id) is not VisualElement node) return;
            _idToNode.Remove(id);
            Remove(node);
        }
        
        public new void Clear()
        {
            base.Clear();
            _idToNode.Clear();
        }

        public Line AddLine<T>(T from, T to) where T : IPositionNotify
        {
            var line = new Line();
            
            line.SetStart(from, from.Center - from.value);
            line.SetEnd(to, from.Center - from.value);
            line.StartColor = Color.green;
            line.EndColor = Color.red;
            line.OnClick = RemoveLink;
            
            AddLine(line);
            return line;
        }

        public void AddLine(Line line)
        {
            Add(line);
            line.ZIndex = -1;
            line.MarkDirtyRepaint();
        }

        public void AddNodeMenu<T>(string path, Action<T> onCreate, Action<T> onClick) where T : BetterVisualElement, IGraphNode, new()
        {
            AddContextMenu(path, (e) =>
            {
                var node = new T();
                node.Position = e.Position + new Vector2(-node.Size.x / 2, node.Size.y / 2) + new Vector2(0, -100);
                AddNode(node, () => onClick?.Invoke(node));
                onCreate?.Invoke(node);
            });
        }

        public virtual void Save()
        {
            
        }

        protected virtual void AddButtons(TopMenu menu)
        {
            
        }
        
        protected virtual void AddLink(TwoElementsChooserManipulatorEvent e)
        {
            var from = e.FirstElement as IGraphNode;
            var to = e.SecondElement as IGraphNode;
            
            AddLine(from, to);
        }

        protected virtual void RemoveLink(Line line)
        {
            if (!EnableContextMenu)
            {
                return;
            }
            Remove(line);
        }

        protected virtual void EditClicked()
        {
            EnableContextMenu = !EnableContextMenu;
            foreach (var dragManipulator in DragManipulators)
            {
                dragManipulator.Enabled = EnableContextMenu;
            }

            foreach (var element in Nodes.Select(x => x as BetterVisualElement))
            {
                element.EnableContextMenu = !EnableContextMenu;
            }

            _elementsChooserManipulator.Enabled = EnableContextMenu;
        }
        
        private void SetButtons()
        {
            _menu.AddButton("Edit mode", EditClicked, true);
            
            AddButtons(_menu);
        }
    }
}
