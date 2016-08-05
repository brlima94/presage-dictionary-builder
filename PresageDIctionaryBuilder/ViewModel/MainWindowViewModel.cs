using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PresageDIctionaryBuilder.Utils;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace PresageDIctionaryBuilder.ViewModel
{
    public class MainWindowViewModel
    {
        public Window MyWindow { get; private set; }

        public MainWindowViewModel(Window wnd)
        {
            MyWindow = wnd;
        }

        public string Text2NgramPath { get; set; }
        public string DictionaryTextPath { get; set; }
        public string DatabasePath { get; set; }

        public Task<bool> SelectText2GramExe()
        {
            bool result = false;
            try
            {
                string path = Dialogs.OpenFile(MyWindow, Dialogs.FileType.Exe);
                if (!string.IsNullOrWhiteSpace(path) &&
                    Path.GetFileName(path).Equals("text2ngram.exe", StringComparison.CurrentCultureIgnoreCase))
                {
                    Text2NgramPath = path;
                    result = true;
                }
                return Task.FromResult<bool>(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> SelectDictionaryTextPath()
        {
            bool result = false;
            try
            {
                string path = Dialogs.OpenFile(MyWindow, Dialogs.FileType.Text);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    DictionaryTextPath = path;
                    result = true;
                }
                return Task.FromResult<bool>(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> SelectDatabasePath()
        {
            bool result = false;
            try
            {
                string path = Dialogs.SaveFile(MyWindow, Dialogs.FileType.SQLite);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    DatabasePath = path;
                    result = true;
                }
                return Task.FromResult<bool>(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> CreateSQLiteDatabase()
        {
            try
            {

                string inputFile = FormatDictionary(DictionaryTextPath);
                Process p = new Process();

                p.StartInfo.FileName = Text2NgramPath;
                p.StartInfo.Arguments = string.Format("-n3 -f sqlite -o \"{0}\" \"{1}\"", DatabasePath, inputFile);

                p.Start();
                p.WaitForExit();
                if (p.ExitCode != 0)
                    return Task.FromResult<bool>(false);


                p.StartInfo.Arguments = string.Format("-a -n2 -f sqlite -o \"{0}\" \"{1}\"", DatabasePath, inputFile);
                p.Start();
                p.WaitForExit();
                if (p.ExitCode != 0)
                    return Task.FromResult<bool>(false);

                p.StartInfo.Arguments = string.Format("-a -n1 -f sqlite -o \"{0}\" \"{1}\"", DatabasePath, inputFile);
                p.Start();
                p.WaitForExit();
                if (p.ExitCode != 0)
                    return Task.FromResult<bool>(false);

                return Task.FromResult<bool>(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string FormatDictionary(string dictionaryTextPath)
        {
            try
            {
                string tempFile = Path.GetTempFileName();
                string currentLine;
                string[] words;
                decimal tempDecimal = 0;

                using (StreamReader sr = new StreamReader(dictionaryTextPath, Encoding.Default))
                {
                    using (StreamWriter sw = new StreamWriter(tempFile, false, Encoding.Default))
                    {
                        while ((currentLine = sr.ReadLine()) != null)
                        {
                            if (!string.IsNullOrWhiteSpace(currentLine))
                            {
                                // prevent numbers from being added to dictionary
                                words = currentLine.Split(" ".ToCharArray(), StringSplitOptions.None).Where(i => !decimal.TryParse(i, out tempDecimal)).ToArray();
                                for (int i = 0; i < words.Length; i++)
                                {
                                    if (!string.IsNullOrWhiteSpace(words[i])
                                     && char.IsUpper(words[i][0]))
                                    {
                                        words[i] = CapitalizeWord(words[i]);
                                    }
                                }

                                if (words.Length > 0)
                                {
                                    sw.WriteLine(string.Join(" ", words));
                                }
                            }
                        }
                    }
                }
                return tempFile;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string CapitalizeWord(string word)
        {
            string result;
            List<char> chars = new List<char>();
            for (int i = 0; i < word.Length; i++)
            {
                if (i == 0)
                    chars.Add(word[i]);
                else
                    chars.Add(char.ToLower(word[i], CultureInfo.CurrentCulture));
            }

            result = new string(chars.ToArray());

            return result;
        }
    }
}
