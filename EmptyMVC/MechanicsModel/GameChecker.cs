using System;
using System.Collections.Generic;
using System.Linq;
using RumExceptions;

namespace MechanicsModel
{
    /// <summary>
    /// Класс проверки игровой ситуации
    /// </summary>
    public class GameChecker
    {
        private readonly CombinationChecker _combinationChecker;

        /// <summary>
        /// Конструктор
        /// </summary>
        public GameChecker()
        {
            _combinationChecker = new CombinationChecker();
        }

        /// <summary>
        /// Валидация игровой ситуации
        /// </summary>
        /// <param name="game">Объект игровой ситации</param>
        /// <returns>Кортеж (валидна ли игровая ситация, список всех невалидных комбинаций фишек, список всех невалидных фишек)</returns>
        /// <exception cref="RumException">Кидает исключение, если возникает невозможная игровая ситуация</exception>
        public (bool validationResult, List<CombinationModel> badCombinations, List<Card> badCards) ValidateGame(GameModel game)
        {
            if (!game.Hand.Any())
            {
                throw new RumException(ExceptionType.GameCheckerError01,"Нет фишек в руке, что невозможно");
            }

            var validationResult = true;
            var badCombinations = ValidateCombinations(game.Table);

            var badCards = ValidateCards(game.Table, game.Hand);

            if (badCards.Count != 0 || badCombinations.Count != 0)
            {
                validationResult = false;
            }
            else
            {
                var newCombinations = SplitTableCombinations(game);

                foreach (var combination in newCombinations)
                {
                    (combination.isValid, combination.Type) = _combinationChecker.CheckCombination(combination);
                }

                game.Table.AddRange(newCombinations);
            }

            return (validationResult, badCombinations, badCards);
        }

        /// <summary>
        /// Удаляет слишком длинные комбинации из игры и возворащает список список разделённых комбинаций
        /// </summary>
        /// <param name="game">Объект игровой ситации</param>
        /// <returns>Список маленьких разделённых комбинаций</returns>
        private List<CombinationModel> SplitTableCombinations(GameModel game)
        {
            var newCombinations = new List<CombinationModel>();
            for (var i = 0; i < game.Table.Count; i++)
            {
                var comb = game.Table[i];
                if (comb.Cards.Count > 5)
                {
                    game.Table.RemoveAt(i);
                    i--;
                    newCombinations.AddRange(SplitCombination(comb));
                }
            }

            return newCombinations;
        }

        /// <summary>
        /// Разделяет большую комбинацию на маленькие
        /// </summary>
        /// <param name="combModel">Большая комбинация</param>
        /// <returns>Список маленьких комбинаций</returns>
        private List<CombinationModel> SplitCombination(CombinationModel combModel)
        {
            var newCombinations = new List<CombinationModel>();

            for (int i = 0; i < combModel.Length / 3; i++)
            {
                var cardsToTake = i != combModel.Length / 3 - 1 ? 3 : combModel.Length - i * 3;
                newCombinations.Add(new CombinationModel(combModel.Cards.GetRange(i * 3, cardsToTake)));
            }

            return newCombinations;
        }

        /// <summary>
        /// Возвращает невалидные комбинации из общего списка комбинаций
        /// </summary>
        /// <param name="allCombinations">Список всех комбинаций</param>
        /// <returns>Список невалидных комбинаций</returns>
        /// <exception cref="RumException">Кидает исключение, если возникает необработанное исключение при валидации комбинации</exception>
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

        /// <summary>
        /// Валидирует имеющиеся фишки в игровой ситуации
        /// </summary>
        /// <param name="allCombinations">Все комбинации на столе</param>
        /// <param name="handCards">Фишки на руке</param>
        /// <returns>Список фишек, которые встречаются больше 2 раз в игровой ситуации</returns>
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