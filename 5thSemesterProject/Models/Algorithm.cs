using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _5thSemesterProject.Models
{
    public class Algorithm
    {

        public List<Employee> randomizeList(int numOfEmployeeToReturn, List<Employee> list) {

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