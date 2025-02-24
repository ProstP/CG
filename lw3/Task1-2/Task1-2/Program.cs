using Window;

Func<float, float> func = (float x) =>
    {
        return 2 * x * x - 3 * x - 8;
    };

using (Game game = new Game(-2, 3, func, 800, 600, "The parabola"))
{
    game.Run();
}