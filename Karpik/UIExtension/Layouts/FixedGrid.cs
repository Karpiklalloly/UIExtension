using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    [UxmlElement]
    public partial class FixedGrid : ExtendedVisualElement
    {
        public Vector2Int Size
        {
            get => new Vector2Int(ColumnsCount, RowsCount);
            set
            {
                RowsCount = value.y;
                ColumnsCount = value.x;
            }
        }
        
        [UxmlAttribute][Min(1)]
        public int ColumnsCount
        {
            get => _columns;
            set
            {
                if (value < 0)
                {
                    throw new Exception("Invalid number of columns");
                }
                
                _columns = value;
                
                for (int i = 0; i < _grid.Length; i++)
                {
                    Array.Resize(ref _grid[i], value);
                }

                foreach (var container in _containers)
                {
                    while (container.childCount > value)
                    {
                        container.RemoveAt(container.childCount - 1);
                    }
                }
                
                if (AutoUpdate)
                    UpdateLayout();
            }
        }
    
        [UxmlAttribute][Min(1)]
        public int RowsCount
        {
            get => _rows;
            set
            {
                if (value < 0)
                {
                    throw new Exception("Invalid number of rows");
                }

                _rows = value;

                Array.Resize(ref _grid, value);
                for (int i = 0; i < _grid.Length; i++)
                {
                    if (_grid[i] == null) _grid[i] = new VisualElement[ColumnsCount];
                }

                Array.Resize(ref _containers, value);
                for (int i = 0; i < _containers.Length; i++)
                {
                    if (_containers[i] == null)
                    {
                        _containers[i] = new VisualElement();
                        _containers[i].ToContainer(Selectors.Direction.Horizontal);
                    }
                }
                
                if (AutoUpdate)
                    UpdateLayout();
            }
        }
        
        [UxmlAttribute][Min(1)]
        public int MaxRows
        {
            get => _maxRows;
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Invalid number of max rows");
                }

                _maxRows = value;
                
                if (AutoUpdate)
                    UpdateLayout();
            }
        }
        
        [UxmlAttribute][Min(1)]
        public int MaxColumns
        {
            get => _maxColumns;
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Invalid number of max columns");
                }
                
                _maxColumns = value;
                
                if (AutoUpdate)
                    UpdateLayout();
            }
        }

        [UxmlAttribute]
        public float CellMargin { get; set; } = 4f;
        
        public bool AutoUpdate { get; set; } = true;

        public Func<int, int, VisualElement> MakeItem
        {
            set
            {
                _makeItem = value;
                if (AutoUpdate)
                    UpdateLayout();
            }
        }

        public override VisualElement contentContainer => _container;

        private ScrollView _container = new ScrollView();

        private VisualElement[] _containers = { null };
        private VisualElement[][] _grid = { new VisualElement[1] { null } };
        private Func<int, int, VisualElement> _makeItem;

        private float _sizeX;
        private float _sizeY;
        private float _spaceX;
        private float _spaceY;

        private int _rows = 1;
        private int _maxRows = 1;
        private int _columns = 1;
        private int _maxColumns = 1;

        public FixedGrid()
        {
            _container.StretchToParentSize();
            _container.name = "Container";
            _container.contentContainer.StretchToParentSize();
            _container.contentContainer.style.flexWrap = new StyleEnum<Wrap>(Wrap.NoWrap);
            _container.verticalScrollerVisibility = ScrollerVisibility.Auto;
            _container.horizontalScrollerVisibility = ScrollerVisibility.Auto;
            hierarchy.Add(_container);
        }

        public void UpdateLayout()
        {
            _sizeY = 100f / (_maxRows >= _rows
                ? _rows
                : _maxRows);
            
            _sizeX = 100f / (_maxColumns >= _columns
                ? _columns
                : _maxColumns);
            
            for (int i = 0; i < _grid.Length; i++)
            {
                for (int j = 0; j < _grid[i].Length; j++)
                {
                    int x = j;
                    int y = i;
                    var element = _makeItem?.Invoke(x, y);
                    if (element == null) element = new VisualElement();
                    _grid[i][j] = element;
                }
            }

            if (_rows > 0 && _columns > 0)
            {
                Display();
                this.ForceUpdate();
                _container.ForceUpdate();
                _container.horizontalScroller.ForceUpdate();
                _container.verticalScroller.ForceUpdate();
            }
        }

        private void Display()
        {
            Clear();
            
            foreach (var container in _containers)
            {
                Add(container);
                container.Clear();
                container.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
                container.style.height = new StyleLength(new Length(_sizeY, LengthUnit.Percent));
                container.style.minHeight = new StyleLength(new Length(_sizeY, LengthUnit.Percent));
                container.style.justifyContent = new StyleEnum<Justify>(Justify.FlexStart);
                container.style.alignItems = new StyleEnum<Align>(Align.Stretch);
                container.FlexSize(new StyleLength(new Length(_sizeY, LengthUnit.Percent)));
            }
            
            for (int i = 0; i < _rows; i++)
            {
                var container = _containers[i];
                container.name = i.ToString();
                foreach (var element in _grid[i])
                {
                    var mainContainer = new VisualElement();
                    mainContainer.style.width = new StyleLength(new Length(_sizeX, LengthUnit.Percent));
                    mainContainer.style.minWidth = new StyleLength(new Length(_sizeX, LengthUnit.Percent));
                    mainContainer.style.alignSelf = new StyleEnum<Align>(Align.Stretch);
                    mainContainer.style.justifyContent = new StyleEnum<Justify>(Justify.Center);

                    var subContainer = new VisualElement();
                    subContainer.StretchToParentSize();
                    subContainer.style.alignSelf = new StyleEnum<Align>(Align.Stretch);
                    subContainer.style.justifyContent = new StyleEnum<Justify>(Justify.Center);
                    subContainer.style.Margin(CellMargin, LengthUnit.Pixel);

                    element.StretchToParentSize();
                    container.Add(mainContainer);
                    mainContainer.Add(subContainer);
                    subContainer.Add(element);
                }
            }
        }
    }
}
