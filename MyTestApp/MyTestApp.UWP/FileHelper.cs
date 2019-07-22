using System;
using System.IO;
using Windows.Storage;
using MyTestApp.Models;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using MyTestApp.Models.Interfaces;

namespace MyTestApp.UWP
{
    class FileHelper : IFileHelper
    {
        public async Task<FileModel> ReadDocument(string path = null)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".csv");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file is null)
                return null;

            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            {
                using (var reader = new StreamReader(stream.AsStream()))
                {
                    string fileData = await reader.ReadToEndAsync();
                    return new FileModel
                    {
                        FileName = file.Name,
                        FilePath = file.Path,
                        FileExtension = Path.GetExtension(file.Name),
                        FileData = fileData
                    };
                }
            }
        }

        public async Task WriteDocument(string fileName, string fileText)
        {
            string fileExtension = Path.GetExtension(fileName);
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedFileName = Path.GetFileName(fileName);
            picker.FileTypeChoices.Add("Texto Plano", new List<string>() { fileExtension });

            StorageFile file = await picker.PickSaveFileAsync();
            if (file is null)
                return;

            CachedFileManager.DeferUpdates(file);

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var writer = new StreamWriter(stream.AsStream()))
                {
                    await writer.WriteAsync(fileText);
                }
            }
        }
    }
}
