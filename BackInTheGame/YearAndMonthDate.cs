using System.Globalization;
using System.Text.RegularExpressions;

namespace BackInTheGame
{
    /// <summary>
    /// Класс, хранящий год и месяц
    /// </summary>
    public class YearAndMonthDate
    {
        // Значения по умолчанию
        private const int DefaultYear = 2000;
        private const int DefaultMonth = 1; // 1-12
        private const string DefaultMonthString = "January";

        public int Year { get; set; }
        public int Month { get; set; }

        public YearAndMonthDate()
        {
            Year = DefaultYear;
            Month = DefaultMonth;
        }

        public YearAndMonthDate(string datestring)
        {
            Year = GetYearFromString(datestring);
            Month = GetMonthNumberFromString(GetMonthFromString(datestring));
        }

        public static bool operator <(YearAndMonthDate a, YearAndMonthDate b)
        {
            return (int)a < (int)b;
        }

        public static bool operator >(YearAndMonthDate a, YearAndMonthDate b)
        {
            return (int)a > (int)b;
        }

        /// <summary>
        /// Ищет в строке подстроку, равную число от 1900 до 2100.
        /// </summary>
        /// <param name="datestring"></param>
        /// <returns>Числовое представление найденной строки или значение по умолчанию.</returns>
        private static int GetYearFromString(string datestring)
        { 
            int year = DefaultYear;
            
            // Регулярное выражение, которое ищет подстроку 1900-2100.
            string yearPattern = @"\b(19[0-9]{2}|20[0-9]{2}|2100)\b";

            Match match = Regex.Match(datestring, yearPattern);
            if (match.Success)
            {
                year = int.Parse(match.Value);
            }

            return year;
        }

        /// <summary>
        /// Ищет в строке подстроку, равную названию месяца.
        /// </summary>
        /// <param name="datestring"></param>
        /// <returns>Числовое представление найденной строки или значение по умолчанию.</returns>
        private static string GetMonthFromString(string datestring)
        {
            string month = DefaultMonthString;

            // Регулярное выражение, которое ищет название месяца
            string monthPattern = @"\b(Jan(uary)?|Feb(ruary)?|Mar(ch)?|Apr(il)?|May|Jun(e)?|Jul(y)?|Aug(ust)?|Sep(tember)?|Oct(ober)?|Nov(ember)?|Dec(ember)?)\b";
            
            Match match = Regex.Match(datestring, monthPattern);
            if (match.Success)
            {
                month = match.Value;
            }

            return month;
        }

        /// <summary>
        /// Возвращает номер месяца с данным названием.
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        private static int GetMonthNumberFromString(string month)
        {
            if (string.IsNullOrWhiteSpace(month))
            {
                return DefaultMonth;
            }

            // Получаем информацию о формате даты-время на английском языке.
            DateTimeFormatInfo dateTimeFormat = CultureInfo.GetCultureInfo("en-US").DateTimeFormat;

            for (int i = 0; i < 12; i++)
            {
                // Сравниваем строки игнорируя upper-lower case.
                if (month == dateTimeFormat.MonthNames[i])
                {
                    return i + 1;
                }
            }

            return DefaultMonth;
        }

        /// <summary>
        /// Возвращает название месяца по его номеру.
        /// </summary>
        /// <param name="number">Номер месяца.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static string GetMonthName(int number)
        {
            if (number is <= 0 or > 12)
            {
                throw new ArgumentException("Указан неверный номер месяца");
            }

            // Получаем информацию о формате даты-время на английском языке.
            DateTimeFormatInfo dateTimeFormat = CultureInfo.GetCultureInfo("en-US").DateTimeFormat;

            return dateTimeFormat.MonthNames[number - 1];
        }

        public override string ToString()
        {
            return Year + GetMonthName(Month);
        }


        /// <summary>
        /// Возвращает количество месяцев в дате в нашей эре
        /// </summary>
        /// <param name="game"></param>
        public static implicit operator int(YearAndMonthDate date)
        {
            return date.Month + (date.Year * 12);
        }
    }
}
