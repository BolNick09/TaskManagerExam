using Azure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerExam
{
    public class TaskObject : SQLRequests
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CadastralNumber { get; set; }
        public List<int> TasksID { get; set; }

        public TaskObject() { }

        public TaskObject(int id, string name, string address, string cadastralNum, List <int> tasksID)
        {
            Id = id;
            Name = name;
            Address = address;
            CadastralNumber = cadastralNum;
            TasksID = tasksID;
        }

        public TaskObject(int id, string name, string address, string cadastralNum)//TODO - добавить получение ID тасков
        {
            Id = id;
            Name = name;
            Address = address;
            CadastralNumber = cadastralNum;

        }
        public TaskObject(string name, string address, string cadastralNum)
        {
            Name = name;
            Address = address;
            CadastralNumber = cadastralNum;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name} Address: {Address}, Cadastral number: {CadastralNumber}, Number of tasks: {TasksID?.Count}";
        }

        public static void GetInfo(List<TaskObject> itemList)
        {
            sqlQuery = @"SELECT TaskObjects.Id, TaskObjects.TOName, TaskObjects.TOAddress, TaskObjects.TOCadastralNumber,
                        STRING_AGG(Objects_Tasks.OT_FK_Tasks_Id, ',') AS TaskIds
                        FROM TaskObjects
                        LEFT JOIN Objects_Tasks ON TaskObjects.Id = Objects_Tasks.OT_FK_TaskObjects_Id
                        GROUP BY TaskObjects.Id, TaskObjects.TOName, TaskObjects.TOAddress, TaskObjects.TOCadastralNumber";

            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = (int)reader.GetValue(0);
                    string name = (string)reader.GetValue(1);
                    string address = (string)reader.GetValue(2);
                    string cadastralNum = (string)reader.GetValue(3);
                    string taskIds = reader.GetValue(4).ToString();
                    List<int> tasksID = taskIds != "" ? taskIds.Split(',').Select(int.Parse).ToList() : new List<int>();

                    itemList.Add(new TaskObject(id, name, address, cadastralNum, tasksID));
                }
            }
            reader.Close();
            sqlConnection.Close();
        }

        public override void Insert() 
        {

            sqlQuery =  $"INSERT INTO TaskObjects (TOName, TOAddress, TOCadastralNumber) " +
                        $"VALUES (@TOName, @TOAddress, @TOCadastralNumber); " +
                        $"SELECT CAST(SCOPE_IDENTITY() AS INT)";
            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parName = new SqlParameter("@TOName", Name);
                SqlParameter parAddress = new SqlParameter("@TOAddress", Address);
                SqlParameter parCadastralNum = new SqlParameter("@TOCadastralNumber", CadastralNumber);
                cmd.Parameters.Add(parName);
                cmd.Parameters.Add(parAddress);
                cmd.Parameters.Add(parCadastralNum);
                int id = (int)cmd.ExecuteScalar();
                this.Id = id;

                sqlConnection.Close();
                Console.WriteLine($"Запись добавлена в БД, Id: {id}");

            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }            
        }

        public override void Update()
        {
            sqlQuery = @"UPDATE TaskObjects
                        SET TOName = @parTOName, TOAddress = @parTOAddress, 
                        TOCadastralNumber = @parTOCadastralNumber
                        WHERE Id = @parId;
                        DELETE FROM Objects_Tasks WHERE OT_FK_TaskObjects_Id = @parId;
                        INSERT INTO Objects_Tasks (OT_FK_TaskObjects_Id, OT_FK_Tasks_Id)
                        SELECT @parId, TasksId FROM (VALUES (@parTasksId)) AS t(TasksId)";

            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parId = new SqlParameter("@parId", Id);
                SqlParameter parName = new SqlParameter("@parTOName", Name);
                SqlParameter parAddress = new SqlParameter("@parTOAddress", Address);
                SqlParameter parCadastralNum = new SqlParameter("@parTOCadastralNumber", CadastralNumber);
                cmd.Parameters.Add(parId);
                cmd.Parameters.Add(parName);
                cmd.Parameters.Add(parAddress);
                cmd.Parameters.Add(parCadastralNum);

                foreach (int taskId in TasksID)
                {
                    SqlParameter parTasksId = new SqlParameter("@parTasksId", taskId);
                    cmd.Parameters.Add(parTasksId);
                }


                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                Console.WriteLine($"Запись обновлена в БД");
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }
        }
        public override void Delete()
        {
            try
            {
                sqlQuery = @"DELETE FROM TaskObjects
                            WHERE Id = @parId;
                            DELETE FROM Tasks
                            WHERE Id IN (SELECT OT_FK_Tasks_Id FROM Objects_Tasks WHERE OT_FK_TaskObjects_Id = @parId);
                            DELETE FROM Objects_Tasks
                            WHERE OT_FK_TaskObjects_Id = @parId;";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);

                SqlParameter parId = new SqlParameter("@parId", Id);
                cmd.Parameters.Add(parId);

                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                Console.WriteLine($"Запись удалена из БД");
            }
            catch ( Exception ex ) 
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }
        }
    }
