using Task2_1.Model;
using Task2_1.ObjectHandler;

namespace Task2_1.ShipView;

public class ShipView
{
    const float Z = 0f;
    const float Scale = 0.2f;

    public ObjView View { get; private init; }

    public IShip Ship { get; private set; }

    public ShipView(float x, float y, string fileName, IShip ship)
    {
        Ship = ship;
        View = new(fileName, x, y, Z, Scale);
    }

    public void Move(float dx, float dy)
    {
        View.X += dx;
        View.Y += dy;
    }

    public void Paint()
    {
        View.Paint();
    }

    public bool IsPointInShip(float x, float y)
    {
        return View.X - Scale <= x
            && View.X + Scale >= x
            && View.Y - Scale <= y
            && View.Y + Scale >= y;
    }
}
