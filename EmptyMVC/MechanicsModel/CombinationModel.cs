using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicsModel
{
    public class CombinationModel
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
        /// Получение фишки из комбинации по индексу
        /// </summary>
        /// <param name="index">Индекс фишки</param>
        /// <returns>Фишка по индексу</returns>
        public Card this[int index] => this.Cards[index];

        /// <summary>
        /// Строковое представление комбинации
        /// </summary>
        /// <returns>Строковое представление комбинации</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("Combination:");

            foreach (var card in Cards)
            {
                sb.Append(card);
                sb.Append(Equals(card, Cards.Last()) ? ';' : ' ');
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
            this.Cards.ForEach(card => res += card.GetHashCode());
            return res;
        }

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
