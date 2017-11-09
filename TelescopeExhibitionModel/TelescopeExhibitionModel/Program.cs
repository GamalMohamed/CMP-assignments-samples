using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelescopeExhibitionModel
{
    class Program
    {
        public static Queue<int> PrivilegeQueue;
        public static Queue<int> NonPrivilegeQueue;
        public static List<double> Arrivals;
        public static List<double> ServiceDuration;
        public static double PrivligedWaitedTime;
        public static double NonPrivligedWaitedTime;
        public static double TimeStamp;
        public static double LasrSerivceDuration;


        public static void Initialize(double a, double s)
        {
            PrivilegeQueue = new Queue<int>();
            NonPrivilegeQueue = new Queue<int>();
            Arrivals = new List<double>();
            ServiceDuration = new List<double>();
            TimeStamp = LasrSerivceDuration = 0.0;

            GenerateRandomDurations(a, s);
        }

        public static void GenerateRandomDurations(double a, double s)
        {
            var timeStamp = 0.0;
            var u = new Random();
            while (true)
            {
                var arrivalRv = Math.Log10(1 - u.NextDouble()) / -(1 / a);
                timeStamp += arrivalRv;
                if (timeStamp >= 360)
                {
                    break;
                }
                Arrivals.Add(timeStamp);

                var serviceRv = Math.Log10(1 - u.NextDouble()) / -(1 / s);
                ServiceDuration.Add(serviceRv);
            }
        }

        public static void ProcessQueue(double currentArrivalTime)
        {
            while (true)
            {
                // Handling gaps
                if (LasrSerivceDuration + TimeStamp > currentArrivalTime)
                {
                    LasrSerivceDuration -= currentArrivalTime - TimeStamp;
                    TimeStamp = currentArrivalTime;
                    break;
                }

                TimeStamp += LasrSerivceDuration;

                // Priority-based computations
                if (PrivilegeQueue.Count != 0)
                {
                    var c = PrivilegeQueue.Dequeue();
                    PrivligedWaitedTime += TimeStamp - Arrivals[c];
                    LasrSerivceDuration = ServiceDuration[c];
                }
                else if (NonPrivilegeQueue.Count != 0)
                {
                    var c = NonPrivilegeQueue.Dequeue();
                    NonPrivligedWaitedTime += TimeStamp - Arrivals[c];
                    LasrSerivceDuration = ServiceDuration[c];
                }
                else
                {
                    LasrSerivceDuration = 0;
                    TimeStamp = currentArrivalTime;
                    break;
                }
            }
        }

        static void Main(string[] args)
        {
            Console.Write("Enter n: ");
            var n = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter a: ");
            var a = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter s: ");
            var s = Convert.ToDouble(Console.ReadLine());

            int privilegedCount;
            var nonPrivilegeCount = privilegedCount = 0;
            for (int i = 0; i < n; i++)
            {
                Initialize(a, s);
                int cPrivCount=0;
                var p = new Random();
                for (int j = 0; j < Arrivals.Count; j++)
                {
                    ProcessQueue(Arrivals[j]);

                    // One arrives while other inside
                    if (NonPrivilegeQueue.Count != 0)
                    {
                        //Decide whether to go priv or not!
                        if (p.NextDouble() > 0.5 && cPrivCount <= 0.5 * Arrivals.Count)
                        {
                            NonPrivilegeQueue.Enqueue(j);
                            nonPrivilegeCount++;
                        }
                        else
                        {
                            PrivilegeQueue.Enqueue(j);
                            privilegedCount++;
                            cPrivCount++;
                        }
                    }
                    else
                    {
                        if (PrivilegeQueue.Count != 0)
                        {
                            //Decide whether to go priv or not!
                            if (p.NextDouble() > 0.5 && PrivilegeQueue.Count <= 0.5 * Arrivals.Count)
                            {
                                NonPrivilegeQueue.Enqueue(j);
                                nonPrivilegeCount++;
                            }
                            else
                            {
                                PrivilegeQueue.Enqueue(j);
                                privilegedCount++;
                                cPrivCount++;
                            }
                        }
                        else
                        {
                            NonPrivilegeQueue.Enqueue(j);
                            nonPrivilegeCount++;
                        }
                    }
                }
            }

            Console.WriteLine("\nAvg total waiting time: {0}", (PrivligedWaitedTime + NonPrivligedWaitedTime) / (privilegedCount + nonPrivilegeCount));
            Console.WriteLine("Avg waiting time priv: {0}", PrivligedWaitedTime / privilegedCount);
            Console.WriteLine("Avg waiting time non-priv: {0}", NonPrivligedWaitedTime / nonPrivilegeCount);

            Console.WriteLine("Profit of Privilege tickets: {0}\n", privilegedCount * 30 / 1000); // Not sure!!

            Console.ReadKey();
        }
    }
}
