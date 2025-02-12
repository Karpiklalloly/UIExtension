using System;
using Karpik.UIExtension;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Grid = Karpik.UIExtension.Grid;
using Random = UnityEngine.Random;

public class Demo : EditorWindow
{
    [SerializeField]
    public StyleSheet StyleSheet;
    
    private VisualElement _root;
    private TabView _tabs;
    
    [MenuItem("Tools/UI Extension/Demo")]
    public static void ShowExample()
    {
        Demo wnd = GetWindow<Demo>();
        wnd.titleContent = new GUIContent("UI Extension Demo");
    }

    public void CreateGUI()
    {
        _root = new VisualElement();
        rootVisualElement.styleSheets.Add(StyleSheet);
        rootVisualElement.Add(_root);
        _tabs = new();
        _tabs.reorderable = false;
        _root.Add(_tabs);

        AddFixedGrid();
        AddGrid();
        AddStack();
        AddFlexGrid();
        AddDnD();
        AddGraph();
    }

    private Tab AddTab(string name)
    {
        var tab = new Tab(name);
        _tabs.Add(tab);
        return tab;
    }

    private void AddFixedGrid()
    {
        var tab = AddTab("Fixed Grid");
        var grid = new FixedGrid();
        int x = 2;
        int y = 2;
        int maxX = 4;
        int maxY = 4;
        
        Stack stack = new Stack();
        stack.Direction = Selectors.Direction.Horizontal;
        stack.name = "Horizontal stack";
        tab.Add(stack);
        {
            Stack verticalStack = new Stack();
            verticalStack.Direction = Selectors.Direction.Vertical;
            verticalStack.style.minWidth = 300;
            verticalStack.name = "Vertical stack";
            stack.Add(verticalStack);
            {
                AddSlider(verticalStack, "Columns", e =>
                {
                    grid.ColumnsCount = e.newValue;
                    x = e.newValue;
                }, x).style.minWidth = 300;
        
                AddSlider(verticalStack, "Rows", e =>
                {
                    grid.RowsCount = e.newValue;
                    y = e.newValue;
                }, y).style.minWidth = 300;
        
                AddSlider(verticalStack, "MaxColumns", e =>
                {
                    grid.MaxColumns = e.newValue;
                    maxX = e.newValue;
                }, maxX).style.minWidth = 300;
        
                AddSlider(verticalStack, "MaxRows", e =>
                {
                    grid.MaxRows = e.newValue;
                    maxY = e.newValue;
                }, maxY).style.minWidth = 300;
            }
        }
        {
            stack.Add(grid);
            grid.style.backgroundColor = Color.cyan;
            grid.style.minHeight = 400;
            grid.style.minWidth = 400;
            grid.AutoUpdate = false;
            grid.RowsCount = y;
            grid.ColumnsCount = x;
            grid.MaxRows = maxY;
            grid.MaxColumns = maxY;
            grid.delegatesFocus = true;
            
            grid.EnableContextMenu = true;
            grid.AutoUpdate = true;
            
            grid.MakeItem = (x, y) =>
            {
                var element = new ExtendedVisualElement
                {
                    style =
                    {
                        backgroundColor = Color.red
                    },
                    EnableContextMenu = true,
                    focusable = true
                };

                var label = new Label
                {
                    text = $"{x + 1}:{y + 1}",
                    style =
                    {
                        fontSize = 50
                    }
                };
                label.ToCenter();
                element.Add(label);
            
                element.AddManipulator(new TooltipManipulator(
                    () => rootVisualElement,
                    () => label.text,
                    () => (x + y + 2).ToString(),
                    TooltipManipulator.Mode.FollowCursor));
            
                return element;
            };
            grid.UpdateLayout();
        }
        
    }

    private void AddGrid()
    {
        var tab = AddTab("Grid");
        var grid = new Grid();
        int margin = 10;
        int lineHeight = 50;
        int countPerLine = 2;
        
        Stack stack = new Stack();
        stack.Direction = Selectors.Direction.Horizontal;
        stack.name = "Horizontal stack";
        tab.Add(stack);
        {
            Stack verticalStack = new Stack();
            verticalStack.Direction = Selectors.Direction.Vertical;
            verticalStack.style.minWidth = 300;
            stack.name = "Vertical stack";
            stack.Add(verticalStack);
            {
                AddSlider(verticalStack, "Margin", e =>
                { 
                    grid.Margin = margin = e.newValue;
                }, margin, 100).style.minWidth = 300;
        
                AddSlider(verticalStack, "Line Height", e =>
                {
                    grid.LineHeight = lineHeight = e.newValue;
                }, lineHeight, 100).style.minWidth = 300;
        
                AddSlider(verticalStack, "Count Per Line", e =>
                {
                    grid.CountPerLine = countPerLine = e.newValue;
                }, countPerLine, 4).style.minWidth = 300;
                
                verticalStack.Add(new Button(() => grid.Add(new VisualElement()
                {
                    style =
                    {
                        backgroundColor = new StyleColor(new Color(Random.value, Random.value, Random.value, 1))
                    }
                }))
                {
                    text = "Add Element",
                });
            }
        }
        {
            grid.Margin = margin;
            grid.LineHeight = lineHeight;
            grid.CountPerLine = countPerLine;
        
            grid.style.backgroundColor = Color.cyan;
            grid.style.minHeight = 400;
            grid.style.minWidth = 400;
            grid.style.maxHeight = 400;
            grid.style.maxWidth = 400;
            stack.Add(grid);
        }
    }

