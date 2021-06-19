using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSpace.Repository
{
    public class BaseRepository
    {
        protected ILogService LogService;
        public BaseRepository(ILogService logService)
        {
            LogService = logService;
        }
    }
}