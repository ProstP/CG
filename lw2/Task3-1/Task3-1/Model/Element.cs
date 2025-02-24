namespace Task3_1.Model
{
    public class Element
    {
        string _id;
        double _left;
        double _top;
        double _width;
        double _height;
        ElementType _type;
        public string Id
        {
            get => _id;
        }
        public double Left
        {
            get => _left;
            set => _left = value;
        }
        public double Top
        {
            get => _top;
            set => _top = value;
        }
        public double Width
        {
            get => _width;
            set => _width = value;
        }
        public double Height
        {
            get => _height;
            set => _height = value;
        }
        public ElementType Type
        {
            get => _type;
        }

        public Element(string id, ElementType type)
            : this(id, type, 0, 0)
        { }

        public Element(string id, ElementType type, double left, double top)
            : this(id, type, left, top, 0, 0)
        { }
        public Element(string id, ElementType type, double left, double top, double width, double height)
        {
            _id = "Elt" + id;
            _type = type;
            _left = left;
            _top = top;
            _width = width;
            _height = height;
        }
    }
}