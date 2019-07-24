using System;
using Xamarin.Forms;
using PropertyChanged;
using Newtonsoft.Json;
using MyTestApp.Models;
using Xamarin.Essentials;
using MyTestApp.Services;
using Newtonsoft.Json.Linq;
using System.Windows.Input;
using MyTestApp.Models.Interfaces;
using MyTestApp.ViewModels.Commands;

namespace MyTestApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class Exercise2ViewModel : BaseViewModel
    {
        public Exercise2ViewModel() : base()
        {
            Title = "Ejercicio 2";
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
                    OperationModel operation = await service.GenerateAdditionOperation();

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

                    string fileName = NewFileName ?? "Ejercicio2";

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
