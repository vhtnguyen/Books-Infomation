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
    /// Interaction logic for editBook.xaml
    /// </summary>
    public partial class editBook : Window
    {
        public Book editedBook { get; set; }
        public delegate void bookDelegate(Book newBook);
        public event bookDelegate handleEdit;
        public delegate void restoreDelegate();
        public event restoreDelegate handleRestore;
     
        public editBook(Book oldBook)
        {
            InitializeComponent();
            editedBook = (Book)oldBook.Clone();
            this.DataContext = editedBook;
        }

        private void btn_edit(object sender, RoutedEventArgs e)
        {
            if (handleEdit != null)
            {
                handleEdit(editedBook);
                DialogResult = true;
            }

        }

        private void btn_restore(object sender, RoutedEventArgs e)
        {
            if (handleRestore != null)
            {
                handleRestore();
            }
        }
    }
}
