using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RumExceptions;

namespace MechanicsModel
{
    public class Card : IComparable<Card>
    {
        public Card(CardColor Color, int Number)
        {
            this.Color = Color;

            if (Color is CardColor.Joker)
            {
                this.Number = 0;
                return;
            }

            if (Number > 13)
            {
                throw new RumException(ExceptionType.CardError01, "Значение фишки не может быть больше 13");
            }

            if (Number < 1)
            {
                throw new RumException(ExceptionType.CardError01, "Значение фишки не может быть меньше 1");
            }

            this.Number = Number;
        }

        public CardColor Color { get; }

        public int Number { get; }

        public bool IsJoker => this.Color == CardColor.Joker;
        
        /// <summary>
        /// Возвращает строковое представление фишки
        /// </summary>
        /// <returns>Строковое представление фишки</returns>
        public override string ToString()
        {
            return this.Color != CardColor.Joker ? $"{this.Number}_{Convert.ToString(this.Color)}" : Convert.ToString(this.Color);
        }

        public override bool Equals(object obj)
        {
            var anotherCard = obj as Card;

            if (anotherCard is null)
            {
                return false;
            }

            if (this.Color != anotherCard.Color || this.Number != anotherCard.Number)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.Number + (int)this.Color;
        }

        public int CompareTo(Card other)
        {
            var colorCompare = ((int)Color).CompareTo((int)other.Color);

            if (colorCompare == 0)
            {
                return Number.CompareTo(other.Number);
            }

            return colorCompare;
        }
    }

    public enum CardColor
    {
        Red = 0,
        Yellow,
        Blue,
        Black,
        Joker
    }
}
