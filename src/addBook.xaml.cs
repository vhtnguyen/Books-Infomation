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
using System.Windows.Shapes;
using static _20120540_week07.MainWindow;

namespace _20120540_week07
{
    /// <summary>
    /// Interaction logic for addBook.xaml
    /// </summary>
    public partial class addBook : Window
    {
        public Book newBook { get; set; }
        public delegate void addBookdele(Book newB);
        public event addBookdele handleAdd;
        public addBook()
        {
            InitializeComponent();
            newBook = new Book();
            detail_panel.DataContext = newBook;
            preview_panel.DataContext = newBook;

        }

        private void btn_add(object sender, RoutedEventArgs e)
        {

            if (handleAdd != null)
            {
                handleAdd(newBook);
            }
            DialogResult = true;
        }

        private void btn_restore(object sender, RoutedEventArgs e)
        {
            newBook.Title = "";
            newBook.Cover = "";
            newBook.Author = "";
            newBook.Year = "";

        }
    }
}
