using OpenTK.Mathematics;
using Task2_1.Model;
using Task2_1.TextureService;
using static Task2_1.View.IView;

namespace Task2_1.Area;

public class GameArea
{
    public event EventHandler OnInvalidated;
    public event ClickToCellEventHandler OnClickToCell;

    Dictionary<string, int> _valueToTexture = [];
    List<CellView> _cells = [];
    List<CellView> _toCloseNext = [];
    List<CellView> _toRemoveNext = [];
    Queue<(List<CellView> cells, bool needRemove)> _nextCells = [];

    float _angle = 0;
    float _speed = (float)Math.PI / 20;
    System.Windows.Forms.Timer _timer = new();
    bool _isRotating = false;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public GameArea()
    {
        _timer.Interval = 1;
        _timer.Tick += OnTimerTick;
    }

    public void Init(int width, int height)
    {
        float r1 = 2f / height;
        float r2 = 2f / width;
        float size = Math.Min(r1, r2);
        float startX = -(float)width / 2 * size;
        float startY = -(float)height / 2 * size;

        Width = width;
        Height = height;

        _cells.Clear();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell cell = new(x, y);

                _cells.Add(new(cell, size, startX, startY) { TextureId = _valueToTexture["ok"] });
            }
        }
    }

    // Paint game area
    public void Paint()
    {
        _cells.ForEach(c => c.Paint());
    }

    // Open cell
    public void OpenCell(Cell cell, string value)
    {
        if (!_valueToTexture.ContainsKey(value))
        {
            throw new ArgumentException("Unknwon value for cell");
        }

        CellView cellView = _cells.FirstOrDefault(c => c.Cell == cell);
        cellView.TextureId = _valueToTexture[value];

        AddToRotate([cellView]);
    }
    public void AddToClose(Cell cell)
    {
        CellView cellView = _cells.FirstOrDefault(c => c.Cell == cell);
        _toCloseNext.Add(cellView);
    }
    public void BeginCloseCell()
    {
        AddToRotate(_toCloseNext);
        _toCloseNext.Clear();
    }
    public void AddToRemove(Cell cell)
    {
        CellView cellView = _cells.FirstOrDefault(c => c.Cell == cell);
        _toRemoveNext.Add(cellView);
    }
    public void BeginRemove()
    {
        AddToRemove(_toRemoveNext);
        _toRemoveNext.Clear();
    }

    // Rotate on timer tick
    private void AddToRotate(List<CellView> cells)
    {
        _nextCells.Enqueue((new(cells), false));

        if (_nextCells.Count == 1)
        {
            _timer.Start();
            _isRotating = true;
        }
    }
    private void AddToRemove(List<CellView> cells)
    {
        if (!_isRotating)
        {
            foreach (CellView cell in cells)
            {
                _cells.Remove(cell);
            }
        }
        else
        {
            _nextCells.Enqueue((new(cells), true));
        }
    }
    private void OnTimerTick(object sender, EventArgs args)
    {
        if (!_isRotating || _nextCells.Count == 0)
        {
            EndTimer();
            return;
        }

        _angle += _speed;

        _nextCells.First().cells.ForEach(c => c.Rotate(_speed));

        if (_angle > Math.PI)
        {
            RoundAngleOfRotating();
            _nextCells.Dequeue();
            _angle = 0;

            if (_nextCells.Count > 0)
            {
                if (_nextCells.First().needRemove)
                {
                    while (_nextCells.Count > 0 && _nextCells.First().needRemove)
                    {
                        RemoveCellViewByCell(_nextCells.First().cells);
                        _nextCells.Dequeue();
                    }
                    EndTimer();
                }
            }
            else
            {
                EndTimer();
            }
        }

        OnInvalidated.Invoke(this, new());
    }
    private void EndTimer()
    {
        _timer.Stop();
        _isRotating = false;
    }
    private void RemoveCellViewByCell(List<CellView> cells)
    {
        foreach (CellView cell in cells)
        {
            _cells.Remove(cell);
        }
    }

    // Click to area
    public void ClickToArea(
        Vector3 screenCords,
        Matrix4 projectionMatrix,
        Matrix4 viewMatrix,
        Matrix4 modelMatrix,
        int width,
        int height)
    {
        if (_isRotating)
        {
            return;
        }

        Vector3 vector = UnProject(
            screenCords,
            projectionMatrix,
            viewMatrix,
            modelMatrix,
            new Vector2(width, height));

        vector.Y -= 0.07f;

        if (CheckIsClickToCell(vector, out CellView cell))
        {
            if (cell != null)
            {
                OnClickToCell.Invoke(cell.Cell);
            }
        }
    }
    private void RoundAngleOfRotating()
    {
        foreach (CellView cell in _nextCells.First().cells)
        {
            float deltaFromZero = -cell.Angle;
            float deltaFromPi = (float)Math.PI - cell.Angle;

            float delta = Math.Abs(deltaFromPi) > Math.Abs(deltaFromZero) ? deltaFromZero : deltaFromPi;

            cell.Rotate(delta);
        }
    }
    private bool CheckIsClickToCell(Vector3 vector, out CellView cell)
    {
        float r1 = 2f / Height;
        float r2 = 2f / Width;
        float size = Math.Min(r1, r2);
        float startX = -(float)Width / 2 * size;
        float startY = -(float)Height / 2 * size;

        float globalX = vector.X - startX;
        float globalY = vector.Y - startY;

        int x = (int)(globalX / size);
        int y = (int)(globalY / size);

        CellView cellView = _cells.FirstOrDefault(c => c.Cell.X == x && c.Cell.Y == y);

        if (cellView != null)
        {
            cell = cellView;
            return true;
        }
        else
        {
            cell = null;
            return false;
        }
    }
    private Vector3 UnProject(
        Vector3 screenCoords,
        Matrix4 projectionMatrix,
        Matrix4 viewMatrix,
        Matrix4 modelMatrix,
        Vector2 viewport)
    {
        Matrix4 combinedMatrix = modelMatrix * viewMatrix * projectionMatrix;
        Matrix4 invertedMatrix = combinedMatrix.Inverted();

        Vector3 normalizedCoords = new Vector3(
            (screenCoords.X / viewport.X) * 2.0f - 1.0f,
            1.0f - (screenCoords.Y / viewport.Y) * 2.0f,
            screenCoords.Z * 2.0f - 1.0f);

        Vector4 worldCoords = new Vector4(normalizedCoords, 1.0f) * invertedMatrix;

        if (worldCoords.W != 0.0f)
        {
            worldCoords /= worldCoords.W;
        }

        return new Vector3(worldCoords.X, worldCoords.Y, worldCoords.Z);
    }

    // Init textures
    public void LoadTextures(string fileName = "Values/valueToImage.txt")
    {
        if (!File.Exists(fileName))
        {
            throw new ArgumentException("File not exists");
        }

        using (StreamReader reader = new(fileName))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(':');

                string key = parts[0];
                string value = "";
                for (int i = 1; i < parts.Length; i++)
                {
                    value += parts[i];
                }

                _valueToTexture[key] = TextureLoader.LoadTexture(value);
            }
        }
    }
}
