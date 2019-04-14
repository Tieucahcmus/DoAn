using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aspose.Cells;
using System.IO;

namespace DoAn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// khai báo DBEntities
        /// </summary>

        public storeDBEntities db = new storeDBEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();

            screen.Filter = "Excel file (*.xlsx)| *xlsx";
            if (screen.ShowDialog() == true)
            {
                var filename = screen.FileName;
                var workbook = new Workbook(filename);
                var info = new FileInfo(filename);
                var folder = info.Directory;
                int sheet_index = 0;
                var sheet = workbook.Worksheets[sheet_index];
                var Start_column = 'A';
                var Start_row = 3;
                var cell = sheet.Cells[$"{Start_column}{Start_row}"];

                List<string> list_catName = new List<string>();
                try
                {
                    while (sheet != null)
                    {
                        var _category = new category()
                        {
                            name = sheet.Name.ToString()
                        };
                        db.categories.Add(_category);
                        list_catName.Add(sheet.Name.ToString());
                        db.SaveChanges();
                        sheet_index++;
                        sheet = workbook.Worksheets[sheet_index];

                        var _name = sheet.Cells[$"B{Start_row}"].StringValue;
                        var _price = sheet.Cells[$"C{Start_row}"].StringValue;
                        var _quantity = sheet.Cells[$"D{Start_row}"].StringValue;
                        var _img = sheet.Cells[$"E{Start_row}"].StringValue;
                        var _product = new product()
                        {
                            catid = sheet_index,
                            name = _name,
                            quantity = Int32.Parse(_quantity),
                            price = Int32.Parse(_price)
                        };
                        Start_row++;
                    }

                }
                catch (Exception) { }
                cbbtype.ItemsSource = db.categories.Select(d => d.name).ToList();
            }
        }
    }
}
