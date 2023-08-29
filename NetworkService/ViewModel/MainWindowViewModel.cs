using System;
using NetworkService.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkService.Model;
using NetworkService.ViewModel;
using System.Windows.Input;

namespace NetworkService.ViewModel
{
    class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> NavCommand { get; private set; }
        public MyICommand<string> UndoCommand { get; private set; }
        private NetworkDataViewModel networkDataViewModel = new NetworkDataViewModel();
        private NetworkViewViewModel networkViewViewModel = new NetworkViewViewModel();
        private DataChartViewModel dataChartViewModel = new DataChartViewModel();
        private BindableBase currentViewModel;
        public int monitor = 0;
        public MainWindowViewModel()
        {
            NavCommand = new MyICommand<string>(OnNav);
            UndoCommand = new MyICommand<string>(OnUndo);
            CurrentViewModel = networkDataViewModel;
        }

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "NetworkData":
                    CurrentViewModel = networkDataViewModel;
                    break;
                case "NetworkView":
                    CurrentViewModel = networkViewViewModel;
                    break;

                case "DataChart":
                    CurrentViewModel = dataChartViewModel;
                    break;
            }
        }


        private void OnUndo(string s)
        {
            string x = "";
            NetworkDataViewModel.UndoCommand.Execute(x);
        }

    }
}
