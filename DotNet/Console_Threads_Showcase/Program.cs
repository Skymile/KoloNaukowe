// Task    | Do jednorazowego użytku, małe lub rzadko używane operacje
// Thread  | Do wielokrotnego, częstego, trwa długo
// Process | Całkowicie oddzielny 

var threads = new Threads();
Thread.Sleep(500);
Console.WriteLine(threads);

var t = Task.FromResult(
    new string[] { "a", "b" }
);

var coll = new System.Collections.Concurrent.BlockingCollection<int>();

public class Threads
{
    private readonly object obj = new();

    public Threads()
    {
        if (Monitor.TryEnter(obj))
            try
            {
                // asda
            }
            finally
            {
                Monitor.Exit(obj);
            }

        lock (obj)
        {
            // asda
        }

        MutexTest();
        if (t1?.ThreadState != ThreadState.Running)
            t1?.Start();
        t2?.Start();
    }

    private void MutexTest()
    {
        var mutex = new Mutex();

        t1 = new Thread(() =>
        {
            mutex.WaitOne();
            LongOperation();
            a += 4;
            mutex.ReleaseMutex();
        });

        t1.Start();

        t2 = new Thread(() =>
        {
            mutex.WaitOne();
            a *= 10;
            mutex.ReleaseMutex();
        });
    }

    private void SemaphoreTest()
    {
        var sem = new Semaphore(0, 2);

        t1 = new Thread(() =>
        {
            LongOperation();
            a += 4;
            sem.Release();
        });

        t2 = new Thread(() =>
        {
            sem.WaitOne();
            a *= 10;
        });
    }

    private void BarrierTest()
    {
        var barrier = new Barrier(2);

        t1 = new Thread(() =>
        {
            LongOperation();
            a += 4;
            barrier.SignalAndWait();
        });

        t2 = new Thread(() =>
        {
            barrier.SignalAndWait();
            a *= 10;
        });
    }

    public override string ToString() =>
        string.Join(' ', a, t1?.ThreadState, t2?.ThreadState);

    private void LongOperation() => Thread.Sleep(10);

    private int a;

    private Thread? t1;
    private Thread? t2;
}