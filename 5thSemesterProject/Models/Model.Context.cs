﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB5thSemesterEntities1 : DbContext
    {
        public DB5thSemesterEntities1()
            : base("name=DB5thSemesterEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<Shift> Shift { get; set; }
        public virtual DbSet<SpecialAgreement> SpecialAgreement { get; set; }
    }
}
