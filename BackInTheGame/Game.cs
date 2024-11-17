using System.Text.RegularExpressions;

namespace BackInTheGame
{
    /// <summary>
    /// Структура, хранящая одну запись игру заданного вида.
    /// </summary>
    public readonly struct Game
    {
        public readonly string Name;
        public readonly string Developer;
        public readonly string Producer;
        public readonly string Genre;
        public readonly string OperatingSystem;
        public readonly YearAndMonthDate DateReleased;
        public readonly string DateReleasedString;

        public Game()
        {
            Name = string.Empty;
            Developer = string.Empty;
            Producer = string.Empty;
            Genre = string.Empty;
            OperatingSystem = string.Empty;
            DateReleased = new YearAndMonthDate();
            DateReleasedString = string.Empty;
        }

        public Game(string name, string developer, string producer, string genre, string operatingSystem, string dateReleased)
        {
            Name = name;
            Developer = developer;
            Producer = producer;
            Genre = genre;
            OperatingSystem = operatingSystem;
            DateReleased = new YearAndMonthDate(dateReleased);
            DateReleasedString = dateReleased;
        }

        public Game(string[] param)
        {
            if (param.Length != 6)
            {
                throw new ArgumentException("Неверное количество аргументов");
            }

            Name = param[0];
            Developer = param[1];
            Producer = param[2];
            Genre = param[3];
            OperatingSystem = param[4];
            DateReleasedString = param[5];
            DateReleased = new YearAndMonthDate(param[5]);
        }

        public string GetRecord(char separator)
        {
            return $"{Name}{separator}{Developer}{separator}{Producer}" +
                $"{separator}{Genre}{separator}{OperatingSystem}{separator}{DateReleasedString}";
        }
    }
}