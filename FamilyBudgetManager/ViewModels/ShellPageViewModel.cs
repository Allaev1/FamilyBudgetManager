using FamilyBudgetManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;

namespace FamilyBudgetManager.ViewModels
{
    public class ShellPageViewModel : ViewModelBase
    {
        #region Fields
        INavigationService navigationService;
        #endregion

        #region Constructor
        public ShellPageViewModel()
        {
            navigationService = WindowWrapper.Current().NavigationServices.FirstOrDefault();
        }
        #endregion

        #region Bindable properties
        public ObservableCollection<NavigationMenuItem> NavigationMenuItems
        {
            get
            {
                return new ObservableCollection<NavigationMenuItem>()
                {
                    new NavigationMenuItem(){Content="Report",Symbol=Symbol.Home,PageType=typeof(MasterDetailPage)},
                    new NavigationMenuItem(){Content="Work panel",Symbol=Symbol.Edit,PageType=typeof(AddEditMoneyActionPage)},
                    new NavigationMenuItem(){Content="Statistic",Symbol=Symbol.Pictures,PageType=typeof(StatisticPage)},
                    new NavigationMenuItem(){Content="Journal",Symbol=Symbol.ViewAll,PageType=typeof(JournalPage)},
                    new NavigationMenuItem(){Content="Categories",Symbol=Symbol.List,PageType=typeof(CategoriesPage)}
                };
            }
        }

        NavigationMenuItem _selectedItem;
        public NavigationMenuItem SelectedItem
        {
            set
            {
                Set(ref _selectedItem, value);
                Type pageType = SelectedItem.PageType;
                navigationService.Navigate(pageType);
            }
            get { return _selectedItem; }
        }
        #endregion
    }

    #region Screen object
    /// <summary>
    /// Represent data for menus` item 
    /// </summary>
    public class NavigationMenuItem
    {
        public object Content { set; get; }
        /// <summary>
        /// Symbol that shown next to content
        /// </summary>
        public Symbol Symbol { set; get; }
        /// <summary>
        /// Page to that make navigation after item was tapped
        /// </summary>
        public Type PageType { set; get; }
    }
    #endregion
}
