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
using PresageDIctionaryBuilder.ViewModel;
using PresageDIctionaryBuilder.Utils;

namespace PresageDIctionaryBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;
        private ErrorViewModel error;

        public MainWindow()
        {
            viewModel = new MainWindowViewModel(this);
            error = new ErrorViewModel(this);
            InitializeComponent();
        }

        private async void StarConversion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(viewModel.Text2NgramPath) ||
                   string.IsNullOrWhiteSpace(viewModel.DictionaryTextPath) ||
                   string.IsNullOrWhiteSpace(viewModel.DatabasePath))
                {
                    throw new InvalidOperationException("Not all file paths were informed!");
                }
                
                Cursor = Cursors.Wait;
                ((UIElement)Content).IsEnabled = false;

                ProgressIndicator.IsIndeterminate = true;

                bool result = await viewModel.CreateSQLiteDatabase();
                
                ProgressIndicator.IsIndeterminate = false;

                if (result)
                {
                    ProgressIndicator.Value = 100;
                    textBlock_Copy3.FontWeight = FontWeights.Bold;
                }
            }
            catch (Exception ex)
            {
                error.TratarExcecao(ex);
            }
            finally
            {
                ((UIElement)Content).IsEnabled = true;
                Cursor = null;
            }
        }

        private async void text2ngramSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cursor = Cursors.Wait;
                ((UIElement)Content).IsEnabled = false;
                bool result = await viewModel.SelectText2GramExe();
                if (result)
                {
                    text2ngramDisplay.Text = viewModel.Text2NgramPath;
                }
            }
            catch (Exception ex)
            {
                error.TratarExcecao(ex);
            }
            finally
            {
                ((UIElement)Content).IsEnabled = true;
                Cursor = null;
            }
        }

        private async void dictionaryPathSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cursor = Cursors.Wait;
                ((UIElement)Content).IsEnabled = false;
                bool result = await viewModel.SelectDictionaryTextPath();

                if (result)
                {
                    dictionaryPathDisplay.Text = viewModel.DictionaryTextPath;
                }
            }
            catch (Exception ex)
            {
                error.TratarExcecao(ex);
            }
            finally
            {
                ((UIElement)Content).IsEnabled = true;
                Cursor = null;
            }
        }

        private async void databasePathSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cursor = Cursors.Wait;
                ((UIElement)Content).IsEnabled = false;
                bool result = await viewModel.SelectDatabasePath();
                if (result)
                {
                    databasePathDisplay.Text = viewModel.DatabasePath;
                }
            }
            catch (Exception ex)
            {
                error.TratarExcecao(ex);
            }
            finally
            {
                ((UIElement)Content).IsEnabled = true;
                Cursor = null;
            }
        }
    }
}
