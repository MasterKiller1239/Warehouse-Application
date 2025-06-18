using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IMessageService
    {
        void ShowInfo(string message, string title = "Info");
        void ShowWarning(string message, string title = "Warning");
        void ShowError(string message, string title = "Error");
    }

}
