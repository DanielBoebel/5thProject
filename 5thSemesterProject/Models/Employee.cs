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
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.SpecialAgreement = new HashSet<SpecialAgreement>();
            this.Schedule = new HashSet<Schedule>();
        }
    
        public int employee_id { get; set; }
        public string cpr { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int position_id { get; set; }
        public string initials { get; set; }
        public string password { get; set; }

        public int points { get; set; }
        public double totalHours { get; set; }
        public bool isEligible { get; set; }

        public Employee(int employee_id, int points, double totalHours, bool isEligible)
        {
            this.employee_id = employee_id;
            this.points = points;
            this.totalHours = totalHours;
            this.isEligible = isEligible;
        }

        public virtual Position Position { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecialAgreement> SpecialAgreement { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Schedule> Schedule { get; set; }
    }
}
