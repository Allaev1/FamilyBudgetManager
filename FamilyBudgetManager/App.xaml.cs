using FamilyBudgetManager.Services.MoneyActionService;
using FamilyBudgetManager.ViewModels;
using FamilyBudgetManager.Views;
using GalaSoft.MvvmLight.Ioc;
using SQLite;
using Syncfusion.Licensing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FamilyBudgetManager
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : BootStrapper
    {
        const string DATABASE_NAME = "FamilyBudget.db";

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            SyncfusionLicenseProvider.RegisterLicense("MzIxNDYxQDMxMzgyZTMyMmUzMFBVQTk5SW13NFUyQ2RJdFMxQi9NOUtSOThBRkY5MkVOUjhHZDNDczROWW89");
        }

        private StorageFile dataBase;
        private SQLiteConnection _dataBaseConnection;
        public SQLiteConnection DataBaseConnection
        {
            get
            {
                if (_dataBaseConnection == null)
                {
                    _dataBaseConnection = new SQLiteConnection(dataBase.Path);
                }
                return _dataBaseConnection;
            }
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            ShellView shellView = new ShellView();

            NavigationServiceFactory(BackButton.Ignore, ExistingContent.Include, shellView.ContentFrame);

            return new ModalDialog()
            {
                Content = shellView
            };
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            if (await ApplicationData.Current.LocalFolder.TryGetItemAsync(DATABASE_NAME) == null)
            {
                var appDb = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///{DATABASE_NAME}"));
                await appDb.CopyAsync(ApplicationData.Current.LocalFolder);
            }

            dataBase = await ApplicationData.Current.LocalFolder.GetFileAsync(DATABASE_NAME);

            SimpleIoc.Default.Register<MasterDetailPageViewModel>();
            SimpleIoc.Default.Register<AddEditMoneyActionViewModel>();
            SimpleIoc.Default.Register<JournalPageViewModel>();
            SimpleIoc.Default.Register<StatisticPageViewModel>();
            SimpleIoc.Default.Register<AddEditCategoryViewModel>();
            SimpleIoc.Default.Register<CategoriesViewModel>();
            SimpleIoc.Default.Register<IFamilyBudgetService, FamilyBudgetService>();

            NavigationService.Navigate(typeof(MasterDetailPage));
        }

        public async override Task OnSuspendingAsync(object s, SuspendingEventArgs e, bool prelaunchActivated)
        {
            var internalDb = await ApplicationData.Current.LocalFolder.GetFileAsync(DATABASE_NAME);
            var externalDb = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///{DATABASE_NAME}"));

            await internalDb.CopyAndReplaceAsync(externalDb);
        }

        public override INavigable ResolveForPage(Page page, NavigationService navigationService)
        {
            if (page is MasterDetailPage)
                return SimpleIoc.Default.GetInstance<MasterDetailPageViewModel>();
            else if (page is AddEditMoneyActionPage)
                return SimpleIoc.Default.GetInstance<AddEditMoneyActionViewModel>();
            else if (page is JournalPage)
                return SimpleIoc.Default.GetInstance<JournalPageViewModel>();
            else if (page is StatisticPage)
                return SimpleIoc.Default.GetInstance<StatisticPageViewModel>();
            else if (page is AddEditCategoryPage)
                return SimpleIoc.Default.GetInstance<AddEditCategoryViewModel>();
            else if (page is CategoriesPage)
                return SimpleIoc.Default.GetInstance<CategoriesViewModel>();
            else
                return base.ResolveForPage(page, navigationService);
        }
    }
}



