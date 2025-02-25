
using MainWindow;
using PainterImp;

using (Game game = new Game(800, 600, "Meadow"))
{
    //game.AddPainter(new FigurePainter(0.5f, 0.1f, 0.4f, 0.3f));
    //game.AddPainter(new FigurePainter(0.1f, 0.6f, 0.3f, 0.2f));
    game.AddPainter(new FigurePainter());

    game.Run();
}
