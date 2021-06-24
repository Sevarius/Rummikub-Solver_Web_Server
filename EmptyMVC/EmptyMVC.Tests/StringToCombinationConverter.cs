using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MechanicsModel;

namespace EmptyMVC.Tests
{
    public class StringToCombinationConverter
    {
        public CombinationModel stringToCombination(string combStr, CombinationStringFormat format)
        {
            var cardStrs = combStr.Trim(' ', '\n', '\r', '\t').Split(' ');
            var cards = cardStrs.Select(card => stringToCard(card, format)).ToList();
            return new CombinationModel(cards)
            {
                Type = CombinationType.Unknown,
                isValid = false
            };
        }

        public Card stringToCard(string cardStr, CombinationStringFormat format)
        {
            var numberStr = "";
            var colorStr = "";
            if (format == CombinationStringFormat.Short)
            {
                foreach (char c in cardStr)
                {
                    if (char.IsNumber(c))
                        numberStr += c;
                    else
                        colorStr += c;
                }
            }
            else
            {
                var split = cardStr.Split('_');
                numberStr = split[0];
                colorStr = split[1];
            }

            var color = stringToColor(colorStr);
            var number = stringToNumber(numberStr);

            return new Card(color, number);
        }

        public CardColor stringToColor(string colorStr)
        {
            switch (colorStr)
            {
                case "b":
                case "Blue":
                    return CardColor.Blue;
                case "y":
                case "Yellow":
                    return CardColor.Yellow;
                case "bb":
                case "Black":
                    return CardColor.Black;
                case "r":
                case "Red":
                    return CardColor.Red;
                case "j":
                case "J":
                case "Joker":
                    return CardColor.Joker;
                default:
                    throw new InvalidCastException($"Нет такого цвета: {colorStr}");
            }
        }

        public int stringToNumber(string numberStr)
        {
            if (numberStr == string.Empty)
                return 0;
            else
                return Convert.ToInt32(numberStr);
        }
    }

    public enum CombinationStringFormat
    {
        /// <summary>
        /// Короткая запись фишек (1r 1bb j)
        /// </summary>
        Short,

        /// <summary>
        /// Длинная запись фишек (1_Red 1_Black Joker)
        /// </summary>
        Long
    }
}
