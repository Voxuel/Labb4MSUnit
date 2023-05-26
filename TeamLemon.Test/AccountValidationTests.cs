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
        public void ValidateAccountNumber_WhenGivenInputAccountNumber_ReturnCorrect()
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
        [TestMethod]
        public void LoginValidation_WhenGivenRightPassword_ReturnTrue()
        {
            ValidationMock mock = new ValidationMock();
            var username = "Leo";
            var pass = "MTG";

            var action = mock.LoginValidation(username, pass);

            Assert.IsTrue(action);
        }
        [TestMethod]
        public void LoginValidation_WhenGivenWronguserNameAndPassword_ReturnFalse()
        {
            ValidationMock mock = new ValidationMock();
            var username = "ewrrw";
            var pass = "234";

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
    }
}
