using FamilyBudgetManager.Data;
using FamilyBudgetManager.Services.MoneyActionService;
using FamilyBudgetManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Utils;
using Windows.UI.Xaml.Navigation;

namespace FamilyBudgetManager.ViewModels
{
    public class CategoriesViewModel : ViewModelBase
    {
        #region Fields
        IFamilyBudgetService familyBudgetService;
        ObservableCollection<CategoriesView> _categories;

        DelegateCommand _deleteCommand;
        DelegateCommand _editCommand;

        CategoriesView _selectedCategory;
        #endregion

        #region Constructor
        public CategoriesViewModel(IFamilyBudgetService familyBudgetService)
        {
            this.familyBudgetService = familyBudgetService;
            _deleteCommand = new DelegateCommand(ExecuteDeleteCategory, CanExecuteDeleteCategory);
            _editCommand = new DelegateCommand(ExecuteEditCategory, CanExecuteEditCategory);
        }
        #endregion

        #region Navigation events
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            _categories = familyBudgetService.DataBaseConnection.Table<Categories>().Select(a =>
            {
                string type;

                if (a.Type == 1) type = "Income";
                else type = "Expenditure";

                var category = new CategoriesView()
                {
                    ID = (int)a.ID,
                    Name = a.CategoryName,
                    Description = a.Description,
                    Type = type
                };

                return category;
            }).ToObservableCollection();

            return Task.CompletedTask;
        }
        #endregion

        #region Properties
        public ObservableCollection<CategoriesView> Categories { get { return _categories; } }

        public CategoriesView SelectedCategory 
        {
            set 
            { 
                Set(ref _selectedCategory, value);

                DeleteCategoryCommand.RaiseCanExecuteChanged();
                EditCategoryCommand.RaiseCanExecuteChanged();
            }
            get { return _selectedCategory; }
        }
        #endregion

        #region Commands

        #region Delete Command
        public DelegateCommand DeleteCategoryCommand { get { return _deleteCommand; } }

        private void ExecuteDeleteCategory()
        {
            var selectedCategory = familyBudgetService.DataBaseConnection.Table<Categories>().First(a => a.ID == SelectedCategory.ID);

            familyBudgetService.DataBaseConnection.Table<Categories>().Connection.Delete(selectedCategory);

            _categories.Remove(SelectedCategory);
        }

        private bool CanExecuteDeleteCategory()
        {
            if (SelectedCategory != null) return true;
            else return false;
        }
        #endregion

        #region Edit Command
        public DelegateCommand EditCategoryCommand { get { return _editCommand; } }

        private void ExecuteEditCategory()
        {
            var category = familyBudgetService.DataBaseConnection.Table<Categories>().First(a => a.ID == SelectedCategory.ID);
            NavigationService.Navigate(typeof(AddEditCategoryPage), category);
        }

        private bool CanExecuteEditCategory()
        {
            if (SelectedCategory != null) return true;
            else return false;
        }
        #endregion

        #endregion
    }

    #region Screen object
    public class CategoriesView
    {
        public int ID { set; get; }

        public string Type { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
    }
    #endregion
}
