using System;
using Xamarin.Forms;
using PropertyChanged;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace MyTestApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            Errors = new List<string>();
            Randomizer = new Random();
        }

        #region Properties

        public bool IsBusy { get; set; }

        public string Title { get; set; }

        public List<string> Errors { get; set; }

        public Random Randomizer { get; private set; }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        protected bool ValidateNewFileName(string fileName)
        {
            Errors.Clear();
            if (Device.RuntimePlatform == Device.Android)
                return true;

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

        #endregion
    }
}
