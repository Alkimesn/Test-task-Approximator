using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slar
{
    static class Gauss
    {
        static Matrix GaussUD(Matrix a, Matrix B)
        {
            Matrix A = a*1;
            for (int i = 0; i < A.height; i++)
            {
                if (A[i, i] == 0)
                {
                    int j = i + 1;
                    while (A[j, i] == 0)
                    {
                        j++;
                        if (j >= A.height) throw new Exception("Unsolvable system");
                    }
                    A.SwapRows(i, j);
                    B.SwapRows(i, j);
                }
                double c = A[i, i];
                A.MultiplyRow(i, 1 / c);
                B.MultiplyRow(i, 1 / c);
                for (int k = i + 1; k < A.height; k++)
                {
                    c = A[k, i];
                    A.AddRowToRow(i, k, -c);
                    B.AddRowToRow(i, k, -c);
                }
            }
            for (int i = A.height - 1; i > 0; i--)
            {
                double c;
                for (int k = i - 1; k >= 0; k--)
                {
                    c = A[k, i];
                    A.AddRowToRow(i, k, -c);
                    B.AddRowToRow(i, k, -c);
                }
            }
            return B;
        }
        public static double[] Solve(Matrix A, Matrix B)
        {
            return GaussUD(A,B).GetCol(0);
        }
    }
}
