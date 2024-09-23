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
}
