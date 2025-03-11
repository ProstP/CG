using Task3_2.ModelImp;
using Task3_2.ViewImp;
using Task3_2.ViewModel;

using (GameView gameWindow = new(1000, 800, "Game"))
{
    TetrisGame game = new();
    ViewModel viewModel = new(gameWindow, game);

    gameWindow.Run();
}
