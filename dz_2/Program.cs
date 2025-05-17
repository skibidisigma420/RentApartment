using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    static void Main(string[] args)
    {
        // Загружаем данные из JSON-файлов
        var tanks = LoadFromJson<Tank[]>("tanks.json");
        var units = LoadFromJson<Unit[]>("units.json");
        var factories = LoadFromJson<Factory[]>("factories.json");

        if (tanks == null || units == null || factories == null)
        {
            Console.WriteLine("Ошибка загрузки данных. Проверьте файлы JSON.");
            return;
        }

        Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

        var foundUnit = FindUnit(units, tanks, "Резервуар 2");
        var factory = FindFactory(factories, foundUnit);

        Console.WriteLine($"Резервуар 2 принадлежит установке {foundUnit?.Name ?? "Не найдено"} и заводу {factory?.Name ?? "Не найдено"}");

        int totalVolume = GetTotalVolume(tanks);
        Console.WriteLine($"Общий объем резервуаров: {totalVolume}");

        // Вывод информации по заводам для выбора
        Console.WriteLine("Выберите завод для отображения резервуаров:");
        foreach (var fac in factories)
            Console.WriteLine($"{fac.Id}: {fac.Name}");

        Console.WriteLine("Введите ID завода (или 0 — чтобы показать все резервуары):");

        if (int.TryParse(Console.ReadLine(), out int selectedFactoryId))
        {
            Console.WriteLine("\nРезервуары:");

            // Фильтруем резервуары по выбранному заводу
            var filteredTanks = selectedFactoryId == 0
                ? tanks
                : tanks.Where(t =>
                {
                    var unit = units.FirstOrDefault(u => u.Id == t.UnitId);
                    return unit != null && unit.FactoryId == selectedFactoryId;
                }).ToArray();

            foreach (var tank in filteredTanks)
            {
                var unit = units.FirstOrDefault(u => u.Id == tank.UnitId);
                if (unit == null) continue;

                var fac = factories.FirstOrDefault(f => f.Id == unit.FactoryId);
                if (fac == null) continue;

                Console.WriteLine($"{tank.Name} — Установка: {unit.Name}, Завод: {fac.Name}");
            }
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Попробуйте снова.");
        }


        // Поиск по имени резервуара
        Console.WriteLine("Введите название резервуара для поиска:");
        var input = Console.ReadLine();
        var result = tanks.FirstOrDefault(t => t.Name.Equals(input, StringComparison.OrdinalIgnoreCase));
        if (result != null)
            Console.WriteLine($"Найден: {result.Name}, Объем: {result.Volume}/{result.MaxVolume}");
        else
            Console.WriteLine("Резервуар не найден.");

        // Сохраняем все объекты в JSON-файл (выгрузка)
        var allData = new
        {
            Tanks = tanks,
            Units = units,
            Factories = factories
        };

        SaveToJson("output.json", allData);

        Console.WriteLine("Данные успешно сохранены в output.json");
    }

    // Универсальный метод загрузки данных из JSON файла
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

    // Сохраняем данные в JSON-файл
    public static void SaveToJson<T>(string fileName, T data)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(fileName, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении файла {fileName}: {ex.Message}");
        }
    }


    // Реализация с LINQ: FindUnit
    public static Unit FindUnit(Unit[] units, Tank[] tanks, string tankName)
    {
        // Синтаксис запросов:
        /*
        var query = from t in tanks
                    where t.Name == tankName
                    join u in units on t.UnitId equals u.Id
                    select u;
        return query.FirstOrDefault();
        */

        // Синтаксис методов:
        var tank = tanks.FirstOrDefault(t => t.Name == tankName);
        if (tank == null) return null;
        return units.FirstOrDefault(u => u.Id == tank.UnitId);
    }

    // Реализация с LINQ: FindFactory
    public static Factory FindFactory(Factory[] factories, Unit unit)
    {
        if (unit == null) return null;

        // Синтаксис запросов:
        /*
        var query = from f in factories
                    where f.Id == unit.FactoryId
                    select f;
        return query.FirstOrDefault();
        */

        // Синтаксис методов:
        return factories.FirstOrDefault(f => f.Id == unit.FactoryId);
    }

    // Реализация с LINQ: GetTotalVolume
    public static int GetTotalVolume(Tank[] tanks)
    {
        // Синтаксис запросов:
        /*
        var query = from t in tanks
                    select t.Volume;
        return query.Sum();
        */

        // Синтаксис методов:
        return tanks.Sum(t => t.Volume);
    }
}

// Классы данных
public class Unit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int FactoryId { get; set; }
}

public class Factory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class Tank
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Volume { get; set; }
    public int MaxVolume { get; set; }
    public int UnitId { get; set; }
}