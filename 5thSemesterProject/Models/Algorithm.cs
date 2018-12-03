using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _5thSemesterProject.Models
{
    public class Algorithm
    {
        // List containing the the final product of the algorithm
        public List<Schedule> FinalList { get => FinalList; set => FinalList = value; }

        // First method called from the SchedulesController
        public void GenerateSchedule(List<int> list, DateTime startDate, DateTime endDate) // List containing all employees of the same position
        {
            var firstDayOfSchedule = new DateTime(startDate.Year, startDate.Month, 1);
            var lastDayOfSchedule = firstDayOfSchedule.AddMonths(1).AddDays(-1); // if more than once month, replace firstDayOfScedule with endDate

        }



        public List<Employee> RandomizeList(int numOfEmployeeToReturn, List<Employee> list) {

        Random rnd = new Random();
        List<Employee> returnList = new List<Employee>();
        Employee employee = new Employee();

        //Create new list and populate with randomly selected employees
        for (int num = 0; num <= numOfEmployeeToReturn; num++) {

            var selectedEmployeeNum = rnd.Next(1, list.Count());
            returnList.Add(list[selectedEmployeeNum]);
            list.RemoveAt(selectedEmployeeNum);
        }
            return returnList;
        }
    }
}