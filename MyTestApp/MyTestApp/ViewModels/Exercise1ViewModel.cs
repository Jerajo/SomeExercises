﻿using System;
using System.Linq;
using Xamarin.Forms;
using PropertyChanged;
using MyTestApp.Models;
using System.Windows.Input;
using System.Collections.Generic;
using MyTestApp.Models.Interfaces;
using MyTestApp.ViewModels.Commands;
using System.Text.RegularExpressions;
using MyTestApp.ViewModels.Extensions;

namespace MyTestApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class Exercise1ViewModel : BaseViewModel
    {
        public Exercise1ViewModel() : base()
        {
            Title = "Ejercicio 1";
            FilePath = "Seleccione un archivo.";
            IsFileLoaded = false;
            IsDocumentOrdened = false;
            Dominicans = new Dictionary<string, string>();
        }

        #region Properties

        public string FilePath { get; set; }

        public string NewFileName { get; set; }

        public string DocumentText { get; set; }

        public string OrganizedDocumentText { get; set; }

        public bool IsFileLoaded { get; set; }

        public bool IsDocumentOrdened { get; set; }

        public Dictionary<string, string> Dominicans { get; set; }

        #endregion

        #region Commands

        public ICommand PickFile
        {
            get => new DelegateCommand(new Action(PickFileCommand));
        }

        public ICommand OrderDocumentInverseAscendance
        {
            get => new DelegateCommand(new Action(OrderDocumentInverseAscendanceCommand));
        }

        public ICommand SaveFile
        {
            get => new DelegateCommand(new Action(SaveFileCommand));
        }

        #endregion

        #region Methods

        private async void PickFileCommand()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                IFileHelper service = DependencyService.Get<IFileHelper>();

                FileModel fileModel = await service?.ReadDocument();
                if (fileModel is null)
                    return;

                LoadDocumentData(fileModel.FileData);

                FilePath = fileModel.FilePath;
                DocumentText = fileModel.FileData;

                IsFileLoaded = true;
            }
            catch (Exception ex)
            {
                await ShowError($"Error al seleccionar el archivo: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OrderDocumentInverseAscendanceCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            if (Dominicans.Count > 2)
            {

                string newDocument = "";
                var organizedListOfDominicans = Dominicans.OrderBy(m => m.Key);

                foreach (var dominican in organizedListOfDominicans)
                    newDocument += $"{dominican.Value},{dominican.Key.Reverse()}\n";

                OrganizedDocumentText = newDocument;
            }
            else OrganizedDocumentText = DocumentText;
            IsDocumentOrdened = true;
            IsBusy = false;
        }

        private async void SaveFileCommand()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                if (ValidateNewFileName(NewFileName))
                {
                    IFileHelper service = DependencyService.Get<IFileHelper>();

                    if (!NewFileName.Contains(".csv"))
                        NewFileName += ".csv";

                    await service?.WriteDocument(NewFileName, OrganizedDocumentText);
                }
                else
                {
                    string message = "";
                    foreach (string error in Errors)
                        message += error + "\n";

                    await ShowAlert(message);
                }
            }
            catch (Exception ex)
            {
                await ShowError($"Error al seleccionar el archivo: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void LoadDocumentData(string documentText)
        {
            Regex regex = new Regex(@"\d{3}-\d{7}-\d{1}");
            string[] listOfNamesAndIds = documentText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            Dominicans.Clear();

            foreach (string item in listOfNamesAndIds)
            {
                Match match = regex.Match(item);
                string matchResoult = match.Value;

                if (string.IsNullOrEmpty(matchResoult))
                    continue;

                string[] dominican = item.Split(',');

                if (dominican.Length < 2)
                    continue;

                Dominicans.Add(dominican[1].Reverse(), dominican[0]);
            }
        }

        #endregion
    }
}
