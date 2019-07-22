using System.Threading.Tasks;

namespace MyTestApp.Models.Interfaces
{
    public interface IFileHelper
    {
        Task<FileModel> ReadDocument(string path = null);
        Task WriteDocument(string fileExtension, string fileName, string fileText);
    }
}
