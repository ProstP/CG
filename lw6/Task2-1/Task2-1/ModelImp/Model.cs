using Task2_1.Model;
using static Task2_1.Model.IModel;

namespace Task2_1.ModelImp;

public class Model : IModel
{
    List<IShip> _ships = [];
    ShipCreator _shipCreator = new();

    public IReadOnlyList<IShip> Ships => _ships;
    public int HP { get; private set; }
    public bool CanShoot { get; private set; }
    public bool IsShooting { get; private set; }

    public event WithoutArgsEventHandler OnLost;
    public event ShipEventHandler OnShipRemoved;
    public event IntEventHandler OnHPUpdate;
    public event ShipEventHandler OnShipCreated;
    public event WithoutArgsEventHandler OnCanShoot;

    public Model()
    {
        _shipCreator.OnCreate += OnShipCreatorCreate;
    }

    // Interface
    public void ShipeDoDamage(IShip ship)
    {
        _ships.Remove(ship);

        OnShipRemoved(ship);

        HP -= ship.Damage;
        OnHPUpdate.Invoke(HP);

        if (HP <= 0)
        {
            _shipCreator.Stop();
            OnLost.Invoke();
        }
    }

    public void ShipeHasDamage(IShip ship)
    {
        ship.DoDamage(1);

        if (ship.HP <= 0)
        {
            _ships.Remove(ship);

            OnShipRemoved.Invoke(ship);
        }
    }

    public void StartGame()
    {
        _ships.Clear();
        CanShoot = true;
        IsShooting = false;

        _shipCreator.Start();
    }

    // Creator event
    private void OnShipCreatorCreate(IShip ship)
    {
        _ships.Add(ship);

        OnShipCreated.Invoke(ship);
    }
}
