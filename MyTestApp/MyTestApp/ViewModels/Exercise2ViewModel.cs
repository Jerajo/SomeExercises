using System;
using Xamarin.Forms;
using PropertyChanged;
using Newtonsoft.Json;
using MyTestApp.Models;
using Newtonsoft.Json.Linq;
using System.Windows.Input;
using System.Collections.Generic;
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
            Operator = '+';
        }

        #region Properties

        public string NewFileName { get; set; }

        public char Operator { get; set; }

        public string JSONText { get; set; }

        public bool IsOperationCreated { get; set; }

        public JObject JSON { get; set; }

        public OperationModel Operation { get; set; }

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

            try
            {
                IsBusy = true;

                Operation = new OperationModel
                {
                    Instruccion = "Selecciona el resultado de la siguiente suma.",
                    Problem = new List<string>
                    {
                        Randomizer.Next(100000).ToString("n0"),
                        Randomizer.Next(100000).ToString("n0")
                    },
                    Options = new List<string>
                    {
                        Randomizer.Next(100000).ToString("n0"),
                        Randomizer.Next(100000).ToString("n0"),
                        Randomizer.Next(100000).ToString("n0"),
                        Randomizer.Next(100000).ToString("n0")
                    },
                    Resoult = 0
                };

                InsertCorretOptions(Operation, Operator);

                JSON = JObject.FromObject(Operation);
                JSONText = JSON.ToString(Formatting.Indented);
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

                    await service?.WriteDocument(".json", NewFileName, JSONText);
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
