using Task2_1.Model;
using Task2_1.View;

namespace Task2_1.ModelView;

public class ViewModel
{
    readonly IModel _model;
    readonly IView _view;

    public ViewModel(
        IModel model,
        IView view)
    {
        _model = model;
        _view = view;

        _model.OnOpenCell += OnModelOpenCell;
        _model.OnCloseCell += OnModelCloseCell;
        _model.OnDoubleCloseCell += OnModelDoubleCloseCell;
        _model.OnClearCell += OnModelClearCell;
        _model.OnWin += OnModelWin;
        _model.OnInit += OnModelInit;

        _view.OnClickToCell += OnViewClickToCell;
        _view.OnStartNewGame += OnViewStartNewGame;
    }

    ~ViewModel()
    {
        _model.OnOpenCell -= OnModelOpenCell;
        _model.OnCloseCell -= OnModelCloseCell;
        _model.OnClearCell -= OnModelClearCell;
        _model.OnWin -= OnModelWin;

        _view.OnClickToCell -= OnViewClickToCell;
        _view.OnStartNewGame -= OnViewStartNewGame;
    }

    // Events from Model
    private void OnModelOpenCell(Cell cell, string value)
    {
        _view.OpenCell(cell, value);
    }
    private void OnModelCloseCell(Cell cell)
    {
        _view.CloseCell(cell);
    }
    private void OnModelDoubleCloseCell(Cell first, Cell second)
    {
        _view.CloseCell(first, second);
    }
    private void OnModelClearCell(Cell first, Cell second)
    {
        _view.ClearCell(first, second);
    }
    private void OnModelWin()
    {
        _view.Win();
    }
    private void OnModelInit(int width, int height)
    {
        _view.Init(width, height);
    }

    // Events from View
    private void OnViewClickToCell(Cell cell)
    {
        _model.ClickToCell(cell);
    }
    private void OnViewStartNewGame(int difficulty)
    {
        _model.NewGame(difficulty);
    }
}
