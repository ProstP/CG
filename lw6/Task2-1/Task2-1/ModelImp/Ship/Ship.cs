using Task2_1.Model;

namespace Task2_1.ModelImp.Ship;

public class Ship : IShip
{
    public ShipType Type { get; private set; }

    public int Damage { get; private set; }

    public int Speed { get; private set; }

    public int HP { get; private set; }

    public Ship(ShipType type, int damage, int speed, int hP)
    {
        Type = type;
        Damage = damage;
        Speed = speed;
        HP = hP;
    }

    public void DoDamage(int damage)
    {
        HP -= damage;
    }
}
