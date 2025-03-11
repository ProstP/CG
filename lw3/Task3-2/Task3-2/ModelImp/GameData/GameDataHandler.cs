using static Task3_2.Model.IModel;

namespace Task3_2.ModelImp.GameData;

public class GameDataHandler
{
    public event IntUpdateEventHandler OnScoreUpdate;
    public event IntUpdateEventHandler OnLevelUpdate;

    int _score = 0;

    public int Level { get; private set; } = 1;

    public void Reset()
    {
        _score = 0;
        Level = 1;
    }

    public void AddScore(int value)
    {
        _score += value;

        OnScoreUpdate.Invoke(_score);

        CheckScoreAndChangeLevel();
    }

    private void CheckScoreAndChangeLevel()
    {
        int level;

        if (10 < _score && _score <= 30)
        {
            level = 2;
        }
        else if (30 < _score && _score <= 60)
        {
            level = 3;
        }
        else if (60 < _score && _score <= 100)
        {
            level = 4;
        }
        else if (100 < _score && _score <= 120)
        {
            level = 5;
        }
        else if (120 < _score)
        {
            level = 6;
        }
        else
        {
            level = 1;
        }

        //if (300 < _score && _score <= 500)
        //{
        //    level = 2;
        //}
        //else if (500 < _score && _score <= 800)
        //{
        //    level = 3;
        //}
        //else if (800 < _score && _score <= 1500)
        //{
        //    level = 4;
        //}
        //else if (1500 < _score && _score <= 2200)
        //{
        //    level = 5;
        //}
        //else if (2200 < _score)
        //{
        //    level = 6;
        //}
        //else
        //{
        //    level = 1;
        //}

        if (level != Level)
        {
            Level = level;
            OnLevelUpdate.Invoke(Level);
        }
    }
}
