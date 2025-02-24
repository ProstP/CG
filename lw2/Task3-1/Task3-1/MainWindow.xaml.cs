using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Task3_1.ViewModels;

namespace Task3_1
{
    public partial class MainWindow : Window
    {
        public delegate void CreateElementEventHandler(string type);
        public delegate void MoveElementEventHandler(string id, double left, double top);
        public delegate void ElementIdEventHandler(string id);

        public event CreateElementEventHandler OnCreateElement;
        public event MoveElementEventHandler OnMoveElement;
        public event ElementIdEventHandler OnRemoveElement;

        bool _showStoreMenu = false;
        Dictionary<string, int> _zIndexOfElts = new Dictionary<string, int>();
        Image _draggingImage = null;
        Point _mouseClickPos = new Point();
        double _originalImageLeft = 0;
        double _originalImageTop = 0;

        public double CanvasWidth
        {
            get => SCREEN_WIDTH;
        }
        public double CanvasHeight
        {
            get => SCREEN_HEIGHT - Elts.Margin.Bottom;
        }
        public double ImageWidth
        {
            get => IMAGE_WIDTH;
        }
        public double ImageHeight
        {
            get => IMAGE_HEIGHT;
        }

        public const double SCREEN_WIDTH = 1000;
        public const double SCREEN_HEIGHT = 600;
        public const double IMAGE_WIDTH = 60;
        public const double IMAGE_HEIGHT = 60;


        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel viewModel = new MainWindowViewModel(this);
            viewModel.OnCreateElement += OnViewModelCreateElement;
            viewModel.OnMoveElement += OnViewModelMoveElement;
            viewModel.OnRemoveElement += OnViewModelRemoveElement;
            viewModel.OnOpenInStore += OnViewModelOpenInStore;
            DataContext = viewModel;
            InitStore();

            viewModel.Init();

            SetWindowSize();
        }

        private void OnViewModelCreateElement(string id, string type, double left, double top)
        {
            _zIndexOfElts.Add(id, _zIndexOfElts.Count);

            CreateElt(id, type, left, top);

            UpdateZIndexes();
        }
        private void OnViewModelMoveElement(string id, double left, double top)
        {
            MoveToTopElement(id);

            SetNewElementPosition(id, left, top);
        }
        private void OnViewModelRemoveElement(string id)
        {
            Image image = Elts.Children.OfType<Image>().FirstOrDefault(i => i.Name == id);
            TextBlock textBlock = Elts.Children.OfType<TextBlock>().FirstOrDefault(t => t.Name == id + "Text");

            if (textBlock != null)
            {
                Elts.Children.Remove(textBlock);
            }

            if (image is null)
            {
                return;
            }

            MoveToTopElement(id);
            _zIndexOfElts.Remove(id);
            Elts.Children.Remove(image);

            UpdateZIndexes();
        }
        private void OnViewModelOpenInStore(string type)
        {
            OpenElement(type);
        }

        private void SetWindowSize()
        {
            MaxWidth = SCREEN_WIDTH;
            MaxHeight = SCREEN_HEIGHT;

            MinWidth = SCREEN_WIDTH;
            MinHeight = SCREEN_HEIGHT;
        }
        private void BtnClick(object sender, RoutedEventArgs e)
        {
            ToggleCanvas();

            SetBtnContent();
        }
        private void SetBtnContent()
        {
            Btn.Content = _showStoreMenu ? "Нажмите чтобы закрыть" : "Нажмите чтобы посмотреть открытые элементы";
        }
        private void InitStore()
        {
            string path = GetPathByType("Empty");
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path);
            bitmapImage.EndInit();

