using System.Text;

public class CSVTable
{
    // Общие для файлов CSV константы.
    private const char QuoteBorder = '"';
    private const char LinesBreak = '\n';


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
    /// <returns></returns>
    private static string[][] GetRecordsFromFile(string path, char separator, int columnsCount)
    {
        string[] records = File.ReadAllText(path).Split(LinesBreak);

        // Обрабатываем записи и оставляем только удовлетворяющие количеству столбцов.
        return [..records
            .Select(record => ConvertRecord(record, separator))
            .Where(record => record.Length == columnsCount) 
        ];
    }


    public CSVTable() { }
    
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
    public void ReadCSV(string path, char separator, int columnCount)
    {
        _records = GetRecordsFromFile(path, separator, columnCount);
        _tableHeader = _records[0];
        ColumnCount = _records.Length - 1;
    }
}