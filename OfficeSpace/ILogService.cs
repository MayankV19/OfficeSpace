using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeSpace
{
   public interface ILogService
    {
         Logger LogWriter { get; set; }

    }
    public class LogService:ILogService
    {
        public LogService()
        {
            LogWriter = LogManager.GetCurrentClassLogger();

        }
        public Logger LogWriter { get; set; }

    }
}
