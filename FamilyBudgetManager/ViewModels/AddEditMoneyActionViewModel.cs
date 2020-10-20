using FamilyBudgetManager.Services.MoneyActionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using FamilyBudgetManager.ProxyModel;
using FamilyBudgetManager.Data;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.Web.Syndication;
using FamilyBudgetManager.Views;

namespace FamilyBudgetManager.ViewModels
{
    public class AddEditMoneyActionViewModel : ViewModelBase
    {
        #region Fields
        IFamilyBudgetService familyBudgetService;
        DelegateCommand _saveCommand;
        DelegateCommand _clearCommand;
        DelegateCommand _navigateToAddCategoryCommand;
        List<string> _categories;
        string _selectedCategory;
        string _selectedType;
        bool _isAvailable;
        string _sum;
        string _note;
        bool isEditMode;
        DateTimeOffset _date;
        MoneyActions moneyActionToEdit;
        #endregion

        #region Constructor
        public AddEditMoneyActionViewModel(IFamilyBudgetService familyBudgetService)
        {
            this.familyBudgetService = familyBudgetService;
            _saveCommand = new DelegateCommand(SaveCommandExecute, CanSaveCommand);
            _clearCommand = new DelegateCommand(ClearCommandExecute);
            _navigateToAddCategoryCommand = new DelegateCommand(NavigateToAddCategoryExecute);
            Date = DateTimeOffset.Now;
        }
        #endregion

        #region Navigation events
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            TempMoneyAction = new ProxyMonneyAction();

            if (parameter != null)
            {
                isEditMode = true;
                Header = "Edit money action";
                BackButtonContent = "Back";
                moneyActionToEdit = (MoneyActions)parameter;
                SelectedCategory = moneyActionToEdit.Category;
                Sum = moneyActionToEdit.Sum.ToString();
                Note = moneyActionToEdit.Note;
                var date = Convert.ToDateTime(moneyActionToEdit.Date);

                Date = new DateTimeOffset(date);
            }
            else
            {
                isEditMode = false;
                Header = "Add money action";
                BackButtonContent = "Clear";
            }

            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            Sum = string.Empty;
            Note = string.Empty;
            SelectedCategory = null;
            Date = DateTime.Now;

            return Task.CompletedTask;
        }
        #endregion

        #region Bindable properties
        public ProxyMonneyAction TempMoneyAction { set; get; }

        public List<string> Categories
        {
            get { return _categories; }
            set { Set(ref _categories, value); }
        }

        public string SelectedCategory
        {
            set
            {
                Set(ref _selectedCategory, value);

                if (string.IsNullOrEmpty(value)) return;

                var category = familyBudgetService.DataBaseConnection.Table<Categories>().First(a => a.CategoryName == value);
                TempMoneyAction.Category = value;
                TempMoneyAction.Type = (int)category.Type;
                SaveCommand.RaiseCanExecuteChanged();
            }
            get { return _selectedCategory; }
        }

        public string SelectedType
        {
            set
            {
                Set(ref _selectedType, value);

                if (SelectedType == "Income")
                    Categories = new List<string>(familyBudgetService.DataBaseConnection.Table<Categories>().Where(a => a.Type == 0).Select(a => a.CategoryName));
                else if (SelectedType == "Expenditure")
                    Categories = new List<string>(familyBudgetService.DataBaseConnection.Table<Categories>().Where(a => a.Type == 1).Select(a => a.CategoryName));

            }
            get { return _selectedType; }
        }

        public string Sum
        {
            set
            {
                Set(ref _sum, value);
                TempMoneyAction.Sum = value;
                SaveCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _sum;
            }
        }

        public string Note
        {
            set
            {
                Set(ref _note, value);
                TempMoneyAction.Note = value;
            }
            get
            {
                return _note;
            }
        }

        public DateTimeOffset Date
        {
            set
            {
                Set(ref _date, value);
                if (TempMoneyAction != null)
                {
                    var dateString = new DateTime(value.Year, value.Month, value.Day).ToLongDateString();
                    TempMoneyAction.Date = dateString;
                }
            }
            get
            {
                return _date;
            }
        }

        public bool IsAvailable { set { Set(ref _isAvailable, value); } get { return _isAvailable; } }

        public string Header { set; get; }

        public string BackButtonContent { set; get; }
        #endregion

        #region Commands

        #region Save command
        public DelegateCommand SaveCommand
        {
            get { return _saveCommand; }
        }

        private void SaveCommandExecute()
        {
            MoneyActions newMoneyAction = new MoneyActions();
            newMoneyAction.Category = TempMoneyAction.Category;
            newMoneyAction.Type = TempMoneyAction.Type;
            newMoneyAction.Date = TempMoneyAction.Date;
            newMoneyAction.Sum = Convert.ToDouble(TempMoneyAction.Sum);
            newMoneyAction.Note = TempMoneyAction.Note;

            if (isEditMode)
            {
                newMoneyAction.ID = moneyActionToEdit.ID;
                familyBudgetService.DataBaseConnection.Table<MoneyActions>().Connection.Update(newMoneyAction);
                NavigationService.GoBack();
            }
            else
                familyBudgetService.DataBaseConnection.Table<MoneyActions>().Connection.Insert(newMoneyAction);

            SelectedCategory = null;
            SelectedType = null;
            Sum = string.Empty;
            Note = string.Empty;
        }

        private bool CanSaveCommand()
        {
            if (TempMoneyAction.Category != null & TempMoneyAction.Sum.All(char.IsDigit)) return true;
            else return false;
        }
        #endregion

        #region Clear command
        public DelegateCommand ClearCommand { get { return _clearCommand; } }

        private void ClearCommandExecute()
        {
            if (isEditMode)
                NavigationService.GoBack();
            else
            {
                SelectedCategory = null;
                SelectedType = null;
                Sum = string.Empty;
                Note = string.Empty;
            }
        }
        #endregion

        #region Navigate to add category
        public DelegateCommand NavigateToAddCategoryCommand
        {
            get { return _navigateToAddCategoryCommand; }
        }

        private void NavigateToAddCategoryExecute() => NavigationService.Navigate(typeof(AddEditCategoryPage));

        #endregion

        #endregion
    }
}
