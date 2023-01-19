using ApiTesting;
using ApiTestinng;
using ApiTestinng.DTO;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace ApiTest
{
    
    [TestClass]
    public class RegressionTest
    {
        public TestContext TestContext { get; set; }
        public int numericCode { get; private set; }

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            var dir = testContext.TestRunDirectory;
            Reporter.SetUpExtentReport("API Regression Test", "API Regression Test Report",dir);
        }


        [TestInitialize]
        public void SetupTest()
        {
            Reporter.CreateTest(TestContext.TestName);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            var testStatus = TestContext.CurrentTestOutcome;
            Status logStatus;

            switch (testStatus)
            {
                case UnitTestOutcome.Failed:
                    logStatus = Status.Fail;
                    Reporter.TestStatus(logStatus.ToString());
                    break;
                case UnitTestOutcome.Inconclusive:
                    break;
                case UnitTestOutcome.Passed:
                    break;
                case UnitTestOutcome.InProgress:
                    break;
                case UnitTestOutcome.Error:
                    break;
                case UnitTestOutcome.Timeout:
                    break;
                case UnitTestOutcome.Aborted:
                    break;
                case UnitTestOutcome.Unknown:
                    break;
                case UnitTestOutcome.NotRunnable:
                    break;
                default:
                    break;
            }
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            Reporter.FlushReport();
        }

        [TestMethod]
        public void VerifyListOfUsers()
        {
            var demo = new Demo();
            var user = demo.GetUsers("api/users?page=2");
            Assert.AreEqual(2, user.page);
            Reporter.LogToReport(Status.Pass, "Page number does not match");
            Assert.AreEqual("Michael", user.data[0].first_name);
            Reporter.LogToReport(Status.Fail, "User first name does not match");

        }

        [DeploymentItem("Test Data\\TestCase.csv"),
            DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\bin\\Debug\\TestCase.csv", "TestCase#csv",
            DataAccessMethod.Sequential)]
        [TestMethod]
        public void CreateNewUser()
        {
            var users = new CreateUserRequestDTO();
            users.name = TestContext.DataRow["name"].ToString();
            Reporter.LogToReport(Status.Info, "Test Data for name is " + users.name);
            users.job = TestContext.DataRow["job"].ToString();

            var demo = new Demo();
            var user = demo.CreateUser("api/users", users);
            Assert.AreEqual("Mike", user.name);
            Assert.AreEqual("Lead", user.job);
        }

        [TestMethod]
        public void UpdateNewUser()
        {
            string payload = @"{
                                 ""name"": ""morpheus"",
                                 ""job"": ""zion resident"",
                                 ""updatedAt"": ""2023-01-09T11:52:20.947Z""
                               }";
            var demo = new Demo();
            var user = demo.UpdateUser("api/users/2",payload);
            Assert.AreEqual("morpheus", user.name);
        }

        [TestMethod]
        public void VerifyDeleteUser()
        {
            var demo = new Demo();
            var user = demo.DeleteUser("api/users/2");
        }
    }
}

