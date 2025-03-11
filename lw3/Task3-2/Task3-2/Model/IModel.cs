using Task3_2.Model.Tetraminoes;
using Task3_2.Model.Tetraminoes.Cells;

namespace Task3_2.Model;
public interface IModel
{
    delegate void TetraminoChangedEventHandler(Tetramino tetramino);
    delegate void NextTetraminoUpdateEventHandler(TetraminoType type);
    delegate void ClearRowEventHanlder(List<int> rows);
    delegate void IntUpdateEventHandler(int score);
    delegate void LostEventHandler();

    event TetraminoChangedEventHandler OnTetraminoMove;
    event TetraminoChangedEventHandler OnTetraminoRotate;
    event TetraminoChangedEventHandler OnTetraminoBlocked;
    event TetraminoChangedEventHandler OnCreateNewTetramino;
    event NextTetraminoUpdateEventHandler OnNextTetraminoUpdate;
    event ClearRowEventHanlder OnClearRow;
    event IntUpdateEventHandler OnScoreUpdate;
    event IntUpdateEventHandler OnLevelUpdate;
    event LostEventHandler OnLost;

    // Получение данных игры

    int WIDTH { get; }
    int HEIGHT { get; }

    void NewGame();

    void BeginMoveFaster();
    void EndMoveFaster();

    void RotateTetramino();

    // Не интуитивно
    void MoveTetraminoByX(bool toRight);

    bool IsPause { get; }
    void Pause();
    void Resume();
}
