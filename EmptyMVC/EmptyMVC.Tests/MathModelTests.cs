using System;
using System.Collections.Generic;
using COIN;
using Xunit;

using MathModel;
using MechanicsMaps;
using Sonnet;
using Xunit.Abstractions;


namespace EmptyMVC.Tests
{
    public class MathModelTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly CombinationsMap _map;
        public MathModelTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _map = new CombinationsMap();
            _map.GenerateMaps();
        }

        [Fact(Skip = "skip")]
        public void SimpleCheck()
        {
            Model model = new Model();
            List<Variable> variables = new List<Variable>();
            for (int i = 0; i < 10; i++)
            {
                variables.Add(new Variable(0,2, VariableType.Integer));
            }

            Constraint con = new RangeConstraint(4,
                Expression.ScalarProduct(new List<double>() {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, variables), 6);
            model.Add("seva", con);
            model.Objective = new Objective("max",
                Expression.ScalarProduct(new List<double>() {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, variables));
            model.ObjectiveSense = ObjectiveSense.Maximise;
            Solver solver = new Solver(model, typeof(OsiCbcSolverInterface));
            solver.Export("seva.lp");
            
            var warmStart = solver.GetEmptyWarmStart();
            solver.SetWarmStart(warmStart);
        }

        [Fact(Skip = "skip")]
        public void SimpleTest()
        {
            Model model = new Model("Seva");
            int variableCount = 500_000;
            var varList = new List<Variable>(variableCount);
            _testOutputHelper.WriteLine("start");
            for (int i = 0; i < variableCount; i++)
            {
                varList.Add(new Variable(-1000, 1000, VariableType.Integer));
            }
            _testOutputHelper.WriteLine("variables done");
            for (int i = 0; i < variableCount; i += 2)
            {
                model.Add(varList[i] + varList[i + 1] == 1);
            }
            _testOutputHelper.WriteLine("constraints done");

            var solution = new List<double>(variableCount);
            for (int i = 0; i < variableCount; i += 2)
            {
                solution.AddRange(new []{1000D,-999D});
            }
            _testOutputHelper.WriteLine("solution done");
            
            model.Objective = Expression.Sum(varList);
            Solver solver = new Solver(model, typeof(COIN.OsiCbcSolverInterface));
            var cbc = (solver.OsiSolver as OsiCbcSolverInterface).getModelPtr() as CbcModel;
            cbc.setWarmStart(solution.ToArray());
            //cbc.setMaxSeconds(3);
            var start = DateTime.Now;
            solver.Maximise();
            var time = DateTime.Now - start;
            _testOutputHelper.WriteLine(time.Seconds.ToString());
            _testOutputHelper.WriteLine(solver.IsProvenOptimal.ToString());
            //_testOutputHelper.WriteLine(solver.ToSolutionString());
            for (int i = 0; i < variableCount; i++)
            {
                if (solution[i] != varList[i].Value)
                {
                    _testOutputHelper.WriteLine($"{i} : {solution[i]} : {varList[i].Value}");
                }
            }
            
            Model model2 = new Model("Seva2");
            varList = new List<Variable>(variableCount);
            _testOutputHelper.WriteLine("start");
            for (int i = 0; i < variableCount; i++)
            {
                varList.Add(new Variable(-1000, 1000, VariableType.Integer));
            }
            _testOutputHelper.WriteLine("variables done");
            for (int i = 0; i < variableCount; i += 2)
            {
                model2.Add(varList[i] + varList[i + 1] == 1);
            }
            _testOutputHelper.WriteLine("constraints done");

            model2.Objective = Expression.Sum(varList);
            Solver solver2 = new Solver(model2, typeof(COIN.OsiCbcSolverInterface));
            cbc = (solver2.OsiSolver as OsiCbcSolverInterface).getModelPtr() as CbcModel;
            solver2.SetWarmStart(solver.GetWarmStart());
            cbc.setMaxSeconds(3);
            start = DateTime.Now;
            solver.Maximise();
            time = DateTime.Now - start;
            _testOutputHelper.WriteLine(time.Seconds.ToString());
            _testOutputHelper.WriteLine(solver.IsProvenOptimal.ToString());
            //_testOutputHelper.WriteLine(solver.ToSolutionString());
            // for (int i = 0; i < variableCount; i++)
            // {
            //     if (solution[i] != varList[i].Value)
            //     {
            //         _testOutputHelper.WriteLine($"{i} : {solution[i]} : {varList[i].Value}");
            //     }
            // }

        }
    }
}
