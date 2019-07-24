using MyTestApp.Models;
using MyTestApp.PortableClases;
using MyTestApp.ViewModels.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyTestApp.Services
{
    public class OperationGeneratorService : IDisposable
    {
        #region Atributes

        bool? isDisposing;
        OperationModel operation;
        Random randomizer;

        #endregion

        public OperationGeneratorService()
        {
            isDisposing = false;
            randomizer = new Random();
        }

        #region Methods

        public Task<OperationModel> CreateAdditionOperation()
        {
            operation = new OperationModel
            {
                Instruccion = "Selecciona el resultado de la siguiente suma.",
                Problem = new List<string>
                    {
                        randomizer.Next(100000).ToString("n0"),
                        randomizer.Next(100000).ToString("n0")
                    },
                Options = new List<string>
                    {
                        randomizer.Next(100000).ToString("n0"),
                        randomizer.Next(100000).ToString("n0"),
                        randomizer.Next(100000).ToString("n0"),
                        randomizer.Next(100000).ToString("n0")
                    },
                Resoult = 0
            };

            var problems = operation.Problem as List<string>;
            double number1 = double.Parse(problems[0]);
            double number2 = double.Parse(problems[1]);
            int randomIndex = randomizer.Next(4);

            string answer = (number1 + number2).ToString("n0");

            if (!operation.Options.Contains(answer))
                operation.Options[randomIndex] = answer;

            operation.Resoult = answer;

            return Task.FromResult(operation);
        }

        internal Task<OperationModel> GenerateAdditionOperation()
        {
            throw new NotImplementedException();
        }

        public Task<OperationModel> GeneratePercentageOperation()
        {
            operation = new OperationModel
            {
                Instruccion = "Completa correctamente la oración arrastrando al espacio en blanco la cantidad que corresponda.",
                Problem = "Aumentar en un {0}% la cantidad de {1}, resulta en:",
                Options = new List<string>
                    {
                        randomizer.Next(10000).ToString("n0"),
                        randomizer.Next(10000).ToString("n0"),
                        randomizer.Next(10000).ToString("n0"),
                        randomizer.Next(10000).ToString("n0")
                    },
                Resoult = 0
            };

            int problemNumber = randomizer.Next(5000);
            int percentage = randomizer.Next(101);
            string text = operation.Problem.ToString();
            double problemPercentage = ((double)percentage / 100d);
            int randomIndex = randomizer.Next(4);

            operation.Problem = string.Format(text, percentage, problemNumber);

            problemNumber += (int)(problemNumber * problemPercentage);

            string answer = problemNumber.ToString("n0");

            if (!operation.Options.Contains(answer))
                operation.Options[randomIndex] = answer;

            operation.Resoult = answer;

            return Task.FromResult(operation);

        }

        public Task<OperationModel> GenerateOrganizeWordsOperation()
        {
            operation = new OperationModel
            {
                Instruccion = "Selecciona el resultado de la siguiente suma.",
                Problem = $"{randomizer.Next(100000)}",
                Resoult = 0
            };

            List<string> options = new List<string>();
            int number = int.Parse(operation.Problem.ToString());

            string numberInWords = number.ToText();

            numberInWords = FixNumber(numberInWords);

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
                    newWord = randomizer.Next(100).ToText();
                }
                while (options.Contains(newWord));
                options.Add(newWord);
            }

            operation.Options = options;
            operation.Options.Shuffle();

            return Task.FromResult(operation);
        }

        #endregion

        #region Auxiliary Methods

        private string FixNumber(string numberInWords)
        {
            if (numberInWords.Contains("uno mil"))
                numberInWords = numberInWords.Replace("uno mil", "un mil");

            if (Regex.IsMatch(numberInWords, "(cien )") && !numberInWords.Contains("cien mil"))
                numberInWords = numberInWords.Replace("cien ", "ciento ");

            return numberInWords;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            isDisposing = true;
            operation = null;
            randomizer = null;
            isDisposing = null;
        }

        #endregion
    }
}
