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
                var column = 'B';
                var row = 2;
                var temp_id = 1;
                var cell = sheet.Cells[$"{column}{row}"];
                var _list = new List<product>();
                try
                {
                    while (sheet != null)
                    {
                        var _category = new category()
                        {
                            name = sheet.Name.ToString()
                        };
                        db.categories.Add(_category);
                        db.SaveChanges();
                        sheet_index++;
                        sheet = workbook.Worksheets[sheet_index];                     
                    }
                }
                catch (Exception) { }
              
                cbbtype.ItemsSource = db.categories.Select(d => d.name).ToList();
            }
        }
    }
}

// try
//                {
//                    int id_temp = 1;
//var row = 2;
//var temp = sheet.Cells[$"B{row}"];
//                        while (temp.Value != null)
//                        {
//                            var _name = sheet.Cells[$"B{row}"].StringValue;
//var _price = sheet.Cells[$"C{row}"].StringValue;
//var _quantity = sheet.Cells[$"D{row}"].StringValue;
//var _img = sheet.Cells[$"E{row}"].StringValue;
//var _product = new product()
//{
//    catid = id_temp,
//    name = _name,
//    quantity = Int32.Parse(_quantity),
//    price = Int32.Parse(_price),
//    img = _img
//};
//db.products.Add(_product);
//                            db.SaveChanges();
//                            row++;
//                            id_temp++;
//                        }
//                }
//                catch { }