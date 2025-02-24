using System;
using System.Collections.Generic;
using System.Linq;

namespace Task3_1.Model
{
    public class ElementsStore
    {
        public delegate void CreateElementEventHanlder(string id, ElementType type, double left, double top);
        public delegate void MoveElementEventHanlder(string id, double left, double top);
        public delegate void RemoveElementEventHanlder(string id);
        public delegate void OpenedElementEventHanlder(string type);

        public event CreateElementEventHanlder OnCreateElement;
        public event MoveElementEventHanlder OnMoveElement;
        public event RemoveElementEventHanlder OnRemoveElement;
        public event OpenedElementEventHanlder OnElementOpened;

        readonly ElementsCreator _elementsCreator = new ElementsCreator();
        readonly List<Element> _elements = new List<Element>();
        readonly List<ElementType> _openedElts = new List<ElementType>()
        {
            ElementType.Water,
            ElementType.Fire,
            ElementType.Air,
            ElementType.Earth
        };
        int _count;

        public IReadOnlyList<ElementType> OpenedElts
        {
            get => _openedElts;
        }
        public Element this[string id] => GetElement(id);
        public IReadOnlyList<Element> Elements => _elements;

        public double Width { get; private set; }
        public double Height { get; private set; }
        public double ElementWidth { get; private set; }
        public double ElementHeight { get; private set; }
        public int Count
        {
            get => _elements.Count;
        }

        public ElementsStore(double width, double height, double elementWidth, double elementHeight)
        {
            Width = width;
            Height = height;
            ElementWidth = elementWidth;
            ElementHeight = elementHeight;
            _count = 0;

            double centerX = (Width - elementWidth) / 2;
            double centerY = (Height - elementHeight) / 2;
            _elements.Add(new Element((_count++).ToString(), ElementType.Fire,
                centerX - elementWidth, centerY, elementWidth, elementHeight));
            _elements.Add(new Element((_count++).ToString(), ElementType.Water,
                centerX + elementWidth, centerY, elementWidth, elementHeight));
            _elements.Add(new Element((_count++).ToString(), ElementType.Earth,
                centerX, centerY - elementHeight, elementWidth, elementHeight));
            _elements.Add(new Element((_count++).ToString(), ElementType.Air,
                centerX, centerY + elementHeight, elementWidth, elementHeight));
        }

        public void AddElement(ElementType type)
        {
            if (!_openedElts.Contains(type))
            {
                throw new ArgumentException("This type not created");
            }

            double left = (Width - ElementWidth) / 2;
            double top = (Height - ElementHeight) / 2;
            Element element = new Element((_count++).ToString(), type, left, top, ElementWidth, ElementHeight);
            _elements.Add(element);

            OnCreateElement.Invoke(_elements.Last().Id, type, left, top);
        }
        public void SetNewPosition(string id, double x, double y)
        {
            Element element = GetElement(id);

            if (x < 0 || y < 0 || x + ElementWidth > Width || y + ElementHeight > Height)
            {
                throw new ArgumentException("Element position is out of canvas");
            }

            element.Left = x;
            element.Top = y;

            OnMoveElement.Invoke(id, x, y);

            CheckAndCombineElements(element);
        }

        public void Remove(string id)
        {
            if (TryGetElement(id, out Element element))
            {
                _elements.Remove(element);

                OnRemoveElement.Invoke(id);
            }
        }

        Element GetElement(string id)
        {
            if (!TryGetElement(id, out var element))
            {
                throw new ArgumentException("Unknown id");
            }

            return element;
        }
        bool TryGetElement(string id, out Element element)
        {
            element = _elements.FirstOrDefault(e => e.Id == id);

            if (element == null)
            {
                return false;
            }

            return true;
        }

        private void CheckAndCombineElements(Element element)
        {
            foreach (Element item in _elements)
            {
                if (item.Id == element.Id)
                {
                    continue;
                }

                if (
                    CheckIsPointInArea(item.Left, item.Top,
                        element.Left, element.Left + element.Width, element.Top, element.Top + element.Height)
                    || CheckIsPointInArea(item.Left + item.Width, item.Top,
                        element.Left, element.Left + element.Width, element.Top, element.Top + element.Height)
                    || CheckIsPointInArea(item.Left, item.Top + item.Height,
                        element.Left, element.Left + element.Width, element.Top, element.Top + element.Height)
                    || CheckIsPointInArea(item.Left + item.Width, item.Top + item.Height,
                        element.Left, element.Left + element.Width, element.Top, element.Top + element.Height)
                    )
                {
                    List<ElementType> newElts = _elementsCreator.CreateNewElement(element.Type, item.Type);

                    if (newElts != null && newElts.Count > 0)
                    {
                        CreateElement(element, item, newElts);
                        return;
                    }
                }
            }
        }
        private void CreateElement(Element first, Element second, List<ElementType> typesOfNew)
        {
            double stepX = (first.Left - second.Left) / (typesOfNew.Count + 1);
            double stepY = (first.Top - second.Top) / (typesOfNew.Count + 1);

            double currentX = first.Left + stepX;
            double currentY = first.Top + stepY;

            List<string> toDeleteIds = new List<string>();

            for (int i = 0; i < 2; i++)
            {
                Element toDelete = i == 0 ? first : second;
                toDeleteIds.Add(toDelete.Id);

                _elements.Remove(toDelete);
            }
            // Вынести в методы
            List<ElementType> newTypes = new List<ElementType>();
            for (int i = 0; i < typesOfNew.Count; i++)
            {
                if (!_openedElts.Contains(typesOfNew[i]))
                {
                    _openedElts.Add(typesOfNew[i]);

                    newTypes.Add(typesOfNew[i]);
                }

                _elements.Add(new Element((_count++).ToString(), typesOfNew[i], currentX, currentY, ElementWidth, ElementHeight));

                currentX += stepX;
                currentY += stepY;
            }

            foreach (string id in toDeleteIds)
            {
                OnRemoveElement.Invoke(id);
            }
            foreach (ElementType type in newTypes)
            {
                OnElementOpened.Invoke(type.ToString());
            }
            for (int i = 0; i < typesOfNew.Count; i++)
            {
                Element element = _elements[_elements.Count - i - 1];
                OnCreateElement(element.Id, element.Type, element.Left, element.Top);
            }
        }
        private static bool CheckIsPointInArea(double x, double y, double left, double right, double top, double bottom)
        {
            return left <= x && x <= right && top <= y && y <= bottom;
        }
    }
}
