using Task3_2.Model.Tetraminoes;

namespace Task3_2.ModelImp.TetraminoRandom;

public static class TetraminoRandomizer
{
    private static Random _random = new();

    public static TetraminoType GetNextType()
    {
        Array types = Enum.GetValues(typeof(TetraminoType));

        int next = _random.Next(types.Length);

        return (TetraminoType)next;
    }
}
