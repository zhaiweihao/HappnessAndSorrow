using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreTest_1010.Controllers
{
    public class ValuesController : Controller
    {
        // GET: /<controller>/
        public string Test()
        {
            string agent = Request.Headers["User-Agent"].ToString();
            return agent;
        }
    }
}
