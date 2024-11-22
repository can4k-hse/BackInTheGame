namespace BackInTheGame
{
    /// <summary>
    /// Статический класс, который работает с выводом (консолью)
    /// </summary>
    public static class Display
    {
        // Символ начала строки
        private const char LineBeginSymbol = '>';

        /// <summary>
        /// Выводит строку белым цветом.
        /// </summary>
        /// <param name="msg"></param>
        public static void DisplayLine(string msg)
        {
            try
            {
                Console.WriteLine(msg);
            }
            catch (IOException)
            {
                return;
            }
        }

        /// <summary>
        /// Выводит символы белым цветом.
        /// </summary>
        /// <param name="msg"></param>
        public static void DisplayWrite(string msg)
        {
            try
            {
                Console.Write(msg);
            }
            catch (IOException)
            {
                return;
            }
        }
            /// <summary>
            /// Выводит сообщение зеленым цветом.
            /// </summary>
            /// <param name="ex"></param>
            public static void DisplaySuccessLine(string msg)
        {
            try
            {
                ConsoleColor prevColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(msg);
                Console.ForegroundColor = prevColor;
            }
            catch (IOException)
            {
                return;
            }
        }

        /// <summary>
        /// Выводит сообщение красным цветом.
        /// </summary>
        /// <param name="ex"></param>
        public static void DisplayErrorLine(string msg)
        {
            try
            {
                ConsoleColor prevColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg);
                Console.ForegroundColor = prevColor;
            }
            catch (IOException)
            {
                return;
            }
        }

        /// <summary>
        /// Выводит символ, символизирующий начало команды
        /// </summary>
        public static void DisplayLineBegin()
        {
            Console.Write(LineBeginSymbol);
        }

        /// <summary>
        /// Отчищает ввод
        /// </summary>
        public static void ClearDisplay()
        {
            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
                return;
            }
        }

        /// <summary>
        /// Считывает строку
        /// </summary>
        /// <returns></returns>
        public static string InputLine()
        {
            try
            {
                return Console.ReadLine() ?? "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        internal static void DisplayLine(object v)
        {
            throw new NotImplementedException();
        }
    }
}
