using System;
using System.Collections.Generic;
using System.ComponentModel;
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


namespace _20120540_week07
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        BindingList<Book> _books;
        List<Book> _pagingBooks;
        BindingList<Book> _searchResultBooks;
        Book _currentSelected = null;
        Book _default = null;
        int _current;
        int _perPage;
        int _total;
        int _prevTab;

        public MainWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));

        }


        public class Book : INotifyPropertyChanged,ICloneable
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public string Year { get; set; }
            public string Cover { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public object Clone()
            {
                return this.MemberwiseClone() as Book;
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            _books = new BindingList<Book>()
            {
                new Book() {Title="SAPIENS", Author="Yuval Noah Harari",
                    Year="2015", Cover="/images/1.png"},
                new Book() {Title="THE BODY KEEPS THE SCORE", Author="Bessel van der Kolk",
                    Year="2015", Cover="/images/2.png"},
                new Book() {Title="BEING MORTAL", Author="Atul Gawande",
                    Year="2015", Cover="/images/3.png"},
                new Book() {Title="THINKING, FAST AND SLOW", Author="Daniel Kahneman",
                    Year="2015", Cover="/images/4.png"},
                new Book() {Title="THE MOSQUITO", Author="Timothy C Winegard",
                    Year="2015", Cover="/images/5.png"},
                new Book() {Title="THE IMMORTAL LIFE OF HENRIETTA LACKS", Author="Rebecca Skloot",
                    Year="2015", Cover="/images/6.png"},
                new Book() {Title="DOPESICK", Author="Beth Macy",
                    Year="2015", Cover="/images/7.png"},
                new Book() {Title="WHY WE SLEEP", Author="Matthew Walker",
                    Year="2015", Cover="/images/8.png"},
                new Book() {Title="21 LESSONS FOR THE 21ST CENTURY", Author="Yuval Noah Harari",
                    Year="2015", Cover="/images/9.png"},
                new Book() {Title="QUIET", Author="Susan Cain",
                    Year="2015", Cover="/images/10.png"}

            };
            _default = new Book()
            {
                Title = "Book's title",
                Author = "Author",
                Year = "yyyy",
                Cover = "/images/0.png"
            };
            _setDefault();
            bookList.ItemsSource = _books;
            _current = 0;
            _perPage = 6;
            _total= _books.Count()/_perPage+1;
            _prevTab = -1;
            _searchResultBooks = new BindingList<Book>();
            searchResult_List.ItemsSource = _searchResultBooks;


        }




        private void _showDetail()
        {
            int index = bookList.SelectedIndex;


            if (index < 0)
            {
                _setDefault();
            }
            else
            {

                _currentSelected = _books[index];
                detail_panel.DataContext = _currentSelected;
            }

        }

        private void _setDefault()
        {

            detail_panel.DataContext = _default;
        }
        private void _goToPage(int pageNum)
        {
            if(pageNum>=_total|| pageNum < 0)
            {
                return;
            }
            else
            {
                _current= pageNum;

            }
            _pagingBooks = _books
               .Skip(_current * _perPage)
               .Take(_perPage)
               .ToList();
            pageList.ItemsSource = _pagingBooks;
            lb_pageNum.Content = $"{_current+1}/{_total}";
        }



        private void btn_add(object sender, RoutedEventArgs e)
        {
            var screen = new addBook();
            screen.handleAdd += (newBook) =>
            {
                _books.Add(newBook);
            };
            screen.ShowDialog();
        }

        private void btn_edit(object sender, RoutedEventArgs e)
        {
            int index = bookList.SelectedIndex;

            if (index >= 0)
            {
                var book = _books[index];
                var screen = new editBook(book);
                screen.handleEdit += (newBook) =>
                {
                    if (newBook!=null)
                    {
                        _currentSelected.Title = newBook.Title;
                        _currentSelected.Author=    newBook.Author;
                        _currentSelected.Year= newBook.Year;
                        _currentSelected.Cover= newBook.Cover;
                    }

                };
                screen.handleRestore += () =>
                {
                    screen.editedBook.Title = _currentSelected.Title;
                    screen.editedBook.Author = _currentSelected.Author;
                    screen.editedBook.Year = _currentSelected.Year;
                    screen.editedBook.Cover = _currentSelected.Cover;
                };
                screen.ShowDialog();
            }

        }

        private void btn_remove(object sender, RoutedEventArgs e)

        {
            int index = bookList.SelectedIndex;
            if (index >= 0)
            {
                _books.RemoveAt(index);
                _setDefault();
            }

        }

        private void bookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            _showDetail();
        }

        private void btn_prev(object sender, RoutedEventArgs e)
        {
            
            _goToPage(_current-1);

        }

        private void btn_next(object sender, RoutedEventArgs e)
        {
            
            _goToPage(_current+1);
        }

      

        private void pageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = pageList.SelectedIndex;

            Console.WriteLine(index);

            if (index < 0)
            {
                detail_book.DataContext = _default;
            }
            else
            {

                _currentSelected = _pagingBooks[index];
                detail_book.DataContext = _currentSelected;
            }

        }

     

        private void tabList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index=tabList.SelectedIndex;
            if (index != _prevTab)
            {
               _prevTab=index;
                if (index == 1)
                {
                    _goToPage(0);
                    detail_book.DataContext = _default;
                }
                if (index == 2)
                {
                    _setDefaultSearch();
                }
                Keyboard.ClearFocus();
            }

        }
        private void _setDefaultSearch()
        {
            input_seacrch.Clear();
            search_detailPanel.DataContext = _default;
            search_option.SelectedIndex = 0;
            img_noresult.Visibility = Visibility.Visible;
            searchResult_List.Visibility = Visibility.Collapsed;
        }

        private void btn_search(object sender, RoutedEventArgs e)
        {
            _searchResultBooks.Clear();
            if (!(input_seacrch.Text==""))
            {
                switch (search_option.SelectedIndex)
                {
                    case 1:
                        foreach (Book book in _books)
                        {
                            if (book.Author.ToLower() == input_seacrch.Text.ToLower())
                            {
                                _searchResultBooks.Add((Book)book.Clone());

                            }

                        }

                        break;
                    case 2:
                        foreach (Book book in _books)
                        {
                            if (book.Year == input_seacrch.Text)
                            {
                                _searchResultBooks.Add((Book)book.Clone());

                            }

                        }
                        break;
                    default:
                        foreach (Book book in _books)
                        {
                            if (book.Title.ToLower() == input_seacrch.Text.ToLower())
                            {
                                _searchResultBooks.Add((Book)book.Clone());

                            }

                        }
                        break;
                }

                if (_searchResultBooks==null||_searchResultBooks.Count == 0)
                {
                    img_noresult.Visibility = Visibility.Visible;
                    searchResult_List.Visibility = Visibility.Collapsed;
                    return;
                }
                else
                {
                    //searchResult_List.ItemsSource = _searchResultBooks;
                    img_noresult.Visibility = Visibility.Collapsed;
                    searchResult_List.Visibility = Visibility.Visible;
                    input_seacrch.Clear();
                }
                        
            }
            else
            {
                
            }

            

        }

        private void searchResult_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            int index = searchResult_List.SelectedIndex;

            if (index < 0)
            {
                detail_book.DataContext = _default;
            }
            else
            {

                _currentSelected = _searchResultBooks[index];
                search_detailPanel.DataContext = _currentSelected;
            }


        }

        private void input_seacrch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_search(this, new RoutedEventArgs());
            }

        }
    }
}
