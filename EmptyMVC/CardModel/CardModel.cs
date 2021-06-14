using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RumExceptions;

namespace MechanicsModel
{
    public class Card
    {
        private CardColor _color;
        private int _number;

        public Card(CardColor Color, int Number)
        {
            _color = Color;

            if (Color is CardColor.Joker)
            {
                _number = 0;
                return;
            }

            if (Number > 13)
            {
                throw new RumExceptionException(ExceptionType.CardError01, "Значение фишки не может быть больше 13");
            }

            if (Number < 1)
            {
                throw new RumExceptionException(ExceptionType.CardError01, "Значение фишки не может быть меньше 1");
            }

            _number = Number;
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
