using System;
using System.Linq;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;

class PolynomialApproximation
{
    static void Main(string[] args)
    {
        // Поліноміальна апроксимація
        double[] xValues = { 0, 1, 2, 3, 4 };
        double[] yValues = xValues.Select(x => Math.Pow(x, 3) + 3 * Math.Pow(x, 2) - 2 * Math.Pow(x, 3) + 4).ToArray();

        var coefficients = PolynomialFit(xValues, yValues);
        Console.WriteLine("Coefficients of the polynomial:");
        foreach (var coef in coefficients)
        {
            Console.WriteLine(coef);
        }

        // Квадратичний сплайн
        Console.WriteLine("\nQuadratic Spline:");
        double[] xSpline = { 0, 2, 4 };
        double[] ySpline = xSpline.Select(x => Math.Pow(x, 3) + 3 * Math.Pow(x, 2) - 2 * Math.Pow(x, 3) + 4).ToArray();
        double[] deriv = { DerivativeAt(xSpline[0]), DerivativeAt(xSpline[^1]) };

        var splineQuadratic = QuadraticSpline(xSpline, ySpline, deriv);
        Console.WriteLine("Spline equations:");
        foreach (var eq in splineQuadratic)
        {
            Console.WriteLine(eq);
        }

        // Лінійний сплайн
        Console.WriteLine("\nLinear Spline:");
        double[] xLinear = Enumerable.Range(0, 9).Select(i => i * 0.5).ToArray();
        double[] yLinear = xLinear.Select(x => Math.Pow(x, 3) + 3 * Math.Pow(x, 2) - 2 * Math.Pow(x, 3) + 4).ToArray();

        var splineLinear = LinearSpline(xLinear, yLinear);
        Console.WriteLine("Spline segments:");
        foreach (var segment in splineLinear)
        {
            Console.WriteLine(segment);
        }
    }

    static double[] PolynomialFit(double[] x, double[] y)
    {
        int n = x.Length;
        var A = Matrix<double>.Build.DenseOfArray(Enumerable.Range(0, n).Select(i =>
            Enumerable.Range(0, n).Select(j => Math.Pow(x[i], j)).ToArray()).ToArray());
        var b = Vector<double>.Build.DenseOfArray(y);

        var coefficients = A.Solve(b);
        return coefficients.ToArray();
    }

    static double DerivativeAt(double x)
    {
        return 8 * Math.Pow(x, 7) + 12 * Math.Pow(x, 3) - 6 * Math.Pow(x, 2);
    }

    static string[] QuadraticSpline(double[] x, double[] y, double[] derivatives)
    {
        int n = x.Length - 1;
        string[] equations = new string[n];
        for (int i = 0; i < n; i++)
        {
            double a = y[i];
            double b = derivatives[i];
            double c = (y[i + 1] - y[i] - b * (x[i + 1] - x[i])) / Math.Pow(x[i + 1] - x[i], 2);

            equations[i] = $"{a} + {b}*(x-{x[i]}) + {c}*(x-{x[i]})^2";
        }
        return equations;
    }

    static string[] LinearSpline(double[] x, double[] y)
    {
        int n = x.Length - 1;
        string[] segments = new string[n];
        for (int i = 0; i < n; i++)
        {
            double m = (y[i + 1] - y[i]) / (x[i + 1] - x[i]);
            segments[i] = $"{y[i]} + {m}*(x-{x[i]})";
        }
        return segments;
    }
}
