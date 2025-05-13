using Task2_1.Model;
using static Task2_1.Model.IModel;

namespace Task2_1.View;

public interface IView
{
    event ShipEventHandler OnShipHasDamage;
    event ShipEventHandler OnShipDoDamage;
    event WithoutArgsEventHandler OnStartGame;

    // Game proccess
    void StartGame();
    void Lost();

    // Ship
    void CreateShip(IShip ship);
    void RemoveShip(IShip ship);

    // HP
    void SetHp(int value);

    // Torpedo
    void CanShoot();
}
