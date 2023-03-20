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
    public class ScheduleTaskWorkerRepository: IScheduleTaskWorkerRepository
    {
        public static string connectionString = Environment.GetEnvironmentVariable("SQLConn", EnvironmentVariableTarget.User);
        IWorkerAvailabilityRepository workerAvailabilityRepository;
        IWorkerStatusRepository workerStatusRepository;
        IRequestRepository requestRepository;
        ITaskStatusRepository taskStatusRepository;
        public ScheduleTaskWorkerRepository(IWorkerAvailabilityRepository workerAvailabilityRepository, IWorkerStatusRepository workerStatusRepository, IRequestRepository requestRepository, ITaskStatusRepository taskStatusRepository)
        {
            this.workerAvailabilityRepository = workerAvailabilityRepository;
            this.workerStatusRepository = workerStatusRepository;
            this.requestRepository = requestRepository;
            this.taskStatusRepository = taskStatusRepository;
        }
        public async Task<List<IScheduleTaskWorker>> FindByWorker(Guid workerId, DateTime date)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                List<IWorkerAvailability> workerAvailabilityThisWeek = await workerAvailabilityRepository.GetAvailableWorkerByWeekAsync(workerId, date);
                List<IScheduleTaskWorker> schedule = new List<IScheduleTaskWorker>();
                IScheduleTaskWorker scheduleDay = new ScheduleTaskWorker();
                int today = (int)date.DayOfWeek;
                if (today > 0)
                    today -= 1;
                else
                    today = 7;
                DateTime StartOfTheWeek = DateTime.Now.AddDays(-today);
                DateTime EndOfTheWeek = DateTime.Now.AddDays(7 - today);
                foreach (IWorkerAvailability workerAvailability in workerAvailabilityThisWeek)
                {

                    SqlCommand findByWorker = new SqlCommand("Select * From ScheduleTaskWorker where workerAvailabilityId = @workerAvailabilityId And StartDateTime >= @StartOfTheWeek And EndDateTime <= @EndOfTheeWeek;", connectionAsync);
                    await connectionAsync.OpenAsync();
                    findByWorker.Parameters.AddWithValue("@workerAvailabilityId", workerAvailability.Id);
                    findByWorker.Parameters.AddWithValue("@StartOfTheWeek", StartOfTheWeek);
                    findByWorker.Parameters.AddWithValue("@EndOfTheWeek", EndOfTheWeek);
                    SqlDataReader readerAsync = await findByWorker.ExecuteReaderAsync();
                    if (readerAsync.HasRows)
                    {
                        while(await readerAsync.ReadAsync())
                        {
                            scheduleDay.Id = (Guid)readerAsync[0];
                            scheduleDay.TaskId = (Guid)readerAsync[1];
                            scheduleDay.WorkerAvailabilityId = (Guid)readerAsync[2];
                            scheduleDay.StartDateTime = (DateTime)readerAsync[3];
                            scheduleDay.EndDateTime = (DateTime)readerAsync[4];
                            scheduleDay.TaskDuration = (int)readerAsync[5];
                            schedule.Add(scheduleDay);
                        }
                    }
                    readerAsync.Close();
                }
                connectionAsync.Close();
                return schedule;
            }
        }
        public async Task<List<IScheduleTaskWorker>> GetAllAsync()
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                List<IScheduleTaskWorker> schedule = new List<IScheduleTaskWorker>();
                IScheduleTaskWorker scheduleDay = new ScheduleTaskWorker();
                SqlCommand findByWorker = new SqlCommand("Select * From ScheduleTaskWorker;", connectionAsync);
                await connectionAsync.OpenAsync();
                SqlDataReader readerAsync = await findByWorker.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    while (await readerAsync.ReadAsync())
                    {
                        scheduleDay.Id = (Guid)readerAsync[0];
                        scheduleDay.TaskId = (Guid)readerAsync[1];
                        scheduleDay.WorkerAvailabilityId = (Guid)readerAsync[2];
                        scheduleDay.StartDateTime = (DateTime)readerAsync[3];
                        scheduleDay.EndDateTime = (DateTime)readerAsync[4];
                        scheduleDay.TaskDuration = (int)readerAsync[5];
                        schedule.Add(scheduleDay);
                    }
                }
                readerAsync.Close();

                connectionAsync.Close();
                return schedule;
            }
        }
        public async Task<string> AddToSchedule(IScheduleTaskWorker scheduleTaskWorker, Guid adminId)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                ITaskStatus taskStatus = await taskStatusRepository.GetAsync("InProgress");
                SqlCommand getTask = new SqlCommand("Select Id From Task where Id = @TaskId And TaskStatusId = @TaskStatusId;", connectionAsync);
                await connectionAsync.OpenAsync();
                getTask.Parameters.AddWithValue("@TaskId", scheduleTaskWorker.TaskId);
                getTask.Parameters.AddWithValue("@TaskStatusId", taskStatus.Id);
                SqlDataReader readerAsync = await getTask.ExecuteReaderAsync();
                if (!readerAsync.HasRows)
                {
                    readerAsync.Close();
                    connectionAsync.Close();
                    return "Error while selecting Task";
                }
                readerAsync.Close();
                Guid available = await workerStatusRepository.GetAvailableIdAsync();

                SqlCommand getWorkerAvailability = new SqlCommand("Select Id From WorkerAvailability Left join Request on Request.Id = WorkerAvailability.RequestId where WorkerAvailability.Id = @WorkerAvailabilityId And Request.WorkerStatusId = @available;", connectionAsync);
                await connectionAsync.OpenAsync();
                getWorkerAvailability.Parameters.AddWithValue("@WorkerAvailabilityId", scheduleTaskWorker.WorkerAvailabilityId);
                getWorkerAvailability.Parameters.AddWithValue("@available", available);
                readerAsync = await getWorkerAvailability.ExecuteReaderAsync();
                if (!readerAsync.HasRows)
                {
                    readerAsync.Close();
                    connectionAsync.Close();
                    return "Error while selecting Worker";
                }
                connectionAsync.Close();
                SqlCommand addToSchedule = new SqlCommand("Insert Into ScheduleTaskWorker Values(@Id, @TaskId, @WorkerAvailabilityId, @StartDateTime, @EndDateTime, @TaskDuration);", connectionAsync);
                await connectionAsync.OpenAsync();
                addToSchedule.Parameters.AddWithValue("@Id", scheduleTaskWorker.Id);
                addToSchedule.Parameters.AddWithValue("@WorkerAvailabilityId", scheduleTaskWorker.WorkerAvailabilityId);
                addToSchedule.Parameters.AddWithValue("@TaskId", scheduleTaskWorker.TaskId);
                addToSchedule.Parameters.AddWithValue("@StartDateTime", scheduleTaskWorker.StartDateTime);
                addToSchedule.Parameters.AddWithValue("@EndDateTime", scheduleTaskWorker.EndDateTime);
                addToSchedule.Parameters.AddWithValue("@TaskDuration", scheduleTaskWorker.TaskDuration);
                IRequest request = new Request();
                request.StartDate = scheduleTaskWorker.StartDateTime;
                request.EndDate = scheduleTaskWorker.EndDateTime;

                IWorkerAvailability workerAvailability = await workerAvailabilityRepository.GetAvailabilityByIdAsync(scheduleTaskWorker.WorkerAvailabilityId);
                workerAvailability.RequestId = request.Id;
                workerAvailability.StartDate = request.StartDate;
                workerAvailability.EndDate = request.EndDate;
                workerAvailabilityRepository.UpdateAvailabilityByIdAsync(request.CreatedByUser, workerAvailability, adminId);
                if ("Worker not available at that time period!" == await workerAvailabilityRepository.UpdateAvailabilityAsync(request.CreatedByUser, workerAvailability, request.UpdatedByUser))
                {
                    return "Error while asigning worker";
                }

                if (await requestRepository.CreateOnTaskWorkerAsync(scheduleTaskWorker.WorkerAvailabilityId, request, adminId))
                {
                    return "Error while asigning worker";
                }
                await addToSchedule.ExecuteNonQueryAsync();
                connectionAsync.Close();
                return "Element added to Schedule";
            }
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                SqlCommand findWorker = new SqlCommand("Select * From ScheduleTaskWorker where Id = @id;", connectionAsync);
                findWorker.Parameters.AddWithValue("@id", id);
                await connectionAsync.OpenAsync();
                SqlDataReader readerAsync = await findWorker.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    readerAsync.Close();
                    SqlCommand deleteWorker = new SqlCommand("Delete From ScheduleTaskWorker where Id = @id;", connectionAsync);
                    deleteWorker.Parameters.AddWithValue("@id", id);
                    await deleteWorker.ExecuteNonQueryAsync();
                    connectionAsync.Close();
                    return true;
                }
                readerAsync.Close();
                connectionAsync.Close();
                return false;
            }
        }
    }
}
