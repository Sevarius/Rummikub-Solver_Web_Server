using System;
using System.Linq;

namespace MechanicsModel
{
    /// <summary>
    /// Класс конвертации строкового представления комбинаций фишек в объекты комбинаций
    /// </summary>
    public sealed class StringToCombinationConverter
    {
        /// <summary>
        /// Формат преобразования строк
        /// </summary>
        private readonly CombinationStringFormat _format;

        /// <summary>
        /// Конструктор конвертора
        /// </summary>
        /// <param name="format">Формат строкового представления фишек представления </param>
        public StringToCombinationConverter(CombinationStringFormat format)
        {
            _format = format;
        }

        /// <summary>
        /// Преобразовывает строковок представление комбинации в объект комбинации
        /// </summary>
        /// <param name="combStr">Строковое представление комбинации</param>
        /// <returns>Объект комбинации</returns>
        public CombinationModel StringToCombination(string combStr)
        {
            var cardStrs = combStr.Trim(' ', '\n', '\r', '\t').Split(' ');
            var cards = cardStrs.Select(StringToCard).ToList();
            return new CombinationModel(cards)
            {
                Type = CombinationType.Unknown,
                isValid = false
            };
        }

        /// <summary>
        /// Преобразовывает строковок представление фишки в объект фишки
        /// </summary>
        /// <param name="cardStr">Строковое представление фишки</param>
        /// <returns>Объект фишки</returns>
        public CardModel StringToCard(string cardStr)
        {
            var numberStr = "";
            var colorStr = "";
            if (_format == CombinationStringFormat.Short)
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

            var color = StringToColor(colorStr);
            var number = StringToNumber(numberStr);

            return new CardModel(color, number);
        }

        /// <summary>
        /// Преобразовывает строковое представление цвета фишки в цвет
        /// </summary>
        /// <param name="colorStr">Строковое представление цвета фишки</param>
        /// <returns>Цвет</returns>
        /// <exception cref="InvalidCastException">Кидаетс, если была подана неверная строка</exception>
        private CardColor StringToColor(string colorStr)
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

        /// <summary>
        /// Преобразовывает строковое представление целого числа в число
        /// </summary>
        /// <param name="numberStr">Строковое представление числа</param>
        /// <returns>Число</returns>
        private int StringToNumber(string numberStr)
        {
            if (numberStr == string.Empty)
                return 0;
            else
                return Convert.ToInt32(numberStr);
        }
    }

    /// <summary>
    /// Формат строкового представления комбинации фишек
    /// </summary>
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
