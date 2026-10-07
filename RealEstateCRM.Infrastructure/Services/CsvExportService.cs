using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;

namespace RealEstateCRM.API.Services
{
    public class CsvExportService
    {
        public byte[] ExportToCsv<T>(IEnumerable<T> records)
        {
            using var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(records);
            }

            return memoryStream.ToArray();
        }
    }
}