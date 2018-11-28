namespace _5thSemesterProject.Models
{
<<<<<<< HEAD
	using System;
	using System.Collections.Generic;

	public partial class Log
	{


		public Log()
		{

		}

		public Log(string employee_name, string action, DateTime timestamp)
		{
			this.employee_name = employee_name;
			this.action = action;
			this.timestamp = timestamp;
		}

		public int log_id { get; set; }
		public int employee_id { get; set; }
		public string employee_name { get; set; }
		public string action { get; set; }
		public System.DateTime timestamp { get; set; }
	}
}
=======
    using System;
    using System.Collections.Generic;
    
    public partial class Log
    {
        public int log_id { get; set; }
        public int employee_id { get; set; }
        public string employee_name { get; set; }
        public string action { get; set; }
        public System.DateTime timestamp { get; set; }
    }
}
>>>>>>> CaspersHomeBranch
