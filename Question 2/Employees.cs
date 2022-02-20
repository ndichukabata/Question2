using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question_2
{
    public  class Employees
    {
        private static string hierarchy = "";
 
        public Employees(string emp_hierarchy)
        {
            hierarchy = emp_hierarchy;

            List<EmployeeList> list = new List<EmployeeList>();
            if (String.IsNullOrEmpty(hierarchy))
                throw new ArgumentException("Employee hierarchy cannot be empty");

            char[] newline = new[] { '\r', '\n' };
            string[] hierarchylist = hierarchy.Split(newline, StringSplitOptions.RemoveEmptyEntries);
            char[] delimiter = new[] { ',' };

            // Chek if hierarchylist contains 3 columns. EmployeeId,ManagerId,Salary

            var data = hierarchylist.Where(x => x.Split(delimiter, StringSplitOptions.None).Length != 3);
            if (data.Count() == 0)
            {

                list = CreateList();
               
                //1. The salaries in the CSV are valid integer numbers.
                if (list.Where(x => int.TryParse(x.Salary.Trim(), out int n) == false).Count() > 0)
                {
                    throw new ArgumentException("The salaries in the CSV should have valid integer numbers");
                }

                // 2.One employee does not report to more than one manager.
                if (list.Where(e => e.ManagerId!="").GroupBy(x => x.EmployeeId).Select(n => new
                {
                    Employee = n.Key,
                    Count = n.Count()
                }).ToList().Where(x => x.Count > 1).Count() > 0)
                {
                    throw new ArgumentException("One employee should not report to more than one manager");
                }

                // 3.There is only one CEO, i.e.only one employee with no manager.

                if (list.Where(x => x.ManagerId == "").Count() > 1)
                {
                    throw new ArgumentException("The list should have only one employee with no manager");
                }

                // 4.There is no circular reference, i.e.a first employee reporting to a second employee that is also under the first employee.
                foreach (EmployeeList employee in list)
                {
                    var mngr = list.Where(x => x.EmployeeId == employee.ManagerId);
                    if (mngr.Where(x => x.ManagerId == employee.EmployeeId).Count() > 0)
                    {
                        throw new ArgumentException("Circular reference found, i.e. a first employee reporting to a second employee that is also under the first employee.");
                    }


                }

                //  5.There is no manager that is not an employee, i.e.all managers are also listed in the employee column.

                List<string> managers = list.Where(x => x.ManagerId != "").Select(x => x.ManagerId).Distinct().ToList();

                var employees = list.Select(x => x.EmployeeId).Distinct().ToList();
                var not_employees = managers.Except(employees);
                if (not_employees.Count() > 0)
                {
                    throw new ArgumentException("All managers should be employees");
                   
                }

            }
            else
            {
                throw new ArgumentException("Employee hierarchy Contains Invalid Columns");

            }



          
        }
         private List<EmployeeList> CreateList()
        {
            List<EmployeeList> list = new List<EmployeeList>();
           char[] newline = new[] { '\r', '\n' };
            char[] delimiter = new[] { ',' };
            string[] hierarchylist = hierarchy.Split(newline, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in hierarchylist)
            {

                EmployeeList employee = new EmployeeList
                {
                    EmployeeId = line.Split(delimiter, StringSplitOptions.None)[0],
                    ManagerId = line.Split(delimiter, StringSplitOptions.None)[1],
                    Salary = line.Split(delimiter, StringSplitOptions.None)[2]
                };

                list.Add(employee);
            }
            return list;
        }

        public  long SalaryBudget(string ManagerId)
        {
            List<EmployeeList> list = CreateList();

            long total_Salary = 0;


            long manager_Salary = Convert.ToInt32(list.Where(e => e.EmployeeId == ManagerId).FirstOrDefault().Salary);

            List<EmployeeList> manager_Employees = GetManagerEmployees(ManagerId, list).ToList();

            var manager_Employees_Salary = manager_Employees.Sum(x => Convert.ToInt32(x.Salary));

            total_Salary= manager_Salary + manager_Employees_Salary;


            return total_Salary;

        }
        static List<EmployeeList> GetManagerEmployees(string ManagerId, List<EmployeeList> list)
        {

            var result = list.Where(x => x.ManagerId == ManagerId).ToList();

            var data = new List<EmployeeList>();
            foreach (var employee in result)
            {
                data.AddRange(GetManagerEmployees(employee.EmployeeId, list));
            }
            var t = result.Union(data);

            return t.ToList();

        }


    }
     class EmployeeList
    {
        public string EmployeeId { get; set; }
        public string ManagerId { get; set; }
        public string Salary { get; set; }
    }
}
