using System.Dynamic;

namespace BackInTheGame
{
    public class GamesStatistics
    {
        private List<Game> _games = [];

        /// <summary>
        /// Отчищает список игр.
        /// </summary>
        public void Clear()
        {
            _games = [];
        }

        /// <summary>
        /// Добавляет игру в список.
        /// </summary>
        /// <param name="game"></param>
        public void AddGame(Game game)
        {
            _games.Add(game);
        }

        /// <summary>
        /// Возвращает массив игр данного разработчика.
        /// </summary>
        /// <param name="developer"></param>
        /// <returns></returns>
        public Game[] GetGamesByDevelper(string developer = "Maxis")
        {
            return [.. _games.Where(game => game.Developer == developer)];
        }

        /// <summary>
        /// Вовзвращает массив разработчиков для всех игр.
        /// </summary>
        /// <returns></returns>
        public string[] GetGamesProducers()
        {
            HashSet<string> hs = [];
            foreach(Game game in _games)
            {
                _ = hs.Add(game.Producer);
            }
            return [.. hs];
        }


        /// <summary>
        /// Вовзвращает массив жанров для всех игр.
        /// </summary>
        /// <returns></returns>
        public string[] GetGamesGenres()
        {
            HashSet<string> hs = [];
            foreach(Game game in _games)
            {
                _ = hs.Add(game.Genre);
            }
            return [.. hs];
        }

        /// <summary>
        /// Возвращает массив игр данного производителя.
        /// </summary>
        /// <param name="producer"></param>
        /// <returns></returns>
        public Game[] GetGamesByProducer(string producer)
        {
            return [.. _games.Where(game => game.Producer == producer)];
        }

        /// <summary>
        /// Возвращает массив игр данного жанра.
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        public Game[] GetGamesByGenre(string genre)
        {
            return [.. _games.Where(game => game.Genre == genre)];
        }

        /// <summary>
        /// Возвращает массив игр, изданных в данном месяце.
        /// </summary>
        /// <param name="month">Номер месяца 1-12</param>
        /// <returns></returns>
        public Game[] GetGamesByRealeseMonth(int month = 12)
        {
            return month is < 1 or > 12
                ? throw new ArgumentException("Указан несуществующий месяц")
                : ([.._games.Where(game => game.DateReleased.Month == month)]);
        }

        /// <summary>
        /// Возвращает массив игр, выпущенных для данной операционной системы.
        /// </summary>
        /// <param name="operatingSystem"></param>
        /// <returns></returns>
        public Game[] GetGamesByOS(string operatingSystem)
        {
            return [.. _games.Where(game => game.OperatingSystem == operatingSystem)];
        }

        /// <summary>
        /// Возвращает саму ранюю игру.
        /// </summary>
        /// <param name="OperatingSystem"></param>
        /// <param name="Genre"></param>
        /// <returns></returns>
        public Game GetOldestGame(string OperatingSystem = "Microsoft Windows", string Genre = "First-person shooter")
        {
            if (_games.Count == 0)
            {
                throw new Exception();
            }

            // Берем некоторое значение по умолчанию.
            Game game = _games[0];
            for (int i = 1; i < _games.Count; i++)
            {
                if (_games[i].Genre != Genre || _games[i].OperatingSystem != OperatingSystem)
                {
                    continue;
                }

                // Пробуем обновить игру.
                if (game.DateReleased > _games[i].DateReleased)
                {
                    game = _games[i];
                }
            }

            return game;
        }

        /// <summary>
        /// Находит producer с наименьшим количеством игр.
        /// </summary>
        /// <returns></returns>
        public string? GetProducerAssociatedMinGames()
        {
            // Создаем словарь для подсчета статистики.
            Dictionary<string, int> producerGamesNumber = [];

            foreach(Game game in  _games)
            {
                if (!producerGamesNumber.ContainsKey(game.Producer))
                {
                    producerGamesNumber[game.Producer] = 1;
                } else
                {
                    producerGamesNumber[game.Producer] += 1;
                }
            }

            if (producerGamesNumber.Count == 0)
            {
                return null;
            }
            
            //  Находим producer с наименьшим количеством игр.
            KeyValuePair<string, int> producerItem = producerGamesNumber.First();

            foreach (KeyValuePair<string, int> producer in producerGamesNumber)
            {
                if (producerItem.Value > producer.Value)
                {
                    producerItem = producer;
                }
            }

            return producerItem.Key;
        }

        /// <summary>
        /// Вычисляет средний год выпуска игры
        /// </summary>
        /// <exception cref="DivideByZeroException"></exception>
        /// <returns></returns>
        public int GetAverageReleaseYear()
        {
            try
            {
                int yearSum = 0;

                foreach (Game game in _games)
                {
                    yearSum += game.DateReleased.Year;
                }

                return yearSum / _games.Count;
            } catch(DivideByZeroException)
            {
                throw new Exception("Невозможно определить средний год");
            }
        }
        

        /// <summary>
        /// Выдает список игр с годом выхода большим, чем данный
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>

        public Game[] GetGamesReleaseYearBigger(int year)
        {
            return [.. _games.Where(game => game.DateReleased.Year > year)];
        }
    }
}
