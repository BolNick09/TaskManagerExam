using Azure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public class Task : SQLRequests
    {
        private int id;
        private string taskDescription;
        private string decisionDescription;
        private DateTime startDate;
        private DateTime endDate;
        private Statuses status;
        private int userId;

    }
    public class User : SQLRequests
    {
        private int id;
        private string username;
        private string password;
        private UserTypes userType;
    }

}
