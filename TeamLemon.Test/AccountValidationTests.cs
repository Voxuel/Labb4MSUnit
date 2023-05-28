using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamLemon.Controls;
using TeamLemon.Mock;
using TeamLemon.Models;

namespace TeamLemon.Test
{
    //Testclass for unit tests
    [TestClass]
    public class AccountValidationTests
    {
        [TestMethod]
        public void ValidateAmountToTransfer_WhenGivenRightAmount_ReturnTrue()
        {
            AccountManagement mock = new AccountManagement();
            var user = new User()
            {
                ID = 1004,
            };
            var amount = 103.0m;
            var fromacc = 1;

            var action = mock.ValidateAmount(user, amount, fromacc);

            Assert.IsTrue(action);
        }
        [TestMethod]
        public void ValidateAmountToTransfer_WhenGivenWrongAmount_ReturnFalse()
        {
            AccountManagement mock = new AccountManagement();
            var user = new User()
            {
                ID = 1004,
            };
            var amount = 19348445.94m;
            var fromacc = 1;

            var action = mock.ValidateAmount(user, amount, fromacc);

            Assert.IsFalse(action);
        }
        
        
        [DataRow("Leo","MTG")]
        [DataTestMethod]
        public void LoginValidation_WhenGivenRightUserNameAndPassword_ReturnTrue(string username,string pass)
        {
            LoginClass mock = new LoginClass();

            var action = mock.LoginValidation(username, pass,out User currentU, out Admin currentA);

            Assert.IsTrue(action);
        }
        
        [TestMethod]
        [DataRow("leo","mtG")]
        [DataRow("234242","2343251")]
        [DataRow("las",".......")]
        public void LoginValidation_WhenGivenWrongUsernameAndPassword_ReturnFalse(string username,string pass)
        {
            LoginClass mock = new LoginClass();

            var action = mock.LoginValidation(username, pass,out User currentU, out Admin currentA);

            Assert.IsFalse(action);
        }

        [DataRow(123, 432.0f)]
        [ExpectedException(typeof(ArgumentException),
            "Invalid type was passed to method")]
        [TestMethod]
        public void LoginValidation_WhenGivenWrongInputType_ShouldRaiseException(string username,string pass)
        {
            LoginClass mock = new LoginClass();

            var action = mock.LoginValidation(username, pass,out User currentU, out Admin currentA);

            Assert.Fail();
        }

        [TestMethod]
        public void LoginValidation_WhenGivenRightUsernameAndWrongPassword_ShouldReturnFalse()
        {
            LoginClass mock = new LoginClass();
            var username = "Leo";
            var pass = "erwwrwr";

            var action = mock.LoginValidation(username, pass,out User currentU, out Admin currentA);
            
            Assert.IsFalse(action);
        }
        
        [TestMethod]
        public void LoginValidation_WhenUserIsLockedOut_ReturnFalse()
        {
            User.AllUsers.Add(new User()
            {
                Name = "Lasse",
                Password = "Something",
                LockedUser = true
            });

            LoginClass mock = new LoginClass();

            var action = mock.LoginValidation("Lasse", "Something"
                ,out User currentU, out Admin currentA);

            Assert.IsFalse(action);
        }
        [TestMethod]
        public void CreateNewUser_ShouldAddNewUserToDataSource()
        {
            Admin mock = new Admin();
            
            var action = mock.CreateUser("Johanna","Secret",1);

            CollectionAssert.Contains(User.AllUsers,action);
        }

        [TestMethod]
        public void CreateNewUser_WhenGivenWrongAccountTypeChoice_ShouldReturnErrorMessage()
        {
            Admin mock = new Admin();
            var sw = new StringWriter();
            Console.SetOut(sw);
            string expected = "Wrong input, select 1 or 2";
            var action = mock.CreateUser("Johanna", "Something", 6);
            
            string result = sw.ToString();
            
            Assert.AreEqual(expected,result);
        }

        [TestMethod]
        [DataRow(123, 432.0f,"wrong")]
        [ExpectedException(typeof(ArgumentException),
            "wrong datatype in input System.ArgumentException: Object of type: INPUT," +
            "can not be converted to type: EXPECTED ")]
        public void CreateNewUser_WhenGivenWrongDataType_ShouldRaiseException(string username,string pass,int userchoice)
        {
            Admin mock = new Admin();
            Assert.ThrowsException<ArgumentException>(() => mock.CreateUser(username, pass, userchoice));
        }
    }
}
