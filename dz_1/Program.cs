using System;

class Program
{
    static void Main(string[] args)
    {
        var tanks = GetTanks();
        var units = GetUnits();
        var factories = GetFactories();

        Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

        var foundUnit = FindUnit(units, tanks, "Резервуар 2");
        var factory = FindFactory(factories, foundUnit);

        Console.WriteLine($"Резервуар 2 принадлежит установке {foundUnit?.Name ?? "Не найдено"} и заводу {factory?.Name ?? "Не найдено"}");

        var totalVolume = GetTotalVolume(tanks);
        Console.WriteLine($"Общий объем резервуаров: {totalVolume}");

        // Вывод информации по заводам для выбора
        Console.WriteLine("Выберите завод для отображения резервуаров:");
        foreach (var fac in factories)
        {
            Console.WriteLine($"{fac.Id}: {fac.Name}");
        }
        Console.WriteLine("Введите ID завода (или 0 — чтобы показать все резервуары):");

        if (int.TryParse(Console.ReadLine(), out int selectedFactoryId))
        {
            Console.WriteLine("\nРезервуары:");

            foreach (var tank in tanks)
            {
                var unit = Array.Find(units, u => u.Id == tank.UnitId);
                if (unit == null) continue;

                var fac = Array.Find(factories, f => f.Id == unit.FactoryId);
                if (fac == null) continue;

                // Если выбран завод не 0, фильтруем по нему
                if (selectedFactoryId != 0 && fac.Id != selectedFactoryId)
                    continue;

                Console.WriteLine($"{tank.Name} — Установка: {unit.Name}, Завод: {fac.Name}");
            }
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Попробуйте снова.");
        }



        //поиск по имени
        Console.WriteLine("Введите название резервуара для поиска:");
        var input = Console.ReadLine();
        var result = Array.Find(tanks, t => t.Name.Equals(input, StringComparison.OrdinalIgnoreCase));
        if (result != null)
            Console.WriteLine($"Найден: {result.Name}, Объем: {result.Volume}/{result.MaxVolume}");
        else
            Console.WriteLine("Резервуар не найден.");
    }


    // реализуйте этот метод, чтобы он возвращал массив резервуаров, согласно приложенным таблицам
    public static Tank[] GetTanks()
    {

        var data = new object[,]
        {
        { 1, "Резервуар 1", "Надземный - вертикальный", 1500, 2000, 1 },
        { 2, "Резервуар 2", "Надземный - горизонтальный", 2500, 3000, 1 },
        { 3, "Дополнительный резервуар 24", "Надземный - горизонтальный", 3000, 3000, 2 },
        { 4, "Резервуар 35", "Надземный - вертикальный", 3000, 3000, 2 },
        { 5, "Резервуар 47", "Подземный - двустенный", 4000, 5000, 2 },
        { 6, "Резервуар 256", "Подводный", 500, 500, 3 }
        };

        var tanks = new Tank[data.GetLength(0)];

        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i] = new Tank
            {
                Id = (int)data[i, 0],
                Name = (string)data[i, 1],
                Description = (string)data[i, 2],
                Volume = (int)data[i, 3],
                MaxVolume = (int)data[i, 4],
                UnitId = (int)data[i, 5]
            };
        }

        return tanks;
    }

    // реализуйте этот метод, чтобы он возвращал массив установок, согласно приложенным таблицам
    public static Unit[] GetUnits()
    {
        var data = new object[,]
        {
        { 1, "ГФУ-2", "Газофракционирующая установка", 1 },
        { 2, "ABT-6", "Атмосферно-вакуумная трубчатка", 1 },
        { 3, "ABT-10", "Атмосферно-вакуумная трубчатка", 2 }
        };

        var units = new Unit[data.GetLength(0)];

        for (int i = 0; i < units.Length; i++)
        {
            units[i] = new Unit
            {
                Id = (int)data[i, 0],
                Name = (string)data[i, 1],
                Description = (string)data[i, 2],
                FactoryId = (int)data[i, 3]
            };
        }

        return units;
    }

    // реализуйте этот метод, чтобы он возвращал массив заводов, согласно приложенным таблицам
    public static Factory[] GetFactories()
    {
        var data = new object[,]
        {
        { 1, "HП3No1", "Первый нефтеперерабатывающий завод" },
        { 2, "HП3No2", "Второй нефтеперерабатывающий завод" }
        };

        var factories = new Factory[data.GetLength(0)];

        for (int i = 0; i < factories.Length; i++)
        {
            factories[i] = new Factory
            {
                Id = (int)data[i, 0],
                Name = (string)data[i, 1],
                Description = (string)data[i, 2]
            };
        }

        return factories;
    }

    // реализуйте этот метод, чтобы он возвращал установку (Unit), которой
    // принадлежит резервуар (Tank), найденный в массиве резервуаров по имени
    // учтите, что по заданному имени может быть не найден резервуар

    public static Unit FindUnit(Unit[] units, Tank[] tanks, string tankName)
    {
        var tank = Array.Find(tanks, t => t.Name == tankName);
        if (tank == null) return null;
        return Array.Find(units, u => u.Id == tank.UnitId);
    }

    // реализуйте этот метод, чтобы он возвращал объект завода, соответствующий установке
    public static Factory FindFactory(Factory[] factories, Unit unit)
    {
        if (unit == null) return null;
        return Array.Find(factories, f => f.Id == unit.FactoryId);
    }

    // реализуйте этот метод, чтобы он возвращал суммарный объем резервуаров в массиве
    public static int GetTotalVolume(Tank[] tanks)
    {
        int total = 0;
        foreach (var tank in tanks)
            total += tank.Volume;
        return total;
    }
}

/// <summary>
/// Установка
/// </summary>
public class Unit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int FactoryId { get; set; }
}

/// <summary>
/// Завод
/// </summary>
public class Factory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

/// <summary>
/// Резервуар
/// </summary>
public class Tank
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Volume { get; set; }
    public int MaxVolume { get; set; }
    public int UnitId { get; set; }
}