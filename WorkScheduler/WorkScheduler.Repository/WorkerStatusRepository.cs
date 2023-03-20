using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;

namespace WorkScheduler.Repository
{
    public class WorkerStatusRepository : IWorkerStatusRepository
    {
        public static string connectionString = Environment.GetEnvironmentVariable("SQLConn", EnvironmentVariableTarget.User);
        public async Task<List<IWorkerStatus>> GetAllAsync()
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                List<IWorkerStatus> workerStatuses = new List<IWorkerStatus>();
                SqlCommand findStatus = new SqlCommand("Select * From Status;", connectionAsync);
                await connectionAsync.OpenAsync();
                SqlDataReader readerAsync = await findStatus.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    while (await readerAsync.ReadAsync())
                    {
                        IWorkerStatus workerStatus = new WorkerStatus();
                        workerStatus.Id = readerAsync.GetGuid(0);
                        workerStatus.Status = readerAsync.GetString(1);
                        workerStatuses.Add(workerStatus);
                    }
                }
                readerAsync.Close();
                connectionAsync.Close();
                return workerStatuses;
            }
        }

        public async Task<IWorkerStatus> GetAsync(Guid id)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                IWorkerStatus workerStatus = new WorkerStatus();
                SqlCommand findStatus = new SqlCommand("Select * From Status where id = @id;", connectionAsync);
                findStatus.Parameters.AddWithValue("@id", id);
                await connectionAsync.OpenAsync();
                SqlDataReader readerAsync = await findStatus.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    await readerAsync.ReadAsync();
                    workerStatus.Id = (Guid)readerAsync[0];
                    workerStatus.Status = (string)readerAsync[1];

                }
                readerAsync.Close();
                connectionAsync.Close();
                return workerStatus;
            }
        }
        public async Task<Guid> GetAvailableIdAsync()
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                Guid workerStatus = Guid.Empty;
                SqlCommand findStatus = new SqlCommand("Select Id From WorkerStatus where Status = 'Available';", connectionAsync);
                await connectionAsync.OpenAsync();
                SqlDataReader readerAsync = await findStatus.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    await readerAsync.ReadAsync();
                    workerStatus = (Guid)readerAsync[0];

                }
                readerAsync.Close();
                connectionAsync.Close();
                return workerStatus;
            }
        }
        public async Task<Guid> GetOnTaskIdAsync()
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                Guid workerStatus = Guid.Empty;
                SqlCommand findStatus = new SqlCommand("Select Id From WorkerStatus where Status = 'OnTask';", connectionAsync);
                await connectionAsync.OpenAsync();
                SqlDataReader readerAsync = await findStatus.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    await readerAsync.ReadAsync();
                    workerStatus = (Guid)readerAsync[0];

                }
                readerAsync.Close();
                connectionAsync.Close();
                return workerStatus;
            }
        }
    }
}
