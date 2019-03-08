using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slar
{
    class Matrix
    {
        double[,] vals;
        public int height{get;private set;}
        public int width { get; private set; }
        public Matrix(double[,] vals, int height, int width)
        {
            this.vals = vals;
            this.height = height;
            this.width = width;
        }
        public static Matrix Zero(int height, int width)
        {
            double[,] vals = new double[height, width];
            return new Matrix(vals, height, width);
        }
        public static Matrix Unit(int size)
        {
            double[,] vals = new double[size, size];
            for (int i = 0; i < size; i++)
                vals[i, i] = 1;
            return new Matrix(vals, size, size);
        }
        public Matrix(Matrix A, Matrix B)
        {
            if (A.height != B.height) throw new ArgumentException();
            this.height = A.height;
            this.width = A.width + B.width;
            this.vals = new double[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    if (j < A.width) vals[i, j] = A[i, j];
                    else vals[i, j] = B[i, j - A.width];
        }
        public double[] GetRow(int rownum)
        {
            double[] row = new double[width];
            for (int i = 0; i < width; i++)
                row[i] = vals[rownum, i];
            return row;
        }
        public double[] GetCol(int colnum)
        {
            double[] col = new double[height];
            for (int i = 0; i < height; i++)
                col[i] = vals[i, colnum];
            return col;
        }
        public void MultiplyRow(int row, double coef)
        {
            for (int i = 0; i < width; i++)
            {
                vals[row, i] *= coef;
            }
        }
        public void SwapRows(int row1, int row2)
        {
            for(int i=0;i<width;i++)
            {
                double t = vals[row1, i];
                vals[row1, i] = vals[row2, i];
                vals[row2, i] = t;
            }
        }
        public void AddRowToRow(int addedrow, int targetrow, double coef = 1)
        {
            for (int i = 0; i < width; i++) vals[targetrow, i] += vals[addedrow, i] * coef;
        }
        public Matrix Transpose()
        {
            double[,] newvals = new double[width, height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    newvals[j, i] = vals[i, j];
                }
            }
            return new Matrix(newvals, width, height);
        }
        public double this[int row, int col]
        {
            get { return vals[row, col]; }
        }
        public static Matrix operator *(Matrix m, double c)
        {
            double[,] newvals = new double[m.height, m.width];
            for (int i = 0; i < m.height; i++)
                for (int j = 0; j < m.width; j++)
                    newvals[i, j] = m.vals[i, j] * c;
            return new Matrix(newvals, m.height, m.width);
        }
    }
}
