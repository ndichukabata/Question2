using Microsoft.VisualStudio.TestTools.UnitTesting;
using Question_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question_2.Tests
{
    [TestClass()]
    public class EmployeesTests
    {
        [TestMethod()]
        public void Employees_Test_Salaries_integer_numbers()
        {
            string csv = @"Emplyee4,Employee2,500
Employee3,Employee1,500T
Employee1,,1000
Employee5,Employee1,500
Employee6,Employee2,500
Employee2,Employee1,800";

            try
            {
                Employees employees = new Employees(csv);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "The salaries in the CSV should have valid integer numbers");

                return;

            }



            Assert.Fail("No Exeption was thrown");
        }
        [TestMethod()]
        public void Employees_Test_Report_More_Than_One_Manager()
        {
            string csv = @"Emplyee4,Employee2,500
Employee3,Employee1,500
Employee1,,1000
Employee5,Employee1,500
Employee6,Employee2,500
Employee6,Employee1,800";

            try
            {
                Employees employees = new Employees(csv);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "One employee should not report to more than one manager");

                return;

            }



            Assert.Fail("No Exeption was thrown");

            Assert.Fail();
        }
        [TestMethod()]
        public void Employees_Test_Circular_Reference()
        {
            string csv = @"Emplyee4,Employee2,500
Employee3,Employee1,500
Employee1,,1000
Employee5,Employee1,500
Employee6,Employee2,500
Employee2,Employee6,800";

            try
            {
                Employees employees = new Employees(csv);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Circular reference found, i.e. a first employee reporting to a second employee that is also under the first employee.");

                return;

            }



            Assert.Fail("No Exeption was thrown");

            Assert.Fail();
        }
        [TestMethod()]
        public void Employees_Test_Only_One_Ceo()
        {
            string csv = @"Employee4,Employee2,500
Employee3,Employee1,500
Employee1,,1000
Employee5,,500
Employee6,Employee2,500
Employee7,Employee1,800";

            try
            {
                Employees employees = new Employees(csv);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "The list should have only one employee with no manager");

                return;

            }



            Assert.Fail("No Exeption was thrown");

            Assert.Fail();
        }
        [TestMethod()]
        public void Employees_Test_All_Managers_Are_Employeee()
        {
            string csv = @"Employee4,Employee20,500
Employee3,Employee1,500
Employee1,,1000
Employee5,Employee1,500
Employee6,Employee2,500
Employee7,Employee10,800";

            try
            {
                Employees employees = new Employees(csv);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "All managers should be employees");

                return;

            }



            Assert.Fail("No Exeption was thrown");

            Assert.Fail();
        }
        [TestMethod()]
        public void Employees_Test_Salary_Budget()
        {

            string csv2 = @"Employee4,Employee2,500
Employee3,Employee1,500
Employee1,,1000
Employee5,Employee1,500
Employee6,Employee2,500
Employee2,Employee1,800";

            long expected = 3800;

            Employees employees = new Employees(csv2);

            long actual = employees.SalaryBudget("Employee1");


            Assert.AreEqual(expected, actual);
        }



    }
}