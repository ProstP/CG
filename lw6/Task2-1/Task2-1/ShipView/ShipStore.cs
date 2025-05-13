using Task2_1.Model;
using static Task2_1.Model.IModel;

namespace Task2_1.ShipView;

public class ShipStore
{
    public event ShipEventHandler OnShipDoDamage;

    List<ShipView> _ships = [];
    List<ShipView> _destroyed = [];

    float _drowningSpeed = -0.1f;

    System.Windows.Forms.Timer _timer;

    const float ShipStartX = -1f;
    const float ShipY = 0f;
    const float ShipDestroyY = -1f;
    const float ShipEndX = 1f;

    public ShipStore()
    {
        _timer = new();
        _timer.Tick += OnTimerTick;
        _timer.Interval = 1;
    }

    public void StartGame()
    {
        _timer.Start();
    }

    public void StopGame()
    {
        _timer.Stop();
    }

    // Timer tick
    private void OnTimerTick(object sender, EventArgs e)
    {
        MoveShips();
    }

    // Paint
    public void Paint()
    {
        _ships.ForEach(ship => ship.Paint());
        _destroyed.ForEach(destroyed => destroyed.Paint());
    }

    // Create and remove methods
    public void CreateNewShip(IShip ship)
    {
        _ships.Add(ShipViewCreator.Create(ship, ShipStartX, ShipY));
    }
    public void RemoveShip(IShip ship)
    {
        _ships.Remove(_ships.FirstOrDefault(shipView => shipView.Ship == ship));
    }
    public void ShipDestroy(IShip ship)
    {
        ShipView shipView = _ships.FirstOrDefault(s => s.Ship == ship);

        if (shipView != null)
        {
            _ships.Remove(shipView);
            _destroyed.Add(shipView);
        }
    }

    // Moving
    private void MoveShips()
    {
        foreach (ShipView ship in _ships)
        {
            ship.Move(ship.Ship.Speed, 0);
        }

        foreach (ShipView ship in _destroyed)
        {
            ship.Move(0, _drowningSpeed);
        }

        CheckAllShips();
    }

    // Is torpedo do damage ship
    public bool IsTorpedoDoDamageToShip(float x, float y, out IShip ship)
    {
        foreach (ShipView shipView in _ships)
        {
            if (shipView.IsPointInShip(x, y))
            {
                ship = shipView.Ship;
                return true;
            }
        }

        ship = null;
        return false;
    }

    // Ships checks
    private void CheckAllShips()
    {
        CheckIsShipGotDestination();
        CheckIsShipDestroyed();
    }
    private void CheckIsShipGotDestination()
    {
        List<ShipView> ships = [];

        foreach (ShipView ship in _ships)
        {
            if (ship.View.X >= ShipEndX)
            {
                ships.Add(ship);
            }
        }

        ships.ForEach(ship => OnShipDoDamage.Invoke(ship.Ship));
    }
    private void CheckIsShipDestroyed()
    {
        _destroyed.RemoveAll(ship => ship.View.Y >= ShipDestroyY);
    }
}
