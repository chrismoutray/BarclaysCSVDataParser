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
using System.Windows.Shapes;
using ElencySolutions.CsvHelper;

namespace BarclaysCSVDataParser.App
{
    /// <summary>
    /// Interaction logic for CaptureMapper.xaml
    /// </summary>
    public partial class CaptureMapper : Window
    {
        public CaptureMapperModel Model { get; set; }

        public CaptureMapper(CsvRecord record)
        {
            InitializeComponent();

            Model = new CaptureMapperModel();

            Model.CsvRecord = record.ToString();

            string memo = string.Empty;

            int refIndex = record.Fields[5].IndexOf(" REF ");

            if (refIndex > 0)
                memo = record.Fields[5].Substring(0, record.Fields[5].IndexOf(" REF "));
            else
                memo = record.Fields[5];

            Model.RegexPattern = record.Fields[4] + ".*" + memo.Trim();

            this.DataContext = Model;
        }

        public class CaptureMapperModel : PropertyChangedBase
        {
            public CaptureMapperModel()
            {
                CategoryList = CategoryListProvider.GetCategoryList();
            }

            private string _csvRecord;

            public string CsvRecord
            {
                get { return _csvRecord; }
                set {_csvRecord = value;
                NotifyOfPropertyChange(() => CsvRecord);
                }
            }

            private string _RegexPattern;

            public string RegexPattern
            {
                get { return _RegexPattern; }
                set { _RegexPattern = value;
                NotifyOfPropertyChange(() => RegexPattern);
                }
            }

            private string _Category;

            public string Category
            {
                get { return _Category; }
                set { _Category = value;
                NotifyOfPropertyChange(() => Category);
                }
            }

            private List<string> _CategoryList;

            public List<string> CategoryList
            {
                get { return _CategoryList; }
                set { _CategoryList = value;
                    NotifyOfPropertyChange(() => CategoryList);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
