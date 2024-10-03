using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerExam
{
    public class Manager
    {
        private Menu menu;

        public Manager()
        {
            objects = new List<TaskObject>();
            tasks = new List<Task>();
            decisions = new List<Decision>();
            users = new List<User>();
            this.CurrentUser = new User(1, "Admin", "Admin", UserTypes.ADMIN);
            this.menu = new Menu(CurrentUser);
            addObjectsToMenu();
            addTasksToMenu();
            addDecisionsToMenu();
            addUsersToMenu();
        }

        public Menu Menu => this.menu;

        private List<TaskObject> objects;
        private List<Task> tasks;
        private List<Decision> decisions;
        private List<User> users;

        public User CurrentUser { get; set; }


        private void addObjectsToMenu()
        {
            TaskObject.GetInfo(objects);
            foreach (var obj in objects)
                menu.LTaskObjectMenuItems.Add(new MenuItem<TaskObject>(obj));

        }
        private void addTasksToMenu()
        {
            Task.GetInfo(tasks);
            foreach (var task in tasks)
                menu.LTaskMenuItems.Add(new MenuItem<Task>(task));
        }

        private void addDecisionsToMenu()
        {
            Decision.GetInfo(decisions);
            foreach (var decision in decisions)
                menu.LDecisionMenuItems.Add(new MenuItem<Decision>(decision));
        }

        private void addUsersToMenu()
        {
            User.GetInfo(users);
            foreach (var user in users)
                menu.LUserMenuItems.Add(new MenuItem<User>(user));
        }



    }
}
