namespace MyPLINQ;

public class WaitableQueue<T>
{
    private Queue<T> _tasks = new();
    private readonly object _monitor = new();

    public void Enqueue(T action)
    {
        lock (_monitor)
        {
            _tasks.Enqueue(action);
            Monitor.Pulse(_monitor);
        }
    }

    public bool TryDequeue(out T? result)
    {
        lock (_monitor)
        {
            return _tasks.TryDequeue(out result);
        }
    }
    
    public T Dequeue()
    {
        while (true)
        {
            lock (_monitor)
            {
                if (_tasks.TryDequeue(out var action))
                    return action;
                Monitor.Wait(_monitor);
            }
        }
    }

    public int Length()
    {
        lock (_monitor)
        {
            return _tasks.Count;
        }
    }

    public Queue<T> DequeueHalf()
    {
        lock (_monitor)
        {
            var count = (_tasks.Count + 1) / 2;
            if (count == 0)
                return new Queue<T>();

            var remaining = _tasks.Count - count;
            
            var result = _tasks;
            _tasks = new Queue<T>();
            
            for (var i = 0; i < remaining; i++)
            {
                _tasks.Enqueue(result.Dequeue());
            }

            return result;
        }
    }
}