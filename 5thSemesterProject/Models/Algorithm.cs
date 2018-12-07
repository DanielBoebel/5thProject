using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _5thSemesterProject.Models
{
    public class Algorithm
    {
        private DB5thSemesterEntities1 db = new DB5thSemesterEntities1();

        public int numberOfEmployeesNight = 4;
        public int numberOfEMplyeesDay = 25;

        public DateTime firstDayOfSchedule;
        public DateTime lastDayOfSchedule;

        // List containing the the final product of the algorithm
        public List<List<Schedule>> FinalList = new List<List<Schedule>>();

        // First method called from the SchedulesController
        public void GenerateSchedule(DateTime startDate, DateTime endDate) 
        {
            // List containing all employees of the same position
            var employees = db.Employee.Where(e => e.Position.name.Contains("Obstetriker")).Select(e => e.employee_id).ToList();

            // List containing all shift types
            var dbShiftTypes = db.Shift.ToList();

            // Converts list of employeeIDs to list of Employee objects (ID, isEligeble, points, totalHours)
            List<Employee> allEmployees = GenerateEmployeeElements(employees);

            // Gets all dates of current schedule
            List<DateTime> dates = GetDatesOfSchedule(startDate, endDate);

            foreach (var day in dates)
            {
                // Finds the day (weekday, weekend or holiday)
                string dayOfWeek = day.DayOfWeek.ToString();
                string shiftType = GetDayType(dayOfWeek);

                // Set number of employees to work depending on weekday and shifttype
                //SetNumberOfWorkingEmployees();

                // Finds all eligeble employees for the specific day
                List<Employee> unsortedEligibleEmployees = findEligebleEmployees(allEmployees);

                // Sorts all eligeble employees by point, highest to lowest
                List<Employee> eligebleEmployees = unsortedEligibleEmployees.OrderBy(o => o.points).ToList();

                // Finds the eligeble employees with the lowest amount of points (night shift first) 
                List<Employee> employeesNightShift = findEligebles(eligebleEmployees, numberOfEmployeesNight);

                // Updates points, hours and isEligeble in allEmployees
                UpdateEmployeesForAllEmployees(allEmployees, employeesNightShift, dbShiftTypes, shiftType+"Night");

                //remove all employees selected for nightshift from  eligebleEmployees
                RemoveNightShiftFromEligible(employeesNightShift, eligebleEmployees);

                // Finds the eligeble employees with the lowest amount of points 
                List<Employee> employeesDayShift = findEligebles(eligebleEmployees, numberOfEMplyeesDay);

                // Updates points, hours and isEligeble in allEmployees
                UpdateEmployeesForAllEmployees(allEmployees, employeesDayShift, dbShiftTypes, shiftType + "Day");

                // Populate FinalList with type List<Schedule> (Night)
                List<Schedule> finalNightEmployeeList = GenerateDaySchedule(day, dbShiftTypes, employeesNightShift, "Night");

                // Populate FinalList with type List<Schedule> (Day)
                List<Schedule> finalDayEmployeeList = GenerateDaySchedule(day, dbShiftTypes, employeesDayShift, "Day");

                PopulateFinalList(finalNightEmployeeList, finalDayEmployeeList);
            }

            // Adds to Schedule in Database
            AddToDatabase(FinalList);

            foreach (var item in allEmployees)
            {
                Console.WriteLine("HELLO");
            }
        }

        public List<DateTime> GetDatesOfSchedule(DateTime startDate, DateTime endDate)
        {
            List<DateTime> dates = new List<DateTime>();
            firstDayOfSchedule = new DateTime(startDate.Year, startDate.Month, 1);
            lastDayOfSchedule = firstDayOfSchedule.AddMonths(1).AddDays(-1);
            int totalDays = Convert.ToInt32((lastDayOfSchedule - firstDayOfSchedule).TotalDays + 1);

            for (int i = 0; i < totalDays; i++)
            {
                dates.Add(firstDayOfSchedule.AddDays(i));
            }
            return dates;
        }

        public string GetDayType(string dayOfWeek)
        {
            string shiftType = "";
            switch (dayOfWeek)
            {
                case "Monday": case "Tuesday": case "Wednesday": case "Thursday": 
                    shiftType = "Weekday";
                    break;
                case "Friday":
                    shiftType = "Friday";
                    break;
                case "Saturday": case "Sunday":
                    shiftType = "Weekend";
                    break;
            }
            return shiftType;
        }

        // Creates a List of Employees with (ID, isEligeble, points, totalHours)
        public List<Employee> GenerateEmployeeElements(List<int> employeeID)
        {
            List<Employee> employeeObjects = new List<Employee>();
            Random rnd = new Random();

            foreach (var item in employeeID)
            {
                //int random = rnd.Next(0, 10);
                Employee employee = new Employee(item, 0, 0.0, true);
                employeeObjects.Add(employee);
            }
            return employeeObjects;
        }

        // Removes all employees who is not eligeble to work
        public List<Employee> findEligebleEmployees(List<Employee> allEmployees)
        {
            List<Employee> tempList = new List<Employee>(allEmployees);

            foreach (var item in allEmployees)
            {
                if (!item.isEligible) tempList.Remove(item);
            }
            return tempList;
        }

        // Returns list of the decided number of employees with the lowest points
        public List<Employee> findEligebles(List<Employee> eligibleEmployees, int numberOfEmployees)
        {
            List<Employee> eligebles = new List<Employee>();
            int temp = eligibleEmployees[0].points; // lowest value
            // Populate nightShiftEligebles with employees
            foreach (var item in eligibleEmployees)
            {
                if (item.points == temp)
                {
                    eligebles.Add(item);
                }
                else
                {
                    if (eligebles.Count < numberOfEmployees)
                    {
                        eligebles.Add(item);
                        temp = item.points;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // If list contains more than the desired amount emplooyes
            if (eligebles.Count > numberOfEmployees)
            {
                List<Employee> highestPointsToBeRandomized = new List<Employee>();
                List<Employee> tempList = new List<Employee>(eligebles);
                int highestPoint = eligebles[numberOfEmployees - 1].points;
                int count = 0;
                foreach (var item in tempList)
                {
                    if (item.points == highestPoint)
                    {
                        highestPointsToBeRandomized.Add(item);
                        eligebles.Remove(item);
                    } else
                    {
                        count++;
                    }
                }
                // Randomizes remaining spots in nightShiftEligebles
                List<Employee> randomizedList = RandomizeList(numberOfEmployees - count, highestPointsToBeRandomized);

                foreach (var item in randomizedList)
                {
                    eligebles.Add(item);
                }
            }
            return eligebles;
        }

        public List<Employee> RandomizeList(int numOfEmployeeToReturn, List<Employee> list) {

        Random rnd = new Random();
        List<Employee> returnList = new List<Employee>();
        Employee employee = new Employee();

        //Create new list and populate with randomly selected employees
        for (int num = 0; num < numOfEmployeeToReturn; num++) {
            var selectedEmployeeNum = rnd.Next(0, list.Count-1);
            returnList.Add(list[selectedEmployeeNum]);
            list.RemoveAt(selectedEmployeeNum);
        }
            return returnList;
        }

        public void UpdateEmployeesForAllEmployees(List<Employee> allEmployees, List<Employee> list, List<Shift> shifts, string shiftType)
        {
            // Set all employees isEligible variable to true
            if (shiftType.Contains("Night")) {
                foreach (var item in allEmployees)
                {
                    item.isEligible = true;
                }
            }

            // Updates the employees variables
            foreach (var item in list)
            {
               var element = allEmployees.Find(e => e.employee_id == item.employee_id);
                switch (shiftType)
                {
                    case "WeekdayNight":
                        Shift s1 = shifts.Find(e => e.name.Equals("Aftenvagt"));
                        element.points += s1.point;
                        element.isEligible = false;
                        element.totalHours += GetHoursOfShift(s1.start_time, s1.end_time);
                        break;
                    case "WeekdayDay":
                    case "FridayDay":
                        Shift s2 = shifts.Find(e => e.name.Equals("Dagvagt"));
                        element.points += s2.point;
                        element.totalHours += GetHoursOfShift(s2.start_time, s2.end_time);
                        break;
                    case "WeekendDay":
                        Shift s3 = shifts.Find(e => e.name.Equals("wDagvagt"));
                        element.points += s3.point;
                        element.totalHours += GetHoursOfShift(s3.start_time, s3.end_time);
                        break;
                    case "WeekendNight":
                    case "FridayNight":
                        Shift s4 = shifts.Find(e => e.name.Equals("wAftenvagt"));
                        element.points += s4.point;
                        element.isEligible = false;
                        element.totalHours += GetHoursOfShift(s4.start_time, s4.end_time);
                        break;
                }
            }
        }

        // Converst start and end time to a double
        public double GetHoursOfShift(string start, string end)
        {            
            double startTime = Convert.ToDouble(start) / 100;
            double endTime = Convert.ToDouble(end) / 100;

            double time = 0.0;

            // night shift
            if (startTime > endTime)
            {
                time = 24 - (startTime - endTime);
            } else if (startTime < endTime)
            {
                time = endTime - startTime;
            } else
            {
                time = 24;
            }

            return time;
        }


        // Remove selected employee (for night shift) from eligible employees
        public void RemoveNightShiftFromEligible(List<Employee> nighShifts, List<Employee> eligible) {

            List<Employee> tempList = new List<Employee>(eligible);
            foreach (var item in tempList)
            {
                if (nighShifts.Find(e => e.employee_id == item.employee_id) != null) {
                    eligible.Remove(item);    
                } 

            }
        }

        // Generates schedule for 1 type of shift and return a list
        public List<Schedule> GenerateDaySchedule(DateTime day, List<Shift> shiftType, List<Employee> employees, string timeOfDay)
        {
            string date = day.Date.ToString("dd-MM-yyyy");
            int shiftId = GetShiftId(day, shiftType, timeOfDay);
            List<Schedule> tempDaySchedule = new List<Schedule>();
            foreach (var employee in employees)
            {
                Schedule tempSchedule = new Schedule(employee.employee_id, shiftId, date);
                tempDaySchedule.Add(tempSchedule);
            }
            return tempDaySchedule;
        }

        // Finds the desired shift ID
        public int GetShiftId(DateTime day, List<Shift> shifts, string timeOfDay)
        {
            int id = 0;
            switch(day.DayOfWeek.ToString())
            {
                case "Monday":
                case "Tuesday":
                case "Wednesday":
                case "Thursday":
                    if (timeOfDay.ToLower().Contains("day"))
                    {
                        Shift s1 = shifts.Find(e => e.name.Equals("Dagvagt"));
                        id = s1.shift_id;
                    } else
                    {
                        Shift s2 = shifts.Find(e => e.name.Equals("Aftenvagt"));
                        id = s2.shift_id;
                    }
                    break;
                case "Friday":
                    if (timeOfDay.ToLower().Contains("day"))
                    {
                        Shift s1 = shifts.Find(e => e.name.Equals("Dagvagt"));
                        id = s1.shift_id;
                    }
                    else
                    {
                        Shift s2 = shifts.Find(e => e.name.Equals("wAftenvagt"));
                        id = s2.shift_id;
                    }
                    break;
                case "Saturday":
                case "Sunday":
                    if (timeOfDay.ToLower().Contains("day"))
                    {
                        Shift s1 = shifts.Find(e => e.name.Equals("wDagvagt"));
                        id = s1.shift_id;
                    }
                    else
                    {
                        Shift s2 = shifts.Find(e => e.name.Equals("wAftenvagt"));
                        id = s2.shift_id;
                    }
                    break;
            }
            return id;
        }

        // Adds to FinalList which will be saved in the database
        public void PopulateFinalList(List<Schedule> nightList, List<Schedule> dayList)
        {
            foreach (var schedule in nightList)
            {
                dayList.Add(schedule);
            }
            FinalList.Add(dayList);
        }

        // Save to Database
        public void AddToDatabase(List<List<Schedule>> schedules)
        {
            foreach (var scheduleList in schedules)
            {
                foreach (var schedule in scheduleList)
                {
                    db.Schedule.Add(schedule);
                }
            }

            db.SaveChanges();
        }

    }
}