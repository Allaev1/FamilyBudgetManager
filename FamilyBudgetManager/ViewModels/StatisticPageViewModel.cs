using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using FamilyBudgetManager.Services;
using FamilyBudgetManager.Services.MoneyActionService;
using Windows.UI.Xaml.Navigation;
using FamilyBudgetManager.Data;

namespace FamilyBudgetManager.ViewModels
{
    public class StatisticPageViewModel : ViewModelBase
    {
        #region Fields
        IFamilyBudgetService familyBudgetService;
        List<SumByCategory> _allCategories;
        List<SumByCategory> _incomeCategories;
        List<SumByCategory> _expenditureCategories;
        #endregion

        #region Constructor
        public StatisticPageViewModel(IFamilyBudgetService familyBudgetService)
        {
            this.familyBudgetService = familyBudgetService;
        }
        #endregion

        #region Navigation events
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            await Task.Run(() =>
            {
                _allCategories = familyBudgetService.DataBaseConnection.Table<MoneyActions>().
                    GroupBy(a => a.Category).
                    Select(b => new SumByCategory() { CategoryName = b.Key, Sum = b.Sum(c => c.Sum) }).
                    OrderBy(c => c.Sum).
                    ToList();
            });

            await Task.Run(() =>
            {
                _incomeCategories = familyBudgetService.DataBaseConnection.Table<MoneyActions>().
                    Where(ma => ma.Type == 0).
                    GroupBy(a => a.Category).
                    Select(b => new SumByCategory() { CategoryName = b.Key, Sum = b.Sum(c => c.Sum) }).
                    ToList();
            });

            await Task.Run(() =>
            {
                _expenditureCategories = familyBudgetService.DataBaseConnection.Table<MoneyActions>().
                    Where(ma => ma.Type == 1).
                    GroupBy(a => a.Category).
                    Select(b => new SumByCategory() { CategoryName = b.Key, Sum = b.Sum(c => c.Sum) }).
                    ToList();
            });
        }
        #endregion

        #region Bindable properties
        public List<SumByCategory> AllCategories
        {
            get { return _allCategories; }
        }

        public List<SumByCategory> IncomeCategories
        {
            get { return _incomeCategories; }
        }

        public List<SumByCategory> ExpenditureCategorys
        {
            get { return _expenditureCategories; }
        }
        #endregion
    }

    #region Screen objects
    public class SumByCategory
    {
        public string CategoryName { set; get; }

        public double Sum { set; get; }
    }
    #endregion
}
