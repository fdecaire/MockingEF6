using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DatabaseTestConsole
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestQuery()
        {
            var data = new List<account> 
            { 
                new account { username = "test",pass="testpass1" }, 
                new account { username = "ZZZ",pass="testpass2" }, 
                new account { username = "AAA",pass="testpass3" } 
            }.AsQueryable();

            var mockSet = new Mock<DbSet<account>>();
            mockSet.As<IQueryable<account>>().Setup(m => m.Provider)
                .Returns(data.Provider);
            mockSet.As<IQueryable<account>>().Setup(m => m.Expression)
                .Returns(data.Expression);
            mockSet.As<IQueryable<account>>().Setup(m => m.ElementType)
                .Returns(data.ElementType);
            mockSet.As<IQueryable<account>>().Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());

            var mockContext = new Mock<AccountingContext>();
            mockContext.Setup(c => c.accounts).Returns(mockSet.Object);

            UserRights rights = new UserRights(mockContext.Object);
            Assert.AreEqual("testpass1", rights.LookupPassword("test"),
                "password for account test is incorrect");
            Assert.AreEqual("testpass2", rights.LookupPassword("ZZZ"), 
                "password for account ZZZ is incorrect");
            Assert.AreEqual("testpass3", rights.LookupPassword("AAA"), 
                "password for account AAA is incorrect");
        }

        [TestMethod]
        public void TestNumber2()
        {
            var deptData = new List<department>
            {
                new department {name="Operations"},
                new department {name="Sales"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<department>>();
            mockSet.As<IQueryable<department>>().Setup(m => m.Provider).Returns(deptData.Provider);
            mockSet.As<IQueryable<department>>().Setup(m => m.Expression)
                .Returns(deptData.Expression);
            mockSet.As<IQueryable<department>>().Setup(m => m.ElementType)
                .Returns(deptData.ElementType);
            mockSet.As<IQueryable<department>>().Setup(m => m.GetEnumerator())
                .Returns(deptData.GetEnumerator());

            var mockContext = new Mock<DepartmentContext>();
            mockContext.Setup(c => c.departments).Returns(mockSet.Object);

            Departments dept = new Departments(mockContext.Object);
            List<string> nameList = dept.ReadDepartmentList();

            Assert.AreEqual(2, nameList.Count, "record count incorrect");
            Assert.AreEqual("Operations", nameList[0], "first record incorrect name");
            Assert.AreEqual("Sales", nameList[1], "second record incorrect name");
        }

        [TestMethod]
        public void TestTwoContexts()
        {
            //account table
            var accountData = new List<account> 
            { 
                new account { username = "test",pass="testpass1" }, 
                new account { username = "ZZZ",pass="testpass2" }, 
                new account { username = "AAA",pass="testpass3" } 
            }.AsQueryable();

            var accountMockSet = new Mock<DbSet<account>>();
            accountMockSet.As<IQueryable<account>>().Setup(m => m.Provider)
                .Returns(accountData.Provider);
            accountMockSet.As<IQueryable<account>>().Setup(m => m.Expression)
                .Returns(accountData.Expression);
            accountMockSet.As<IQueryable<account>>().Setup(m => m.ElementType)
                .Returns(accountData.ElementType);
            accountMockSet.As<IQueryable<account>>().Setup(m => m.GetEnumerator())
                .Returns(accountData.GetEnumerator());

            var accountMockContext = new Mock<AccountingContext>();
            accountMockContext.Setup(c => c.accounts).Returns(accountMockSet.Object);

            // department table
            var deptData = new List<department>
            {
                new department {name="Operations"},
                new department {name="Sales"}
            }.AsQueryable();

            var deptMockSet = new Mock<DbSet<department>>();
            deptMockSet.As<IQueryable<department>>().Setup(m => m.Provider).Returns(deptData.Provider);
            deptMockSet.As<IQueryable<department>>().Setup(m => m.Expression)
                .Returns(deptData.Expression);
            deptMockSet.As<IQueryable<department>>().Setup(m => m.ElementType)
                .Returns(deptData.ElementType);
            deptMockSet.As<IQueryable<department>>().Setup(m => m.GetEnumerator())
                .Returns(deptData.GetEnumerator());

            var deptMockContext = new Mock<DepartmentContext>();
            deptMockContext.Setup(c => c.departments).Returns(deptMockSet.Object);

            DepartmentsAndAccounts deptAndAccts = new DepartmentsAndAccounts(deptMockContext.Object, accountMockContext.Object);
            int total = deptAndAccts.TotalDeptAndAccountRecords();

            Assert.AreEqual(5, total, "total does not equal expected value");
        }

        [TestMethod]
        public void TestTwoTables()
        {
            var deptData = new List<department>
            {
                new department {id=1, name="Operations"},
                new department {id=2, name="Sales"}
            }.AsQueryable();

            var deptMockSet = new Mock<DbSet<department>>();
            deptMockSet.As<IQueryable<department>>().Setup(m => m.Provider).Returns(deptData.Provider);
            deptMockSet.As<IQueryable<department>>().Setup(m => m.Expression)
                .Returns(deptData.Expression);
            deptMockSet.As<IQueryable<department>>().Setup(m => m.ElementType)
                .Returns(deptData.ElementType);
            deptMockSet.As<IQueryable<department>>().Setup(m => m.GetEnumerator())
                .Returns(deptData.GetEnumerator());

            // department table
            var persData = new List<person>
            {
                new person {id=1, first="Joe",last="Smith",department=1},
                new person {id=2, first="Jane", last="Summers",department=1},
                new person {id=2, first="Bob", last="Anders",department=1},
            }.AsQueryable();

            var personMockSet = new Mock<DbSet<person>>();
            personMockSet.As<IQueryable<person>>().Setup(m => m.Provider)
                .Returns(persData.Provider);
            personMockSet.As<IQueryable<person>>().Setup(m => m.Expression)
                .Returns(persData.Expression);
            personMockSet.As<IQueryable<person>>().Setup(m => m.ElementType)
                .Returns(persData.ElementType);
            personMockSet.As<IQueryable<person>>().Setup(m => m.GetEnumerator())
                .Returns(persData.GetEnumerator());

            var mockContext = new Mock<DepartmentContext>();
            mockContext.Setup(c => c.departments).Returns(deptMockSet.Object);
            mockContext.Setup(c => c.people).Returns(personMockSet.Object);

            PersonnelPerDepartment persDept = new PersonnelPerDepartment(mockContext.Object);
            int total = persDept.TotalPersonnel();

            Assert.AreEqual(3, total);
        }

    }
}