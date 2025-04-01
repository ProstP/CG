using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Task1_6.Primitives;
using Task1_6.TextureService;

namespace Task1_6.Map;

public class GameMap
{
    List<List<CellType>> _map = new();

    public const float CUBE_SIZE = 1.0f;

    Color _floorColor = Color.Coral;
    Color _wallColor = Color.DarkBlue;

    Dictionary<CellType, int> _textures;

    public IReadOnlyList<IReadOnlyList<CellType>> Map => _map;

    public GameMap(List<List<int>> map)
    {
        List<List<CellType>> newMap = new();

        foreach (List<int> row in map)
        {
            List<CellType> newRow = new();
            foreach (int cell in row)
            {
                newRow.Add((CellType)cell);
            }

            newMap.Add(newRow);
        }

        _map = newMap;
    }
    public GameMap(List<List<CellType>> map)
    {
        _map = map;
    }

    public void InitTextures()
    {
        _textures = new()
        {
            {CellType.Bricks, TextureLoader.LoadTexture("Images/bricks.jpg") },
            {CellType.SoftWall, TextureLoader.LoadTexture("Images/soft_walls.png") },
            {CellType.Tile, TextureLoader.LoadTexture("Images/tile.jpg") },
            {CellType.WhiteBricks, TextureLoader.LoadTexture("Images/white_bricks.png") },
            {CellType.Wodden, TextureLoader.LoadTexture("Images/wooden_walls.jpg") },
            {CellType.Hexagonal, TextureLoader.LoadTexture("Images/hexagonal.png") },
        };
    }

    public void PaintMap()
    {
        PaintFloor();
        PaintBlocks();
    }

    public bool CorrectPosition(float x, float z)
    {
        float start = -CUBE_SIZE * 8;
        float offset = CUBE_SIZE / 5;

        return IsCellFree((int)((x + offset - start) / CUBE_SIZE), (int)((z + offset - start) / CUBE_SIZE))
            && IsCellFree((int)((x - offset - start) / CUBE_SIZE), (int)((z + offset - start) / CUBE_SIZE))
            && IsCellFree((int)((x - offset - start) / CUBE_SIZE), (int)((z - offset - start) / CUBE_SIZE))
            && IsCellFree((int)((x + offset - start) / CUBE_SIZE), (int)((z - offset - start) / CUBE_SIZE));
    }
    private bool IsCellFree(int x, int z)
    {
        if (x < 0 || z < 0)
        {
            return true;
        }
        if (_map.Count <= z)
        {
            return true;
        }
        if (_map[z].Count <= x)
        {
            return true;
        }

        return _map[z][x] == CellType.None || _map[z][x] == CellType.PlayerStart;
    }

    public void FindCameraPosition(out int x, out int z)
    {
        List<CellType> row = _map.FirstOrDefault(row => row.Contains(CellType.PlayerStart));

        if (row == null)
        {
            throw new ArgumentException("Position of camera not found");
        }
        z = _map.IndexOf(row);
        x = row.IndexOf(CellType.PlayerStart);
    }

    // Floor drawing
    private void PaintFloor()
    {
        GL.BindTexture(TextureTarget.Texture2D, 0);
        GL.Begin(PrimitiveType.Quads);

        GL.Color3(_floorColor);

        GL.Vertex3(-CUBE_SIZE * 8, 0, -CUBE_SIZE * 8);
        GL.Vertex3(-CUBE_SIZE * 8, 0, CUBE_SIZE * 8);
        GL.Vertex3(CUBE_SIZE * 8, 0, CUBE_SIZE * 8);
        GL.Vertex3(CUBE_SIZE * 8, 0, -CUBE_SIZE * 8);

        GL.End();
    }

