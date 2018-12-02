using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _5thSemesterProject.Models
{
	public class CorrectOrder
	{
		public int OrderId { get; set; }

		public CorrectOrder(int OrderId)
		{
			this.OrderId = OrderId;
		}
	}
}