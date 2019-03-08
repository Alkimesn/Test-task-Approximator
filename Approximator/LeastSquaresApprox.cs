using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slar;

namespace Approximator
{
    public static class LeastSquaresApprox
    {
        const int matrSize = 4;
        public static Polynomial GetApprox(double[] xvals, double[] yvals)
        {
            if (xvals.Length != yvals.Length) throw new ArgumentException();
            int n = xvals.Length;
            double[,] coefs = new double[matrSize,matrSize];
            double GetCoef(int row, int column)
            {
                double res = 0;
                for (int i = 0; i < n; i++)
                    res += Math.Pow(xvals[i], row + column);
                return res;
            }
            for (int i = 0; i < matrSize; i++)
                for (int j = 0; j < matrSize; j++)
                    coefs[i, j] = GetCoef(i, j);
            double GetFr(int row)
            {
                double res = 0;
                for (int i = 0; i < n; i++)
                    res += Math.Pow(xvals[i], row) * yvals[i];
                return res;
            }
            double[,] fr = new double[matrSize,1];
            for (int i = 0; i < matrSize; i++)
                fr[i,0] = GetFr(i);
            double[] solution = Gauss.Solve(new Matrix(coefs, matrSize, matrSize), new Matrix(fr, matrSize,1));
            return new Polynomial(solution);
        }
    }
}
