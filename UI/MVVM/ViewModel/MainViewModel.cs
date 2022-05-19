using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UI.MVVM.ViewModel
{
    class MainViewModel : UI.Core.ObservableObject
    {
        public UI.Core.RelayCommand SwitchToRuleTab { get; private set; }
        public UI.Core.RelayCommand SwitchToPalletTab { get; private set; }

        private bool _ruleTabEnabled;
        private bool _palletTabEnabled;

        
        public Visibility RuleTabVisible{ get; private set; }
        public Visibility PalletTabVisible { get; private set; }
        public bool RuleTabEnabled { 
            get { return _ruleTabEnabled; }
            private set 
            {
                _ruleTabEnabled = value;
                if (_ruleTabEnabled)
                    RuleTabVisible = Visibility.Visible;
                else
                    RuleTabVisible = Visibility.Collapsed;
            } 
        }
        public bool PalletTabEnabled {
            get { return _palletTabEnabled; }
            private set
            {
                _palletTabEnabled = value;
                if (_palletTabEnabled)
                    PalletTabVisible = Visibility.Visible;
                else
                    PalletTabVisible = Visibility.Collapsed;
            }
        }

        public MainViewModel()
        {
            EditorVM = new EditorViewModel(this);
            CurrentView = EditorVM;
            RuleTabEnabled = false;
            PalletTabEnabled = false;

            SwitchToPalletTab = new Core.RelayCommand(o =>
            {

            });

        }

        public EditorViewModel EditorVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

    
    }
}
