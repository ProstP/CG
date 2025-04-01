namespace Task1_6.Camera;

public class CameraController
{
    public delegate void FloatEventHanlder(float value);
    public event FloatEventHanlder OnMoveObject;
    public event FloatEventHanlder OnRotateObject;

    System.Windows.Forms.Timer _rotTimer;
    System.Windows.Forms.Timer _posTimer;
    float _posSpeed = 0.05f;
    float _rotSpeed = (float)Math.PI / 120;

    public bool IsMoving { get; private set; }
    public bool IsRotating { get; private set; }

    public CameraController()
    {
        _rotTimer = new();
        _rotTimer.Tick += RotTimerTimeout;
        _rotTimer.Interval = 1;

        _posTimer = new();
        _posTimer.Tick += PosTimerTimeout;
        _posTimer.Interval = 1;
    }

    // Moving
    public void BeginMove(MoveDirection direction)
    {
        if (IsMoving)
        {
            return;
        }

        _posSpeed = Math.Abs(_posSpeed) * (direction == MoveDirection.Forward ? 1 : -1);

        _posTimer.Start();
        IsMoving = true;
    }
    public void EndMove()
    {
        if (!IsMoving)
        {
            return;
        }

        _posTimer.Stop();
        IsMoving = false;
    }

    // Rotating
    public void BeginRotate(RotationDirection direction)
    {
        if (IsRotating)
        {
            return;
        }

        _rotSpeed = Math.Abs(_rotSpeed) * (direction == RotationDirection.Right ? 1 : -1);

        _rotTimer.Start();
        IsRotating = true;
    }
    public void EndRotate()
    {
        if (!IsRotating)
        {
            return;
        }

        _rotTimer.Stop();
        IsRotating = false;
    }

    // Timeouts
    private void PosTimerTimeout(object sender, EventArgs args)
    {
        if (!IsMoving)
        {
            _posTimer.Stop();
        }

        OnMoveObject.Invoke(_posSpeed);
    }
    private void RotTimerTimeout(object sender, EventArgs args)
    {
        if (!IsRotating)
        {
            _rotTimer.Stop();
        }

        OnRotateObject.Invoke(_rotSpeed);
    }
}
