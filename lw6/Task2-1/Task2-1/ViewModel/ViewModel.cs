using Task2_1.Model;
using Task2_1.View;

namespace Task2_1.ViewModel;

public class ViewModel
{
    IModel _model;
    IView _view;

    public ViewModel(IModel model, IView view)
    {
        _model = model;
        _view = view;

        _model.OnLost += OnModelLost;
        _model.OnCanShoot += OnModelCanShoot;
        _model.OnHPUpdate += OnModelHPUpdate;
        _model.OnShipCreated += OnModelShipCreated;
        _model.OnShipRemoved += OnModelShipRemoved;

        _view.OnShipDoDamage += OnViewShipDoDamage;
        _view.OnShipHasDamage += OnViewShipHasDamage;
        _view.OnStartGame += OnViewStartGame;
    }

    // Model events
    private void OnModelLost()
    {
        _view.Lost();
    }
    private void OnModelCanShoot()
    {
        _view.CanShoot();
    }
    private void OnModelShipCreated(IShip ship)
    {
        _view.CreateShip(ship);
    }
    private void OnModelShipRemoved(IShip ship)
    {
        _view.RemoveShip(ship);
    }
    private void OnModelHPUpdate(int value)
    {
        _view.SetHp(value);
    }

    // View events
    private void OnViewShipDoDamage(IShip ship)
    {
        _model.ShipeDoDamage(ship);
    }
    private void OnViewShipHasDamage(IShip ship)
    {
        _model.ShipeHasDamage(ship);
    }
    private void OnViewStartGame()
    {
        _model.StartGame();
        _view.StartGame();
    }
}
