using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataLibrary.Db
{
    public class SqlDb : IDataAccess
    {
        private readonly IConfiguration config;

        public SqlDb(IConfiguration config)
        {
            this.config = config; //receives config data object as a param. Local "config" points to it.
        }

        //Potentially threaded function that will receive the stored proc name, expected parameters, and name of connection string in order
        //to return a list of data. Seeing we are connecting to another server, we do not want to lock up the rest of the user interface
        //while waiting for a response. This is a generic method to allow this function to handle multiple data types.
        //<T> is the data type (model), <U> is stored proc params in this situation.
        // ! could this function be used to store or update data? if so what would be core requirement?

        public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            //get the connection string from config file and store it locally (allows us to get any connection string we want at build or runtime)
            string connectionString = this.config.GetConnectionString(connectionStringName);

            //configure the connection temporarily in order for it to be released once we are done with it
            //using a generic interface for a connection and solidifying as SQL Connection after =
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                //uses Dapper for QueryAsync to allow threading where possible.
                //rows var will be a DataTable that contain the data from the specified stored procedure
                var rows = await connection.QueryAsync<T>(storedProcedure,
                                                          parameters,
                                                          commandType: CommandType.StoredProcedure);

                return rows.ToList();   //make sure the Rows is converted to a List in order for this to work
            }
        }

        //Method used to save data
        //Food for thought: Could this be used to return data?

        public async Task<int> SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            //get the connection strong from config file and store it locally (allows us to get any connection string we want at build or runtime)
            string connectionString = this.config.GetConnectionString(connectionStringName);

            //configure the connection temporarily in order for it to be released once we are done with it
            //using a generic interface for a connection and solidifying as SQL Connection after =
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                //uses Dapper for ExecuteAsync to allow threading where possible.
                //runs the save and returns the number of records affected (saved)
                return await connection.ExecuteAsync(storedProcedure,
                                                     parameters,
                                                     commandType: CommandType.StoredProcedure);
            }
        }

    }
}
