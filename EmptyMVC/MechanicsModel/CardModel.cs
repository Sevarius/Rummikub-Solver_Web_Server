using System;
using System.Collections.Generic;
using RumExceptions;

namespace MechanicsModel
{
    public sealed class Card : IComparable<Card>
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
            string str = Color != CardColor.Joker ? $"{Number}_{Convert.ToString(Color)}" : Convert.ToString(Color);
            return new string(' ', 9 - str.Length) + str;
        }

        public override bool Equals(object obj)
        {
            var anotherCard = obj as Card;

            if (anotherCard is null)
            {
                return false;
            }

            if (Color != anotherCard.Color || Number != anotherCard.Number)
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

        public static List<Card> AllPossibleCards()
        {
            var res = new List<Card>(13 * 4 + 1);

            for (var i = 1; i <= 13; i++)
            {
                foreach (CardColor color in new []{CardColor.Black, CardColor.Blue, CardColor.Red, CardColor.Yellow})
                {
                    res.Add(new Card(color, i));
                }
            }

            res.Add(new Card(CardColor.Joker, 0));

            return res;
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
