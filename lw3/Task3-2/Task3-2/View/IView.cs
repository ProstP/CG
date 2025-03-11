using Task3_2.Model.Tetraminoes;
using Task3_2.Model.Tetraminoes.Cells;

namespace Task3_2.View;
public interface IView
{
    delegate void WithoutArgsEventHandler();
    delegate void MoveByXEventHandler(bool toRight);

    event WithoutArgsEventHandler OnBeginMoveFaster;
    event WithoutArgsEventHandler OnEndMoveFaster;
    event WithoutArgsEventHandler OnRotateClicked;
    event WithoutArgsEventHandler OnStartNewGame;
    event MoveByXEventHandler OnMoveByX;
    event WithoutArgsEventHandler OnPauseClicked;

    void UpdateTetramino(Tetramino tetramino);
    void BlockTetramino(Tetramino tetramino);
    void ClearRows(List<int> rows);

    void SetNextTetramino(TetraminoType type);

    void UpdateScore(int score);
    void UpdateLevel(int level);

    void Lost();
}
