using System;
using System.Threading.Tasks;

public static class Helpers
{
    public delegate void Callback();

    public static async void Delay(float timeToDelay, Callback callback)
    {
        await Task.Delay((int)(timeToDelay * 1000));
        callback?.Invoke();
    }

    public static async void Delay(Func<bool> waitForCondition, Callback callback, Callback toDoWhileWaiting = null, float timeForTick = 0.02f, bool isTickBeforeToDoWhileWaiting = false)
    {
        if (isTickBeforeToDoWhileWaiting)
        {
            while (!waitForCondition())
            {
                toDoWhileWaiting?.Invoke();
                await Task.Delay((int)(timeForTick * 1000));
            }
            callback?.Invoke();
        }
        else
        {
            while (!waitForCondition())
            {
                await Task.Delay((int)(timeForTick * 1000));
                toDoWhileWaiting?.Invoke();
            }
            callback?.Invoke();
        }
    }
}