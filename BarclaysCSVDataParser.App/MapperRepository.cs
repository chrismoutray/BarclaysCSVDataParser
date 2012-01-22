using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElencySolutions.CsvHelper;
using System.IO;

namespace BarclaysCSVDataParser.App
{
    public static class MapperRepository
    {
        static int RegexPatternIndex = 0;
        static int CategoryIndex = 1;

        static string path = "mappers.csv";

        public static List<Mapper> GetMappers()
        {
            var mappers = new List<Mapper>();

            if (!File.Exists(path))
                return mappers;

            using (CsvReader reader = new CsvReader(path, Encoding.Default))
            {
                while (reader.ReadNextRecord())
                {
                    mappers.Add(new Mapper() 
                    { 
                        Category = reader.Fields[CategoryIndex],
                        RegexPattern = reader.Fields[RegexPatternIndex]
                    });
                }
            }

            return mappers;
        }

        public static void SetMappers(List<Mapper> mappers)
        {
            var csvFile = new CsvFile();

            csvFile.Headers.Add("RegexPattern");
            csvFile.Headers.Add("Category");

            foreach (var mapper in mappers)
            {
                var record = new CsvRecord();

                record.Fields.Add(mapper.RegexPattern);
                record.Fields.Add(mapper.Category);

                csvFile.Records.Add(record);
            }

            using (CsvWriter writer = new CsvWriter())
                writer.WriteCsv(csvFile, "t"+path);

            File.Delete(path);
            File.Move("t" + path, path);
        }
    }
}
