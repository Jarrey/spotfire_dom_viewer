using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotfireDomViewer
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class DomViewerViewModel
    {
        private DomNode rootNode;
        public DomNode DomRoot
        {
            get
            {
                return rootNode;
            }
            set
            {
                rootNode = value;
                this.OnNotifyPropertyChanged();
            }
        }

        public DomViewerViewModel(object obj)
        {
            DomRoot = new DomNode(obj, NodeTypes.Property);
        }

        #region Interface Members

        private void OnNotifyPropertyChanged([CallerMemberName] string name = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
