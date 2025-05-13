using Task2_1.Model;
using Task2_1.ModelImp.Factory.Ship;
using static Task2_1.Model.IModel;

namespace Task2_1.ModelImp;

public class ShipCreator
{
    public event ShipEventHandler OnCreate;

    System.Windows.Forms.Timer _timer;
    Random _random = new();
    ShipFactory _factory = new();

    public ShipCreator(int interval = 5000)
    {
        _timer.Interval = interval;

        _timer.Tick += TimerTick;
    }

    public void Start()
    {
        _timer.Start();
    }
    public void Stop()
    {
        _timer.Stop();
    }

    // On tick of timer
    private void TimerTick(object sender, EventArgs args)
    {
        IShip ship = _factory.Create(GetRandomShipType());

        OnCreate.Invoke(ship);
    }

    // Get random
    private ShipType GetRandomShipType()
    {
        Array types = Enum.GetValues(typeof(ShipType));

        int type = _random.Next(types.Length);

        return (ShipType)type;
    }
}
