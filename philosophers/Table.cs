using System;
using System.Threading;

namespace DiningPhilosophers
{
    class Table
    {
        private readonly SemaphoreSlim[] forks;
        private readonly SemaphoreSlim waiter;
        private static readonly object consoleLock = new object(); // Об'єкт для синхронізації логування

        public Table(int forksCount)
        {
            forks = new SemaphoreSlim[forksCount];
            for (int i = 0; i < forksCount; i++)
            {
                forks[i] = new SemaphoreSlim(1, 1); // Початкове значення 1, максимум 1
            }
            waiter = new SemaphoreSlim(forksCount - 1, forksCount - 1); // Дозволяє 4 філософам
        }

        private int LeftForkId(int philosopherId)
        {
            return philosopherId;
        }

        private int RightForkId(int philosopherId)
        {
            return (philosopherId + 1) % forks.Length;
        }

        public void RequestForks(Philosopher philosopher)
        {
            waiter.Wait(); // Запит дозволу в офіціанта
            int left = LeftForkId(philosopher.Id);
            int right = RightForkId(philosopher.Id);

            forks[left].Wait();
            lock (consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Philosopher {philosopher.Id} took left fork ({left})");
                Console.ResetColor();
            }
            forks[right].Wait();
            lock (consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Philosopher {philosopher.Id} took right fork ({right})");
                Console.ResetColor();
            }
        }

        public void ReturnForks(Philosopher philosopher)
        {
            int left = LeftForkId(philosopher.Id);
            int right = RightForkId(philosopher.Id);

            forks[left].Release();
            forks[right].Release();
            lock (consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Philosopher {philosopher.Id} put down both forks");
                Console.ResetColor();
            }
            waiter.Release(); // Повідомляємо офіціанта
        }
    }
}