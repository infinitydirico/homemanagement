using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HomeManagement.API.Data.Entities;
using HomeManagement.Contracts.Repositories;
using Microsoft.Data.Sqlite;

namespace HomeManagement.API.Data.Repositories
{
    public interface IDataLogRepository : IRepository<DataLog> { }

    public class DataLogRepository : IDataLogRepository
    {
        public IQueryable<DataLog> All => throw new NotImplementedException();

        public void Add(DataLog entity)
        {
            Task.Run(async () =>
            {
                await Task.Delay(100);
                await AddAsync(entity);
            });
        }

        public async Task AddAsync(DataLog entity)
        {
            //When using entity framework, a stackoverflow exception is thrown when attempting to write a log record
            using (var connection = new SqliteConnection("Data Source=HomeManagement.db"))
            {
                try
                {
                    await connection.OpenAsync();

                    await CreateTableIfNotExists(connection);
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO DataLogs(Name,Description,Level,TimeStamp) VALUES(@Name,@Description,@Level,@TimeStamp);";
                        command.Parameters.AddWithValue("@Name", entity.Name ?? string.Empty);
                        command.Parameters.AddWithValue("@Description", entity.Description ?? string.Empty);
                        command.Parameters.AddWithValue("@Level", entity.Level);
                        command.Parameters.AddWithValue("@TimeStamp", entity.TimeStamp);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private async Task CreateTableIfNotExists(SqliteConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@table_name;";
                command.Parameters.AddWithValue("@table_name", "DataLogs");

                var result = (await command.ExecuteScalarAsync());

                var count = int.Parse(result.ToString());

                if(count <= 0)
                {
                    command.Parameters.Clear();

                    command.CommandText = "CREATE TABLE DataLogs (Id integer PRIMARY KEY,Name text,Description text,Level integer,TimeStamp text);";
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<DataLog, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Exists(DataLog entity)
        {
            throw new NotImplementedException();
        }

        public DataLog FirstOrDefault()
        {
            throw new NotImplementedException();
        }

        public DataLog FirstOrDefault(Expression<Func<DataLog, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataLog> GetAll()
        {
            throw new NotImplementedException();
        }

        public DataLog GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(DataLog entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public decimal Sum(Expression<Func<DataLog, int>> selector, Expression<Func<DataLog, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public void Update(DataLog entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataLog> Where(Expression<Func<DataLog, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public decimal Sum(Expression<Func<DataLog, decimal>> selector, Expression<Func<DataLog, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IDbTransaction CreateTransaction()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }
    }
}
