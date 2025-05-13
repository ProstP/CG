using Task2_1.Model;

namespace Task2_1.ShipView;

public static class ShipViewCreator
{
    public static ShipView Create(IShip ship, float x, float y)
    {
        return new ShipView(x, y, FileNameByShipType(ship.Type), ship);
    }
    private static string FileNameByShipType(ShipType shipType)
    {
        return shipType switch
        {
            ShipType.Fast => "Ship1.obj",
            ShipType.Big => "Ship1.obj",
            ShipType.Bomber => "Ship1.obj",
            _ => "Ship1.obj",
        };
    }
}
