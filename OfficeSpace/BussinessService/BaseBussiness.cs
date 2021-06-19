using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSpace.BussinessService
{
    public class BaseBussiness
    {
        protected ILogService LogService;
        public BaseBussiness(ILogService logService)
        {
            LogService = logService;
        }
    }
}