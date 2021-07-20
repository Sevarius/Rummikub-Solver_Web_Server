using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using COIN;
using MechanicsMaps;
using MechanicsModel;
using Sonnet;
using Constraint = Sonnet.Constraint;
using Expression = Sonnet.Expression;

namespace MathModel
{
    public class MathProblem
    {
        private readonly GameModel _game;
        private readonly CombinationsMap _map;

        public MathProblem(GameModel game, CombinationsMap map)
        {
            _game = game;
            _map = map;
        }

        public void Solve()
        {
            var model = ConstructModel();

            Solver solver = new Solver(model, typeof(OsiCbcSolverInterface));

            solver.Solve();
            
            
        }

        public Model ConstructModel()
        {
            var model = new Model("Rummikub model");
            (var combinationVariables, var cardVariables) = ConstructVariables();

            (var tableConstraints, var handConstraints) = ConstructConstraints(combinationVariables, cardVariables);

            var objective = ConstructObjective(cardVariables);

            model.Add(tableConstraints);
            model.Add(handConstraints);
            model.ObjectiveSense = ObjectiveSense.Maximise;
            model.Objective = objective;

            return model;
        }

        public (List<Variable>, List<Variable>) ConstructVariables()
        {
            var combinationVariables = new List<Variable>(_map.CombinationCount);
            for (int i = 0; i < _map.CombinationCount; i++)
            {
                combinationVariables.Add(new Variable($"Set {i+1}", 0, 2, VariableType.Integer));
            }

            var cardVariables = new List<Variable>(_map.CombinationCount);

            for (int i = 0; i < _map.CardCount; i++)
            {
                cardVariables.Add(new Variable($"Card {i + 1}", 0, 2, VariableType.Integer));
            }

            return (combinationVariables, cardVariables);
        }

        public (List<Constraint>, List<Constraint>) ConstructConstraints(List<Variable> combinationVariables, List<Variable> cardVariables)
        {
            var tableConstraints = new List<Constraint>();
            var handConstraints = new List<Constraint>();

            foreach (Card card in _map.Cards)
            {
                var lhsTableConstraint = Expression.ScalarProduct(_map.GetCoefficientsForCard(card), combinationVariables);
                var rhsTableConstraint = _game.CountCardOnTable(card) + cardVariables[_map.GetCardIndex(card)];
                tableConstraints.Add(new Constraint($"table constraint for card: {card}", lhsTableConstraint, ConstraintType.EQ, rhsTableConstraint));

                var lhsHandConstraint = new Expression(cardVariables[_map.GetCardIndex(card)]);
                var rhsHandConstraint = new Expression(_game.CountCardInHand(card));
                handConstraints.Add(new Constraint($"hand constraint for card: {card}", lhsHandConstraint, ConstraintType.LE, rhsHandConstraint));
            }

            return (tableConstraints, handConstraints);
        }

        public Expression ConstructObjective(List<Variable> cardVariables)
        {
            return Expression.Sum(cardVariables);
        }
    }
}