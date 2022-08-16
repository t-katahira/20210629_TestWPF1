using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private BitmapSource bmpSrc;
        public BitmapSource BmpSrc
        {
            get
            {
                return this.bmpSrc;
            }
            set
            {
                if (value != this.bmpSrc)
                {
                    this.bmpSrc = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
