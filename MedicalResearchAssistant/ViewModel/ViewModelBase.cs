using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearchAssistant.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                var propertyChangedArgument = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, propertyChangedArgument);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract void Dispose();
    }
}
