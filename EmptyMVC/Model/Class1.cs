using System;
using Sonnet;
using COIN;

namespace MathModel
{
    public class Class1
    {
        public Class1()
        {

            Model model = new Model();
            Variable x = new Variable();
            Variable y = new Variable();
            model.Add(2 * x + 3 * y <= 10);
            model.Objective = 3 * x + y;
            Solver solver = new Solver(model, typeof(OsiClpSolverInterface));
            solver.Maximise();
            Console.WriteLine(solver.ToSolutionString());
        }
    }
}
