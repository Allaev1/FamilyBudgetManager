using FamilyBudgetManager.Data;
using FamilyBudgetManager.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace FamilyBudgetManager.Services.Converters
{
    public class ToCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var viewModel = SimpleIoc.Default.GetInstance<MasterDetailPageViewModel>();

            if ((string)parameter == "Incomes")
                return viewModel.Incomes.GroupBy(a => a.Category).Select(a => new { Category = a.Key, Sum = a.Sum(b => b.Sum).ToString("N1", CultureInfo.InvariantCulture)});
            //return viewModel.Incomes.Select(a => new { Category = a.Category, Sum = a.Sum.ToString("N1", CultureInfo.InvariantCulture), Note = a.Note });
            else
                return viewModel.Expenditures.GroupBy(a => a.Category).Select(a => new { Category = a.Key, Sum = a.Sum(b => b.Sum).ToString("N1", CultureInfo.InvariantCulture)});
            //return viewModel.Expenditures.Select(a => new { Category = a.Category, Sum = a.Sum.ToString("N1", CultureInfo.InvariantCulture), Note = a.Note }); ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
