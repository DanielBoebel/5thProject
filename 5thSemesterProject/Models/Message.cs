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
    
    public partial class Message
    {
        public int message_id { get; set; }
        public string sender { get; set; }
        public string reciever { get; set; }
        public int reciever_id { get; set; }
        public System.DateTime date { get; set; }
        public string content { get; set; }
    }
}
