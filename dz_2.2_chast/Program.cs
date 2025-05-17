using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

// Класс сделки
public class Deal
{
    public int Sum { get; set; }
    public string Id { get; set; }
    public DateTime Date { get; set; }
}

// Record для сумм по месяцам
public record SumByMonth(DateTime Month, int Sum);

class Program
{
    static void Main(string[] args)
    {
        // Загрузка сделок из JSON
        var deals = LoadFromJson<Deal[]>("JSON_sample_1.json");
        if (deals == null)
        {
            Console.WriteLine("Не удалось загрузить сделки из JSON_sample_1.json");
            return;
        }

        // Получаем Id сделок согласно условию
        var selectedDealIds = GetNumbersOfDeals(deals);
        Console.WriteLine($"Найдено сделок: {selectedDealIds.Count}");
        Console.WriteLine("Идентификаторы сделок: " + string.Join(", ", selectedDealIds));

        // Получаем суммы по месяцам
        var sumsByMonth = GetSumsByMonth(deals);
        Console.WriteLine("Суммы сделок по месяцам:");
        foreach (var sumMonth in sumsByMonth)
        {
            Console.WriteLine($"{sumMonth.Month:yyyy-MM}: {sumMonth.Sum}");
        }
    }

    // Метод загрузки из JSON
    public static T LoadFromJson<T>(string fileName)
    {
        try
        {
            var jsonString = File.ReadAllText(fileName);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<T>(jsonString, options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке файла {fileName}: {ex.Message}");
            return default;
        }
    }

    // Метод возвращает Id сделок, которые соответствуют условиям
    public static IList<string> GetNumbersOfDeals(IEnumerable<Deal> deals)
    {
        var filtered = deals
            .Where(d => d.Sum >= 100)            // Фильтрация по сумме >= 100
            .OrderBy(d => d.Date)                // Сортировка по дате по возрастанию
            .Take(5)                            // Берём 5 самых ранних сделок
            .OrderByDescending(d => d.Sum)      // Сортируем по сумме по убыванию
            .Select(d => d.Id)                  // Берём Id
            .ToList();

        return filtered;
    }

    // Метод возвращает сумму сделок по месяцам
    public static IList<SumByMonth> GetSumsByMonth(IEnumerable<Deal> deals)
    {
        var sumsByMonth = deals
            .GroupBy(d => new DateTime(d.Date.Year, d.Date.Month, 1)) // Группировка по месяцу
            .Select(g => new SumByMonth(g.Key, g.Sum(d => d.Sum)))     // Создание объекта SumByMonth
            .OrderBy(s => s.Month)                                    // Сортировка по дате по возрастанию
            .ToList();

        return sumsByMonth;
    }
}
