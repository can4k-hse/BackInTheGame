using System.Text;

namespace BackInTheGame
{

    /// <summary>
    /// Класс предоставляющий интерфейс для работы с CSV таблицами разного типа.
    /// </summary>    
    public class CSVTable
    {
        // Общие для файлов CSV константы.
        private const char QuoteBorder = '"';
        private const string CSVFormat = ".csv";

        private char separator = ',';

        /// <summary>
        /// Преобразует данную строковую запись в массив разделеных сеператором строк, игнорируя сепаратор в цитатах.
        /// </summary>
        /// <param name="record">Строковая запись в формате записи CSV.</param>
        /// <param name="separator">Сепаратор, разделяющий данные разных столбцов.</param>
        /// <returns></returns>
        public static string[] ConvertRecord(string record, char separator)
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
        /// Добавляет новую запись в конец
        /// </summary>
        /// <param name="record"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddRecord(string[] param)
        {
            if (param.Length != ColumnCount)
            {
                throw new ArgumentException("Неверная длина записи");
            }

            _records.Add(param);
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
        private static List<string[]> GetRecordsFromCSVFile(string path, char separator, int columnsCount)
        {
            if (Path.GetExtension(path) != CSVFormat)
            {
                throw new ArgumentException("Указан неверный путь к CSV файлу");
            }

            string[] records;
            try
            {
                records = File.ReadAllText(path).Split(Environment.NewLine);
            }
            catch (Exception)
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

        public CSVTable(char separator, int columnCount) 
        {
            ColumnCount = columnCount;
            this.separator = separator;
        }

        public CSVTable(char separator, int columnCount, string[] header) : this(separator, columnCount)
        {
            _tableHeader = header;
        }

        /// <summary>
        /// Создает CSVTable из CSV файла.
        /// </summary>
        /// <param name="path">Путь к CSV файлу</param>
        /// <param name="separator">Сепаратор, разделяющий данные разных столбцов.</param>
        /// <param name="columnCount">Количество столбцов таблицы</param>
        public CSVTable(string path, char separator, int columnCount)
        {
            ReadCSV(path, separator, columnCount);
        }

        /// <summary>
        /// Число стобцов в таблице.
        /// </summary>
        public int ColumnCount = 0;

        // Названия стобцов CSV таблицы.
        private string[] _tableHeader = [];
        public string[] TableHeader => (string[])_tableHeader.Clone();


        // Массив записей CSV таблицы.
        private List<string[]> _records = [];
        public string[][] Records
        {
            get // Реализуем возвращение глубокой копии массива записей.
            {
                string[][] deepCopy = new string[_records.Count][];

                for (int i = 0; i < _records.Count; i++)
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
        public void ReadCSV(string path, char separator, int columnCount)
        {
            // Обновляем значение separator.
            this.separator = separator;

            _records = GetRecordsFromCSVFile(path, separator, columnCount);

            _tableHeader = _records[0];

            // Избавляемся от записи из заголовка.
            _records = _records[1..];

            ColumnCount = _records.Count - 1;
        }

        /// <summary>
        /// Создает запсись строки.
        /// </summary>
        /// <param name="record"></param>
        public string GetRecordLine(string[] record)
        {
            StringBuilder sb = new();

            foreach (string part in record)
            {
                _ = part.Contains(" ") || part.Contains(",") ? sb.Append($"{QuoteBorder}{part}{QuoteBorder}") : sb.Append(part);
                _ = sb.Append(separator);
            }

            _ = sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary>
        /// Записывает записи в CSV файл.
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public void WriteCSV(string path)
        {
            if (Path.GetExtension(path) != CSVFormat)
            {
                throw new ArgumentException("Указан неверный путь к CSV файлу");
            }

            // Генерируем строку для вывода
            StringBuilder sb = new();

            _ = sb.AppendLine(GetRecordLine(_tableHeader));

            foreach (string[] record in _records)
            {
                _ = sb.AppendLine(GetRecordLine(record));
            }

            try
            {
                File.WriteAllText(path, sb.ToString());
            } catch(Exception)
            {
                throw new Exception("При выгрузке данных произошла ошибка");
            }
        }

        public void DisplayCSV()
        {
            foreach (string[] record in _records)
            {
                Display.DisplayLine(GetRecordLine(record));
            }
        }
    }
}