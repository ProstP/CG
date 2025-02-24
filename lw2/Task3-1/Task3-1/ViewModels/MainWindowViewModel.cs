using System;
using Task3_1.Model;

namespace Task3_1.ViewModels
{
    public class MainWindowViewModel
    {
        public delegate void CreateElementEventHandler(string id, string type, double left, double top);
        public delegate void MoveElementEventHandler(string id, double left, double top);
        public delegate void RemoveElementEventHandler(string id);
        public delegate void OpenInStoreEventHandler(string type);

        public event CreateElementEventHandler OnCreateElement;
        public event MoveElementEventHandler OnMoveElement;
        public event RemoveElementEventHandler OnRemoveElement;
        public event OpenInStoreEventHandler OnOpenInStore;

        readonly ElementsStore _elementsStore;

        // IView
        public MainWindowViewModel(MainWindow mainWindow)
        {
            mainWindow.OnCreateElement += OnViewCreateElt;
            mainWindow.OnMoveElement += OnViewMoveElt;
            mainWindow.OnRemoveElement += OnViewRemoveElt;

            _elementsStore = new ElementsStore(mainWindow.Width, mainWindow.CanvasHeight,
                mainWindow.ImageWidth, mainWindow.ImageHeight);

            _elementsStore.OnCreateElement += OnModelCreateElt;
            _elementsStore.OnMoveElement += OnModelMoveElt;
            _elementsStore.OnRemoveElement += OnModelRemoveElt;
            _elementsStore.OnElementOpened += OnModelOpenedElt;
        }

        public void Init()
        {
            foreach (ElementType item in _elementsStore.OpenedElts)
            {
                OnOpenInStore.Invoke(item.ToString());
            }

            foreach (Element item in _elementsStore.Elements)
            {
                OnCreateElement(item.Id, item.Type.ToString(), item.Left, item.Top);
            }
        }

        private void OnViewCreateElt(string type)
        {
            if (!Enum.TryParse(type, true, out ElementType result))
            {
                throw new ArgumentException("Unknown type of element");
            }

            _elementsStore.AddElement(result);
        }
        private void OnViewMoveElt(string id, double newLeft, double newTop)
        {
            _elementsStore.SetNewPosition(id, newLeft, newTop);
        }
        private void OnViewRemoveElt(string id)
        {
            _elementsStore.Remove(id);
        }

        private void OnModelCreateElt(string id, ElementType type, double left, double top)
        {
            OnCreateElement.Invoke(id, type.ToString(), left, top);
        }
        private void OnModelMoveElt(string id, double left, double top)
        {
            OnMoveElement.Invoke(id, left, top);
        }
        private void OnModelRemoveElt(string id)
        {
            OnRemoveElement.Invoke(id);
        }
        private void OnModelOpenedElt(string type)
        {
            OnOpenInStore.Invoke(type);
        }
    }
}
