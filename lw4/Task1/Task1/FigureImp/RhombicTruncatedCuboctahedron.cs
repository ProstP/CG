using Task1.Figure;

namespace Task1.FigureImp;

public class RhombicTruncatedCuboctahedron : IFigure
{
    float _alpha;
    List<Face> _faces = new();

    Color _octahedronColor = Color.Red;
    Color _hexagonColor = Color.Blue;
    Color _squareColor = Color.Yellow;

    public RhombicTruncatedCuboctahedron(float side, float alpha)
    {
        _alpha = alpha;

        CreateFaces(side);
    }

    public float GetAlpha() => _alpha;
    public List<Face> GetFaces() => _faces;

    private void CreateFaces(float side)
    {
        List<PointF> octahedronPoints = GetOctahedronPoints(side);

        CreateOctahedronFaces(octahedronPoints, side);
        CreateHexagonFaces();
        CreateSquares(side);
    }

    // Octahedrons
    private void CreateOctahedronFaces(List<PointF> points, float side)
    {
        // 1/2 * side + sqrt(2) / 2 * side + sqrt(2) / 2 * side
        float distanceToFaces = (0.5f + (float)Math.Sqrt(2)) * side;

        CreateOctahedronFacesByX(points, distanceToFaces);
        CreateOctahedronFacesByX(points, -distanceToFaces);
        CreateOctahedronFacesByY(points, distanceToFaces);
        CreateOctahedronFacesByY(points, -distanceToFaces);
        CreateOctahedronFacesByZ(points, distanceToFaces);
        CreateOctahedronFacesByZ(points, -distanceToFaces);
    }
    private void CreateOctahedronFacesByX(List<PointF> points, float x)
    {
        List<Point3F> facePoints = new();
        foreach (PointF point in points)
        {
            facePoints.Add(new(x, point.X, point.Y));
        }

        _faces.Add(new Face(_octahedronColor, facePoints));
    }
    private void CreateOctahedronFacesByY(List<PointF> points, float y)
    {
        List<Point3F> facePoints = new();
        foreach (PointF point in points)
        {
            facePoints.Add(new(point.X, y, point.Y));
        }

        _faces.Add(new Face(_octahedronColor, facePoints));
    }
    private void CreateOctahedronFacesByZ(List<PointF> points, float z)
    {
        List<Point3F> facePoints = new();
        foreach (PointF point in points)
        {
            facePoints.Add(new(point.X, point.Y, z));
        }

        _faces.Add(new Face(_octahedronColor, facePoints));
    }

    // Hexagons
    private void CreateHexagonFaces()
    {
        if (_faces.Count != 6)
        {
            throw new ArgumentException("Octahedron faces count must be 6");
        }

        for (int i = 0; i < 2; i++)
        {
            bool isRightOctaherdon = i == 0;

            for (int j = 0; j < 2; j++)
            {
                bool isTopOctaherdon = j == 0;

                for (int k = 0; k < 2; k++)
                {
                    bool isNearOctaherdon = k == 0;

                    CreateHexagonFaceByThreeOctaherdon(isRightOctaherdon, isTopOctaherdon, isNearOctaherdon);
                }
            }
        }
    }
    private void CreateHexagonFaceByThreeOctaherdon(bool isRightOctaherdon, bool isTopOctaherdon, bool isNearOctaherdon)
    {
        Face octahedronByX = isRightOctaherdon ? _faces[0] : _faces[1];
        Face octahedronByY = isTopOctaherdon ? _faces[2] : _faces[3];
        Face octahedronByZ = isNearOctaherdon ? _faces[4] : _faces[5];

        List<Point3F> points =
        [
            .. octahedronByX.Vertexes.Where(point =>
                    CheckTwoPairCondition(isTopOctaherdon, point.Y > 0, isNearOctaherdon, point.Z > 0))
                    .OrderBy(point => Math.Abs(point.Y)),
            .. octahedronByY.Vertexes.Where(point =>
                    CheckTwoPairCondition(isRightOctaherdon, point.X > 0, isNearOctaherdon, point.Z > 0))
                    .OrderBy(point => Math.Abs(point.Z)),
            .. octahedronByZ.Vertexes.Where(point =>
                    CheckTwoPairCondition(isRightOctaherdon, point.X > 0, isTopOctaherdon, point.Y > 0))
                    .OrderBy(point => Math.Abs(point.X)),
        ];

        _faces.Add(new Face(_hexagonColor, points));
    }
    private bool CheckTwoPairCondition(bool firstPair1, bool firstPair2, bool secondPair1, bool secondPair2)
    {
        return (firstPair1 == firstPair2) && (secondPair1 == secondPair2);
    }

