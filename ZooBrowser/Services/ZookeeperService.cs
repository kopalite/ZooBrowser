using org.apache.zookeeper;
using org.apache.zookeeper.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooBrowser.ViewModel;

namespace ZooBrowser.Services
{
    public class ZookeeperService : IZookeeperService
    {
        private string _cnnString;
        
        public string CnnString
        {
            private get
            {
                return _cnnString;
            }
            set
            {
                _api = null;
                _cnnString = value;
            }
        }

        private ZooKeeper _api;

        private ZooKeeper Api
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CnnString))
                {
                    throw new Exception("Zookeeper connection string is not defined. Please add at least one in app.config.");
                }

                if (_api == null || _api.getState() == ZooKeeper.States.NOT_CONNECTED)
                {
                    _api = new ZooKeeper(CnnString, 5000, null, true);
                }

                return _api;
            }
        }

        public async Task<IEnumerable<NodeViewModel>> GetChildrenAsync(string path)
        {
            ValidatePath(path);
            var nodes = await Api.getChildrenAsync(path);
            return nodes.Children.Select(x => new NodeViewModel(this, path, x));
        }

        public async Task<DataViewModel> GetDataAsync(string path)
        {
            ValidatePath(path);
            var data = await Api.getDataAsync(path);
            return new DataViewModel(data.Data, data.Stat);
        }

        public async Task<IEnumerable<NodeViewModel>> GetAllAsync()
        {
            var rootNodes = await Api.getChildrenAsync("/");
            var allTasks = rootNodes.Children.Select(x => ZKUtil.listSubTreeBFS(Api, "/" + x));
            var allNodes = (await Task.WhenAll(allTasks)).SelectMany(x => x).ToArray();
            return allNodes.Select(x => new NodeViewModel(this, x, GetNodeName(x))).ToArray();
        }

        private void ValidatePath(string path)
        {
            try
            {
                PathUtils.validatePath(path);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Node path '{0}' is invalid. {1}", path, ex.Message)); 
            }
        }

        private string GetNodeName(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            var segments = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            return segments.LastOrDefault();
        }
    }
}
