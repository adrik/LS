using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using WebMatrix.WebData;
using System.Text.RegularExpressions;
using MyMvc.Models.DB;

namespace MyMvc.Models
{
    public class MyMembershipProvider : ExtendedMembershipProvider
    {

        public override bool ConfirmAccount(string accountConfirmationToken)
        {
            throw new NotSupportedException();
        }

        public override bool ConfirmAccount(string login, string accountConfirmationToken)
        {
            throw new NotSupportedException();
        }

        public override string CreateAccount(string login, string password, bool requireConfirmationToken)
        {
            throw new NotSupportedException();
        }

        public override string CreateUserAndAccount(string login, string password, bool requireConfirmation, IDictionary<string, object> values)
        {
            throw new NotSupportedException();
        }

        public override bool DeleteAccount(string login)
        {
            throw new NotSupportedException();
        }

        public override string GeneratePasswordResetToken(string login, int tokenExpirationInMinutesFromNow)
        {
            throw new NotSupportedException();
        }

        public override ICollection<OAuthAccountData> GetAccountsForUser(string login)
        {
            throw new NotSupportedException();
        }

        public override DateTime GetCreateDate(string login)
        {
            throw new NotSupportedException();
        }

        public override DateTime GetLastPasswordFailureDate(string login)
        {
            throw new NotSupportedException();
        }

        public override DateTime GetPasswordChangedDate(string login)
        {
            throw new NotSupportedException();
        }

        public override int GetPasswordFailuresSinceLastSuccess(string login)
        {
            throw new NotSupportedException();
        }

        public override int GetUserIdFromPasswordResetToken(string token)
        {
            throw new NotSupportedException();
        }

        public override bool IsConfirmed(string login)
        {
            return true;
        }

        public override bool ResetPasswordWithToken(string token, string newPassword)
        {
            throw new NotSupportedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override bool ChangePassword(string login, string oldPassword, string newPassword)
        {
            throw new NotSupportedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string login, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser CreateUser(string login, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotSupportedException();
        }

        public override bool DeleteUser(string login, bool deleteAllRelatedData)
        {
            throw new NotSupportedException();
        }

        public override bool EnablePasswordReset
        {
            get { return false; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByName(string loginToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotSupportedException();
        }

        public override string GetPassword(string login, string answer)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser GetUser(string login, bool userIsOnline)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Parameter can not be null or empty", "login");
            }

            DbUser user = ModelContext.Instance.FindUserByLogin(login);
            if (user != null)
            {
                return new MembershipUser(Membership.Provider.Name, login, user.Id, login, null, null, true, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
            }
            return null;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotSupportedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotSupportedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 0x7fffffff; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 0; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 0x7fffffff; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return string.Empty; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        public override string ResetPassword(string login, string answer)
        {
            throw new NotSupportedException();
        }

        public override bool UnlockUser(string login)
        {
            throw new NotSupportedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        public override bool ValidateUser(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Parameter can not be null or empty", "login");
            }

            return ModelContext.Instance.FindUserByLogin(login) != null;
        }

        public override int GetUserIdFromOAuth(string provider, string providerUserId)
        {
            return -1;
        }
    }
}