namespace Task2_1.Model;

public interface IShip
{
    ShipType Type { get; }
    int Damage { get; }
    float Speed { get; }
    int HP { get; }

    void DoDamage(int value);
}
