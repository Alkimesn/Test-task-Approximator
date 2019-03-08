using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approximator
{
    public static class LagrApprox
    {
        public static Polynomial GetApprox(double[] xvals, double[] yvals)
        {
            if (xvals.Length != yvals.Length) throw new ArgumentException();
            int n = xvals.Length;
            Polynomial[] normpols = new Polynomial[n];
            for(int i=0;i<n;i++)
            {
                Polynomial normres = new Polynomial(1);
                for (int j = 0; j < n; j++)
                    if (j != i)
                        normres *= new Polynomial(-xvals[j], 1) / (xvals[i] - xvals[j]);
                normpols[i] = normres;
            }
            Polynomial res = new Polynomial(0);
            for (int i = 0; i < n; i++)
                res += yvals[i] * normpols[i];
            return res;
        }
    }
}
