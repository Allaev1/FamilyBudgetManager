using FamilyBudgetManager.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyBudgetManager.Services.MoneyActionService
{
    public class FamilyBudgetService : IFamilyBudgetService
    {
        #region Fields
        SQLiteConnection connection;
        #endregion

        public FamilyBudgetService()
        {
            var app = App.Current as App;
            connection = app.DataBaseConnection;
        }

        public SQLiteConnection DataBaseConnection => connection;

        //public Task AddAsync(MoneyActions newMoneyAction)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task DeleteAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<MoneyActions> GetAll()
        //{
        //    return new List<MoneyActions>(connection.Table<MoneyActions>());
        //}

        //public async Task<List<MoneyActions>> GetAllAsync()
        //{
        //    return await Task.Run(() =>
        //    {
        //        return new List<MoneyActions>(connection.Table<MoneyActions>());
        //    });
        //}

        //public Task GetByIdAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task UpdateAsync(MoneyActions editeMoneyAction)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
