//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _5thSemesterProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Schedule
    {
        public int schedule_id { get; set; }
        public int employee_id { get; set; }
        public int shift_id { get; set; }
        public string date { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Shift Shift { get; set; }

        public Schedule() { }

        public Schedule(int employee_id, int shift_id, string date)
        {
            this.employee_id = employee_id;
            this.shift_id = shift_id;
            this.date = date;
        }
    }
}
