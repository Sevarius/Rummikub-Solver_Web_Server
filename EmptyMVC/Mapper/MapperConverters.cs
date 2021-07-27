using AutoMapper;
using MechanicsModel;
using RumExceptions;

namespace Mapper
{
    public class StringCardColorConverter : ITypeConverter<string, CardColor>
    {
        public CardColor Convert(string source, CardColor destination, ResolutionContext context)
        {
            switch (source)
            {
                case "r":
                case "red":
                case "Red":
                case "R":
                    return CardColor.Red;
                case "b":
                case "blue":
                case "Blue":
                case "B":
                    return CardColor.Blue;
                case "bb":
                case "black":
                case "Black":
                case "BB":
                    return CardColor.Black;
                case "y":
                case "yellow":
                case "Yellow":
                case "Y":
                    return CardColor.Yellow;
                case "j":
                case "joker":
                case "Joker":
                case "J":
                    return CardColor.Joker;
                default:
                    throw new RumException(ExceptionType.CardError01, $"Не удалось преобразовать цвет фишки: {source}");
            }
        }
    }
}