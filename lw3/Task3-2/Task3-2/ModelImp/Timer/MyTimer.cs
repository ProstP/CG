namespace Task3_2.Model.Timer;
public class MyTimer
{
    public delegate void TimeoutEventHandler();

    public event TimeoutEventHandler OnTimeout;

    bool isStart = false;

    // Time in ms
    public void StartTimer(int time)
    {
        if (isStart)
        {
            return;
        }

        isStart = true;
        WaitTime(time);
    }

    private async void WaitTime(int time)
    {
        await Task.Delay(time);

        isStart = false;
        OnTimeout.Invoke();
    }
}
