public static class Program
{
    public static void Main()
    {
        string text = File.ReadAllText("C:/Users/Alexander/Desktop/Project2/computer_games.csv");
        Console.WriteLine(text);
    }
}