    // Square
    private void CreateSquares(float side)
    {
        if (_faces.Count != 14)
        {
            throw new ArgumentException("Face count must be 6 + 8");
        }

        float distance1 = side * (0.5f + (float)Math.Sqrt(2));
        float distance2 = side * (1 + (float)Math.Sqrt(2)) / 2;

        CreateSquaresRing(distance1, distance2, side, CreateSquareXAndZ);
        CreateSquaresRing(distance1, distance2, side, CreateSquareXAndY);
        CreateSquaresRing(distance1, distance2, side, CreateSquareYAndZ);
    }
    private delegate void CreateSquareFunction(float distance1, float distance2, float side, bool isFirstPositive, bool isSecondPositive);
    private void CreateSquaresRing(float first, float second, float side, CreateSquareFunction createQuareFn)
    {
        createQuareFn(first, second, side, true, true);
        createQuareFn(first, second, side, true, false);
        createQuareFn(first, second, side, false, true);
        createQuareFn(first, second, side, false, false);
    }
    private void CreateSquareXAndZ(float distance1, float distance2, float side, bool isFirstPositive, bool isSecondPositive)
    {
        _faces.Add(new(_squareColor,
            [
                new (isFirstPositive ? distance1 : -distance1, 0.5f * side, isSecondPositive ? distance2 : -distance2),
                new (isFirstPositive ? distance1 : -distance1, -0.5f * side, isSecondPositive ? distance2 : -distance2),
                new (isFirstPositive ? distance2 : -distance2, -0.5f * side, isSecondPositive ? distance1 : -distance1),
                new (isFirstPositive ? distance2 : -distance2, 0.5f * side, isSecondPositive ? distance1 : -distance1),
            ]));
    }
    private void CreateSquareXAndY(float distance1, float distance2, float side, bool isFirstPositive, bool isSecondPositive)
    {
        _faces.Add(new(_squareColor,
            [
                new (isFirstPositive ? distance1 : -distance1, isSecondPositive ? distance2 : -distance2, 0.5f * side),
                new (isFirstPositive ? distance1 : -distance1, isSecondPositive ? distance2 : -distance2, -0.5f * side),
                new (isFirstPositive ? distance2 : -distance2, isSecondPositive ? distance1 : -distance1, -0.5f * side),
                new (isFirstPositive ? distance2 : -distance2, isSecondPositive ? distance1 : -distance1, 0.5f * side),
            ]));
    }
    private void CreateSquareYAndZ(float distance1, float distance2, float side, bool isFirstPositive, bool isSecondPositive)
    {
        _faces.Add(new(_squareColor,
            [
                new (0.5f * side, isFirstPositive ? distance1 : -distance1, isSecondPositive ? distance2 : -distance2),
                new (-0.5f * side, isFirstPositive ? distance1 : -distance1, isSecondPositive ? distance2 : -distance2),
                new (-0.5f * side, isFirstPositive ? distance2 : -distance2, isSecondPositive ? distance1 : -distance1),
                new (0.5f * side, isFirstPositive ? distance2 : -distance2, isSecondPositive ? distance1 : -distance1),
            ]));
    }

    // OctahedronPoints
    private static List<PointF> GetOctahedronPoints(float side)
    {
        float radius = side / (2 * (float)Math.Sin(Math.PI / 8));

        int vertexCount = 8;
        float angle = (float)(2 * Math.PI / vertexCount);

        List<PointF> points = new();

        for (int i = 0; i < vertexCount; i++)
        {
            float current = i * angle + angle / 2;

            points.Add(new((float)Math.Cos(current) * radius, (float)Math.Sin(current) * radius));
        }

        return points;
    }
}
