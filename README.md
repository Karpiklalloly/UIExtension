# Karpik.UIExtension
Библиотека предназначена для того, чтобы немного упростить жизнь при работе с UIToolkit от Unity.<br>
Всё, что добавелно в этой библиотеке, нацелено на работу как в редакторе так и в игре.

## Установка
### Ручная
Скачайте репозиторий и поместите его в проект.
### С помощью Package Manager
[Установить библиотеку через Package Manager.](https://docs.unity3d.com/Manual/upm-ui-giturl.html) <br>
Git URL: https://github.com/Karpiklalloly/UIExtension.git

# Что добавляет
### [ExtendedVisualElement](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/ExtendedVisualElement.cs)
Расширяет возможности стандартного VisualElement.<br>
Основные возможности:
- ZIndex - позволяет задать порядок отображения элемента. Является простой оболчкой над VisualElement.Sort.
- События на добавление/удаления ребенка.
- Встроенное контекстное меню.
- Хранение всех манипуляторов, которые были добавлены на элемент. Как следствие - возможность получать эти манипуляторы через метод GetManipulator<T>().
- Номинальная автоочистка через OnDispose() в потомках.
По-факту, нет 100% рабочей возможности отследить, находится элемент в UI или нет.
Потому вызывайте Dispose() вручную, если это необходимо.

## Элементы
### [Line](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Elements/Line.cs)
Позволяет добавлять линию в UI. Линия является полноценным визуальным элементом.

### [Miniature](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Elements/Miniature.cs)
Позволяет добавлять картинку в UI. Является простой оболочкой над Image.

### [GraphNode](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Elements/GraphNode.cs)
Является базовым классом для создания узлов графа в Graph. Сам по себе является Miniature с некоторой дополнительной информацией.

### [TopMenu](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Elements/TopMenu.cs)
Добавляет простую панель вверх родительского элемента.

### [Canvas](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Layouts/Canvas.cs)
Является простым способом накидать элементы по заданным координатам.
В качестве дополнения можно перемещать детей мышью.

### [Graph](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Layouts/Graph.cs)
Является готовоым элементом для создания графа.

### [Grid](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Layouts/Grid.cs)
При добавлении элементов в Grid, они будут додбавляться по сетке
(можно задать количество элементов в строке и высоту строки).

### [FixedGrid](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Layouts/FixedGrid.cs)
Является аналогом Grid. Можно задать размер сетки и makeItem по индексам сетки (x, y).
В общем случае, после задания размеров в сетке будут пустые визуальные элементы.

### [ModalWindow](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Modal/ModalWindow.cs)
Является готовоым всплывающим окном.

## Манипуляторы
### [ChildElementMoverManipulator](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Manipulators/ChildElementMoverManipulator.cs)
Двигает детей по нажатию заданой кнопки мыши.

### [ContextMenuManipulator](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Manipulators/ContextMenuManipulator.cs)
Добавляет возможность добавлять контекстное меню.

### [DragManipulator](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Manipulators/DragManipulator.cs)
Добавляет возможность перемещать элементы по экрану по нажатию ЛКМ.

### [MouseEventsManipulator](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Manipulators/MouseEventsManipulator.cs)
Позволяет легко подписываться на события мыши.

### [SlottableDragAndDropManipulator](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Manipulators/SlottableDragAndDropManipulator.cs)
Позволяет перемещать элемент по слотам.

### [TooltipManipulator](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Manipulators/TooltipManipulator.cs)
Добавляет Tooltip по наведению выши на элемент.

## Механики
### Modal
Позволяет задавать контексты для всплывающих окон. За первым окном добавляет кликабельный задний фон.
В большинстве случаев достаточно задать один контекст, на уровне которого будут показывать окна (например, Graph).

### Painter
Является простой оболочкой над Painter2D.

### [Binding](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Utilities/Utils.cs)
Добавляет метод расширения для упрощенного биндинга визуального элемента с данными.

### Загрузка
В ходе своих разработок мне нужно было сохранять и загружать изображения. Данный модуль является простым сервис-локатором
для загрузки изображений по ключу.
Также есть возможность подгружать PlaceHolder.

### Утилиты
#### [Методоты расширения VisualElement и ExtendedVisualElement](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Utilities/DefaultMethodsExtensions.cs)
Для унификации работы как с VisualElement так и ExtendedVisualElement добавлены методы расширения

#### [Методоты расширения PropertyPath](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Utilities/PropertyPathExtensions.cs)
Странно, что изначально нет подобной шняги. Просто немного упрощает синтаксис.

#### [Селекторы](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Utilities/Selectors.cs)
Позволяют легко задать стили.

#### [Сериализуемый словарь](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Utilities/SerializableDictionary.cs)

#### [Методы расширения стилей](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Utilities/StyleExtensions.cs)
Добавляет типовые методы расширения для задания стилей (например, Margin, Padding).

#### [Методы расширения для VisualElement](https://github.com/Karpiklalloly/UIExtension/blob/main/Karpik/UIExtension/Utilities/VisualElementExtensions.cs)
Добавляет то, чего мне не хватало.