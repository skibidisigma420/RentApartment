using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using static System.Net.Mime.MediaTypeNames;
using Application = Microsoft.Office.Interop.Excel.Application;
using Range = Microsoft.Office.Interop.Excel.Range;

public static class ExcelService
{
    public static IEnumerable<T> LoadFromExcel<T>(string filePath, Func<Range, T> parseRow)
    {
        var list = new List<T>();
        var excelApp = new Application();
        Workbook workbook = excelApp.Workbooks.Open(filePath);
        Worksheet worksheet = (Worksheet)workbook.Sheets[1];
        Range range = worksheet.UsedRange;

        for (int row = 2; row <= range.Rows.Count; row++)
        {
            var rowRange = range.Rows[row];
            var item = parseRow((Range)rowRange);
            list.Add(item);
        }

        workbook.Close(false);
        excelApp.Quit();
        return list;
    }

    public static void SaveToExcel<T>(string filePath, IEnumerable<T> data, Action<Worksheet, T, int> writeRow)
    {
        Application excelApp = new Application();
        Workbook workbook = excelApp.Workbooks.Add();
        Worksheet worksheet = (Worksheet)workbook.Sheets[1];

        int row = 2;
        foreach (var item in data)
        {
            writeRow(worksheet, item, row);
            row++;
        }

        workbook.SaveAs(filePath);
        workbook.Close();
        excelApp.Quit();
    }
}
