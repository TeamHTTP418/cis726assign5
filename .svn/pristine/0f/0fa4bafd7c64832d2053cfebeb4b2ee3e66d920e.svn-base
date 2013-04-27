using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using AuthParser.Models;
using System.Security.Cryptography;
using WebMatrix.Data;
using WebMatrix.WebData;
using MessageParser;

namespace CIS726_Assignment2.Repositories
{
    public class MSMQMembershipProvider : ExtendedMembershipProvider
    {

        private string applicationName = "CIS726";

        public override string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                applicationName = value;
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (ValidateUser(username, oldPassword))
            {
                User user = Request<User>.GetUserRoleByName(username, "A", "B");
                if (user != null)
                {
                    String passHash = Crypto.HashPassword(newPassword);
                    user.password = passHash;
                    return Request<User>.UpdateUserRole(user, user, "A", "B");
                }
            }
            return false;

        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
           if (GetUser(username, true) == null)
            {
                User user = new User()
                {
                    username = username,
                    password = Crypto.HashPassword(password),
                };
                Request<User>.AddUserRole(user, "A", "B");
                status = MembershipCreateStatus.Success;
                return GetUser(username, true);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {

            User user = Request<User>.GetUserRoleByName(username, "A", "B");
            if (user != null)
            {
                return Request<User>.DeleteUserRole(user.ID, "A", "B");
            }
            return false;
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }
        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            User user = Request<User>.GetUserRoleByName(username, "A", "B");
            if (user != null)
            {
                MembershipUser memUser = new MembershipUser("SimpleMembershipProvider", user.username, user.ID, string.Empty, string.Empty, string.Empty, true, false, DateTime.MinValue, DateTime.Now, DateTime.MinValue, DateTime.Now, DateTime.Now);
                return memUser;
            }
            return null;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            int userID = (int)providerUserKey;
            User user = Request<User>.GetUserByID(userID, "A", "B");
            if (user != null)
            {
                MembershipUser memUser = new MembershipUser("SimpleMembershipProvider", user.username, user.ID, string.Empty, string.Empty, string.Empty, true, false, DateTime.MinValue, DateTime.Now, DateTime.MinValue, DateTime.Now, DateTime.Now);
                return memUser;
            }
            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }
        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }
        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            User user = Request<User>.GetUserRoleByName(username, "A", "B");
            if (user != null)
            {
                return Crypto.VerifyHashedPassword(user.password, password);
            }
            return false;
        }

        public override bool ConfirmAccount(string accountConfirmationToken)
        {
            throw new NotImplementedException();
        }
        public override bool ConfirmAccount(string userName, string accountConfirmationToken)
        {
            throw new NotImplementedException();
        }
        public override string CreateAccount(string userName, string password, bool requireConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override string CreateUserAndAccount(string userName, string password, bool requireConfirmation, IDictionary<string, object> values)
        {
            if (GetUser(userName, true) == null)
            {
                User user = new User()
                {
                    username = userName,
                    password = Crypto.HashPassword(password),
                    realName = values["realName"] as string
                };
                Request<User>.AddUserRole(user, "A", "B");
                return userName;
            }
            return userName;
        }

        public override bool DeleteAccount(string userName)
        {
            return DeleteUser(userName, true);
        }

        public override string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow)
        {
            throw new NotImplementedException();
        }

        public override ICollection<OAuthAccountData> GetAccountsForUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetCreateDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetLastPasswordFailureDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetPasswordChangedDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override int GetPasswordFailuresSinceLastSuccess(string userName)
        {
            throw new NotImplementedException();
        }

        public override int GetUserIdFromPasswordResetToken(string token)
        {
            throw new NotImplementedException();
        }

        public override bool IsConfirmed(string userName)
        {
            throw new NotImplementedException();
        }

        public override bool ResetPasswordWithToken(string token, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}