using GalaSoft.MvvmLight;

namespace ZooBrowser.ViewModel
{
    public class CnnString : ViewModelBase
    {
        public string Name { get; set; }

        public string Value { get; private set; }

        public CnnString(string name, string value)
        {
            Name = name;
            Value = value;
        }   
    }
}
