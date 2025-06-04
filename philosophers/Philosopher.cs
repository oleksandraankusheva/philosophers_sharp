using System;
using System.Threading;

namespace DiningPhilosophers
{
    class Philosopher
    {
        public int Id { get; }
        private readonly Table table;
        private static readonly object consoleLock = new object(); // Об'єкт для синхронізації логування

        public Philosopher(int id, Table table)
        {
            Id = id;
            this.table = table;
        }

        public void Run()
        {
            for (int i = 0; i < 1; i++)
            {
                try
                {
                    lock (consoleLock)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Philosopher {Id} is thinking {i} time.");
                        Console.ResetColor();
                    }
                    Thread.Sleep(500); // Імітація роздумів

                    lock (consoleLock)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Philosopher {Id} asks waiter to eat {i} time.");
                        Console.ResetColor();
                    }
                    table.RequestForks(this);

                    lock (consoleLock)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Philosopher {Id} is eating {i} time.");
                        Console.ResetColor();   
                    }
                    Thread.Sleep(1000); // Імітація їжі

                    table.ReturnForks(this);
                }
                catch (Exception ex)
                {
                    lock (consoleLock)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"Philosopher {Id} encountered an error: {ex.Message}");
                        Console.ResetColor();
                    }
                    throw;
                }
            }
        }
    }
}