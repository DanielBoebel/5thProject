namespace _5thSemesterProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Schedule
    {
        public int schedule_id { get; set; }
        public int employee_id { get; set; }
        public int shift_id { get; set; }
        public DateTime date { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Shift Shift { get; set; }
    }
}
