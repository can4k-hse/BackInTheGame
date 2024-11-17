using System.Linq;

namespace BackInTheGame
{
    public class GamesStatistics
    {
        private readonly List<Game> _games = [];

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
        /// Возвращает массив игр данного производителя.
        /// </summary>
        /// <param name="producer"></param>
        /// <returns></returns>
        public Game[] GetGamesByProducer(string producer = "Maxis")
        {
            return [.. _games.Where(game => game.Producer == producer)];
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
        private Game[] GetGamesByOS(string operatingSystem)
        {
            return [.. _games.Where(game => game.OperatingSystem == operatingSystem)];
        }

        /// <summary>
        /// Возвращает саму новую игру.
        /// </summary>
        /// <param name="OperatingSystem"></param>
        /// <param name="Genre"></param>
        /// <returns></returns>
        public Game? GetMostRecentGame(string OperatingSystem = "Microsoft Windows", string Genre = "First-person shooter")
        {
            if (_games.Count == 0)
            {
                return null;
            }

            // Берем некоторое значение по умолчанию.
            Game game = _games[0];
            for (int i = 1; i < _games.Count; i++)
            {
                if (game.Genre != Genre || game.OperatingSystem != OperatingSystem)
                {
                    continue;
                }

                // Пробуем обновить игру.
                if (game.DateReleased < _games[i].DateReleased)
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
                producerGamesNumber[game.Producer] += 1;
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
    }
}
