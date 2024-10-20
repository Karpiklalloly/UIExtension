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
        public sealed override VisualElement contentContainer => base.contentContainer;

        public IReadOnlyList<IGraphNode> Nodes => _idToNode.Values.ToList();
        public IReadOnlyList<Line> Lines => _lines;
        public bool IsEditMode => EnableContextMenu;
        
        private Dictionary<string, IGraphNode> _idToNode = new();
        private List<Line> _lines = new();
        private TopMenu _menu = new();
        
        public Graph()
        {
            hierarchy.Add(_menu);
            SetButtons();
            EnableContextMenu = false;
        }

        public virtual void AddNode<T>(T node) where T : ExtendedVisualElement, IGraphNode
        {
            _idToNode.Add(node.Id, node);
            node.name = node.GetType().ToString() + _idToNode.Count.ToString();
            
            Add(node);
            OnNodeAdded(node);
            
            var manipulator = GetDragManipulator(node);
            manipulator.DragEnded += e => Save();
            manipulator.Enabled = IsEditMode;
        }
        
        public void RemoveNode(string id)
        {
            if (Nodes.FirstOrDefault(x => x.Id == id) is not VisualElement node) return;
            RemoveNode(node as IGraphNode);
        }

        public virtual void RemoveNode(IGraphNode node)
        {
            _idToNode.Remove(node.Id);
            Remove(node as VisualElement);
        }
        
        public new void Clear()
        {
            var lines = _lines;
            var nodes = _idToNode.Values;
            
            while (lines.Count > 0)
            {
                RemoveLine(lines[0]);
            }

            while (nodes.Count > 0)
            {
                RemoveNode(nodes.First());
            }
            
            base.Clear();
            _idToNode.Clear();
        }

        public Line AddLine<T>(T from, T to) where T : IPositionNotify
        {
            var children = Children();
            var visualElements = children as VisualElement[] ?? children.ToArray();
            if (!visualElements.Contains(from as VisualElement) || !visualElements.Contains(to as VisualElement))
            {
                throw new ArgumentException($"From element or To element is not children of Canvas {this}");
            }
            
            var line = new Line();
            
            line.SetStart(from, from.Center - from.value);
            line.SetEnd(to, from.Center - from.value);
            line.StartColor = Color.green;
            line.EndColor = Color.red;
            
            AddLine(line);
            return line;
        }

        public void AddLine(Line line)
        {
            Add(line);
            line.ZIndex = -1;
            line.MarkDirtyRepaint();
            _lines.Add(line);
            OnLineAdded(line);
        }
        
        protected virtual void RemoveLine(Line line)
        {
            _lines.Remove(line);
            Remove(line);
        }

        public virtual void AddNodeMenu<T>(string path, Action<T> onCreate, Action<T> onClick) where T : ExtendedVisualElement, IGraphNode, new()
        {
            AddContextMenu(path, (e) =>
            {
                var node = new T();
                node.Position = e.Position + new Vector2(-node.Size.x / 2, node.Size.y / 2) + new Vector2(0, -100);
                onCreate?.Invoke(node);
                AddNode(node);
                Save();
            });
        }

        public virtual void Save()
        {
            
        }

        public virtual void Load()
        {
            
        }

        protected virtual void AddButtons(TopMenu menu)
        {
            
        }

        protected virtual void EditClicked()
        {
            EnableContextMenu = !EnableContextMenu;
            foreach (var dragManipulator in DragManipulators)
            {
                dragManipulator.Enabled = IsEditMode;
            }
        }

        protected virtual void OnNodeAdded<T>(T node) where T : VisualElement, IGraphNode
        {
            
        }

        protected virtual void OnLineAdded(Line line)
        {
            
        }
        
        private void SetButtons()
        {
            _menu.AddButton("Edit mode", EditClicked, true);
            
            AddButtons(_menu);
        }
    }
}
