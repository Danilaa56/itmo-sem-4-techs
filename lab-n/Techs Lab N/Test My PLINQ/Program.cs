using System.Collections.Concurrent;
using MyPLINQ;

namespace Test_My_PLINQ;

internal class Program
{
    public static void Main()
    {
        // IEnumerable<int> nums = new List<int>
        // {
        //     1, 2, 3, 4, 5, 6, 7, 8,
        //     11, 12, 13, 14, 15, 16, 17, 18,
        // };

        // nums.AsParallel()
        //     .Select(num =>
        // {
        //     Console.WriteLine(num);
        //     return num;
        // }).ToList();
        // var thread = new Thread(ThreadWork);
        // thread.Start();
        // Console.WriteLine(2);

        // object o = new object();
        // object ready1 = new object();
        // object ready2 = new object();

        var a = new MyThreadPool(8);
        a.Start();
        
        Thread.Sleep(100000);

        // RunExperiment(2000);
    }

    public static void RunExperiment(int times)
    {
        Dictionary<string, int> results = new();
        for (var i = 0; i < times; i++)
        {
            var result = Experiment();
            results.TryGetValue(result, out int count);
            results[result] = count + 1;
        }

        foreach (var keyValuePair in results)
        {
            Console.WriteLine(keyValuePair.Key + "\t" + keyValuePair.Value);
        }
    }


    public static string Experiment()
    {
        Thread t1, t2;
        var sync = new object();

        var nums = new ConcurrentQueue<long>();

        long num = 0;
        const int count = 10;

        var tasks = new WaitableQueue<Action>();

        lock (sync)
        {
            t1 = new Thread(() =>
            {
                lock (sync)
                {
                }

                for (var i = 0; i < count; i++)
                {
                    tasks.Enqueue(() => num++);
                }
            });

            t2 = new Thread(() =>
            {
                lock (sync)
                {
                }

                for (var i = 0; i < count; i++)
                {
                    var action = tasks.Dequeue();
                    action.Invoke();
                }

                nums.Enqueue(num);
            });

            t1.Start();
            t2.Start();
        }

        t2.Join();
        t1.Join();

        return nums.Aggregate("", (s, i) => s + ", " + i)[2..];
    }

    public static string ExperimentDefaultQueue()
    {
        Thread t1, t2;
        var sync = new object();
        var monitor = new object();
        var monitor2 = new object();

        var nums = new ConcurrentQueue<long>();

        long num = 0;
        int count = 10;

        var tasks = new Queue<Action>();

        lock (sync)
        {
            t1 = new Thread(() =>
            {
                lock (sync)
                {
                }

                for (int i = 0; i < count; i++)
                {
                    lock (monitor)
                    {
                        tasks.Enqueue(() => num++);
                        Monitor.Pulse(monitor);
                    }
                }
            });

            t2 = new Thread(() =>
            {
                lock (sync)
                {
                }

                int i = 0;
                int j = 0;
                Action? action = null;
                while (i < count)
                {
                    lock (monitor)
                    {
                        if (!tasks.TryDequeue(out action))
                            Monitor.Wait(monitor);
                    }

                    if (action is not null)
                    {
                        action.Invoke();
                        i++;
                    }

                    j++;
                }

                nums.Enqueue(num);
            });

            t1.Start();
            t2.Start();
        }

        t2.Join();
        t1.Join();

        return nums.Aggregate("", (s, i) => s + ", " + i).Substring(2);
    }

    public class WriteAction
    {
        private int _num;

        public WriteAction(int num)
        {
            _num = num;
        }

        public void Run()
        {
            // Console.WriteLine(_num);
        }
    }

    public class MyThreadPool
    {
        private MyWorker[] _threads;

        public MyThreadPool(int size)
        {
            _threads = new MyWorker[size];
            for (var i = 0; i < size; i++)
                _threads[i] = new MyWorker(this);
        }

        public void Start()
        {
            foreach (var thread in _threads)
            {
                thread.Start();
            }
        }
    }

    public class MyWorker
    {
        public WaitableQueue<Action> tasks = new WaitableQueue<Action>();
        private MyThreadPool _pool;
        
        private Thread _thread;

        public MyWorker(MyThreadPool pool)
        {
            _pool = pool;
            _thread = new Thread(Run);
        }

        public void Start()
        {
            _thread.Start();
        }

        private void Run()
        {
            while (true)
            {
                if (tasks.TryDequeue(out var action))
                {
                    action.Invoke();
                    continue;
                }

                if (!TryStealTasks())
                {
                    action = tasks.Dequeue();
                    action.Invoke();
                }

                // var action = tasks.Dequeue();
            }
        }

        private bool TryStealTasks()
        {
            return false;
        }
    }

    public class MyThread
    {
        public Queue<Action> Actions = new Queue<Action>();
        public object monitor = new object();

        private MyThreadPool _myThreadPool;
        private Thread _thread;

        public MyThread(MyThreadPool myThreadPool)
        {
            _myThreadPool = myThreadPool;
            _thread = new Thread(Start);
            _thread.Start();
        }

        private void Start()
        {
            while (true)
            {
                Action? action = null;
                while (action is null)
                {
                    lock (monitor)
                    {
                        if (!Actions.TryDequeue(out action))
                        {
                            Monitor.Wait(this);
                        }
                    }
                }

                action.Invoke();
            }
        }

        // private Action GetAction()
        // {
        //     Action? action = null;
        //     lock (monitor)
        //     {
        //         Actions.TryDequeue(out action);
        //     }
        //
        //     
        //     
        //     if (action is null)
        //     {
        //         
        //     }
        // } 
    }

    // public static void ThreadWork()
    // {
    //     Console.WriteLine(1);
    // }

    // public class MyThread : ProcessThread
    // {
    //     public MyThread()
    //     {
    //         
    //     }
    // }
}