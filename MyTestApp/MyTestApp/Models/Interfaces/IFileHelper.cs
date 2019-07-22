using System.Threading.Tasks;

namespace MyTestApp.Models.Interfaces
{
    public interface IFileHelper
    {
        Task<FileModel> ReadDocument(string path = null);
        Task WriteDocument(string fileName, string fileText);
    }
}
