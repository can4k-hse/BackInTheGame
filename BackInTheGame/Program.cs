namespace BackInTheGame
{
    public static class Program
    {
        public static void Main()
        {
            try
            {
                UserMenu um = new();
                um.Run();
            }
            catch (Exception)
            {
                return; // Ошибка консоли
            }
        }
    }
}