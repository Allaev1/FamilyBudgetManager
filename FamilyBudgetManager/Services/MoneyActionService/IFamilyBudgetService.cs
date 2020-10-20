using FamilyBudgetManager.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;

namespace FamilyBudgetManager.Services.MoneyActionService
{
    /// <summary>
    /// Represent connection with database 
    /// </summary>
    public interface IFamilyBudgetService
    {
        SQLiteConnection DataBaseConnection { get; }

        //Task<List<MoneyActions>> GetAllAsync();

        //List<MoneyActions> GetAll();

        //Task DeleteAsync(int id);

        //Task UpdateAsync(MoneyActions editeMoneyAction);

        //Task AddAsync(MoneyActions newMoneyAction);

        //Task GetByIdAsync(int id);
    }
}
