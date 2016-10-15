using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ZooBrowser.Services;

namespace ZooBrowser.ViewModel
{
    public class NodeViewModel : ViewModelBase
    {
        private readonly IZookeeperService _service;
        private readonly string _root;
        

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public string Path
        {
            get
            {
                return string.Format("{0}/{1}", _root, Name).Replace("//", "/").TrimEnd('/');
            }
        }

        private DataViewModel _data;
        public DataViewModel Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged(() => Data);
            }
        }

        private ObservableCollection<NodeViewModel> _children;
        public ObservableCollection<NodeViewModel> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                RaisePropertyChanged(() => Children);
            }
        }

        public NodeViewModel(IZookeeperService service, string root)
        {
            _service = service;
            _root = root;
            Children = new ObservableCollection<NodeViewModel>();
        }

        public async Task RefreshAsync()
        {
            Data = await _service.GetDataAsync(Path);
            var children = await _service.GetChildrenAsync(Path);
            Children = new ObservableCollection<NodeViewModel>(children);
        }
    }
}
