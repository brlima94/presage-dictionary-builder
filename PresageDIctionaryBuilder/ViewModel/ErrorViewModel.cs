using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PresageDIctionaryBuilder.ViewModel
{
    public class ErrorViewModel
    {
        public Window MyWindow { get; private set; }

        public ErrorViewModel(Window wnd)
        {
            MyWindow = wnd;
        }

        public void TratarExcecao(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            MessageBox.Show(MyWindow, ex.ToString());
        }
    }
}
