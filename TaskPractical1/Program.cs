using System;
using System.Text;

namespace TaskPractical1
{
    internal class Program
    {
        public static void ShowDateAndTime()
        {
            Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}");
        }

        public static bool IsPrime(int num)
        {
            if (num <= 1) return false;
            if (num == 2) return true;
            for (int i = 2; i * i <= num; i++)
                if (num % i == 0) return false;
            return true;
        }

        public static List<int> GetPrimesInRange(Range range)
        {
            List<int> arr = new List<int>();
            for (int i = range.Start.Value; i <= range.End.Value; i++)
            {
                if(IsPrime(i))
                {
                    arr.Add(i);
                }
            }
            return arr;
        }

        static void Main(string[] args)
        {
            int option;
            Console.Write("1 - Current time and date, \n2 - Print prime range, \n3 - Array Calculations, \n4 - Array Continuation tasks: ");
            int.TryParse(Console.ReadLine(), out option);

            Console.Clear();
            switch (option)
            {
                case 1: Task1(); break;

                case 2: Task2(); break;

                case 3: Task3(); break;

                case 4: Task4(); break;
            }
        }

        public static void Task1()
        {
            int option;
            Console.Write("1 - Start, 2 - Task.Factory.StartNew, 3 - Task.Run: ");
            int.TryParse(Console.ReadLine(), out option);

            Console.Clear();
            if(option == 1)
            {
                Task task1 = new Task(() =>
                {
                    ShowDateAndTime();
                });
                task1.Start();
            }
            else if(option == 2)
            {
                Task task1 = Task.Factory.StartNew(() =>
                {
                    ShowDateAndTime();
                }, TaskCreationOptions.LongRunning);
            }
            else if (option == 3)
            {
                Task task1 = Task.Run(() =>
                {
                    ShowDateAndTime();
                });
            }
            Console.ReadKey();
        }
        public static void Task2()
        {
            Console.Write("Enter start range: ");
            int startRange;
            int.TryParse(Console.ReadLine(), out startRange);

            Console.Write("Enter end range: ");
            int endRange;
            int.TryParse(Console.ReadLine(), out endRange);

            if (startRange >= endRange)
            {
                Console.Error.WriteLine("Wrong input. Error");
                Console.ReadKey();
                return;
            }

            Range range = new Range(startRange, endRange);

            Console.Clear();
            Task<List<int>> task1 = Task.Run(() =>
            {
                return GetPrimesInRange(range);
            });
            
            Task.WaitAll(task1);

            foreach(int element in task1.Result)
            {
                Console.WriteLine(element);
            }
        }
        public static void Task3()
        {
            List<int> arr = new List<int>() { 1,2,3,4,5,6,7,8,9,0};

            Task<List<double>> taskResults = Task.Run(() =>
            {
                List<double> results = new List<double>() { arr.Min(), arr.Max(), arr.Average(), arr.Sum()};
                return results;
            });

            Task.WaitAll(taskResults);

            Console.WriteLine($"Minimum - {taskResults.Result.ElementAt(0)}, Maximum - {taskResults.Result.ElementAt(1)}, Average - {taskResults.Result.ElementAt(2)}, Sum - {taskResults.Result.ElementAt(3)}");
        }

        static void RemoveDuplicates(List<int> arr)
        {
            arr = arr.Distinct().ToList();

            foreach (int num in arr)
            {
                Console.Write(num + " ");
            }
        }

        static void SearchValue(List<int> arr, int target)
        {
            int index = arr.BinarySearch(target);
            if (index >= 0)
            {
                Console.WriteLine($"Value {target} was found at index {index}.");
            }
            else
            {
                Console.WriteLine($"Value {target} was not found in the array.");
            }
        }
        public static void Task4()
        {
            List<int> arr = new List<int>() { 0,2,7,3,6,5,6,5,8,9,7};

            Task task = Task.Run(() => RemoveDuplicates(arr))
                .ContinueWith(task => arr.Sort())
                .ContinueWith(task => SearchValue(arr, 5));

            Task.WaitAll(task);

            foreach(var i in arr)
            {
                Console.WriteLine(i);
            }
        }
    }
}
