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
using System.Diagnostics;

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
            string currentFolder = AppDomain.CurrentDomain.BaseDirectory;
            var baseFolder = currentFolder.Substring(0, currentFolder.Length - 1);

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
                int cat_id = 1;
                try
                {
                    while (sheet != null)
                    {
                        category _category = new category()
                        {
                            name = sheet.Name.ToString()
                        };
                        db.categories.Add(_category);
                        db.SaveChanges();
                        try
                        {
                            var row = 2;
                            var cell = sheet.Cells[$"B{row}"];
                            while (cell.Value!=null )
                            {
                                var _catid = cat_id;
                                var _name = sheet.Cells[$"B{row}"].StringValue;
                                var _price = sheet.Cells[$"C{row}"].IntValue;
                                var _quantity = sheet.Cells[$"D{row}"].IntValue;
                                var _img = sheet.Cells[$"E{row}"].StringValue;
                                var newName = "";
                                try
                                {
                                    _img = folder + @"\img\" + _img;
                                    var imgInfo = new FileInfo(_img);
                                    newName = Guid.NewGuid() + "." + imgInfo.Extension;
                                    imgInfo.CopyTo(baseFolder + @"\img\" + newName);
                                }
                                catch (Exception) { }                               
                                product _product = new product()
                                {
                                    catid = _catid,
                                    name = _name,
                                    price = _price,
                                    quantity = _quantity,
                                    img = newName
                                };
                                db.products.Add(_product);
                                db.SaveChanges();
                                row++;
                                cell = sheet.Cells[$"B{row}"];
                            }
                        }
                        catch (Exception) { }
                        cat_id++;
                        sheet_index++;
                        sheet = workbook.Worksheets[sheet_index];
                    }
                }
                catch (Exception) { }
                //cbbtype.ItemsSource = db.categories.Select(d => d.name).ToList();
                listview_product.ItemsSource= db.products.ToList();
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
