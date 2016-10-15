using System.Collections.Generic;
using System.Threading.Tasks;
using ZooBrowser.ViewModel;

namespace ZooBrowser.Services
{
    public interface IZookeeperService
    {
        string CnnString { set; }

        Task<IEnumerable<NodeViewModel>> GetChildrenAsync(string path);

        Task<DataViewModel> GetDataAsync(string path);

        Task<IEnumerable<NodeViewModel>> GetAllAsync();
    }
}
