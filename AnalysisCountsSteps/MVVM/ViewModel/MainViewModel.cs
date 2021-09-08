using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalysisCountsSteps.Cores;
using AnalysisCountsSteps.MVVM.View;

namespace AnalysisCountsSteps.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand ChartDiagramViewCommand { get; set; }

        public ChartDiagram ChartDiagramVM { get; set; }


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ChartDiagramVM = new ChartDiagram();

            CurrentView = ChartDiagramVM;

            ChartDiagramViewCommand = new RelayCommand(x =>
            {
                CurrentView = ChartDiagramVM;
            });
        }
    }
}
