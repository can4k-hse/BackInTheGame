namespace BackInTheGame
{
    /// <summary>
    /// Класс хранящий одну запись игру заданного вида.
    /// </summary>
    public class Game
    {
        public readonly string Name;
        public readonly string Developer;
        public readonly string Producer;
        public readonly string Genre;
        public readonly string OperatingSystem;
        public readonly DateOnly DateReleased;

        public Game()
        {
            Name = string.Empty;
            Developer = string.Empty;
            Producer = string.Empty;
            Genre = string.Empty;
            OperatingSystem = string.Empty;
            DateReleased = new DateOnly();
        }

        public Game(string name, string developer, string producer, string genre, string operatingSystem, string dateReleased)
        {
            Name = name;
            Developer = developer;
            Producer = producer;
            Genre = genre;
            OperatingSystem = operatingSystem;
            DateReleased = DateOnly.Parse(dateReleased);
        }
    }
}