    private void AddStack()
    {
        var tab = AddTab("Stack");
        Stack verticalStack = new Stack();
        verticalStack.Direction = Selectors.Direction.Vertical;
        tab.Add(verticalStack);
        {
            Stack stack = new Stack();
            stack.Direction = Selectors.Direction.Horizontal;
            stack.name = "Horizontal stack";
            verticalStack.Add(stack);
            {
                stack.Add(new Button()
                {
                    text = "Button"
                });
                stack.Add(new Button()
                {
                    text = "Button"
                });
                stack.Add(new Button()
                {
                    text = "Button"
                });
                stack.Add(new Button()
                {
                    text = "Button"
                });
            }
        }
        {
            Stack stack = new Stack();
            stack.Direction = Selectors.Direction.Horizontal;
            stack.name = "Horizontal stack";
            verticalStack.Add(stack);
            {
                stack.Add(new Toggle("Toggle"));
                stack.Add(new Toggle("Toggle"));
                stack.Add(new Toggle("Toggle"));
                stack.Add(new Toggle("Toggle"));
                stack.Add(new Toggle("Toggle"));
            }
        }
        {
            Stack stack = new Stack();
            stack.Direction = Selectors.Direction.Horizontal;
            stack.name = "Horizontal stack";
            verticalStack.Add(stack);
            {
                AddSlider(stack, "Slider", null, 0, 100);
                AddSlider(stack, "Slider", null, 0, 100);
            }
        }
    }

    private void AddFlexGrid()
    {
        var tab = AddTab("FlexGrid");
        var grid = new FlexGrid();
        int lineHeight = 50;
        int countPerLine = 2;
        
        Stack stack = new Stack();
        stack.Direction = Selectors.Direction.Horizontal;
        stack.name = "Horizontal stack";
        tab.Add(stack);
        {
            Stack verticalStack = new Stack();
            verticalStack.Direction = Selectors.Direction.Vertical;
            verticalStack.style.minWidth = 300;
            stack.name = "Vertical stack";
            stack.Add(verticalStack);
            {
                AddSlider(verticalStack, "Line Height", e =>
                {
                    grid.LineHeight = lineHeight = e.newValue;
                }, lineHeight, 100).style.minWidth = 300;
        
                AddSlider(verticalStack, "Count Per Line", e =>
                {
                    grid.CountPerLine = countPerLine = e.newValue;
                }, countPerLine, 4).style.minWidth = 300;
                
                verticalStack.Add(new Button(() => grid.AddChild(new Button()
                {
                    text = "Button"
                }))
                {
                    text = "Add Element",
                });
            }
        }
        {
            grid.LineHeight = lineHeight;
            grid.CountPerLine = countPerLine;
        
            grid.style.backgroundColor = Color.cyan;
            grid.style.minHeight = 400;
            grid.style.minWidth = 400;
            grid.style.maxHeight = 400;
            grid.style.maxWidth = 400;
            stack.Add(grid);
        }
    }

    private void AddDnD()
    {
        var tab = AddTab("DnD");
        
        FixedGrid grid = new FixedGrid();
        tab.Add(grid);
        grid.style.minHeight = 400;
        grid.style.minWidth = 400;
        grid.StretchToParentSize();
        
        var was = grid.AutoUpdate;
        grid.AutoUpdate = false;
        grid.CellMargin = 3f;
        grid.Size = new Vector2Int(5, 5);
        grid.RowsCount = 5;
        grid.MaxRows = 5;
        grid.ColumnsCount = 5;
        grid.MaxColumns = 5;
        grid.MakeItem = (x, y) =>
        {
            VisualElement element = new VisualElement();
            element.style.backgroundColor = new Color(1, 1, 1, 1);
            if (y > 1) element.style.backgroundColor = Color.red;
            else element.ToSlot("DemoSlot");
            
            if (x == 0 && y == 0)
            {
                var child = new Image();
                child.image = PlaceHolder.Texture;
                child.StretchToParentSize();
                child.ToSlottableItem("DemoSlot",
                    rootVisualElement,
                    (slot, item) =>
                    {
                        slot.Add(item);
                        item.StretchToParentSize();
                        item.ToCenter();
                    });
                element.Add(child);
            }
            return element;
        };
        grid.AutoUpdate = was;
        grid.UpdateLayout();
    }

    private void AddGraph()
    {
        var tab = AddTab("Graph");
        
        Graph graph = new DemoGraph();
        graph.style.minWidth = 500;
        graph.style.minHeight = 500;
        tab.Add(graph);
    }
    
    private SliderInt AddSlider(VisualElement parent, string label, EventCallback<ChangeEvent<int>> action, int value, int maxValue = 16)
    {
        var slider = new SliderInt
        {
            label = label,
            value = value,
            lowValue = 1,
            highValue = maxValue,
            showInputField = true
        };
        if (action != null) slider.RegisterValueChangedCallback(action);
        slider.ManipulatorAdd(new TooltipManipulator(
            () => rootVisualElement,
            (() => $"{slider.value}/{maxValue}"),
            () => String.Empty)
        {
            target = null,
            FollowMode = TooltipManipulator.Mode.FollowCursor,
            AdditionalOffset = () => new Vector2(11, 11)
        });
        
        parent.AddChild(slider);

        return slider;
    }
    
    public class DemoGraph : Graph
    {
        public DemoGraph()
        {
            AddNodeMenu<GraphNode>("New Node");
            AddTopMenuButton("Demo", () => Debug.Log("Demo"), false);
        }
    }
}
