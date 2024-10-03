using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerExam
{
    public enum Statuses
    {
        NEW = 0,
        IN_PROGRESS,
        CLOSED        
    }
    public enum UserTypes
    {
        UNKNOWN = 0,
        MANAGER,
        ADMIN
    }
    public enum MenuTasks
    {
        UNKNOWN = 0,
        ADD_OBJECT,
        ADD_TASK,
        ADD_DECISION,
        ADD_USER,

        EDIT_OBJECT,
        EDIT_TASK,
        EDIT_DECISION,
        EDIT_USER,

        DELETE_OBJECT,
        DELETE_TASK,
        DELETE_DECISION,
        DELETE_USER,
        
        SHOW_ALL_OBJECTS,
        SHOW_ALL_TASKS,        
        SHOW_ALL_USERS,

        EXIT

    }
}
