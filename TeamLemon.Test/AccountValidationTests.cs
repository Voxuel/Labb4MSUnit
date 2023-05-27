using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamLemon.Controls;
using TeamLemon.Mock;
using TeamLemon.Models;

namespace TeamLemon.Test
{
    [TestClass]
    public class AccountValidationTests
    {
        [TestMethod]
        public void ValidateAccountNumber_WhenGivenInputAccountNumber_ReturnCorrectAccountId()
        {
            ValidationMock mock = new ValidationMock();
            var accountNumber = "100401";

            var action = mock.ValidateAccountNumber("100401");

            Assert.AreEqual(1004, action);
        }
        [TestMethod]
        public void ValidateAmountToTransfer_WhenGivenRightAmount_ReturnTrue()
        {
            ValidationMock mock = new ValidationMock();
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
            ValidationMock mock = new ValidationMock();
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
            ValidationMock mock = new ValidationMock();

            var action = mock.LoginValidation(username, pass);

            Assert.IsTrue(action);
        }
        [TestMethod]
        [DataRow("leo","mtG")]
        [DataRow("234242","2343251")]
        [DataRow("las",".......")]

        public void LoginValidation_WhenGivenWrongUsernameAndPassword_ReturnFalse(string username,string pass)
        {
            ValidationMock mock = new ValidationMock();


            var action = mock.LoginValidation(username, pass);

            Assert.IsFalse(action);
        }

        [DataRow(123, 432.0f)]
        [ExpectedException(typeof(ArgumentException),
            "Invalid type was passed to method")]
        [TestMethod]
        public void LoginValidation_WhenGivenWrongInputType_ShouldRaiseException(string username,string pass)
        {
            ValidationMock mock = new ValidationMock();

            mock.LoginValidation(username, pass);

            Assert.Fail();
        }

        [TestMethod]
        public void LoginValidation_WhenGivenRightUsernameAndWrongPassword_ShouldReturnFalse()
        {
            ValidationMock mock = new ValidationMock();
            var username = "Leo";
            var pass = "erwwrwr";

            var action = mock.LoginValidation(username, pass);
            
            Assert.IsFalse(action);
        }
        
        [TestMethod]
        public void LoginValidation_WhenUserIsLockedOut_ReturnFalse()
        {
            ValidationMock mock = new ValidationMock();
            User.AllUsers.Add(new User()
            {
                Name = "Lasse",
                Password = "Something",
                LockedUser = true
            });

            var action = mock.LoginValidation("Lasse", "Something");

            Assert.IsFalse(action);
        }
        [TestMethod]
        public void CreateNewUser_ShouldAddNewUserToDataSource()
        {
            ValidationMock mock = new ValidationMock();

            
            var action = mock.CreateNewUser("Kalle","Secret",1);

            CollectionAssert.Contains(User.AllUsers,action);
        }

        [TestMethod]
        public void CreateNewUser_WhenGivenWrongAccountTypeChoice_ShouldReturnErrorMessage()
        {
            ValidationMock mock = new ValidationMock();
            var sw = new StringWriter();
            Console.SetOut(sw);
            string expected = "Wrong input, select 1 or 2";
            var action = mock.CreateNewUser("Johanna", "Something", 6);
            
            string result = sw.ToString();
            
            Assert.AreEqual(expected,result);
        }
    }
}
