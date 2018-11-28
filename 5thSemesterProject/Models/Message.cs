namespace _5thSemesterProject.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Message
    {

        public int message_id { get; set; }
        public int reciever_id { get; set; }
        public string date { get; set; }
        public string content { get; set; }
        public int sender_id { get; set; }

        public Message()
        {

        }

        public Message(int sender_id, int reciever_id, string date, string content)
        {
            this.sender_id = sender_id;
            this.reciever_id = reciever_id;
            this.date = date;
            this.content = content;
        }
    }
}