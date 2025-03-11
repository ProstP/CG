using Task3_2.Model;
using Task3_2.Model.Tetraminoes;
using Task3_2.Model.Timer;
using Task3_2.ModelImp.GameData;
using Task3_2.ModelImp.TetraminoRandom;
using static Task3_2.Model.IModel;

namespace Task3_2.ModelImp;

public class TetrisGame : IModel
{
    public event NextTetraminoUpdateEventHandler OnNextTetraminoUpdate;
    public event LostEventHandler OnLost;

    bool _isPlaying;
    MyTimer _timer = new();
    int _curentDeltaTime;
    GameArea.GameArea _area = new();
    GameDataHandler _data = new();
    TetraminoType _nextType;

    public bool IsPause { get; private set; }

    public int WIDTH => _area.WIDTH;

    public int HEIGHT => _area.HEIGHT;

    public TetrisGame()
    {
        _timer.OnTimeout += OnTimeout;
        _area.OnCreateNewTetramino += GenerateNextType;
        _area.OnClearRow += OnAreaClearRows;
    }

    // Events from area
    event TetraminoChangedEventHandler IModel.OnTetraminoMove
    {
        add
        {
            _area.OnTetraminoMove += value;
        }

        remove
        {
            _area.OnTetraminoMove -= value;
        }
    }
    event TetraminoChangedEventHandler IModel.OnTetraminoRotate
    {
        add
        {
            _area.OnTetraminoRotate += value;
        }

        remove
        {
            _area.OnTetraminoRotate -= value;
        }
    }
    event TetraminoChangedEventHandler IModel.OnTetraminoBlocked
    {
        add
        {
            _area.OnTetraminoBlocked += value;
        }

        remove
        {
            _area.OnTetraminoBlocked -= value;
        }
    }
    event ClearRowEventHanlder IModel.OnClearRow
    {
        add
        {
            _area.OnClearRow += value;
        }

        remove
        {
            _area.OnClearRow -= value;
        }
    }
    event TetraminoChangedEventHandler IModel.OnCreateNewTetramino
    {
        add
        {
            _area.OnCreateNewTetramino += value;
        }

        remove
        {
            _area.OnCreateNewTetramino -= value;
        }
    }

    event IntUpdateEventHandler IModel.OnScoreUpdate
    {
        add
        {
            _data.OnScoreUpdate += value;
        }

        remove
        {
            _data.OnScoreUpdate -= value;
        }
    }

    event IntUpdateEventHandler IModel.OnLevelUpdate
    {
        add
        {
            _data.OnLevelUpdate += value;
        }

        remove
        {
            _data.OnLevelUpdate -= value;
        }
    }

    // Interface impemantation
    public void NewGame()
    {
        if (_isPlaying)
        {
            return;
        }

        _isPlaying = true;
        SetIntervalByLevel();
        GenerateNextType();
        _timer.StartTimer(_curentDeltaTime);
        _area.Start(_nextType);
    }

    public void BeginMoveFaster()
    {
        if (!_isPlaying)
        {
            return;
        }

        _curentDeltaTime = 100;
    }

    public void EndMoveFaster()
    {
        if (!_isPlaying)
        {
            return;
        }

        SetIntervalByLevel();
    }

    void IModel.RotateTetramino()
    {
        if (!_isPlaying)
        {
            return;
        }
        _area.RotateTetramino();
    }

    void IModel.MoveTetraminoByX(bool toRight)
    {
        if (!_isPlaying)
        {
            return;
        }
        _area.MoveTetraminoByX(toRight);
    }

    public void Pause()
    {
        if (IsPause || !_isPlaying)
        {
            return;
        }
        IsPause = true;
    }

    public void Resume()
    {
        if (!IsPause || !_isPlaying)
        {
            return;
        }
        IsPause = false;
        SetIntervalByLevel();
        _timer.StartTimer(_curentDeltaTime);
    }

    // Timer Timeout
    private void OnTimeout()
    {
        if (IsPause || _curentDeltaTime == 0)
        {
            return;
        }

        _area.MoveTetraminoByY(_nextType, out bool isLost);
        if (isLost)
        {
            Lost();
        }
        else
        {
            _timer.StartTimer(_curentDeltaTime);
        }
    }

    // Delta time by level
    private void SetIntervalByLevel()
    {
        _curentDeltaTime = 1000 - (_data.Level - 1) * 100;
    }

    // Lost
    private void Lost()
    {
        if (!_isPlaying)
        {
            return;
        }

        _isPlaying = false;
        IsPause = false;

        OnLost.Invoke();
    }

    //GenerateNextType
    private void GenerateNextType(Tetramino _ = null)
    {
        _nextType = TetraminoRandomizer.GetNextType();

        OnNextTetraminoUpdate.Invoke(_nextType);
    }

    // OnClearRowsAddScore
    private void OnAreaClearRows(List<int> rows)
    {
        int count = rows.Count;

        if (count <= 0)
        {
            return;
        }

        if (count == 1)
        {
            _data.AddScore(10);
        }
        else if (count == 2)
        {
            _data.AddScore(30);
        }
        else if (count == 3)
        {
            _data.AddScore(70);
        }
        else
        {
            _data.AddScore(150);
        }
    }
}
