namespace BackInTheGame
{
    public class UserMenu
    {
        public UserMenu() { }

        /// <summary>
        /// Выводит сообщения-инструкцию с командами
        /// </summary>
        /// 
        public void DisplayCommands()
        {
            Display.DisplayLine("help - вывести список команд");
            Display.DisplayLine("clear - очистить консоль");
            Display.DisplayLine("1 - Ввести данные из файла");
            Display.DisplayLine("2 - Вывести подборку игр Developer = Maxis");
            Display.DisplayLine("2.1 - Сохранить подборку игр Developer = Maxis в файл");
            Display.DisplayLine("3 - Вывести подборку игр, вышедших в декабре");
            Display.DisplayLine("4.1 - Вывести общее количество игр по каждому производителю");
            Display.DisplayLine("4.2 - Вывести данные о самой ранней выпущенной игре, Operating System = Microsoft Windows, Genre = First-person shooter");
            Display.DisplayLine("4.3 - Информация о среднем значении года выпуска игры Date Released по Genre");
            Display.DisplayLine("4.4 - Продюсер, который связан с минимальным количеством компьютерных игр");
            Display.DisplayLine("5 - выйти из консоли");
        }


        public void Run()
        {
            Display.DisplayLine("Добро пожаловать в Back in the Game" + Environment.NewLine);
            DisplayCommands();

            CSVTable table = new();
            GamesStatistics gs = new();

            while (true)
            {
                Display.DisplayLineBegin();
                try
                {
                    string cmd = Display.InputLine() ?? "";
                    switch (cmd)
                    {
                        case "": break;
                        case "help": DisplayCommands(); break;
                        case "clear": Display.ClearDisplay(); break;
                        case "5": return;
                        case "1":
                            {
                                Display.DisplayWrite("Укажите путь к файлу: ");
                                string path = Display.InputLine() ?? "";

                                // Читает файл
                                table.ReadCSV(path, ',', 6);
                                Display.DisplaySuccessLine("Файл успешо загружен");

                                // Преобразуем записи в Game
                                foreach (string[] record in table.Records)
                                {
                                    try
                                    {
                                        Game game = new(record);
                                        gs.AddGame(game);
                                    }
                                    catch (Exception)
                                    {
                                        continue; // Пропускаем неверную запись
                                    }
                                }

                                break;
                            }
                        case "2":
                            {
                                Display.DisplaySuccessLine("Список игр с Developer=Maxis");

                                CSVTable tmp = new(',', 6);
                                foreach (Game game in gs.GetGamesByDevelper())
                                {
                                    tmp.AddRecord(game.GetRecordArray());
                                }

                                tmp.DisplayCSV();

                                break;
                            }
                        case "2.1":
                            {
                                // Указываем значения по умолчанию
                                CSVTable tmp = new(',', 6);

                                // Добавляем все нужные записи в CSVTable
                                
                                foreach (string[] record in gs.GetGamesByDevelper().Select(game => game.GetRecordArray()))
                                {
                                    tmp.AddRecord(record);
                                }

                                tmp.WriteCSV("Developer_Maxis.csv");
                                Display.DisplaySuccessLine("Файл успешно записан");

                                break;
                            }
                        case "3":
                            {
                                Display.DisplaySuccessLine("Список игр, вышедших в Декабре");

                                // Указываем значения по умолчанию
                                CSVTable tmp = new(',', 6);

                                foreach (string[] record in gs.GetGamesByRealeseMonth().Select(game => game.GetRecordArray()))
                                {
                                    tmp.AddRecord(record);
                                }

                                tmp.DisplayCSV();
                                break;
                            }
                        case "4.1":
                            {
                                Display.DisplaySuccessLine("Количесво игр по производителям:");
                                string[] producers = gs.GetGamesProducers();
                                foreach (string producer in producers)
                                {
                                    Display.DisplayLine($"{producer}: {gs.GetGamesByProducer(producer).Length}");
                                }
                                break;
                            }
                        case "4.2":
                            {
                                CSVTable tmp = new(',', 6);

                                try
                                {
                                    tmp.AddRecord(gs.GetOldestGame().GetRecordArray());
                                    Display.DisplaySuccessLine("Самая ранняя игра");
                                    tmp.DisplayCSV();
                                }
                                catch (Exception)
                                {
                                    Display.DisplayErrorLine("Невозможно определить");
                                }

                                break;
                            }
                        case "4.3":
                            {
                                Display.DisplaySuccessLine("Количество игр по жанрам:");
                                foreach(string genre in gs.GetGamesGenres())
                                {
                                    Display.DisplayLine($"{genre}: {gs.GetGamesByGenre(genre).Length}");
                                }

                                break;
                            }
                        case "4.4":
                            {
                                string? producer = gs.GetProducerAssociatedMinGames();
                                if (producer == null)
                                {
                                    Display.DisplayErrorLine("Невозможно определить");
                                    break;
                                }

                                Display.DisplaySuccessLine("Продюсер, который связан с минимальным количеством компьютерных игр");
                                Display.DisplayLine(producer);
                                break;
                            }
                        default: throw new ArgumentException("Неизвестная команда");
                    };
                }
                catch (Exception e)
                {
                    Display.DisplayErrorLine(e.Message);
                }
            }
        }
    }
}
