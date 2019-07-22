using Nut;
using System;
using MyTestApp.Models;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using MyTestApp.ViewModels.Extensions;

namespace MyTestApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            Errors = new List<string>();
            Randomizer = new Random();
        }

        #region Properties

        bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                SetProperty(ref isBusy, value);
                IsNotBusy = !value;
            }
        }

        string title = string.Empty;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        bool isNotBusy = true;
        public bool IsNotBusy
        {
            get => isNotBusy;
            set => SetProperty(ref isNotBusy, value);
        }

        public List<string> Errors { get; set; }

        public Random Randomizer { get; private set; }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged is null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Methods

        protected Task ShowAlert(string message, string acceptText = null)
        {
            if (string.IsNullOrEmpty(acceptText))
                return App.Current.MainPage.DisplayAlert("Información", message, "Aceptar");
            else
                return App.Current.MainPage.DisplayAlert("Información", message, acceptText, "Cancalar");
        }

        protected Task ShowError(string message, string acceptText = null)
        {
            if (string.IsNullOrEmpty(acceptText))
                return App.Current.MainPage.DisplayAlert("Error", message, "Aceptar");
            else
                return App.Current.MainPage.DisplayAlert("Error", message, acceptText, "Cancalar");
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected bool ValidateNewFileName(string fileName)
        {
            Errors.Clear();
            if (string.IsNullOrEmpty(fileName))
                Errors.Add("El nombre del archivo no puede ser nulo.");
            else
            {
                Regex regex = new Regex(@"^[A-Za-z0-9\-_.]*$");
                if (!regex.IsMatch(fileName))
                    Errors.Add("El nombre del archivo solo puede tener caracteres de: \"a-Z0-9._-\".");
            }

            return Errors.Count == 0;
        }

        protected void InsertCorretOptions(OperationModel operation, char? currentOperator = null)
        {
            if (currentOperator is null)
            {
                GenerateStringOptions(operation);

                operation.Options.Shuffle();
            }
            else
            {
                int randomIndex = Randomizer.Next(4);
                string answer = CalculateOperations(operation);

                if (!operation.Options.Contains(answer))
                    operation.Options[randomIndex] = answer;

                operation.Resoult = answer;
            }
        }

        protected string CalculateOperations(OperationModel operation)
        {

            if (operation.Problem is List<string> problems)
            {
                double number1 = double.Parse(problems[0]);
                double number2 = double.Parse(problems[1]);

                return (number1 + number2).ToString("n0");
            }
            else
            {
                int problemNumber = Randomizer.Next(5000);
                int percentage = Randomizer.Next(101);
                string text = operation.Problem.ToString();
                double problemPercentage = ((double)percentage / 100d);

                operation.Problem = string.Format(text, percentage, problemNumber);

                problemNumber += (int)(problemNumber * problemPercentage);

                return problemNumber.ToString("n0");
            }
        }

        protected void GenerateStringOptions(OperationModel operation)
        {
            List<string> options = new List<string>();
            int number = int.Parse(operation.Problem.ToString());

            string numberInWords = number.ToText("es");

            FixeNumber(ref numberInWords);

            operation.Resoult = numberInWords;

            string[] words = numberInWords.Split(' ');
            int wordsCount = words.Length;

            for (int t = 0; t < wordsCount; t++)
            {
                if (words[t] == "y")
                {
                    options.Add($"{words[t - 1]} {words[t]} {words[++t]}");
                }
                else if (words[t] == "mil")
                {
                    if ((t + 2) < wordsCount && words[t + 2] != "y")
                        options.Add($"{words[t]} {words[++t]}");
                    else if ((t - 2) > 0 && words[t - 2] != "y")
                        options.Add($"{words[t - 1]} {words[t]}");
                    else if ((t + 1) < wordsCount)
                        options.Add($"{words[t]} {words[++t]}");
                    else if ((t - 1) > 0)
                        options.Add($"{words[t - 1]} {words[t]}");
                    else
                        options.Add($"{words[t]}");
                }
                else
                {
                    if (t == (wordsCount - 1))
                        options.Add($"{words[t]}");
                    else if ((t + 1) == (wordsCount - 1) || words[t + 1] == "mil")
                        options.Add($"{words[t]} {words[++t]}");
                    else if (words[t + 1] != "y")
                        options.Add($"{words[t]}");
                }
            }

            int remainigOptions = 6 - options.Count;

            for (int i = 0; i < remainigOptions; i++)
            {
                string newWord = "";
                do
                {
                    newWord = Randomizer.Next(100).ToText("es");
                }
                while (options.Contains(newWord));
                options.Add(newWord);
            }

            operation.Options = options;
        }

        private void FixeNumber(ref string numberInWords)
        {
            if (numberInWords.Contains("uno mil"))
                numberInWords = numberInWords.Replace("uno mil", "un mil");

            if ((new Regex("(cien )").IsMatch(numberInWords)) && !numberInWords.Contains("cien mil"))
                numberInWords = numberInWords.Replace("cien ", "ciento ");
        }

        #endregion
    }
}
