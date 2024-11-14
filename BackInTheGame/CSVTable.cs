using System.Text;

namespace BackInTheGame
{

    /// <summary>
    /// Класс предоставляющий интерфейс для работы с CSV таблицами.
    /// </summary>
    public class CSVTable
    {
        // Общие для файлов CSV константы.
        private const char QuoteBorder = '"';
        private const char LinesBreak = '\n';
        private const string CSVFormat = ".csv";


        /// <summary>
        /// Преобразует данную строковую запись в массив разделеных сеператором строк, игнорируя сепаратор в цитатах.
        /// </summary>
        /// <param name="record">Строковая запись в формате записи CSV.</param>
        /// <param name="separator">Сепаратор, разделяющий данные разных столбцов.</param>
        /// <returns></returns>
        private static string[] ConvertRecord(string record, char separator)
        {
            List<string> recordParts = [];

            // Текущая обрабатываемая часть записи.
            StringBuilder part = new();

            // Флаг нахождение внутри цитаты.
            bool inQuote = false;

            for (int i = 0; i <= record.Length; i++) // '<=' служит для избежания дублирования кода.
            {
                // Условие добавления части к ответу.
                if ((i == record.Length) || (record[i] == separator && !inQuote))
                {
                    recordParts.Add(part.ToString());
                    part = part.Clear();
                    continue;
                }

                if (record[i] != QuoteBorder)
                {
                    part = part.Append(record[i]);
                }
                else
                {
                    // Изменяем значение флага на противоположный.
                    inQuote ^= true;
                }
            }

            return [.. recordParts];
        }


        /// <summary>
        /// Считывает, обрабатывает и возвращает массив записей из CSV файла.
        /// </summary>
        /// <param name="path">Путь к CSV файлу.</param>
        /// <param name="separator">Сепаратор, разделяющий данные разных столбцов.</param>
        /// <param name="columnsCount">Количество столбоцов таблицы.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        private static string[][] GetRecordsFromCSVFile(string path, char separator, int columnsCount)
        {
            if (Path.GetExtension(path) != CSVFormat)
            {
                throw new ArgumentException("Указан неверный путь к CSV файлу");
            }

            string[] records;
            try
            {
                records = File.ReadAllText(path).Split(LinesBreak);
            } catch(Exception)
            {
                throw new Exception("При чтении файла произошла ошибка");
            }

            // Обрабатываем записи и оставляем только удовлетворяющие количеству столбцов.
            return [..records
                .Select(record => ConvertRecord(record, separator))
                .Where(record => record.Length == columnsCount) 
            ];
        }


        public CSVTable() { }

        /// <summary>
        /// Создает CSVTable из CSV файла.
        /// </summary>
        /// <param name="path">Путь к CSV файлу</param>
        /// <param name="separator">Сепаратор, разделяющий данные разных столбцов.</param>
        /// <param name="columnCount">Количество столбцов таблицы</param>
        public CSVTable(string path, char separator, int columnCount)
        {
            _ = ReadCSV(path, separator, columnCount);
        }


        /// <summary>
        /// Число стобцов в таблице.
        /// </summary>
        public int ColumnCount = 0;

        // Названия стобцов CSV таблицы.
        private string[] _tableHeader = [];
        public string[] TableHeader => (string[])_tableHeader.Clone();


        // Массив записей CSV таблицы.
        private string[][] _records = [];
        public string[][] Records
        {
            get // Реализуем возвращение глубокой копии массива записей.
            {
                string[][] deepCopy = new string[_records.Length][];

                for (int i = 0; i < _records.Length; i++)
                {
                    deepCopy[i] = (string[])_records[i].Clone();
                }

                return deepCopy;
            }
        }


        /// <summary>
        /// Считвает файл с CSV таблицей, сохраняет и обрабатывает записи.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="separator"></param>
        /// <param name="columnCount"></param>
        /// <returns>Успешность записи CSV таблицы.</returns>
        public bool ReadCSV(string path, char separator, int columnCount)
        {
            try
            {
                _records = GetRecordsFromCSVFile(path, separator, columnCount);
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
            _tableHeader = _records[0];
            ColumnCount = _records.Length - 1;
            return true;
        }
    }
}