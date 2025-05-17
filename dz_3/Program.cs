using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        var ui = new UserInterface();
        ui.UserEnteredNumber += (sender, e) =>
        {
            Console.WriteLine($"Пользователь ввел число {e.Input} в {e.EnteredAt}");
        };

        IEnumerable<Tank> tanks;
        IEnumerable<Unit> units;
        IEnumerable<Factory> factories;

        try
        {
            tanks = DataService.LoadFromJson<Tank>("tanks.json");
            units = DataService.LoadFromJson<Unit>("units.json");
            factories = DataService.LoadFromJson<Factory>("factories.json");
        }
        catch (ApplicationException ex)
        {
            ui.WriteMessage(ex.Message);
            return;
        }

        ui.WriteMessage($"Количество резервуаров: {tanks.Count()}, установок: {units.Count()}");

        ui.WriteMessage("Введите название резервуара для поиска:");
        var tankName = ui.ReadInput();

        try
        {
            var tank = tanks.FirstOrDefault(t => t.Name.Equals(tankName, StringComparison.OrdinalIgnoreCase));
            if (tank == null)
                throw new InvalidOperationException($"Не найдена установка с именем {tankName}!");

            var unit = units.FirstOrDefault(u => u.Id == tank.UnitId);
            var factory = factories.FirstOrDefault(f => f.Id == unit.FactoryId);

            ui.WriteMessage($"Резервуар {tank.Name} принадлежит установке {unit.Name} и заводу {factory.Name}");
        }
        catch (InvalidOperationException ex)
        {
            ui.WriteMessage(ex.Message);
            return;
        }

        int totalVolume = tanks.Sum(t => t.Volume);
        ui.WriteMessage($"Общий объем резервуаров: {totalVolume}");

        ui.WriteMessage("Выберите завод для отображения резервуаров:");
        foreach (var fac in factories)
            ui.WriteMessage($"{fac.Id}: {fac.Name}");

        ui.WriteMessage("Введите ID завода (или 0 — чтобы показать все резервуары):");
        var input = ui.ReadInput();
        if (int.TryParse(input, out int selectedFactoryId))
        {
            var filteredTanks = selectedFactoryId == 0
                ? tanks
                : tanks.Where(t =>
                {
                    var unit = units.FirstOrDefault(u => u.Id == t.UnitId);
                    return unit != null && unit.FactoryId == selectedFactoryId;
                });

            foreach (var tank in filteredTanks)
            {
                var unit = units.FirstOrDefault(u => u.Id == tank.UnitId);
                var fac = factories.FirstOrDefault(f => f.Id == unit.FactoryId);
                ui.WriteMessage($"{tank.Name} — Установка: {unit.Name}, Завод: {fac.Name}");
            }
        }
        else
        {
            ui.WriteMessage("Некорректный ввод. Попробуйте снова.");
        }

        // Сохраняем все объекты в JSON-файл
        try
        {
            DataService.SaveToJson("output_tanks.json", tanks);
            DataService.SaveToJson("output_units.json", units);
            DataService.SaveToJson("output_factories.json", factories);
            ui.WriteMessage("Данные успешно сохранены.");
        }
        catch (ApplicationException ex)
        {
            ui.WriteMessage(ex.Message);
        }
    }
}



//Чтобы вызвать событие SomeEvent извне класса SomeClass, 
//    необходимо добавить в класс метод, 
//    который будет вызывать это событие. 
//    Например:
//    public class SomeClass
//{
//    public event SomeEventHandler SomeEvent;

//    public void RaiseSomeEvent(EventArgs e)
//    {
//        SomeEvent?.Invoke(this, e);
//    }
//}
//Теперь, извне класса, можно вызвать событие следующим образом:
//var s = new SomeClass();
//s.SomeEvent += OnSomeEvent;
//s.RaiseSomeEvent(new EventArgs());
