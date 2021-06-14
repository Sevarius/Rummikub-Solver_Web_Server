using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumExceptions
{
    public class RumExceptionException : ApplicationException
    {
        private ExceptionType _exType;

        public RumExceptionException(ExceptionType exceptionType, string message) : base(message)
        {
            _exType = exceptionType;
        }
    }


    public enum ExceptionType
    {
        /// <summary>
        /// ошибка при формировании фишки
        /// </summary>
        CardError01 = 0
    }
}
