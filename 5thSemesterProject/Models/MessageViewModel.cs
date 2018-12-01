using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _5thSemesterProject.Models
{
	public class MessageViewModel
	{
		public List<Employee> employeeList { get; set; }
		public List<Message> messageSenderList { get; set; }
		public List<Message> messageRecieverList { get; set; }
		public List<CorrectOrder> correctOrderList { get; set; }
	}
}