using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approximator
{
    public class Polynomial
    {
        double[] coefs;
        public Polynomial(params double[] coefs)
        {
            this.coefs = new double[coefs.Length];
            for (int i = 0; i < coefs.Length; i++) this.coefs[i] = coefs[i];
        }
        public static Polynomial operator +(Polynomial p1, Polynomial p2)
        {
            if(p1.coefs.Length<p2.coefs.Length)
            {
                Polynomial t = p1;
                p1 = p2;
                p2 = t;
            }
            int n1 = p1.coefs.Length;
            int n2 = p2.coefs.Length;
            double[] coefs = new double[n1];
            for(int i=0;i<n1;i++)
            {
                if (i < n2) coefs[i] = p1.coefs[i] + p2.coefs[i];
                else coefs[i] = p1.coefs[i];
            }
            return new Polynomial(coefs);
        }
        public static Polynomial operator *(Polynomial p, double c)
        {
            int n = p.coefs.Length;
            double[] coefs = new double[n];
            for (int i = 0; i < n; i++)
            {
                coefs[i] = p.coefs[i]*c;
            }
            return new Polynomial(coefs);
        }
        public static Polynomial operator *(double c, Polynomial p)
        {
            return p * c;
        }
        public static Polynomial operator /(Polynomial p, double c)
        {
            return p * (1/c);
        }
        Polynomial MonoMult(double c, int power)
        {
            int n = this.coefs.Length + power;
            double[] coefs = new double[n];
            for (int i = 0; i < n; i++)
            {
                if (i < power) coefs[i] = 0;
                else coefs[i] = c*this.coefs[i - power];
            }
            return new Polynomial(coefs);
        }
        public static Polynomial operator *(Polynomial p1, Polynomial p2)
        {
            Polynomial res = new Polynomial(0);
            for (int i = 0; i < p2.coefs.Length; i++)
                res += p1.MonoMult(p2.coefs[i], i);
            return res;
        }
        public bool IsZero()
        {
            const double eps= 0.000001;
            foreach (double c in coefs)
                if (Math.Abs(c)>eps) return false;
            return true;
        }
        public double Calculate(double x)
        {
            double X = 1;
            double res = 0;
            for(int i=0;i<coefs.Length;i++)
            {
                res += coefs[i] * X;
                X *= x;
            }
            return res;
        }
    }
}
