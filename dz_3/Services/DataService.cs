using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class DataService
{
    public static IEnumerable<T> LoadFromJson<T>(string fileName)
    {
        try
        {
            var jsonString = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<IEnumerable<T>>(jsonString);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Ошибка при загрузке файла {fileName}: {ex.Message}");
        }
    }

    public static void SaveToJson<T>(string fileName, IEnumerable<T> data)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(fileName, jsonString);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Ошибка при сохранении файла {fileName}: {ex.Message}");
        }
    }
}
