using System;
using Xamarin.Forms;
using PropertyChanged;
using Newtonsoft.Json;
using MyTestApp.Models;
using Xamarin.Essentials;
using Newtonsoft.Json.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using MyTestApp.Models.Interfaces;
using MyTestApp.ViewModels.Commands;
using MyTestApp.Services;

namespace MyTestApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class Exercise3ViewModel : BaseViewModel
    {
        public Exercise3ViewModel() : base()
        {
            Title = "Ejercicio 3";
            IsOperationCreated = false;
        }

        #region Properties

        public string NewFileName { get; set; }

        public string JSONText { get; set; }

        public bool IsOperationCreated { get; set; }

        #endregion

        #region Commands

        public ICommand GenerateOperation
        {
            get => new DelegateCommand(new Action(GenerateOperationCommand));
        }

        public ICommand SaveJSON
        {
            get => new DelegateCommand(new Action(SaveJSONCommand));
        }

        #endregion

        #region Methods

        private async void GenerateOperationCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                using (var service = new OperationGeneratorService())
                {
                    OperationModel operation = await service.GeneratePercentageOperation();

                    JObject json = JObject.FromObject(operation);
                    JSONText = json.ToString(Formatting.Indented);
                }
            }
            catch (Exception ex)
            {
                await ShowError($"Error al generar la operación el archivo: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void SaveJSONCommand()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                if (ValidateNewFileName(NewFileName))
                {
                    IFileHelper service = DependencyService.Get<IFileHelper>();

                    string fileName = NewFileName ?? "Ejercicio3";

                    await service?.WriteDocument(".json", fileName, JSONText);

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        var appDirectory = FileSystem.AppDataDirectory;
                        await ShowAlert($"El archivo se guardo en: {appDirectory}/{fileName}.csv");
                    }
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
                await ShowError($"Error al guardar el archivo: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