//---------------------------------------------------------------------------
    public class Task : SQLRequests
    {
        public int Id { get; set; }
        public string TaskDescription { get; set; }
        public int UserId { get; set; }
        public int DecisionId { get; set; }

        public Task(int id, string taskDescription, int userId, int decisionId)
        {
            Id = id;
            TaskDescription = taskDescription;
            UserId = userId;
            DecisionId = decisionId;
        }
        public Task(int id, string taskDescription, int userId)
        {
            Id = id;
            TaskDescription = taskDescription;
            UserId = userId;
        }
        public Task( string taskDescription, int userId)
        {
            TaskDescription = taskDescription;
            UserId = userId;
        }
        public Task(int id, string taskDescription)
        {
            Id = id;
            TaskDescription = taskDescription;
        }
        public Task(string taskDescription)
        {
            TaskDescription = taskDescription;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Description: {TaskDescription} User id: {UserId}, Decision Id: {DecisionId}";
        }

        public static void GetInfo(List<Task> taskList)
        {
            sqlQuery =  @"SELECT Tasks.Id, TDescription, T_FK_User_ID, T_FK_Decision_Id
                        FROM Tasks";

            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = (int)reader.GetValue(0);
                    string description = (string)reader.GetValue(1);
                    int userId = (int)reader.GetValue(2);
                    int decisionId = reader.IsDBNull(3) ? -1 : (int)reader.GetValue(3);

                    taskList.Add(new Task(id, description, userId, decisionId));
                }
            }
            reader.Close();
            sqlConnection.Close();
        }

        public override void Insert()
        {

            sqlQuery =  $"INSERT INTO Tasks (TDescription, T_FK_User_ID) " +
                        $"VALUES (@TDescription, @T_FK_User_ID); " +
                        $"SELECT CAST(SCOPE_IDENTITY() AS INT)";
            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parDescription = new SqlParameter("@TDescription", TaskDescription);
                SqlParameter parUserId = new SqlParameter("@T_FK_User_ID", UserId);
                cmd.Parameters.Add(parDescription);
                cmd.Parameters.Add(parUserId);
                Id = (int)cmd.ExecuteScalar(); // Get the Id of the inserted record
                sqlConnection.Close();
                Console.WriteLine($"Запись добавлена в БД, Id: {Id}");
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }
        }
        public void СonnectToObject(int objectId)
        {
            sqlQuery = "INSERT INTO Objects_Tasks (OT_FK_TaskObjects_Id, OT_FK_Tasks_Id) " +
                       "VALUES (@OT_FK_TaskObjects_Id, @OT_FK_Tasks_Id)";
            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parTaskObjectsId = new SqlParameter("@OT_FK_TaskObjects_Id", objectId);
                SqlParameter parTasksId = new SqlParameter("@OT_FK_Tasks_Id", Id);
                cmd.Parameters.Add(parTaskObjectsId);
                cmd.Parameters.Add(parTasksId);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                Console.WriteLine($"Запись добавлена в таблицу Objects_Tasks");
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }
        }

        public override void Update()
        {
            sqlQuery = "UPDATE Tasks SET TDescription = @parTDescription, T_FK_User_ID = @parT_FK_User_ID, " +
                        "T_FK_Decision_Id = @parT_FK_Decision_Id WHERE Id = @parId";

            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parId = new SqlParameter("@parId", Id);
                SqlParameter parDescription = new SqlParameter("@parTDescription", TaskDescription);
                SqlParameter parUserId = new SqlParameter("@parT_FK_User_ID", UserId);
                SqlParameter parDecisionId = new SqlParameter("@parT_FK_Decision_Id", DecisionId);
                cmd.Parameters.Add(parId);
                cmd.Parameters.Add(parDescription);
                cmd.Parameters.Add(parUserId);
                cmd.Parameters.Add(parDecisionId);


                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                Console.WriteLine($"Запись обновлена в БД");
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }
        }
        public override void Delete()
        {
            try
            {
                sqlQuery = @"DELETE FROM Tasks
                            WHERE Id = @parId;
                            DELETE FROM Objects_Tasks
                            WHERE OT_FK_Tasks_Id = @parId;";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);

                SqlParameter parId = new SqlParameter("@parId", Id);
                cmd.Parameters.Add(parId);

                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                Console.WriteLine($"Запись удалена из БД");
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }
        }

    }
