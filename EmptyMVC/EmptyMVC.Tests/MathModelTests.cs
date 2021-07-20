using System;
using System.Collections.Generic;
using COIN;
using Xunit;

using MathModel;
using MechanicsModel;
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
            _testOutputHelper.WriteLine("Началась генерация модели");
            var start = DateTime.Now;
            _map.GenerateMaps();
            var spendTime = DateTime.Now - start;
            _testOutputHelper.WriteLine($"Модель была сгенерирована за {spendTime.Seconds} секунд");
        }

        [Fact]
        public void FirstRealTest()
        {
            var game = new GameModel
            {
                Hand = new List<Card>
                {
                    new Card(CardColor.Blue, 4),
                    new Card(CardColor.Blue, 6)
                },
                Table = new List<CombinationModel>
                {
                    new CombinationModel(new List<Card>
                    {
                        new Card(CardColor.Blue, 1),
                        new Card(CardColor.Blue, 2),
                        new Card(CardColor.Blue, 3),
                        new Card(CardColor.Blue, 4),
                        new Card(CardColor.Blue, 5)
                    })
                    {
                        isValid = true,
                        Type = CombinationType.Color
                    },
                }
            };

            var model = new MathProblem(game, _map);
            _testOutputHelper.WriteLine("Решение задачи");
            (var combinationsOnTable, var cardsToPutFromHand, var objValue) = model.Solve();
            if (combinationsOnTable is null || cardsToPutFromHand is null)
            {
                _testOutputHelper.WriteLine("Задача не была решена корректно");
            }
            else
            {
                _testOutputHelper.WriteLine("Задача была решена корректно");
                _testOutputHelper.WriteLine($"Значение целевой функции: {objValue}");
                _testOutputHelper.WriteLine($"{combinationsOnTable.Count} комбинаций на столе\n{cardsToPutFromHand.Count} фишек надо выложить с руки");
                _testOutputHelper.WriteLine("Комбинации:");
                combinationsOnTable.ForEach(c => _testOutputHelper.WriteLine(c.ToStringRaw()));
                _testOutputHelper.WriteLine("Фишки:");
                cardsToPutFromHand.ForEach(c => _testOutputHelper.WriteLine(c.ToString()));
            }
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
