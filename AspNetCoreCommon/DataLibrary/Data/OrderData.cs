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

        public async Task<int> CreateOrder(OrderModel order)
        {
            DynamicParameters p = new DynamicParameters();  //dapper version of parameters

            p.Add("OrderName", order.OrderName);    //Most of these are just specifying the params and the values we fill them with
            p.Add("OrderDate", order.OrderDate);    //Param Direction is Input by default, so we not need to specify here
            p.Add("FoodId", order.FoodId);
            p.Add("Quantity", order.Quantity);
            p.Add("Total", order.Total);
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);    //this is a value we will receive after data is stored (the new Id assigned)

            //perform the actual insert
            await _dataAccess.SaveData("dbo.spOrders_Insert", p, _connectionString.SqlConnectionName);

            return p.Get<int>("Id"); //return to new Id created for the new record that we get from the Scope_Identity command in the stored proc we called
        }
    }
}
