using FamilyBudgetManager.Data;
using FamilyBudgetManager.ProxyModel;
using FamilyBudgetManager.Services.MoneyActionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FamilyBudgetManager.ViewModels
{
    public class AddEditCategoryViewModel : ViewModelBase
    {
        #region Fields
        IFamilyBudgetService familyBudgetService;
        DelegateCommand _goBackCommand;
        DelegateCommand _saveCommand;
        List<string> _types;
        List<Categories> categories;
        string _categoryName;
        string _selectedType;
        string _description;
        string _header;
        int idOfEditedCategory; //Id of category that will be edited
        bool isAddition;
        #endregion

        #region Constructor
        public AddEditCategoryViewModel(IFamilyBudgetService familyBudgetService)
        {
            this.familyBudgetService = familyBudgetService;
            _goBackCommand = new DelegateCommand(GoBackCommandExecute);
            _saveCommand = new DelegateCommand(SaveCommandExecute, CanSaveCommandExecute);
            _types = new List<string>() { "Income", "Expenditure" };
            categories = familyBudgetService.DataBaseConnection.Table<Categories>().ToList();
        }
        #endregion

        #region Commands

        #region Go back command
        public DelegateCommand GoBackCommand
        {
            get { return _goBackCommand; }
        }

        private void GoBackCommandExecute()
        {
            SelectedType = null;
            CategoryName = null;

            NavigationService.GoBack(); 
        }
        #endregion

        #region Save command
        public DelegateCommand SaveCommand
        {
            get { return _saveCommand; }
        }

        private void SaveCommandExecute()
        {
            Categories category = new Categories();
            category.CategoryName = CategoryName;
            if (SelectedType == "Income") category.Type = 0;
            else category.Type = 1;
            category.Description = Description;

            if (isAddition)
                familyBudgetService.DataBaseConnection.Table<Categories>().Connection.Insert(category);
            else
            {
                category.ID = idOfEditedCategory;
                familyBudgetService.DataBaseConnection.Table<Categories>().Connection.Update(category);
            }

            SelectedType = null;
            CategoryName = null;

            NavigationService.GoBack();
        }

        private bool CanSaveCommandExecute()
        {
            bool isCategoryNameUnique = true;

            if (categories.FirstOrDefault(a => a.CategoryName == CategoryName) != null && isAddition) isCategoryNameUnique = false;

            if (string.IsNullOrEmpty(CategoryName) || SelectedType == null || CategoryName.Any(a => char.IsDigit(a)) || !isCategoryNameUnique)
                return false;
            else
                return true;
        }
        #endregion

        #endregion

        #region Bindable properties
        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                Set(ref _categoryName, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                Set(ref _selectedType, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        public string Header { set { Set(ref _header, value); } get { return _header; } }

        public List<string> Types { get { return _types; } }
        #endregion

        #region Navigation events
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter == null)
            {
                isAddition = true;
                Header = "Add category";
            }
            else
            {
                isAddition = false;

                Header = "Edit category";

                var category = (Categories)parameter;
                var type = (category.Type == 0) ? "Income" : "Expenditure";

                idOfEditedCategory = (int)category.ID;
                CategoryName = category.CategoryName;
                SelectedType = type;
                Description = category.Description;

                SaveCommand.RaiseCanExecuteChanged();
            }

            return Task.CompletedTask;
        }
        #endregion
    }
}
