using System;
using System.IO;
using System.Linq;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using XamarinAdventCalendarApp.Models;

namespace XamarinAdventCalendarApp.Services
{
    public class ExcelService
    {
        private string AppFolder => 
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private Cell ConstructCell(string value, CellValues dataType) => 
            new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };

        public string GenerateExcel(string fileName)
        {
            Environment.SetEnvironmentVariable("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");

            var filePath = Path.Combine(AppFolder, fileName);
            var document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);

            var wbPart = document.AddWorkbookPart();
            wbPart.Workbook = new Workbook();

            var part = wbPart.AddNewPart<WorksheetPart>();
            part.Worksheet = new Worksheet(new SheetData());

            var sheets = wbPart.Workbook.AppendChild(new Sheets());
            var sheet = new Sheet()
            {
                Id = wbPart.GetIdOfPart(part),
                SheetId = 1,
                Name = "Publications"
            };
            
            sheets.Append(sheet);

            wbPart.Workbook.Save();
            document.Close();

            return filePath;
        }

        public void InsertDataIntoSheet(string fileName, string sheetName, ExcelData data)
        {
            Environment.SetEnvironmentVariable("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");

            using (var document = SpreadsheetDocument.Open(fileName, true))
            {
                var wbPart = document.WorkbookPart;
                var sheets = wbPart.Workbook.GetFirstChild<Sheets>();

                var sheet = sheets.Elements<Sheet>().FirstOrDefault();
                sheet.Name = sheetName;

                var part = wbPart.WorksheetParts.First();
                var sheetData = part.Worksheet.Elements<SheetData>().First();

                var row = sheetData.AppendChild(new Row());

                foreach (var header in data.Headers)
                {
                    var cell = ConstructCell(header, CellValues.String);
                    row.Append(cell);
                }

                foreach (var value in data.Values)
                {
                    var dataRow = sheetData.AppendChild(new Row());

                    foreach (var dataElement in value)
                    {
                        var cell = ConstructCell(dataElement, CellValues.String);
                        dataRow.Append(cell);
                    }
                }

                wbPart.Workbook.Save();
            }
        }
    }
}
