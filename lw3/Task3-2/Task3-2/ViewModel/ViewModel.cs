using Task3_2.Model;
using Task3_2.Model.Tetraminoes;
using Task3_2.Model.Tetraminoes.Cells;
using Task3_2.View;

namespace Task3_2.ViewModel;

public class ViewModel
{
    IModel _model;
    IView _view;

    public ViewModel(IView view, IModel model)
    {
        _model = model;
        _model.OnTetraminoMove += OnModelTetraminoMove;
        _model.OnTetraminoRotate += OnModelTetraminoRotate;
        _model.OnTetraminoBlocked += OnModelTetraminoBlocked;
        _model.OnNextTetraminoUpdate += OnModelNextTetraminoUpdate;
        _model.OnClearRow += OnModelClearRow;
        _model.OnScoreUpdate += OnModelScoreUpdate;
        _model.OnLevelUpdate += OnModelLevelUpdate;
        _model.OnLost += OnModelLost;
        _model.OnCreateNewTetramino += OnModelCreateNew;

        _view = view;
        _view.OnBeginMoveFaster += OnViewBeginMoveFaster;
        _view.OnEndMoveFaster += OnViewEndMoveFaster;
        _view.OnRotateClicked += OnViewRotateClicked;
        _view.OnStartNewGame += OnViewStartNewGame;
        _view.OnMoveByX += OnViewMoveByX;
        _view.OnPauseClicked += OnViewPauseClicked;
    }

    // Model events
    private void OnModelTetraminoMove(Tetramino tetramino)
    {
        _view.UpdateTetramino(tetramino);
    }
    private void OnModelTetraminoRotate(Tetramino tetramino)
    {
        _view.UpdateTetramino(tetramino);
    }
    private void OnModelTetraminoBlocked(Tetramino tetramino)
    {
        _view.BlockTetramino(tetramino);
    }
    private void OnModelNextTetraminoUpdate(TetraminoType type)
    {
        _view.SetNextTetramino(type);
    }
    private void OnModelClearRow(List<int> rows)
    {
        _view.ClearRows(rows);
    }
    private void OnModelScoreUpdate(int value)
    {
        _view.UpdateScore(value);
    }
    private void OnModelLevelUpdate(int value)
    {
        _view.UpdateLevel(value);
    }
    private void OnModelLost()
    {
        _view.Lost();
    }
    private void OnModelCreateNew(Tetramino tetramino)
    {
        _view.UpdateTetramino(tetramino);
    }

    // View events
    private void OnViewBeginMoveFaster()
    {
        _model.BeginMoveFaster();
    }
    private void OnViewEndMoveFaster()
    {
        _model.EndMoveFaster();
    }
    private void OnViewRotateClicked()
    {
        _model.RotateTetramino();
    }
    private void OnViewStartNewGame()
    {
        _model.NewGame();
    }
    private void OnViewMoveByX(bool toRight)
    {
        _model.MoveTetraminoByX(toRight);
    }
    private void OnViewPauseClicked()
    {
        if (_model.IsPause)
        {
            _model.Resume();
        }
        else
        {
            _model.Pause();
        }
    }
}
