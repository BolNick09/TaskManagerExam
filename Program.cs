using Microsoft.Data.SqlClient;



namespace TaskManagerExam
{
    public class Program
    {        
        static void Main(string[] args)
        {
            //List <TaskObject> tasks = new List <TaskObject>();
            //TaskObject.GetInfo(tasks);
            //Console.ReadKey();
            Manager manager = new Manager();
            //TaskObject task = new TaskObject("Task1", "Address1", "CadastralNum1");

            //task.Insert();

            manager.Menu.Navigate(manager.Menu.LTaskObjectMenuItems);

            //if (manager != null && manager.Menu != null)
            //{
            //    manager.Menu.AddItem(task);
            //    manager.Menu.PrintTaskObjectMenu();
            //}
            //else
            //{
            //    Console.WriteLine("Manager or Menu is null");
            //}

        }
    }
}
