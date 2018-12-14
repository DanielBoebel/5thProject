using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _5thSemesterProject.Models
{
    public class HomepageViewModel
    {
        public Employee employee { get; set; }
        public List<Message> adminmessageList { get; set; }

    }
}