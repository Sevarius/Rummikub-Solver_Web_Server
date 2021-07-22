using System;
using System.ComponentModel;

namespace RumExceptions
{
    /// <summary>
    /// Внутренние исключения работы программы
    /// </summary>
    public sealed class RumException : ApplicationException
    {
        /// <summary>
        /// Тип исключения
        /// </summary>
        private ExceptionType _exType;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="exceptionType">Тип исключения</param>
        /// <param name="message">Сообщение исключения</param>
        /// <param name="innerException">Внутреннее исключение</param>
        public RumException(ExceptionType exceptionType, string message, Exception innerException = null) : base(message, innerException)
        {
            _exType = exceptionType;
        }
    }


    /// <summary>
    /// Тип исключения
    /// </summary>
    public enum ExceptionType
    {
        /// <summary>
        /// Ошибка формирования фишки
        /// </summary>
        [Description("Ошибка формирования фишки")]
        CardError01 = 0,

        /// <summary>
        /// Необработанная ошибка проверки комбинации
        /// </summary>
        [Description("Необработанная ошибка проверки комбинации")]
        CombinationCheckerError00,

        /// <summary>
        /// Ошибка подачи Null'евой комбинации в проверку комбинаций
        /// </summary>
        [Description("Ошибка подачи Null'евой комбинации в проверку комбинаций")]
        CombinationCheckerError01,

        /// <summary>
        /// В комбинации неверное количество фишек
        /// </summary>
        [Description("В комбинации неверное количество фишек")]
        CombinationCheckerError02,

        /// <summary>
        /// Неверное количество Джокеров в комбинации
        /// </summary>
        [Description("Неверное количество Джокеров в комбинации")]
        CombinationCheckerError03,

        /// <summary>
        /// В комбинации имеются повторяющиеся фишки
        /// </summary>
        [Description("В комбинации имеются повторяющиеся фишки")]
        CombinationCheckerError04,

        /// <summary>
        /// Модель соответствий не была сгенерирована
        /// </summary>
        [Description("Модель соответствий не была сгенерирована")]
        CombinationMapError01,

        /// <summary>
        /// Неверные данные, в руке не может не быть фишек
        /// </summary>
        [Description("Неверные данные, в руке не может не быть фишек")]
        GameCheckerError01,

    }
}
