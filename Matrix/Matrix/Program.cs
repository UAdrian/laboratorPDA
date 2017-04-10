using MPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Matrix
{
    class Program
    {
        static int rowA = 8;
        static int colA = 8;
        static int rowB = 8;
        static int colB = 8;

        static int[][] matA;
        static int[][] matB;
        static int[][] matR;
        static int portion;
        static int lb;
        static int ub;


        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                if (comm.Rank == 0)
                {
                    arrayCreate();
                    for(int i = 0; i<= comm.Size; i++)
                    {
                        portion = (rowA / (comm.Size - 1));
                        lb = (i - 1) * portion;
                        if(((i + 1) == comm.Size) && ((rowA % (comm.Size - 1)) != 0))
                        {
                            ub = rowA;
                        }
                        else
                        {
                            ub = lb + portion;
                        }
                        comm.ImmediateSend(lb, i, 1);
                        comm.ImmediateSend(ub, i, 2);
                        comm.ImmediateSend(matA[lb][0], i, 3);
                    }
                }
                comm.Broadcast(ref matB, rowB * colB);

                if(comm.Rank > 0)
                {
                    int u = comm.Receive<int>(0, 1);
                    int l = comm.Receive<int>(0, 2);
                    int[][] mA = comm.Receive<int[][]>(0, 3);

                    for (int i = l; i < u; i++)
                    {
                        for (int j = 0; j < colB; j++)
                        {
                            for (int k = 0; k < rowB; k++)
                            {
                                matR[i][j] += (mA[j][k] * matB[k][j]);
                            }
                        }
                    }
                    comm.ImmediateSend(lb, 0, 4);
                    comm.ImmediateSend(ub, 0, 5);
                    comm.ImmediateSend(matR[lb][0], 0, 6);
                }
              
                if(comm.Rank == 0)
                {
                    for(int i = 0; i < comm.Size; i++)
                    {
                        ub = comm.Receive<int>(i, 4);
                        lb = comm.Receive<int>(i, 5);
                        matR = comm.Receive<int[][]>(i, 6);
                    }
                }               
            }
            printArray();
        }

        static void arrayCreate()
        {
            for (int i = 0; i < rowA; i++)
            {
                for (int j = 0; j < colA; j++)
                {
                    matA[i][j] = i * j;
                }
            }
            for (int i = 0; i < rowB; i++)
            {
                for (int j = 0; j < colB; j++)
                {
                    matB[i][j] = i + j;
                }
            }
        }

        static void printArray()
        {
            for (int i = 0; i < rowA; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < colA; j++)
                    Console.Write("%8.2f  ", matA[i][j]);
            }
            Console.WriteLine("\n\n");
            for (int i = 0; i < rowB; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < colB; j++)
                    Console.Write("%8.2f  ", matB[i][j]);
            }
            Console.WriteLine("\n\n\n");
            for (int i = 0; i < rowA; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < colB; j++)
                    Console.Write("%8.2f  ", matR[i][j]);
            }
            Console.WriteLine("\n");
        }
    }
}

