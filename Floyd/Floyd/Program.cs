using MPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floyd
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = { 1, 2, 3, 4 };
            int n = 4;

            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;

                var watch = System.Diagnostics.Stopwatch.StartNew();

                floyd(n, n / comm.Size, array, comm);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

            }
        }

        static int floyd(int n, int np, int[] array, Intracommunicator com)
        {
            int[] row = new int[n];
            int[] col = new int[n];

            for (int k = 0; k < n; k++)
            {
                for(int i = 0; i <= n; i++)
                {
                    row[i] = array[(k % np) * n + i];
                }
                com.Barrier();
                com.Broadcast(ref row[0], k / np);

                for(int i = 0; i < np; i++ )
                {
                    col[i] = array[i * n + k];
                }

                for(int i = 0; i < np; i++)
                {
                    for(int j = 0; j < n; j++)
                    {
                        array[i * n + j] = array[i * n + j] < col[i] + row[j] ? array[i * n + j] : col[i] + row[j];
                    }
                }
            }
            return 0;
        }
    }
}