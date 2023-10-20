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
    public class OrderData : IOrderData
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
            //p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);    //this is a value we will receive after data is stored (the new Id assigned)

            //perform the actual insert
            await _dataAccess.SaveData("dbo.spOrders_Insert", p, _connectionString.SqlConnectionName);

            return p.Get<int>("Id"); //return to new Id created for the new record that we get from the Scope_Identity command in the stored proc we called
        }

        public Task<int> UpdateOrderName(int orderId, string orderName)
        {
            //update the data and return the response (integer)
            //three pieces of data is needed:
            //1) the stored procedure
            //2) parameters and values. The parameter names on the left side should match those in the stored proc specified.
            //3) the name of the connection string we are using
            return _dataAccess.SaveData("spOrders_UpdateName",
                                       new
                                       {
                                           Id = orderId,
                                           OrderName = orderName
                                       },
                                       _connectionString.SqlConnectionName);
        }

        public Task<int> DeleteOrder(int orderId)
        {
            //delete the specified record and return the response (integer)
            //**note: no async or wait here bc there is one primary task and nothing is waiting for this to continue
            //three pieces of data is needed:
            //1) the stored procedure
            //2) parameters and values - the parameter names on the left side should match those in the the stored proc specified.
            //3) the name of the connection string we are using
            return _dataAccess.SaveData("spOrders_Delete",
                                        new { Id = orderId },
                                        _connectionString.SqlConnectionName);
        }

        public async Task<OrderModel> GetOrderById(int orderId)
        {
            //this returns a list of records that should either consist of one record or null
            //we pass in the Id as a parameter

            //** note: we use async and wait here bc we need the lsit to come back first before getting the first record of the list

            var recs = await _dataAccess.LoadData<OrderModel, dynamic>("dbo.spOrders_GetById",
                                                                       new { Id = orderId },
                                                                       _connectionString.SqlConnectionName);
            //first record (one 1 records) OR Null (Default for an object)
            return recs.FirstOrDefault();
        }
    }
}
