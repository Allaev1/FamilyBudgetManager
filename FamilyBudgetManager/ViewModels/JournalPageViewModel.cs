using FamilyBudgetManager.Data;
using FamilyBudgetManager.Services.MoneyActionService;
using FamilyBudgetManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Utils;
using Windows.UI.Xaml.Navigation;

namespace FamilyBudgetManager.ViewModels
{
    public class JournalPageViewModel : ViewModelBase
    {
        #region Fields
        IFamilyBudgetService familyBudgetService;
        DelegateCommand _editMoneyActionCommand;
        ObservableCollection<MoneyActionView> _moneyActions;
        MoneyActionView _selectedMoneyAction;
        DelegateCommand _deleteMoneyActionCommand;
        #endregion

        #region Constructor
        public JournalPageViewModel(IFamilyBudgetService familyBudgetService)
        {
            this.familyBudgetService = familyBudgetService;
            _editMoneyActionCommand = new DelegateCommand(ExecuteEditMoneyAction, CanEditMoneyAction);
            _deleteMoneyActionCommand = new DelegateCommand(ExecuteDeleteMoneyAction, CanDeleteMoneyAction);
        }
        #endregion

        #region Naviagtion event
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var moneyaactions = new List<MoneyActions>(familyBudgetService.DataBaseConnection.Table<MoneyActions>());

            _moneyActions = moneyaactions.Select(a =>
            {
                var sum = a.Sum.ToString("N1", CultureInfo.InvariantCulture);
                var date = a.Date;
                string type = null;
                if (a.Type == 0)
                    type = "Income";
                else
                    type = "Expenditure";

                return new MoneyActionView()
                {
                    ID = (int)a.ID,
                    Type = type,
                    Category = a.Category,
                    Date = date,
                    Sum = sum,
                    Note = a.Note
                };

            }).ToObservableCollection();

            return Task.CompletedTask;
        }
        #endregion

        #region Bindable properties
        public ObservableCollection<MoneyActionView> MoneyActions { get { return _moneyActions; } }

        public MoneyActionView SelectedMoneyAction 
        { 
            set 
            { 
                Set(ref _selectedMoneyAction, value);
                EditMoneyActionCommand.RaiseCanExecuteChanged();
                DeleteMoneyActionCommand.RaiseCanExecuteChanged();
            } 
            get { return _selectedMoneyAction; } 
        }
        #endregion

        #region Commands

        #region Edit Command
        public DelegateCommand EditMoneyActionCommand { get { return _editMoneyActionCommand; } }

        private bool CanEditMoneyAction()
        {
            if (SelectedMoneyAction != null) return true;
            else return false;
        }

        private void ExecuteEditMoneyAction()
        {
            var selectedMoneyAction = familyBudgetService.DataBaseConnection.Table<MoneyActions>().First(moneyAction => moneyAction.ID == SelectedMoneyAction.ID);

            NavigationService.Navigate(typeof(AddEditMoneyActionPage), selectedMoneyAction);
        }
        #endregion

        #region Delete Command
        public DelegateCommand DeleteMoneyActionCommand { get { return _deleteMoneyActionCommand; } }

        private bool CanDeleteMoneyAction()
        {
            if (SelectedMoneyAction != null) return true;
            else return false;
        }

        private void ExecuteDeleteMoneyAction()
        {
            var selectedMoneyAction = familyBudgetService.DataBaseConnection.Table<MoneyActions>().First(moneyAction => moneyAction.ID == SelectedMoneyAction.ID);

            familyBudgetService.DataBaseConnection.Table<MoneyActions>().Connection.Delete(selectedMoneyAction);

            _moneyActions.Remove(SelectedMoneyAction);
        }
        #endregion

        #endregion
    }

    #region Screen object
    /// <summary>
    /// Represent record of money action in UI friendyl way
    /// </summary>
    public class MoneyActionView
    {
        public int ID { set; get; }
        public string Type { set; get; }
        public string Category { set; get; }
        public string Sum { set; get; }
        public string Date { set; get; }
        public string Note { set; get; }
    }
    #endregion
}
