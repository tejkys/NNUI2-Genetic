using System.Globalization;
using System.Text;
using OfficeOpenXml;

namespace NNUI2_Genetic;

public class ExcelLoader
{
    public List<Pub> ReadPubs(string filename)
    {
        var result = new List<Pub>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(filename)))
        {

            var myWorksheet = xlPackage.Workbook.Worksheets.First();
            var totalRows = myWorksheet.Dimension.End.Row;
            var totalColumns = myWorksheet.Dimension.End.Column;

            for (int rowNum = 3; rowNum <= totalRows; rowNum++)
            {
                var row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
                int index = 0;
                var pub = new Pub();
                foreach (var col in row)
                {
                    switch (index)
                    {
                        case 0:
                            break;
                        case 1:
                            pub.Name = col;
                            break;
                        case 2:
                            var coords = col.Split(", ");
                            pub.Latitude = double.Parse(coords[0], CultureInfo.InvariantCulture);
                            pub.Longitude = double.Parse(coords[1], CultureInfo.InvariantCulture);
                            break;
                    }
                    index++;
                }
                result.Add(pub);
            }
        }
        return result;
    }
}