            foreach (object item in Store.Children)
            {
                if (item is Image image)
                {
                    image.Source = bitmapImage;
                    image.MouseDown += OnStoreEltMouseDown;
                }
            }
        }
        private void ToggleCanvas()
        {
            if (_showStoreMenu)
            {
                Store.Visibility = Visibility.Hidden;
                Elts.Visibility = Visibility.Visible;
            }
            else
            {
                Store.Visibility = Visibility.Visible;
                Elts.Visibility = Visibility.Hidden;
            }

            _showStoreMenu = !_showStoreMenu;
        }
        private void CreateElt(string id, string type, double left, double top)
        {
            string path = GetPathByType(type);
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            Image image = new Image();
            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path);
            bitmap.EndInit();

            image.Source = bitmap;
            image.Name = id;
            image.Width = IMAGE_WIDTH;
            image.Height = IMAGE_WIDTH;

            image.MouseDown += OnEltsMouseDown;
            image.MouseUp += OnEltsMouseUp;
            image.MouseMove += OnEltsMouseMove;

            TextBlock textBlock = new TextBlock();
            textBlock.Name = id + "Text";
            textBlock.FontSize = 12;
            textBlock.Text = GetNameByType(type);
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.Width = image.Width;
            textBlock.Height = image.Height / 2;
            textBlock.TextWrapping = TextWrapping.Wrap;

            if (_zIndexOfElts.ContainsKey(id))
            {
                Canvas.SetZIndex(image, _zIndexOfElts[id]);
            }

            Canvas.SetLeft(image, left);
            Canvas.SetTop(image, top);
            Canvas.SetLeft(textBlock, left);
            Canvas.SetTop(textBlock, top + 60);

            Elts.Children.Add(image);
            Elts.Children.Add(textBlock);
        }
        private void OnEltsMouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Image image)
            {
                _mouseClickPos = e.GetPosition(this);

                _originalImageLeft = Canvas.GetLeft(image);
                _originalImageTop = Canvas.GetTop(image);
                _draggingImage = image;

                image.CaptureMouse();
                MoveToTopElement(image.Name);

                Btn.Visibility = Visibility.Hidden;
            }
        }
        private void OnEltsMouseMove(object sender, MouseEventArgs e)
        {
            if (_draggingImage is null)
            {
                return;
            }

            Point point = e.GetPosition(this);

            double dx = point.X - _mouseClickPos.X;
            double dy = point.Y - _mouseClickPos.Y;
            double newLeft = ClampD(_originalImageLeft + dx, 0, CanvasWidth - ImageWidth - 11);
            double newTop = ClampD(_originalImageTop + dy, 0, SCREEN_WIDTH);

            Canvas.SetLeft(_draggingImage, newLeft);
            Canvas.SetTop(_draggingImage, newTop);

            TextBlock textBlock = Elts.Children.OfType<TextBlock>().FirstOrDefault(t => t.Name == _draggingImage.Name + "Text");

            if (textBlock != null)
            {
                Canvas.SetLeft(textBlock, newLeft);
                Canvas.SetTop(textBlock, newTop + IMAGE_HEIGHT);
            }
        }
        private void OnEltsMouseUp(object sender, MouseEventArgs e)
        {
            if (_draggingImage is null)
            {
                return;
            }

            if (Canvas.GetTop(_draggingImage) + _draggingImage.Height < CanvasHeight - 10)
            {
                OnMoveElement.Invoke(_draggingImage.Name, Canvas.GetLeft(_draggingImage), Canvas.GetTop(_draggingImage));
                _draggingImage.ReleaseMouseCapture();
                _draggingImage = null;
            }
            else
            {
                string toRemoveId = _draggingImage.Name;
                _draggingImage.ReleaseMouseCapture();
                _draggingImage = null;

                OnRemoveElement.Invoke(toRemoveId);
            }

            Btn.Visibility = Visibility.Visible;
        }
        private void OnStoreEltMouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Image image)
            {
                OnCreateElement.Invoke(image.Name);

                ToggleCanvas();
            }
        }
        private void SetNewElementPosition(string id, double left, double top)
        {
            Image image = Elts.Children.OfType<Image>().FirstOrDefault(i => i.Name == id);
            TextBlock textBlock = Elts.Children.OfType<TextBlock>().FirstOrDefault(t => t.Name == id + "Text");

            if (image != null)
            {
                Canvas.SetLeft(image, left);
                Canvas.SetTop(image, top);
            }
            if (textBlock != null)
            {
                Canvas.SetLeft(textBlock, left);
                Canvas.SetTop(textBlock, top + IMAGE_HEIGHT);
            }
        }

        private void MoveToTopElement(string id)
        {
            if (!_zIndexOfElts.ContainsKey(id))
            {
                _zIndexOfElts.Add(id, _zIndexOfElts.Count);
                return;
            }
            if (_zIndexOfElts[id] >= _zIndexOfElts.Count - 1)
            {
                return;
            }
            int currentZ = _zIndexOfElts[id];
            Dictionary<string, int>.KeyCollection ids = _zIndexOfElts.Keys;
            Dictionary<string, int> zIndexes = new Dictionary<string, int>(_zIndexOfElts);
            foreach (string item in ids)
            {
                if (zIndexes[item] > currentZ)
                {
                    zIndexes[item]--;
                }
            }
            zIndexes[id] = zIndexes.Count - 1;

            _zIndexOfElts = zIndexes;

            UpdateZIndexes();
        }
        private void UpdateZIndexes()
        {
            foreach (KeyValuePair<string, int> item in _zIndexOfElts)
            {
                Image image = Elts.Children.OfType<Image>().FirstOrDefault(i => i.Name == item.Key);
                TextBlock textBlock = Elts.Children.OfType<TextBlock>().FirstOrDefault(i => i.Name == item.Key + "Text");

                if (image != null)
                {
                    Canvas.SetZIndex(image, item.Value);
                }
                if (textBlock != null)
                {
                    Canvas.SetZIndex(textBlock, item.Value);
                }
            }
        }

        private void OpenElement(string type)
        {
            Image image = Store.Children.OfType<Image>().FirstOrDefault(i => i.Name == type);
            string path = GetPathByType(type);
            if (image is null || string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path);
            bitmap.EndInit();

            image.Source = bitmap;

            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = 12;
            textBlock.Text = GetNameByType(type);
            textBlock.Width = image.Width;
            textBlock.TextAlignment = TextAlignment.Center;

            double left = Canvas.GetLeft(image);
            double top = Canvas.GetTop(image);

            Canvas.SetLeft(textBlock, left);
            Canvas.SetTop(textBlock, top + image.Height);

            Store.Children.Add(textBlock);
        }
        private string GetPathByType(string type)
        {
            string folderPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images");
            string filePath = System.IO.Path.Combine(folderPath, $"{type}.png");

            if (!File.Exists(filePath))
            {
                return "";
            }

            return filePath;
        }
        private double ClampD(double value, double min, double max)
        {
            if (value < min)
            {
                value = min;
            }
            if (value > max)
            {
                value = max;
            }

            return value;
        }
        private string GetNameByType(string type)
        {
            switch (type)
            {
                case "Fire":
                    return "Огонь";
                case "Water":
                    return "Вода";
                case "Air":
                    return "Воздух";
                case "Earth":
                    return "Земля";
                case "Dust":
                    return "Песок";
                case "Steam":
                    return "Пар";
                case "Lava":
                    return "Лава";
                case "Energy":
                    return "Энергия";
                case "Mud":
                    return "Грязь";
                case "Rain":
                    return "Дождь";
                case "Sea":
                    return "Море";
                case "Pressure":
                    return "Давление";
                case "Cloud":
                    return "Облако";
                case "Ocean":
                    return "Океан";
                case "Gunpowder":
                    return "Порох";
                case "Stone":
                    return "Камень";
                case "Salt":
                    return "Соль";
                case "Plant":
                    return "Растение";
                case "Explosion":
                    return "Взрыв";
                case "Metal":
                    return "Металл";
                case "Sand":
                    return "Песок";
                case "Storm":
                    return "Шторм";
                case "AtomicBomb":
                    return "Атомная бомба";
                case "Beach":
                    return "Пляж";
                case "Desert":
                    return "Пустыня";
                case "Electricity":
                    return "Электричество";
                case "Glass":
                    return "Стекло";
                case "Wave":
                    return "Волна";
                case "Sound":
                    return "Звук";
                case "Wind":
                    return "Ветер";
                case "Smoke":
                    return "Дым";
                default:
                    return "";
            }
        }
    }
}