//---------------------------------------------------------------------------
    public class Decision : SQLRequests
    {
        public int Id { get; private set; }
        public string DecisionDescription { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Statuses Status { get; private set; }

        public Decision(int id, string decisionDescription, DateTime startDate, DateTime endDate, Statuses status)
        {
            Id = id;
            DecisionDescription = decisionDescription;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
        }
        public Decision(string decisionDescription, DateTime startDate, DateTime endDate, Statuses status)
        {
            DecisionDescription = decisionDescription;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
        }
        public override string ToString()
        {
            string stat = "";

            switch (Status)
            {
                case Statuses.NEW:
                {
                    stat = "Новая";
                    break;
                }
                case Statuses.CLOSED:
                {
                    stat = "Закрыта";
                    break;
                }
                case Statuses.IN_PROGRESS:
                {
                    stat = "В процессе";
                    break;
                }
                default:
                {
                    stat = "Ошибка в определении статсуса";
                    break;
                }
            }
            return $"Id: {Id}, Description: {DecisionDescription} Start date: {StartDate}, End date: {EndDate} Status: {stat}";
        }

        public static void GetInfo(List<Decision> decisionList)
        {
            sqlQuery = @"SELECT Decisions.Id, Decisions.DDescription, Decisions.DStartDate, Decisions.DEndDate, Decisions.DStatus
                        FROM Decisions";

            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    {
                        int id = reader.GetInt32(0);
                        string description = reader.GetString(1);
                        DateTime startDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2);
                        DateTime endDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
                        int statusInt = reader.IsDBNull(4) ? -1 : (int)reader.GetInt16(4);

                        Statuses status = (Statuses)statusInt;

                        decisionList.Add(new Decision(id, description, startDate, endDate, status));

                    }
                }
            }
            reader.Close();
            sqlConnection.Close();
        }

        public override void Insert()
        {

            sqlQuery =  $"INSERT INTO Decisions (DDescription, DStartDate, DEndDate, DStatus) " +
                        $"VALUES (@DDescription, @DStartDate, @DEndDate, @DStatus); " +
                        $"SELECT CAST(SCOPE_IDENTITY() AS INT)";
            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parDescription = new SqlParameter("@DDescription", DecisionDescription);
                SqlParameter parStartDate = new SqlParameter("@DStartDate", StartDate);
                SqlParameter parEndDate = new SqlParameter("@DEndDate", EndDate);
                SqlParameter parStatus = new SqlParameter("@DStatus", Status);
                cmd.Parameters.Add(parDescription);
                cmd.Parameters.Add(parStartDate);
                cmd.Parameters.Add(parEndDate);
                cmd.Parameters.Add(parStatus);
                Id = (int)cmd.ExecuteScalar();
                sqlConnection.Close();
                Console.WriteLine($"Запись добавлена в БД. Id: {Id}");
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }
        }

        public override void Update()
        {
            sqlQuery = "UPDATE Decisions SET DDescription = @parDDescription, DStartDate = @parDStartDate, " +
                        "DEndDate = @parDEndDate, DStatus = @DStatus WHERE Id = @parId";

            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parId = new SqlParameter("@parId", Id);
                SqlParameter parDescription = new SqlParameter("@parDDescription", DecisionDescription);
                SqlParameter parStartDate = new SqlParameter("@parDStartDate", StartDate);
                SqlParameter parEndDate = new SqlParameter("@parDEndDate", EndDate);
                SqlParameter parStatus = new SqlParameter("@parDStatus", Status);
                cmd.Parameters.Add(parId);
                cmd.Parameters.Add(parDescription);
                cmd.Parameters.Add(parStartDate);
                cmd.Parameters.Add(parEndDate);
                cmd.Parameters.Add(parStatus);


                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                Console.WriteLine($"Запись обновлена в БД");
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }
        }
        public override void Delete()
        {
            try
            {
                sqlQuery = "DELETE FROM Decisions WHERE Id = @parId";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);

                SqlParameter parId = new SqlParameter("@parId", Id);
                cmd.Parameters.Add(parId);

                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                Console.WriteLine($"Запись удалена из БД");
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }
        }



    }
 //---------------------------------------------------------------------------
    public class User : SQLRequests
    {
        public int Id { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserTypes UserType { get; set; }

        public User (int id, string username, string password, UserTypes userType)
        {
            Id = id;
            Username = username;
            Password = password;
            UserType = userType;
        }
        public User(string username, string password, UserTypes userType)
        {
            Username = username;
            Password = password;
            UserType = userType;
        }


        public override string ToString()
        {
            string type = "";

            switch (UserType)
            {
                case UserTypes.ADMIN:
                {
                    type = "Админ";
                    break;
                }
                case UserTypes.MANAGER:
                {
                    type = "менеджер";
                    break;
                }
                case UserTypes.UNKNOWN:
                {
                    type = "Неизвестен";
                    break;
                }
                default:
                {
                    type = "Ошибка в определении допуска пользователя";
                    break;
                }
            }
            return $"Id: {Id}, User name: {Username}  Тип допуска: {type}";
        }

        public static void GetInfo(List<User> userList)
        {
            sqlQuery = @"SELECT Users.Id, Users.UFullName, Users.UPassWord, Users.UType
                FROM Users";

            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = (int)reader.GetValue(0);
                    string fullName = (string)reader.GetValue(1);
                    string password = (string)reader.GetValue(2);
                    int typeInt = reader.IsDBNull(3) ? -1 : (int)reader.GetInt16(3);


                    UserTypes type = (UserTypes)typeInt;

                    userList.Add(new User(id, fullName, password, type));
                }
            }
            reader.Close();
            sqlConnection.Close();
        }

        public override void Insert()
        {

            sqlQuery =  $"INSERT INTO Users (UFullName, UPassWord, UType) " +
                        $"VALUES (@UFullName, @UPassWord, @UType); " +
                        $"SELECT CAST(SCOPE_IDENTITY() AS INT)";
            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parUFullName = new SqlParameter("@UFullName", Username);
                SqlParameter parUPassWord = new SqlParameter("@UPassWord", Password);
                SqlParameter parUType = new SqlParameter("@UType", UserType);
                cmd.Parameters.Add(parUFullName);
                cmd.Parameters.Add(parUPassWord);
                cmd.Parameters.Add(parUType);
                Id = (int)cmd.ExecuteScalar();;
                sqlConnection.Close();
                Console.WriteLine($"Запись добавлена в БД. Id: {Id}");
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }
        }

        public override void Update()
        {
            sqlQuery = "UPDATE Users SET UFullName = @parUFullName, UPassWord = @parUPassWord, " +
                        "UType = @UType WHERE Id = @parId";

            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parId = new SqlParameter("@parId", Id);
                SqlParameter parUFullName = new SqlParameter("@UFullName", Username);
                SqlParameter parUPassWord = new SqlParameter("@UPassWord", Password);
                SqlParameter parUType = new SqlParameter("@UType", UserType);
                cmd.Parameters.Add(parId);
                cmd.Parameters.Add(parUFullName);
                cmd.Parameters.Add(parUPassWord);
                cmd.Parameters.Add(parUType);


                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                Console.WriteLine($"Запись обновлена в БД");
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }
        }
        public override void Delete()
        {
            try
            {
                sqlQuery = "DELETE FROM Users WHERE Id = @parId";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);

                SqlParameter parId = new SqlParameter("@parId", Id);
                cmd.Parameters.Add(parId);

                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                Console.WriteLine($"Запись удалена из БД");
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }
        }
    }

}
