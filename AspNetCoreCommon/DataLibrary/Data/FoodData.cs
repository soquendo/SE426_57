using DataLibrary.Db; //added
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; //added

namespace DataLibrary.Data
{
    public class FoodData : IFoodData
    {
        //** notice the "_"
        //A neat way of communicating this is a local field/var/att (There is a way to automate this in tools)
        private readonly IDataAccess _dataAccess;
        private readonly ConnectionStringData _connectionString;

        public FoodData(IDataAccess dataAccess, ConnectionStringData connectionString)
        {
            _dataAccess = dataAccess;
            _connectionString = connectionString;
        }

        //FoodModel needs to be creaetd - coming next

        public Task<List<FoodModel>> GetFood()
        {
            //dynamic keyword allows us to create or pull something in on the fly
            //dynamic represents the "U" parameter, so we could create the "new{}" item in the "U" parameter
            //was there a list of params for the spFood_All stored procedure?
            //Notice this is a task but theres no async/await - passed on (expected) by the caller

            return _dataAccess.LoadData<FoodModel, dynamic>("dbo.spFood_All", new { }, _connectionString.SqlConnectionName);
        }
    }
}
