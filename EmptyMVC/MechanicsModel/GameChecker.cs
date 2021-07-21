using System;
using System.Collections.Generic;
using System.Linq;
using RumExceptions;

namespace MechanicsModel
{
    public class GameChecker
    {
        private readonly CombinationChecker _combinationChecker;

        public GameChecker()
        {
            _combinationChecker = new CombinationChecker();
        }

        public (bool validationResult, List<CombinationModel> badCombinations, List<Card> badCards) ValidateGame(GameModel game)
        {
            var validationResult = true;
            var badCombinations = ValidateCombinations(game.Table);

            var badCards = ValidateCards(game.Table, game.Hand);

            if (badCards.Count != 0 || badCombinations.Count != 0)
            {
                validationResult = false;
            }
            else
            {
                SplitTableCombinations(game);
                ValidateCombinations(game.Table.Where(cmb => !cmb.isValid).ToList());
            }

            return (validationResult, badCombinations, badCards);
        }

        private void SplitTableCombinations(GameModel game)
        {
            for (var i = 0; i < game.Table.Count; i++)
            {
                var comb = game.Table[i];
                if (comb.Cards.Count > 5)
                {
                    game.Table.RemoveAt(i);
                    game.Table.InsertRange(i, SplitCombination(comb));
                }
            }
        }

        internal List<CombinationModel> SplitCombination(CombinationModel combModel)
        {
            var newCombinations = new List<CombinationModel>();

            for (int i = 0; i < combModel.Length / 3; i++)
            {
                var cardsToTake = i != combModel.Length / 3 - 1 ? 3 : combModel.Length - i * 3;
                newCombinations.Add(new CombinationModel(combModel.Cards.GetRange(i * 3, cardsToTake)));
            }

            return newCombinations;
        }

        private List<CombinationModel> ValidateCombinations(List<CombinationModel> allCombinations)
        {
            var badCombinations = new List<CombinationModel>();
            foreach (CombinationModel combination in allCombinations)
            {
                try
                {
                    (var checkResult, var combType) = _combinationChecker.CheckCombination(combination);
                    if (checkResult)
                    {
                        combination.isValid = true;
                        combination.Type = combType;
                    }
                    else
                    {
                        combination.isValid = false;
                        combination.Type = CombinationType.Unknown;
                        badCombinations.Add(combination);
                    }
                }
                catch (RumException re)
                {
                    combination.isValid = false;
                    combination.Type = CombinationType.Unknown;
                    badCombinations.Add(combination);
                }
                catch (Exception e)
                {
                    throw new RumException(ExceptionType.CombinationCheckerError00,
                        $"Ошибка при валидации комбинации {combination.ToStringRaw()}", e);
                }
            }

            return badCombinations;
        }

        private List<Card> ValidateCards(List<CombinationModel> allCombinations, List<Card> handCards)
        {
            var badCards = new List<Card>();
            var listOfAllCards = new List<Card>();
            listOfAllCards.AddRange(handCards);
            allCombinations.ForEach(cmb => listOfAllCards.AddRange(cmb.Cards));
            var setOfAllCards = new HashSet<Card>(listOfAllCards);
            foreach (Card card in setOfAllCards)
            {
                if (listOfAllCards.Count(c => c.Equals(card)) > 2)
                {
                    badCards.Add(card);
                }
            }

            return badCards;
        }
    }
}