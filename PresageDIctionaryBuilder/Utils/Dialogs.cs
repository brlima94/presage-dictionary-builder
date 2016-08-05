using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace PresageDIctionaryBuilder.Utils
{
    public class Dialogs
    {
        public enum FileType
        {
            All = 0,
            Text = 1,
            SQLite = 2,
            Exe = 3
        }

        public static string OpenFile(Window wnd, FileType type)
        {
            try
            {
                string result = null;
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Multiselect = false;
                switch (type)
                {
                    case FileType.Text:
                        ofd.Filter = "Text Files|*.txt";
                        ofd.Title = "Open text file";
                        break;
                    case FileType.SQLite:
                        ofd.Filter = "SQLite Database|*.db";
                        ofd.Title = "Open SQLite database";
                        break;
                    case FileType.All:
                        ofd.Filter = "All Files|*.*";
                        ofd.Title = "Open file";
                        break;
                    case FileType.Exe:
                        ofd.Filter = "Applications|*.exe";
                        ofd.Title = "Open application";
                        break;
                }
                if (ofd.ShowDialog(wnd) == true)
                {
                    result = ofd.FileName;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string SaveFile(Window wnd, FileType type)
        {
            try
            {
                string result = null;
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.OverwritePrompt = true;
                switch (type)
                {
                    case FileType.Text:
                        sfd.Filter = "Text Files|*.txt";
                        sfd.Title = "Save text file";
                        sfd.DefaultExt = ".txt";
                        break;
                    case FileType.SQLite:
                        sfd.Filter = "SQLite Database|*.db";
                        sfd.Title = "Save SQLite database";
                        break;
                    case FileType.All:
                        sfd.Filter = "All Files|*.*";
                        sfd.Title = "Save file";
                        break;
                }
                if (sfd.ShowDialog(wnd) == true)
                {
                    result = sfd.FileName;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
