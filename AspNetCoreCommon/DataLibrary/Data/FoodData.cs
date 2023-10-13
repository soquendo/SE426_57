using DataLibrary.Db; //added
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; //added

namespace DataLibrary.Data
{
    public class FoodData
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

        }
    }
}
