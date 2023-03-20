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
    class WorkerAvailabilityRepository : IWorkerAvailabilityRepository
    {
        public static string connectionString = Environment.GetEnvironmentVariable("SQLConn", EnvironmentVariableTarget.User);
        IRequestRepository requestRepository;
        IWorkerStatusRepository statusRepository;
        public WorkerAvailabilityRepository(IRequestRepository requestRepository, IWorkerStatusRepository statusRepository)
        {
            this.requestRepository = requestRepository;
            this.statusRepository = statusRepository;
        }
        public async Task<IWorkerAvailability> GetAvailabilityByIdAsync(Guid id)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                IWorkerAvailability workerAvailability = new WorkerAvailability();
                SqlCommand findAvailability = new SqlCommand("Select * From WorkerAvailability where Id = @id ;", connectionAsync);
                await connectionAsync.OpenAsync();
                findAvailability.Parameters.AddWithValue("@id", id);
                SqlDataReader readerAsync = await findAvailability.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    await readerAsync.ReadAsync();
                    workerAvailability.Id = Guid.Parse(readerAsync[0].ToString());
                    workerAvailability.WorkerId = Guid.Parse(readerAsync[1].ToString());
                    workerAvailability.RequestId = Guid.Parse(readerAsync[2].ToString());
                    workerAvailability.StartDate = DateTime.Parse(readerAsync[3].ToString());
                    workerAvailability.EndDate = DateTime.Parse(readerAsync[4].ToString());
                }
                readerAsync.Close();
                connectionAsync.Close();
                return workerAvailability;
            }
        }
        public async Task<List<IWorkerAvailability>> GetAvailabilityByWorkerAsync(Guid workerId)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                List<IWorkerAvailability> workerAvailabilitys = new List<IWorkerAvailability>();
                IWorkerAvailability workerAvailability = new WorkerAvailability();
                SqlCommand findAvailability = new SqlCommand("Select * From WorkerAvailability where WorkerId = @WorkerId ;", connectionAsync);
                await connectionAsync.OpenAsync();
                findAvailability.Parameters.AddWithValue("@WorkerId", workerId);
                SqlDataReader readerAsync = await findAvailability.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    while (await readerAsync.ReadAsync())
                    {
                        workerAvailability.Id = Guid.Parse(readerAsync[0].ToString());
                        workerAvailability.WorkerId = Guid.Parse(readerAsync[1].ToString());
                        workerAvailability.RequestId = Guid.Parse(readerAsync[2].ToString());
                        workerAvailability.StartDate = DateTime.Parse(readerAsync[3].ToString());
                        workerAvailability.EndDate = DateTime.Parse(readerAsync[4].ToString());
                        workerAvailabilitys.Add(workerAvailability);
                    }
                }
                readerAsync.Close();
                connectionAsync.Close();
                return workerAvailabilitys;
            }
        }
        public async Task<List<IWorkerAvailability>> GetAvailableWorkerByWeekAsync(Guid workerId, DateTime date)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                int today = (int)date.DayOfWeek;
                if (today > 0)
                    today -= 1;
                else
                    today = 7;
                DateTime StartOfWeek = DateTime.Now.AddDays(-today);
                DateTime EndOfWeek = DateTime.Now.AddDays(7 - today);
                List<IWorkerAvailability> workerAvailabilitys = new List<IWorkerAvailability>();
                IWorkerAvailability workerAvailability = new WorkerAvailability();
                Guid availableId = await GetAvailableStatusAsync();
                SqlCommand findAvailability = new SqlCommand("Select WorkerAvailability.* From WorkerAvailability Left join Request on Request.id = WorkerAvailability.RequestId where WorkerId = @WorkerId And Request.WorkerStatusId = @availableId And (StartDate <= @StartOfWeek1 And EndDate >= @StartOfWeek2 OR StartDate <= @EndOfWeek1 AND EndDate >= @EndOfWeek2);", connectionAsync);
                await connectionAsync.OpenAsync();
                findAvailability.Parameters.AddWithValue("@WorkerId", workerId);
                findAvailability.Parameters.AddWithValue("@availableId", availableId);
                findAvailability.Parameters.AddWithValue("@StartOfWeek1", StartOfWeek);
                findAvailability.Parameters.AddWithValue("@StartOfWeek2", StartOfWeek);
                findAvailability.Parameters.AddWithValue("@EndOfWeek1", EndOfWeek);
                findAvailability.Parameters.AddWithValue("@EndOfWeek2", EndOfWeek);
                SqlDataReader readerAsync = await findAvailability.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    while (await readerAsync.ReadAsync())
                    {
                        workerAvailability.Id = Guid.Parse(readerAsync[0].ToString());
                        workerAvailability.WorkerId = Guid.Parse(readerAsync[1].ToString());
                        workerAvailability.RequestId = Guid.Parse(readerAsync[2].ToString());
                        workerAvailability.StartDate = DateTime.Parse(readerAsync[3].ToString());
                        workerAvailability.EndDate = DateTime.Parse(readerAsync[4].ToString());
                        workerAvailabilitys.Add(workerAvailability);
                    }
                }
                readerAsync.Close();
                connectionAsync.Close();
                return workerAvailabilitys;
            }
        }
        public async Task<bool> CreateAvailability(IWorkerAvailability workerAvailability, IRequest request)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                await requestRepository.CreateRequestAsync(request);
                await connectionAsync.OpenAsync();
                SqlCommand insertAvailability = new SqlCommand("Insert Into WorkerAvailability Values ( @id, @WorkerId, @RequestId, @StartDate, @EndDate);", connectionAsync);
                insertAvailability.Parameters.AddWithValue("@id", workerAvailability.Id);
                insertAvailability.Parameters.AddWithValue("@WorkerId", workerAvailability.WorkerId);
                insertAvailability.Parameters.AddWithValue("@RequestId", request.Id);
                insertAvailability.Parameters.AddWithValue("@StartDate", workerAvailability.StartDate);
                insertAvailability.Parameters.AddWithValue("@EndDate", workerAvailability.EndDate);
                try
                {
                    await insertAvailability.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                { 
                    connectionAsync.Close();
                    return false;
                }
                connectionAsync.Close();
                return true;
            }
        }
        public async Task<string> UpdateAvailabilityByIdAsync(Guid id, IWorkerAvailability workerUnavailability, Guid AdminId)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                IWorkerAvailability oldWorkerAvailability = new WorkerAvailability();
                IWorkerAvailability workerAvailability = new WorkerAvailability(workerUnavailability);
                IRequest request = new Request();
                SqlCommand findAvailability = new SqlCommand("Select * From WorkerAvailability where Id = @id ;", connectionAsync);
                await connectionAsync.OpenAsync();
                findAvailability.Parameters.AddWithValue("@id", id);
                SqlDataReader readerAsync = await findAvailability.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    await readerAsync.ReadAsync();
                    oldWorkerAvailability.Id = Guid.Parse(readerAsync[0].ToString());
                    oldWorkerAvailability.WorkerId = Guid.Parse(readerAsync[1].ToString());
                    oldWorkerAvailability.RequestId = Guid.Parse(readerAsync[2].ToString());
                    oldWorkerAvailability.StartDate = DateTime.Parse(readerAsync[3].ToString());
                    oldWorkerAvailability.EndDate = DateTime.Parse(readerAsync[4].ToString());
                }
                bool change = false;
                    request = await requestRepository.GetOneRequestAsync(oldWorkerAvailability.RequestId);
                if (oldWorkerAvailability.EndDate.CompareTo(workerUnavailability.StartDate) > 0 && oldWorkerAvailability.EndDate.CompareTo(workerUnavailability.EndDate) > 0 && request.RequestStatus == "Available")
                {
                    change = true;
                    SqlCommand updateAvailability = new SqlCommand("Update WorkerAvailability set EndDate = @EndDate where Id = @id ;", connectionAsync);
                    updateAvailability.Parameters.AddWithValue("@EndDate", workerUnavailability.StartDate);
                    updateAvailability.Parameters.AddWithValue("@id", oldWorkerAvailability.Id);
                    updateAvailability.ExecuteNonQuery();
                    SqlCommand insertUnavailability = new SqlCommand("Insert Into  WorkerAvailability Values ( @id, @WorkerId, @RequestId, @StartDate, @EndDate);", connectionAsync);
                    insertUnavailability.Parameters.AddWithValue("@id", workerUnavailability.Id);
                    insertUnavailability.Parameters.AddWithValue("@WorkerId", workerUnavailability.WorkerId);
                    insertUnavailability.Parameters.AddWithValue("@RequestId", workerUnavailability.RequestId);
                    insertUnavailability.Parameters.AddWithValue("@StartDate", workerUnavailability.StartDate);
                    insertUnavailability.Parameters.AddWithValue("@EndDate", workerUnavailability.EndDate);

                    IRequest newRequest = new Request();
                    newRequest.RequestStatus = "Accepted";
                    newRequest.StartDate = workerUnavailability.EndDate;
                    newRequest.EndDate = DateTime.MaxValue;
                    newRequest.CreatedByUser = AdminId;
                    newRequest.UpdatedByUser = AdminId;
                    newRequest.WorkerStatusId = await GetAvailableStatusAsync();
                    await requestRepository.CreateRequestAsync(newRequest);
                    SqlCommand insertAvailability = new SqlCommand("Insert Into  WorkerAvailability Values ( @id, @WorkerId, @RequestId, @StartDate, @EndDate);", connectionAsync);
                    insertAvailability.Parameters.AddWithValue("@id", workerAvailability.Id);
                    insertAvailability.Parameters.AddWithValue("@WorkerId", workerAvailability.WorkerId);
                    insertAvailability.Parameters.AddWithValue("@RequestId", newRequest.Id);
                    insertAvailability.Parameters.AddWithValue("@StartDate", workerAvailability.StartDate);
                    insertAvailability.Parameters.AddWithValue("@EndDate", workerAvailability.EndDate);
                }
                
                if (!change)
                {
                    readerAsync.Close();
                    connectionAsync.Close();
                    return "Worker not available at that time period!";
                }
                readerAsync.Close();
                connectionAsync.Close();
                return "Table has been updated";
            }
        }
        public async Task<string> UpdateAvailabilityAsync(Guid workerId ,IWorkerAvailability workerUnavailability, Guid AdminId)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                List<IWorkerAvailability> workerAvailabilitys = new List<IWorkerAvailability>();
                IWorkerAvailability oldWorkerAvailability = new WorkerAvailability();
                IWorkerAvailability workerAvailability = new WorkerAvailability(workerUnavailability);
                IRequest request = new Request();
                SqlCommand findAvailability = new SqlCommand("Select * From WorkerAvailability where WorkerId = @workerId ;", connectionAsync);
                await connectionAsync.OpenAsync();
                findAvailability.Parameters.AddWithValue("@workerId", workerId);
                SqlDataReader readerAsync = await findAvailability.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    await readerAsync.ReadAsync();
                    oldWorkerAvailability.Id = Guid.Parse(readerAsync[0].ToString());
                    oldWorkerAvailability.WorkerId = Guid.Parse(readerAsync[1].ToString());
                    oldWorkerAvailability.RequestId = Guid.Parse(readerAsync[2].ToString());
                    oldWorkerAvailability.StartDate = DateTime.Parse(readerAsync[3].ToString());
                    oldWorkerAvailability.EndDate = DateTime.Parse(readerAsync[4].ToString());
                    workerAvailabilitys.Add(oldWorkerAvailability);
                }
                bool change = false;
                foreach (WorkerAvailability workerAvailability1 in workerAvailabilitys)
                {
                    request = await requestRepository.GetOneRequestAsync(workerAvailability1.RequestId);
                    if (workerAvailability1.EndDate.CompareTo(workerUnavailability.StartDate) > 0 && workerAvailability1.EndDate.CompareTo(workerUnavailability.EndDate) > 0 && request.RequestStatus == "Available")
                    {
                        change = true;
                        SqlCommand updateAvailability = new SqlCommand("Update WorkerAvailability set EndDate = @EndDate where Id = @id ;", connectionAsync);
                        updateAvailability.Parameters.AddWithValue("@EndDate", workerUnavailability.StartDate);
                        updateAvailability.Parameters.AddWithValue("@id", workerAvailability1.Id);
                        updateAvailability.ExecuteNonQuery();
                        SqlCommand insertUnavailability = new SqlCommand("Insert Into  WorkerAvailability Values ( @id, @WorkerId, @RequestId, @StartDate, @EndDate);", connectionAsync);
                        insertUnavailability.Parameters.AddWithValue("@id", workerUnavailability.Id);
                        insertUnavailability.Parameters.AddWithValue("@WorkerId", workerUnavailability.WorkerId);
                        insertUnavailability.Parameters.AddWithValue("@RequestId", workerUnavailability.RequestId);
                        insertUnavailability.Parameters.AddWithValue("@StartDate", workerUnavailability.StartDate);
                        insertUnavailability.Parameters.AddWithValue("@EndDate", workerUnavailability.EndDate);

                        IRequest newRequest = new Request();
                        newRequest.RequestStatus = "Accepted";
                        newRequest.StartDate = workerUnavailability.EndDate;
                        newRequest.EndDate = DateTime.MaxValue;
                        newRequest.CreatedByUser = AdminId;
                        newRequest.UpdatedByUser = AdminId;
                        newRequest.WorkerStatusId = await GetAvailableStatusAsync();
                        await requestRepository.CreateRequestAsync(newRequest);
                        SqlCommand insertAvailability = new SqlCommand("Insert Into  WorkerAvailability Values ( @id, @WorkerId, @RequestId, @StartDate, @EndDate);", connectionAsync);
                        insertAvailability.Parameters.AddWithValue("@id", workerAvailability.Id);
                        insertAvailability.Parameters.AddWithValue("@WorkerId", workerAvailability.WorkerId);
                        insertAvailability.Parameters.AddWithValue("@RequestId", newRequest.Id);
                        insertAvailability.Parameters.AddWithValue("@StartDate", workerAvailability.StartDate);
                        insertAvailability.Parameters.AddWithValue("@EndDate", workerAvailability.EndDate);
                    }
                }
                if(!change)
                {
                    readerAsync.Close();
                    connectionAsync.Close();
                    return "Worker not available at that time period!";
                }
                readerAsync.Close();
                connectionAsync.Close();
                return "Table has been updated";
            }
        }
        private async Task<Guid> GetAvailableStatusAsync()
        {
            List<IWorkerStatus> workerStatuses = new List<IWorkerStatus>();
            workerStatuses = await statusRepository.GetAllAsync();
            foreach (IWorkerStatus workerStatus in workerStatuses)
            {
                if (workerStatus.Status == "Available")
                {
                    return workerStatus.Id;
                }
            }
            return Guid.Empty;
        }
    }
}
