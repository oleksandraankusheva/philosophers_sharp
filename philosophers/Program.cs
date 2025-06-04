using System;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    class Program
    {
        static void Main(string[] args)
        {
            int philosophersCount = 5;

            Table table = new Table(philosophersCount);
            Task[] philosophers = new Task[philosophersCount];

            for (int i = 0; i < philosophersCount; i++)
            {
                int id = i; // Зберігаємо id для замикання
                philosophers[i] = Task.Run(() => new Philosopher(id, table).Run());
            }

            Task.WaitAll(philosophers); // Чекаємо завершення всіх потоків
        }
    }
}