namespace Task3_2.ViewImp.MyColor;

public static class ColorGenerator
{
    static Random _random = new Random();
    static List<ColorRGB> _colors = [
            new (1f, 0f, 0f),
            new (0f, 0f, 1f),
            new (0f, 1f, 0f),
            new (0f, 1f, 1f),
            new (1f, 1f, 0f),
            new (1f, 0f, 1f),
        ];

    public static ColorRGB GenerateRand()
    {
        int newIndex = _random.Next(_colors.Count);

        return _colors[newIndex];
    }
}
