
using OfficeSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;
using System.Net;

namespace OfficeSpace.Controllers
{
    public class BaseController : Controller
    {
        protected ILogService LogService;
        public BaseController(ILogService logService)
        {
            LogService = logService;
        }
    }
}