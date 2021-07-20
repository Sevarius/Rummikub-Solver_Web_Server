using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumExceptions
{
    public class RumException : ApplicationException
    {
        private ExceptionType _exType;

        public RumException(ExceptionType exceptionType, string message) : base(message)
        {
            _exType = exceptionType;
        }
    }


    public enum ExceptionType
    {
        [Description("Ошибка формирования фишки")]
        CardError01 = 0,

        [Description("Ошибка подачи Null'евой комбинации в проверку комбинаций")]
        CombinationCheckerError01,

        [Description("В комбинации неверное количество фишек")]
        CombinationCheckerError02,

        [Description("Неверное количество Джокеров в комбинации")]
        CombinationCheckerError03,

        [Description("В комбинации имеются повторяющиеся фишки")]
        CombinationCheckerError04,

        [Description("Модель соответствий не была сгенерирована")]
        CombinationMapError01,

    }
}
