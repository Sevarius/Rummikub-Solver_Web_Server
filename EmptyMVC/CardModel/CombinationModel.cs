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
        public List<Card> Cards { get; set; }

        /// <summary>
        /// Тип комбинации
        /// </summary>
        public CombinationType Type = CombinationType.Unknown;

        /// <summary>
        /// Валидна ли комбинация фишек
        /// </summary>
        public bool isValid = false;

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
                sb.Append(' ');
            }

            return sb.ToString().TrimEnd(' ');
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
