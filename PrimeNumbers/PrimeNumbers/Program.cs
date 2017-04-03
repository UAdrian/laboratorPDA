using MPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 20;
            int m = 5;
            int poz = n / m;
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                if (comm.Rank == 0)
                {
                    // program for rank 0
                    comm.Send(poz + 1, 1, 0);

                    for(int i = 2; i < poz + 1; i++)
                    {
                        if (i == 2 || isPrime(i))
                        {
                            Console.WriteLine("Rank " + comm.Rank + " found that " + i + " is prime!");
                        }
                    }

                    int msg = comm.Receive<int>(Communicator.anySource, 0);
                }
                else // not rank 0
                {
                    // program for all other ranks
                    int msg = comm.Receive<int>(comm.Rank - 1, 0);
                    comm.Send(msg + poz, (comm.Rank + 1) % comm.Size, 0);

                    //Console.WriteLine(msg);

                    for (int i = msg; i < msg + poz; i++)
                    {
                        if (isPrime(i) || i == 2)
                        {
                            Console.WriteLine("Rank " + comm.Rank + " found that " + i + " is prime!");
                        }
                    }                    
                }
            }
        }

        private static bool isPrime(int i)
        {
            for (int j = 2; j <= i / 2; j++)
            {
                if(i % j == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
