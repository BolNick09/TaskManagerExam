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
        private int id;
        private string name;
        private string address;
        private string cadastralNum;
        private List<int> tasksID;

        public TaskObject(int id, string name, string address, string cadastralNum/*, List<int> tasksId*/)//TODO - добавить получение ID тасков
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.cadastralNum = cadastralNum;
            //his.tasksID = tasksId;
        }

        //public override static void GetInfo(List<Object> itemList)//TODO - добавить получение ID тасков из ассоц таблицы
        //{
        //    sqlQuery = "SELECT * FROM TaskObjects";

        //    sqlConnection.Open();
        //    SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);

        //    SqlDataReader reader = cmd.ExecuteReader();

        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            int id = (int)reader.GetValue(0);
        //            string name = (string)reader.GetValue(1);
        //            string address = (string)reader.GetValue(2);
        //            string cadastralNum = (string)reader.GetValue(3);

        //            itemList.Add(new TaskObject(id, fullName, address, cadastralNum));
        //        }
        //    }
        //    reader.Close();
        //    sqlConnection.Close();
        //}

        public override void Insert() 
        {
            
            sqlQuery = $"INSERT INTO TaskObjects VALUES (@TOName, @TOAddress, @TOCadastralNumber)";
            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parName = new SqlParameter("@TOName", name);
                SqlParameter parAddress = new SqlParameter("@TOAddress", address);
                SqlParameter parCadastralNum = new SqlParameter("@TOCadastralNumber", cadastralNum);
                cmd.Parameters.Add(parName);
                cmd.Parameters.Add(parAddress);
                cmd.Parameters.Add(parCadastralNum);
                cmd.ExecuteNonQuery(); //update delete insert
                sqlConnection.Close();
                Console.WriteLine($"Запись добавлена в БД");
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Console.WriteLine(ex.ToString());
            }            
        }

        public override void Update()
        {
            sqlQuery = "UPDATE TaskObjects SET TOName = @parTOName, TOAddress = @parTOAddress, " +
                        "TOCadastralNumber = @parTOCadastralNumber WHERE Id = @parId";

            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parId = new SqlParameter("@parId", id);
                SqlParameter parName = new SqlParameter("@parTOName", name);
                SqlParameter parAddress = new SqlParameter("@parTOAddress", address);
                SqlParameter parCadastralNum = new SqlParameter("@parTOCadastralNumber", cadastralNum);
                cmd.Parameters.Add(parId);
                cmd.Parameters.Add(parName);
                cmd.Parameters.Add(parAddress);
                cmd.Parameters.Add(parCadastralNum);


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
                sqlQuery = "DELETE FROM TaskObjects WHERE Id = @parId";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);

                SqlParameter parId = new SqlParameter("@parId", id);
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
        private int id;
        private string taskDescription;
        private int userId;
        private int decisionId;

        public Task(int id, string taskDescription)
        {
            this.id = id;
            this.taskDescription = taskDescription;
        }

        public override void Insert()
        {

            sqlQuery = $"INSERT INTO Tasks VALUES (@TDescription, @T_FK_User_ID, @T_FK_Decision_Id)";
            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parDescription = new SqlParameter("@TDescription", taskDescription);
                SqlParameter parUserId = new SqlParameter("@T_FK_User_ID", userId);
                SqlParameter parDecisionId = new SqlParameter("@T_FK_Decision_Id", decisionId);
                cmd.Parameters.Add(parDescription);
                cmd.Parameters.Add(parUserId);
                cmd.Parameters.Add(parDecisionId);
                cmd.ExecuteNonQuery(); //update delete insert
                sqlConnection.Close();
                Console.WriteLine($"Запись добавлена в БД");
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
                SqlParameter parId = new SqlParameter("@parId", id);
                SqlParameter parDescription = new SqlParameter("@parTDescription", taskDescription);
                SqlParameter parUserId = new SqlParameter("@parT_FK_User_ID", userId);
                SqlParameter parDecisionId = new SqlParameter("@parT_FK_Decision_Id", decisionId);
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
                sqlQuery = "DELETE FROM Tasks WHERE Id = @parId";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);

                SqlParameter parId = new SqlParameter("@parId", id);
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
        private int id;
        private string decisionDescription;
        private DateTime startDate;
        private DateTime endDate;
        private Statuses status;

        public Decision(int id, string decisionDescription, DateTime startDate, DateTime endDate, Statuses status)
        {
            this.id = id;
            this.decisionDescription = decisionDescription;
            this.startDate = startDate;
            this.endDate = endDate;
            this.status = status;
        }

        public override void Insert()
        {

            sqlQuery = $"INSERT INTO Decisions VALUES (@DDescription, @DStartDate, @DEndDate, @DStatus)";
            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parDescription = new SqlParameter("@DDescription", decisionDescription);
                SqlParameter parStartDate = new SqlParameter("@DStartDate", startDate);
                SqlParameter parEndDate = new SqlParameter("@DEndDate", endDate);
                SqlParameter parStatus = new SqlParameter("@DStatus", status);
                cmd.Parameters.Add(parDescription);
                cmd.Parameters.Add(parStartDate);
                cmd.Parameters.Add(parEndDate);
                cmd.Parameters.Add(parStatus);
                cmd.ExecuteNonQuery(); //update delete insert
                sqlConnection.Close();
                Console.WriteLine($"Запись добавлена в БД");
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
                SqlParameter parId = new SqlParameter("@parId", id);
                SqlParameter parDescription = new SqlParameter("@parDDescription", decisionDescription);
                SqlParameter parStartDate = new SqlParameter("@parDStartDate", startDate);
                SqlParameter parEndDate = new SqlParameter("@parDEndDate", endDate);
                SqlParameter parStatus = new SqlParameter("@parDStatus", status);
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

                SqlParameter parId = new SqlParameter("@parId", id);
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
        private int id;
        private string username;
        private string password;
        private UserTypes userType;

        public User (int id, string username, string password, UserTypes userType)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.userType = userType;
        }

        public override void Insert()
        {

            sqlQuery = $"INSERT INTO Users VALUES (@UFullName, @UPassWord, @UType)";
            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
                SqlParameter parUFullName = new SqlParameter("@UFullName", username);
                SqlParameter parUPassWord = new SqlParameter("@UPassWord", password);
                SqlParameter parUType = new SqlParameter("@UType", userType);
                cmd.Parameters.Add(parUFullName);
                cmd.Parameters.Add(parUPassWord);
                cmd.Parameters.Add(parUType);
                cmd.ExecuteNonQuery(); //update delete insert
                sqlConnection.Close();
                Console.WriteLine($"Запись добавлена в БД");
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
                SqlParameter parId = new SqlParameter("@parId", id);
                SqlParameter parUFullName = new SqlParameter("@UFullName", username);
                SqlParameter parUPassWord = new SqlParameter("@UPassWord", password);
                SqlParameter parUType = new SqlParameter("@UType", userType);
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

                SqlParameter parId = new SqlParameter("@parId", id);
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
