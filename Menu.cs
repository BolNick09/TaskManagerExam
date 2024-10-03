using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaskManagerExam
{
    public class MenuItem<T>
    {
        public T MenuObj {get; set;}
        public bool IsVisible { get; set; }
        public bool IsSelected { get; set; }


        public MenuItem(T MenuObj)
        {
            this.MenuObj = MenuObj;
            IsSelected = false;
            IsVisible = true;
        }


        public void changeSelected()
        {
            IsSelected = !IsSelected;
        }

        public override string ToString()
        {
            return MenuObj.ToString();
        }
    }

    public class MenuTask
    {
        public string Name { get; set; }
        public MenuTasks Task { get; set; }
        public int Num { get; set; }

        public override string ToString()
        {
            return $"{Num}.{Name}";
        }
        public MenuTask (string name, MenuTasks task, int num)
        {
            Name = name;
            Task = task;
            Num = num;
        }
       
    }

    public class Menu
    {
        public List<MenuTask> LMenuTask { get; }

        public List<MenuItem<TaskObject>> LTaskObjectMenuItems { get; }
        public List<MenuItem<Task>> LTaskMenuItems { get; }
        public List<MenuItem<Decision>> LDecisionMenuItems { get; }
        public List<MenuItem<User>> LUserMenuItems { get; }

        public User CurrentUser { get; set; }

        public Menu(User currentUser)
        {
            LTaskObjectMenuItems = new List<MenuItem<TaskObject>>();
            LTaskMenuItems = new List<MenuItem<Task>>();
            LDecisionMenuItems = new List<MenuItem<Decision>>();
            LUserMenuItems = new List<MenuItem<User>>();
            LMenuTask = new List<MenuTask>();
            CurrentUser = currentUser;
        }

        public void AddItem(TaskObject item)
        {
            LTaskObjectMenuItems.Add(new MenuItem<TaskObject>(item));
        }

        public void AddItem(Task item)
        {
            LTaskMenuItems.Add(new MenuItem<Task>(item));
        }

        public void AddItem(Decision item)
        {
            LDecisionMenuItems.Add(new MenuItem<Decision>(item));
        }

        public void AddItem(User item)
        {
            LUserMenuItems.Add(new MenuItem<User>(item));
        }
        public void AddItem(MenuTask item)
        {
            LMenuTask.Add(item);
        }

        public void PrintTaskObjectMenu()
        {
            PrintMenu(LTaskObjectMenuItems);
        }
        public void PrintTaskMenu()
        {
            PrintMenu(LTaskMenuItems);
        }
        public void PrintDecisionMenu()
        {
            PrintMenu(LDecisionMenuItems);
        }
        public void PrintUserMenu()
        {
            PrintMenu(LUserMenuItems);
        }
        private void PrintMenu<T>(IEnumerable<MenuItem<T>> menuItems) where T : class
        {
            foreach (var menuItem in menuItems)
            {
                if (menuItem.IsVisible == false)
                    continue;
                if (menuItem.IsSelected)
                    Console.Write("->");
                Console.WriteLine("\t" + menuItem);
            }
        }
        public void PrintTasks()
        {
            foreach (var task in LMenuTask)
            {
                Console.WriteLine(task);
            }
        }
        public void Navigate<T>(IEnumerable<MenuItem<T>> menuItems) where T : class
        {   
            if (menuItems is List<MenuItem<TaskObject>>)
            {
                LMenuTask.Clear();
                LMenuTask.Add(new MenuTask("Добавить объект", MenuTasks.ADD_OBJECT, 1));
                LMenuTask.Add(new MenuTask("Изменить объект", MenuTasks.EDIT_OBJECT, 2));
                LMenuTask.Add(new MenuTask("Удалить объект", MenuTasks.DELETE_OBJECT, 3));
            }
            else if (menuItems is List<MenuItem<Task>>)
            {
                LMenuTask.Clear();
                LMenuTask.Add(new MenuTask("Добавить задачу", MenuTasks.ADD_TASK, 1));
                LMenuTask.Add(new MenuTask("Изменить задачу", MenuTasks.EDIT_TASK, 2));
                LMenuTask.Add(new MenuTask("Удалить задачу", MenuTasks.DELETE_TASK, 3));
            }
            else if (menuItems is List<MenuItem<Decision>>)
            {
                LMenuTask.Clear();
                LMenuTask.Add(new MenuTask("Добавить решение", MenuTasks.ADD_DECISION, 1));
                LMenuTask.Add(new MenuTask("Изменить решение", MenuTasks.EDIT_DECISION, 2));
                LMenuTask.Add(new MenuTask("Удалить решение", MenuTasks.DELETE_DECISION, 3));
            }
            else if (menuItems is List<MenuItem<User>>)
            {
                LMenuTask.Clear();
                LMenuTask.Add(new MenuTask("Добавить пользователя", MenuTasks.ADD_USER, 1));
                LMenuTask.Add(new MenuTask("Изменить пользователя", MenuTasks.EDIT_USER, 2));
                LMenuTask.Add(new MenuTask("Удалить пользователя", MenuTasks.DELETE_USER, 3));
            }
            else
                return;

            List<MenuItem<T>> menuItemsCopy = menuItems.ToList();
            bool toContinue = true;
            menuItemsCopy[0].changeSelected();
            int currentIndex = 0;
            while (toContinue)
            {
                Console.Clear();
                PrintMenu(menuItemsCopy);
                PrintTasks();
                Console.WriteLine("0.Выход");
                ConsoleKeyInfo key = Console.ReadKey(true);
                
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                    {
                        currentIndex = menuItemsCopy.FindIndex(item => item.IsSelected);
                        if (currentIndex < menuItemsCopy.Count - 1)
                        {
                            menuItemsCopy[currentIndex].changeSelected();
                            menuItemsCopy[currentIndex + 1].changeSelected();
                            currentIndex++;
                        }
                        break;
                    }
                    case ConsoleKey.UpArrow:
                    {
                        currentIndex = menuItemsCopy.FindIndex(item => item.IsSelected);
                        if (currentIndex > 0)
                        {
                            menuItemsCopy[currentIndex].changeSelected();
                            menuItemsCopy[currentIndex - 1].changeSelected();
                            currentIndex--;
                        }
                        break;
                    }
                    case ConsoleKey.D1:
                    {
                        execTask (LMenuTask[0], currentIndex);
                        //currentIndex = 0;
                        menuItemsCopy[currentIndex].IsSelected = true;
                        break;
                    }
                    case ConsoleKey.D2:
                    {

                        execTask(LMenuTask[1], currentIndex);
                        //currentIndex = 0;
                        menuItemsCopy[currentIndex].IsSelected = true;
                        break; 
                    }
                    case ConsoleKey.D3:
                    {
                        execTask(LMenuTask[2], currentIndex);
                        if (menuItemsCopy.Count != 0)
                        {
                            currentIndex = 0;
                            menuItemsCopy[currentIndex].IsSelected = true;
                        }
                        break; 
                    }
                    case ConsoleKey.D0:
                    {
                        toContinue = false;
                        break;
                    }          
                    
                }
                menuItemsCopy = menuItems.ToList();
                

            }
        }

        public bool Exit()
        {
            return true;
        }

        public void execTask(MenuTask task, int currentIndex)
        {
            switch (task.Task)
            {
                case MenuTasks.ADD_OBJECT:
                {
                    addObject();
                    break;
                }
                case MenuTasks.EDIT_OBJECT:
                {
                    editObject(LTaskObjectMenuItems[currentIndex]);
                    break;
                }
                case MenuTasks.DELETE_OBJECT:
                {
                    deleteObject(currentIndex);
                    break;
                }
                case MenuTasks.ADD_TASK:
                {
                    addTask(LTaskObjectMenuItems[currentIndex].MenuObj.Id);

                    break;
                }
                case MenuTasks.ADD_DECISION:
                {
                    AddDecision();
                    break;
                }
                case MenuTasks.ADD_USER:
                {
                    AddUser();
                    break;
                }
                default:
                {
                    Console.WriteLine("Unknown task");
                    break;
                }
            }
        }

        public void addObject()
        {
            Console.WriteLine("Введите название объекта");
            string name = Console.ReadLine();
            Console.WriteLine("Введите адрес");
            string address = Console.ReadLine();
            Console.WriteLine("Введите кадастровый номер");
            string cadNum = Console.ReadLine();
            TaskObject taskObject = new TaskObject(name, address, cadNum);            

            taskObject.Insert();
            AddItem(taskObject);

        }
        public void editObject(MenuItem<TaskObject> menuItem)
        {
            TaskObject taskObject = menuItem.MenuObj;
            Console.WriteLine(taskObject);
            Console.WriteLine("0 - Измнить объект, 1 - добавить задачу");
            int key = int.Parse(Console.ReadLine());
            if (key == 0)
            {
                Console.WriteLine("Введите новое название объекта");
                taskObject.Name = Console.ReadLine();
                Console.WriteLine("Введите новый адрес");
                taskObject.Address = Console.ReadLine();
                Console.WriteLine("Введите новый кадастровый номер");
                taskObject.CadastralNumber = Console.ReadLine();
                taskObject.Update();
                return;
            }
            else if (key == 1)
            {
                addTask(taskObject.Id);
            }           
        }
        public void deleteObject(int index)
        {
            LTaskObjectMenuItems[index].MenuObj.Delete();
            LTaskObjectMenuItems.RemoveAt(index);
        }
        public void addTask(int objectId)
        {
            Console.WriteLine("Введите описание задачи");
            string description = Console.ReadLine();
            Task task = new Task(description, CurrentUser.Id);
            
            task.Insert();
            task.СonnectToObject(objectId);
            AddItem(task);
        }
        public void AddDecision()
        {
            
            Console.WriteLine("Введите дату начала. Формат: гггг-мм-дд");
            string startDateInput = Console.ReadLine();
            DateTime startDate;
            if (!DateTime.TryParseExact(startDateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                Console.WriteLine("Неверный формат даты. Пожалуйста, повторите ввод в формате гггг-мм-дд");
                return;
            }

            Console.WriteLine("Введите дату конца. Формат: гггг-мм-дд");
            string endDateInput = Console.ReadLine();
            DateTime endDate;
            if (!DateTime.TryParseExact(endDateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                Console.WriteLine("Неверный формат даты. Пожалуйста, повторите ввод в формате гггг-мм-дд");
                return;
            }
            Console.WriteLine("Введите описание решения");
            string description = Console.ReadLine();

            Decision decision = new Decision (description, startDate, endDate, Statuses.NEW);
            
            decision.Insert();
            AddItem(decision);
        }
        public void AddUser()
        {
            Console.WriteLine("Введите имя пользователя");
            string name = Console.ReadLine();
            Console.WriteLine("Введите пароль пользователя");
            string password = Console.ReadLine();
            User user = new User (name, password, UserTypes.MANAGER);
            user.Insert();
            AddItem(user);
        }
    }
}
