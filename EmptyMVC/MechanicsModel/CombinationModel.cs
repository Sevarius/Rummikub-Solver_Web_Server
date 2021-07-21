using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicsModel
{
    public class CombinationModel : IEnumerable<Card>
    {
        /// <summary>
        /// Последовательность фишек
        /// </summary>
        public List<Card> Cards { get; }

        /// <summary>
        /// Тип комбинации
        /// </summary>
        public CombinationType Type;

        /// <summary>
        /// Валидна ли комбинация фишек
        /// </summary>
        public bool isValid;

        /// <summary>
        /// Создаёт комбинацию непроверенную фишек
        /// </summary>
        /// <param name="cards">Последовательность фишек</param>
        public CombinationModel(List<Card> cards)
        {
            Cards = cards;
            isValid = false;
            Type = CombinationType.Unknown;
        }

        /// <summary>
        /// Проверяет содержится ли комбинация данную фишку
        /// </summary>
        /// <param name="card">Фишка для проверки</param>
        /// <returns>Содержит или не содержит</returns>
        public bool ContainsCard(Card card) => Cards.Contains(card);

        /// <summary>
        /// Возвращает количество повторений данной фишки в комбинации
        /// </summary>
        /// <param name="card">Фишка</param>
        /// <returns>Количество повторений</returns>
        public int CountCard(Card card) => Cards.Count(x => x.Equals(card));

        /// <summary>
        /// Получение фишки из комбинации по индексу
        /// </summary>
        /// <param name="index">Индекс фишки</param>
        /// <returns>Фишка по индексу</returns>
        public Card this[int index] => Cards[index];

        /// <summary>
        /// Длина комбинации
        /// </summary>
        public int Length => Cards.Count;

        public IEnumerator<Card> GetEnumerator() => Cards.GetEnumerator();

        /// <summary>
        /// Строковое представление комбинации
        /// </summary>
        /// <returns>Строковое представление комбинации</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("Combination: ");

            for (var i = 0; i < Cards.Count; i++)
            {
                sb.Append(Cards[i]);
                sb.Append(i == Cards.Count - 1 ? "; " : " ");
            }

            sb.Append($"Is valid: {isValid}; Combination type: {Convert.ToString(Type)};");

            return sb.ToString().TrimEnd(' ');
        }

        /// <summary>
        /// Строковое представление комбинации, только фишки
        /// </summary>
        /// <returns>Строковое представление комбинации, только фишки</returns>
        public string ToStringRaw()
        {
            var sb = new StringBuilder();

            foreach (var card in Cards)
            {
                sb.Append(card);
                sb.Append(' ');
            }

            return sb.ToString().TrimEnd(' ');
        }

        public override int GetHashCode()
        {
            int res = 0;
            Cards.ForEach(card => res += card.GetHashCode());
            return res;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object obj)
        {
            if (!(obj is CombinationModel otherComb))
            {
                return false;
            }

            if (this.Type != otherComb.Type ||
                this.isValid != otherComb.isValid ||
                this.Cards.Count != otherComb.Cards.Count)
            {
                return false;
            }

            for (int i = 0; i < this.Cards.Count; i++)
            {
                if (!this[i].Equals(otherComb[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Тип комбинации
    /// </summary>
    public enum CombinationType
    {
        /// <summary>
        /// Пока что не известно
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// По значению
        /// </summary>
        Value,

        /// <summary>
        /// По цвету
        /// </summary>
        Color
    }
}
