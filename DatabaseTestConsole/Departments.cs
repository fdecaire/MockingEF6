using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseTestConsole
{
    public class Departments
    {
        private DepartmentContext _context;

        public Departments(DepartmentContext context)
        {
            _context = context;
        }

        public List<string> ReadDepartmentList()
        {
            List<string> result = new List<string>();

            var query = (from dept in _context.departments select dept.name).ToList();

            foreach (var item in query)
            {
                result.Add(item);
            }

            return result;
        }
    }

    public class DepartmentsAndAccounts
    {
        private DepartmentContext _DeptContext;
        private AccountingContext _AccountContext;

        public DepartmentsAndAccounts(DepartmentContext deptContext, AccountingContext accountContext)
        {
            _DeptContext = deptContext;
            _AccountContext = accountContext;
        }

        public int TotalDeptAndAccountRecords()
        {
            var deptQuery = (from dept in _DeptContext.departments select dept.name).ToList();

            var accountQuery = (from acct in _AccountContext.accounts select acct.username).ToList();

            int result = deptQuery.Count() + accountQuery.Count();

            return result;
        }
    }

    public class PersonnelPerDepartment
    {
        private DepartmentContext _DeptContext;

        public PersonnelPerDepartment()
        {
            _DeptContext = new DepartmentContext();
        }

        ~PersonnelPerDepartment()
        {
            if (_DeptContext is IDisposable)
            {
                ((IDisposable)_DeptContext).Dispose();
            }
        }

        public PersonnelPerDepartment(DepartmentContext deptContext)
        {
            _DeptContext = deptContext;
        }

        public int TotalPersonnel()
        {
            var personnelDeptQuery = (
                from d in _DeptContext.departments
                join p in _DeptContext.people on d.id equals p.department
                select p).ToList();

            return personnelDeptQuery.Count();
        }
    }

    public class SecondObject
    {
        ~SecondObject()
        {
            // dispose allocated stuff here
        }

        public void SomeMethod()
        {
        }
    }

    public class FirstObject
    {
        private SecondObject _secondObject;

        public FirstObject()
        {
            _secondObject = new SecondObject();
        }

        public FirstObject(SecondObject secondObject)
        {
            _secondObject = secondObject;
        }

        ~FirstObject()
        {
            if (_secondObject is IDisposable)
            {
                ((IDisposable)_secondObject).Dispose();
            }
        }

        public void FirstObjectMethod()
        {
            _secondObject.SomeMethod();
        }
    }
}
