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
        public CombinationModel stringToCombination(string combStr)
        {
            var cardStrs = combStr.Trim(' ', '\n', '\r', '\t').Split(' ');
            var cards = cardStrs.Select(stringToCard).ToList();
            return new CombinationModel()
            {
                Cards = cards,
                Type = CombinationType.Unknown,
                isValid = false
            };
        }

        public Card stringToCard(string cardStr)
        {
            var numberStr = "";
            var colorStr = "";

            foreach (char c in cardStr)
            {
                if (char.IsNumber(c))
                    numberStr += c;
                else
                    colorStr += c;
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
                    return CardColor.Blue;
                case "y":
                    return CardColor.Yellow;
                case "bb":
                    return CardColor.Black;
                case "r":
                    return CardColor.Red;
                case "j":
                case "J":
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
}
