using FamilyBudgetManager.Data;
using FamilyBudgetManager.Services.MoneyActionService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace FamilyBudgetManager.ViewModels
{
    public class MasterDetailPageViewModel : ViewModelBase
    {
        #region Fields
        ObservableCollection<string> _datesOfActions;
        List<MoneyActions> _incomes;
        List<MoneyActions> _expenditures;
        string _selectedDate;
        IFamilyBudgetService familyBudgetService;
        List<MoneyActions> moneyActions;
        #endregion

        #region Constructor
        public MasterDetailPageViewModel(IFamilyBudgetService familyBudgetService)
        {
            this.familyBudgetService = familyBudgetService;
        }
        #endregion

        #region Navigation events
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            moneyActions = new List<MoneyActions>(familyBudgetService.DataBaseConnection.Table<MoneyActions>());
            _datesOfActions = new ObservableCollection<string>(moneyActions.GroupBy(a => a.Date.ToString()).Select(b => b.Key).ToList());

            return Task.CompletedTask;
        }
        #endregion

        #region Bindable properties
        public ObservableCollection<string> DatesOfAction { get { return _datesOfActions; } }

        public List<MoneyActions> Incomes 
        { 
            set { Set(ref _incomes, value); } 
            get { return _incomes; } 
        }

        public List<MoneyActions> Expenditures 
        { 
            set { Set(ref _expenditures, value); } 
            get { return _expenditures; } 
        }

        public string SeletceDate 
        { 
            get { return _selectedDate; } 
            set 
            { 
                _incomes = moneyActions.Where(a => a.Date.ToString() == value & a.Type == 0).ToList();
                _expenditures = moneyActions.Where(a => a.Date.ToString() == value & a.Type == 1).ToList();
            }
        }

        public double TotalIncome { get { return Incomes.Sum(a => a.Sum); } }

        public double TotalExpenditure { get { return Expenditures.Sum(a => a.Sum); } }
        #endregion
    }
}
