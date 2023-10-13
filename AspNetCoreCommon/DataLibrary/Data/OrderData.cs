using Dapper;   //added
using DataLibrary.Db;   //added
using DataLibrary.Models;   //added
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Data
{
    public class OrderData
    {
        private readonly IDataAccess _dataAccess;
        private readonly ConnectionStringData _connectionString;

        public OrderData(IDataAccess dataAccess, ConnectionStringData connectionString)
        {
            _dataAccess = dataAccess;
            _connectionString = connectionString;
        }

        //slide 22
    }
}
