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
    
    public partial class SpecialAgreement
    {
        public int special_agreement_id { get; set; }
        public int is_work_allowed_mon { get; set; }
        public int is_work_allowed_tue { get; set; }
        public int is_work_allowed_wed { get; set; }
        public int is_work_allowed_thu { get; set; }
        public int is_work_allowed_fri { get; set; }
        public int is_work_allowed_sat { get; set; }
        public int is_work_allowed_sun { get; set; }
        public int employee_id { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
