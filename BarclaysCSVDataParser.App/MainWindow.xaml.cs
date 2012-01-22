using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElencySolutions.CsvHelper;
using System.Text.RegularExpressions;

namespace BarclaysCSVDataParser.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int NumberIndex = 0;
        int DateIndex = 1;
        int AccountIndex = 2;
        int AmountIndex = 3;
        int TransactionTypeIndex = 4;
        int NotesIndex = 5;
        int CategoryIndex = 6;
        int SubCategoryIndex = 7;

        private const string DIRECT_DEBIT = "DIRECTDEBIT";
        private const string DIRECT_DEPOSIT = "DIRECTDEP";
        private const string PAYMENT = "PAYMENT";
        private const string CASH_WITHDRAWAL = "CASH";
        private const string STANDING_ORDER = "REPEATPMT";
        private const string OTHER = "OTH";

        CsvFile csvFile = new CsvFile();
        List<Mapper> mappers = new List<Mapper>();

        public MainWindow()
        {
            InitializeComponent();

            mappers = MapperRepository.GetMappers();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.FileName = "data"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "Text documents (.csv)|*.csv"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();

            if (!result.HasValue || !result.Value)
                return;

            string filename = dlg.FileName;

            var records = new List<List<string>>();

            using (CsvReader reader = new CsvReader(filename, Encoding.Default))
            {
                while (reader.ReadNextRecord())
                    records.Add(reader.Fields);
            }

            if (records[0][0] == "Number")
                records.Remove(records[0]);

            //number,date,acc,amount,transactiontype,notes

            csvFile = new CsvFile();

            var headers = new List<string> { "Number", "Date", "Account", "Amount", "Transaction Type", "Notes", "Category", "Sub Category" };

            headers.ForEach(header => csvFile.Headers.Add(header));

            foreach (var fields in records)
            {
                var record = new CsvRecord();

                fields.ForEach(field => record.Fields.Add(field));

                SetCategory(record);

                csvFile.Records.Add(record);
            }
        }

        private void SetCategory(CsvRecord record)
        {
            string transactionType = record.Fields[TransactionTypeIndex];
            string category = string.Empty, subcategory = string.Empty;

            string csvline = record.ToString();
            bool found = false;

            foreach (var mapper in mappers)
            {
                if (Regex.IsMatch(csvline, mapper.RegexPattern))
                {
                    found = true;
                    record.Fields.Add(mapper.Category);
                    break;
                }
            }

            if (!found)
            {
                var view = new CaptureMapper(record);
                view.ShowDialog();

                if (!view.DialogResult.Value)
                    throw new Exception("Mapper not set!");

                var mapper = new Mapper()
                {
                    RegexPattern = view.Model.RegexPattern,
                    Category = view.Model.Category
                };

                mappers.Add(mapper);

                MapperRepository.SetMappers(mappers);
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.FileName = "accounts" + DateTime.Now.ToString("YYYYmmDD"); // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "Text documents (.csv)|*.csv"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();

            if (!result.HasValue || !result.Value)
                return;

            string filename = dlg.FileName;

            using (CsvWriter writer = new CsvWriter())
            {
                writer.WriteCsv(csvFile, filename);
            }
        }
    }
}
