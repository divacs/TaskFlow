using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Utility.Interface
{
    public interface IProjectEndReminderJob
    {

        // send reminder to all project that ends in 5 days
        Task SendRemindersAsync();

        // send reminder for one project per id
        Task SendReminderAsync(int projectId);
    }
}
