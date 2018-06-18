using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Telegram.Bot.Library.PredefinedEmployees;
using Telegram.Bot.Library.Services;

namespace Telegram.Bot.Library.Repositories
{
    internal class MySqlEmployeesRepository : IEmployeesRepository
    {
        private readonly string _connectionString;

        public MySqlEmployeesRepository(IConfiguration config)
        {
            _connectionString = config["MYSQLCONNSTR_localdb"];
            if (string.IsNullOrEmpty(_connectionString))
                throw new ConfigurationErrorsException("MySql connection string missing from configuration");
        }

        public IEnumerable<EmployeeRecord> GetAll(long chatId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                return connection.Query("select * from EmployeesOverride where ChatId = @ChatId", chatId)
                    .Select(record => new EmployeeRecord() { EmployeeId = record.EmployeeId, EmployeeName = record.EmployeeName });
            }
        }

        public void Save(long chatId, EmployeeRecord employee)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Execute("insert into EmployeesOverride values (@ChatId, @EmployeeId, @EmployeeName)",
                    new { ChatId = chatId, employee.EmployeeName, employee.EmployeeId });
            }
        }
    }
}