    // Block drawing
    private void PaintBlocks()
    {
        GL.Color3(Color.White);
        float startX = -CUBE_SIZE * 8;
        float startZ = -CUBE_SIZE * 8;

        for (int z = 0; z < _map.Count; z++)
        {
            for (int x = 0; x < _map[z].Count; x++)
            {
                if (_map[z][x] == CellType.None || _map[z][x] == CellType.PlayerStart)
                {
                    continue;
                }

                float offsetX = startX + x * CUBE_SIZE + CUBE_SIZE / 2;
                float offsetZ = startZ + z * CUBE_SIZE + CUBE_SIZE / 2;
                DrawCube(new Vector3(offsetX, CUBE_SIZE / 2, offsetZ), CUBE_SIZE / 2, _map[z][x]);
            }
        }
    }

    // Cube drawing
    private void DrawCube(Vector3 center, float side, CellType type)
    {
        GL.BindTexture(TextureTarget.Texture2D, _textures[type]);

        DrawCubeFaceWithTexture(new FlatFace(
            [
                Point3F.Create(center.X + side, center.Y + side, center.Z + side),
                Point3F.Create(center.X + side, center.Y + side, center.Z - side),
                Point3F.Create(center.X + side, center.Y - side, center.Z - side),
                Point3F.Create(center.X + side, center.Y - side, center.Z + side),
            ],
            _wallColor));
        DrawCubeFaceWithTexture(new FlatFace(
            [
                Point3F.Create(center.X - side, center.Y + side, center.Z - side),
                Point3F.Create(center.X - side, center.Y + side, center.Z + side),
                Point3F.Create(center.X - side, center.Y - side, center.Z + side),
                Point3F.Create(center.X - side, center.Y - side, center.Z - side),
            ],
            _wallColor));
        DrawCubeFaceWithTexture(new FlatFace(
            [
                Point3F.Create(center.X - side, center.Y + side, center.Z + side),
                Point3F.Create(center.X + side, center.Y + side, center.Z + side),
                Point3F.Create(center.X + side, center.Y - side, center.Z + side),
                Point3F.Create(center.X - side, center.Y - side, center.Z + side),
            ],
            _wallColor));
        DrawCubeFaceWithTexture(new FlatFace(
            [
                Point3F.Create(center.X + side, center.Y + side, center.Z - side),
                Point3F.Create(center.X - side, center.Y + side, center.Z - side),
                Point3F.Create(center.X - side, center.Y - side, center.Z - side),
                Point3F.Create(center.X + side, center.Y - side, center.Z - side),
            ],
            _wallColor));
        DrawCubeFaceWithTexture(new FlatFace(
            [
                Point3F.Create(center.X - side, center.Y + side, center.Z - side),
                Point3F.Create(center.X + side, center.Y + side, center.Z - side),
                Point3F.Create(center.X + side, center.Y + side, center.Z + side),
                Point3F.Create(center.X - side, center.Y + side, center.Z + side),
            ],
            _wallColor));
        DrawCubeFaceWithTexture(new FlatFace(
            [
                Point3F.Create(center.X - side, center.Y - side, center.Z + side),
                Point3F.Create(center.X + side, center.Y - side, center.Z + side),
                Point3F.Create(center.X + side, center.Y - side, center.Z - side),
                Point3F.Create(center.X - side, center.Y - side, center.Z - side),
            ],
            _wallColor));
    }

    // Drawing points
    private void DrawCubeFaceWithTexture(FlatFace cubeFace)
    {
        GL.Begin(PrimitiveType.Quads);

        GL.TexCoord2(0f, 0f);
        GL.Vertex3(cubeFace[0].X, cubeFace[0].Y, cubeFace[0].Z);
        GL.TexCoord2(1f, 0f);
        GL.Vertex3(cubeFace[1].X, cubeFace[1].Y, cubeFace[1].Z);
        GL.TexCoord2(1f, 1f);
        GL.Vertex3(cubeFace[2].X, cubeFace[2].Y, cubeFace[2].Z);
        GL.TexCoord2(0f, 1f);
        GL.Vertex3(cubeFace[3].X, cubeFace[3].Y, cubeFace[3].Z);

        GL.End();
    }
}
