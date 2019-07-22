using System.IO;
using System.Text;
using Xamarin.Forms;
using MyTestApp.Android;
using System.Threading.Tasks;

[assembly: Dependency(typeof(FileHelper))]
namespace MyTestApp.Android
{
    public class FileHelper : global::MyTestApp.Models.Interfaces.IFileHelper
    {
        public async Task<global::MyTestApp.Models.FileModel> ReadDocument(string path = null)
        {
            var fileData = await global::Plugin.FilePicker.CrossFilePicker.Current.PickFile(new[] { ".csv" });
            if (fileData == null)
                return null; // user canceled file picking

            return new global::MyTestApp.Models.FileModel
            {
                FileName = fileData.FileName,
                FilePath = fileData.FilePath,
                FileExtension = Path.GetExtension(fileData.FileName),
                FileData = Encoding.UTF8.GetString(fileData.DataArray)
            };
        }

        public async Task WriteDocument(string fileExtension, string fileName, string fileText)
        {
            var fileData = await global::Plugin.FilePicker.CrossFilePicker.Current.PickFile(new[] { fileExtension });
            if (fileData == null)
                return;

            using (var stream = fileData.GetStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    await writer.WriteAsync(fileText);
                }
            }
        }
    }
}