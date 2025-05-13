namespace Task2_1.Model;

public interface IModel
{
    delegate void WithoutArgsEventHandler();
    delegate void ShipEventHandler(IShip ship);
    delegate void IntEventHandler(int value);

    event WithoutArgsEventHandler OnLost;
    event ShipEventHandler OnShipRemoved;
    event IntEventHandler OnHPUpdate;
    event ShipEventHandler OnShipCreated;
    event WithoutArgsEventHandler OnCanShoot;

    IReadOnlyList<IShip> Ships { get; }
    int HP { get; }
    bool CanShoot { get; }
    bool IsShooting { get; }

    // Game proccess
    void StartGame();

    // Ships
    void ShipeDoDamage(IShip ship);
    void ShipeHasDamage(IShip ship);
}
