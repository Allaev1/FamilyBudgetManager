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
    public class ToTotalConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var viewModel = SimpleIoc.Default.GetInstance<MasterDetailPageViewModel>();

            if ((string)parameter == "Income")
                return viewModel.TotalIncome.ToString("N1", CultureInfo.InvariantCulture);
            else if ((string)parameter == "Expenditure")
                return viewModel.TotalExpenditure.ToString("N1", CultureInfo.InvariantCulture);
            else
                return (viewModel.TotalIncome - viewModel.TotalExpenditure).ToString("N1", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
