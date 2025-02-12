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
        private Dictionary<string, List<Action<IGraphNode>>> _pathToAction = new();
        private List<Line> _lines = new();
        private TopMenu _menu = new();
        
        private IGraphNode _startLinkNode;
        
        public Graph()
        {
            hierarchy.Add(_menu);
            SetButtons();
            EnableContextMenu = false;
        }

        public void RegisterMenuOpened(string path, Action<IGraphNode> action)
        {
            if (!_pathToAction.ContainsKey(path))
            {
                _pathToAction.Add(path, new List<Action<IGraphNode>>());
            }
            _pathToAction[path].Add(action);
        }

        public virtual void AddNode<T>(T node) where T : ExtendedVisualElement, IGraphNode
        {
            _idToNode.Add(node.Id, node);
            node.name = node.GetType().ToString() + _idToNode.Count.ToString();
            
            Add(node);
            
            var manipulator = GetDragManipulator(node);
            manipulator.DragEnded += e => Save();
            manipulator.Enabled = IsEditMode;

            node.EnableContextMenu = true;
            
            node.AddContextMenu("Remove", (e) =>
            {
                RemoveNode(node);
                Save();
            }, () => EnableContextMenu);
            
            node.AddContextMenu("Start Link", e =>
            {
                _startLinkNode = node;
            }, () => EnableContextMenu && _startLinkNode is null);
            
            node.AddContextMenu("Link", e =>
            {
                AddLine(_startLinkNode, node);
                _startLinkNode = null;
                Save();
            }, () => EnableContextMenu && _startLinkNode is not null && _startLinkNode != node);
            
            OnNodeAdded(node);
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
            
            foreach (var line in _lines.ToList())
            {
                if (Nodes.Any(x =>
                        line.StartElement == x
                        || line.EndElement == x))
                {
                    RemoveLine(line);
                }
            }

            if (node is ExtendedVisualElement e)
            {
                e.GetManipulator<ContextMenuManipulator>().Remove("Remove");
                e.GetManipulator<ContextMenuManipulator>().Remove("Start Link");
                e.GetManipulator<ContextMenuManipulator>().Remove("Link");
            }
            
            OnNodeRemoved(node);
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
            
            var drag = line.GetManipulator<DragManipulator>();
            if (drag == null) return line;
            
            drag.target = null;
            DragManipulators.Remove(drag);
            line.RemoveManipulator(drag);
            return line;
        }

        public void AddLine(Line line)
        {
            Add(line);
            line.ZIndex = -1;
            line.MarkDirtyRepaint();
            _lines.Add(line);
            
            line.EnableContextMenu = true;
            line.AddContextMenu("Remove", (e) =>
            {
                RemoveLine(line);
                Save();
            }, () => EnableContextMenu);
            
            OnLineAdded(line);
        }
        
        protected virtual void RemoveLine(Line line)
        {
            _lines.Remove(line);
            Remove(line);
            
            line.GetManipulator<ContextMenuManipulator>().Remove("Remove");
            OnLineRemoved(line);
        }

        protected void AddNodeMenu<T>(string path) where T : ExtendedVisualElement, IGraphNode, new()
        {
            AddContextMenu(path, (e) =>
            {
                var node = new T();
                node.Position = this.WorldToLocal(e.Position) + new Vector2(-node.Size.x / 2, node.Size.y / 2) + new Vector2(0, -100);
                if (_pathToAction.TryGetValue(path, out var actions))
                {
                    for (int i = 0; i < actions.Count; i++)
                    {
                        actions[i](node);
                    }
                    
                }
                AddNode(node);
                Save();
            });
        }

        protected void AddTopMenuButton(string text, Action onClick, bool toggle = false)
        {
            _menu.AddButton(text, onClick, toggle);
        }

        public virtual void Save()
        {
            
        }

        public virtual void Load()
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

        protected virtual void OnNodeRemoved(IGraphNode node)
        {
            
        }

        protected virtual void OnLineAdded(Line line)
        {
            
        }
        
        protected virtual void OnLineRemoved(Line line)
        {
            
        }
        
        private void SetButtons()
        {
            AddTopMenuButton("Edit mode", EditClicked, true);
        }
    }
}
