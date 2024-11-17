namespace BackInTheGame
{
    public static class Program
    {
        public static void Main()
        {
            CSVTable table = new();
            _ = table.ReadCSV("C:/Users/Alexander/Desktop/Project2/computer_games.csv", ',', 6);

            GamesStatistics gs = new();

            foreach (var record in table.Records)
            {
                Game game = new (record);
                gs.AddGame(game);
                Console.WriteLine(game.GetRecord('_'));
            }
        }
    }
}