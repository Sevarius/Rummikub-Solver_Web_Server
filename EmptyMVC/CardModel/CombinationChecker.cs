using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RumExceptions;

namespace MechanicsModel
{
    /// <summary>
    /// Реализует логику проверки комбинации фишек
    /// </summary>
    public class CombinationChecker
    {
        
        /// <summary>
        /// Основная функция проверки комбинации фишек
        /// </summary>
        /// <param name="combination">Комбинация фишек для проверки</param>
        /// <returns>Кортеж (валидна ли комбинация, тип кобинации)</returns>
        public (bool, CombinationType) CheckCombination(CombinationModel combination)
        {
            // проверки на правильность комбинации
            NullChecking(combination);

            CombinationLengthChecking(combination);

            JokerChecking(combination);

            CardsRepeatChecking(combination);

            // выясняем, верна ли комбинация
            var ColorFlag = CheckCombinationColorType(combination);

            var NumberFlag = CheckCombinationNumberType(combination);

            if (!ColorFlag && !NumberFlag)
                return (false, CombinationType.Unknown);

            return (true, ColorFlag ? CombinationType.Color : CombinationType.Value);
        }

        /// <summary>
        /// Проверка полей комбинации на null
        /// </summary>
        /// <param name="combination">Комбинация</param>
        private void NullChecking(CombinationModel combination)
        {
            if (combination is null)
            {
                throw new RumException(ExceptionType.CombinationCheckerError01, "Была подана Null'евая комбинация");
            }

            if (combination.Cards is null)
            {
                throw new RumException(ExceptionType.CombinationCheckerError01, "В комбинации был задан Null'евой список фишек");
            }

            foreach (var card in combination.Cards)
            {
                if (card is null)
                {
                    throw new RumException(ExceptionType.CombinationCheckerError01, "В комбинации находится Null'евая фишка");
                }
            }
        }

        /// <summary>
        /// Проверка длины комбинации
        /// </summary>
        /// <param name="combination">Комбинация</param>
        private void CombinationLengthChecking(CombinationModel combination)
        {
            if (combination.Cards.Count < 3)
            {
                throw new RumException(ExceptionType.CombinationCheckerError02, "В комбинации меньше трёх фишек");
            }

            if (combination.Cards.Count > 13)
            {
                throw new RumException(ExceptionType.CombinationCheckerError02, "В комбинации больше 13 фишек");
            }
        }

        /// <summary>
        /// Проверка на количество Джокеров в комбинации
        /// </summary>
        /// <param name="combination">Комбинация</param>
        private void JokerChecking(CombinationModel combination)
        {
            int jokerCount = combination.Cards.Where(x => x.IsJoker).Count();

            if (jokerCount > 2)
            {
                throw new RumException(ExceptionType.CombinationCheckerError03, "В комбинации более двух Джокеров");
            }
        }

        /// <summary>
        /// Проврека на повторяющиеся фишки в комбинации
        /// </summary>
        /// <param name="combination">Комбинация</param>
        private void CardsRepeatChecking(CombinationModel combination)
        {
            var listWithoutJokers = combination.Cards.Where(card => !card.IsJoker).ToList();

            var repetitions = listWithoutJokers.GroupBy(card => card).Where(g => g.Count() > 1).ToList();

            if (repetitions.Count > 0)
            {
                string errorMessage = "";
                repetitions.Select(g => $"\n{g.Key} : {g.Count()}").ToList().ForEach(x => errorMessage += x);
                throw new RumException(ExceptionType.CombinationCheckerError04, errorMessage);
            }
        }

        /// <summary>
        /// Проверка, что комбинация фишек идёт по цвету
        /// </summary>
        /// <param name="combination">Комбинация</param>
        /// <returns>Имеют ли фишки все один цвет и идут ли они в порядке возрастания значения</returns>
        private bool CheckCombinationColorType(CombinationModel combination)
        {
            var firstNonJokerindex = combination.Cards.IndexOf(combination.Cards.First(x => !x.IsJoker));

            var prevCard = combination.Cards[firstNonJokerindex];

            if (prevCard.Number < firstNonJokerindex + 1)
                return false;

            for (int i = firstNonJokerindex + 1; i < combination.Cards.Count; i++)
            {
                var currentCard = combination.Cards[i];

                if (currentCard.IsJoker)
                {
                    if (prevCard.Number == 13)
                        return false;

                    prevCard = new Card(prevCard.Color, prevCard.Number + 1);
                }
                else
                {
                    if (prevCard.Color != currentCard.Color || prevCard.Number + 1 != currentCard.Number)
                    {
                        return false;
                    }

                    prevCard = currentCard;
                }
            }

            return true;
        }

        /// <summary>
        /// Проверка, что комбинация фишек идёт по значения
        /// </summary>
        /// <param name="combination">Комбинация фишек</param>
        /// <returns>Имеют ли фишки одно значение и при этом все различаются по цвету</returns>
        private bool CheckCombinationNumberType(CombinationModel combination)
        {
            if (combination.Cards.Count > 4)
                return false;

            var comWithoutJokers = combination.Cards.Where(x => !x.IsJoker).ToList();

            if (comWithoutJokers.GroupBy(x => x.Number).Count() != 1)
                return false;

            if (comWithoutJokers.GroupBy(x => x.Color).Count() != comWithoutJokers.Count)
                return false;

            return true;
        }

    }
}
