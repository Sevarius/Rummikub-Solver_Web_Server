using System.Collections.Generic;
using System.Linq;

namespace MechanicsModel
{
    /// <summary>
    /// Модель игровой ситуации, состоящая из комбинаций на столе и фишек на руке игрока
    /// </summary>
    public sealed class GameModel
    {
        /// <summary>
        /// Комбинации на столе
        /// </summary>
        public List<CombinationModel> Table { get; set; }

        /// <summary>
        /// Фишки на руке игрока
        /// </summary>
        public List<Card> Hand { get; set; }

        /// <summary>
        /// Валидна ли игровая ситуация
        /// </summary>
        public bool IsValid;

        /// <summary>
        /// Подсчёт количества повторений фишки в руке игрока
        /// </summary>
        /// <param name="card">Фишка для подсчёта количества</param>
        /// <returns>Количество повторений данной фишки в руке игрока</returns>
        public int CountCardInHand(Card card)
        {
            return Hand.Count(x => x.Equals(card));
        }

        /// <summary>
        /// Подсчёт количества повторений фишки в коминациях на столе
        /// </summary>
        /// <param name="card">Фишка для подсчёта количества</param>
        /// <returns>Количество повторений данной фишки в комбинациях на столе</returns>
        public int CountCardOnTable(Card card)
        {
            return Table.Select(x => x.CountCard(card)).Sum();
        }
    }
}