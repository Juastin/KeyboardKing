using Cryptography;
using DatabaseController;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Controller
{
    /// <summary>
    /// Class <c>RegisterController</c> handles and checks the user input from RegisterPage and LoginPage.
    /// </summary>
    public static class AuthenticationController
    {
        /// <summary>
        /// Private properties used for checking passwordrequirements. 
        /// </summary>
        private static readonly Regex _containNumber = new(@"[0-9]+");
        private static readonly Regex _containUpperCase = new(@"[A-Z]+");
        private static readonly Regex _containLowerCase = new(@"[a-z]+");
        private static readonly Regex _containMinLength8Char = new(@".{8,}");

        /// <summary>
        /// Checks if email is valid with EmailAddressAttribute.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

        public static User GetUserInfo(string email)
        {
            return DBQueries.GetUserInfo(email);
        }

        /// <summary>
        /// Checks if password is valid with Regex.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsPasswordValid(string password)
        {
            return _containNumber.IsMatch(password) && _containUpperCase.IsMatch(password) && _containLowerCase.IsMatch(password) && _containMinLength8Char.IsMatch(password);
        }

        /// <summary>
        /// Hash string password and returns encrypted. 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string[] HashPassword(string password)
        {
            string salt = Argon2.CreateSalt();
            string passHashed = Argon2.HashPassword(password, salt);
            return new string[] { passHashed, salt };
        }

        /// <summary>
        /// Add user in database with data from user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="hashedPassword"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static bool AddUser(string username, string email, string hashedPassword, string salt)
        {
            return DBQueries.AddUser(username, email, hashedPassword, salt);
        }

        /// <summary>
        /// Checks if password is correct.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordInput"></param>
        /// <returns></returns>
        public static bool VerifyPassword(User user, string passwordInput)
        {
            return Argon2.VerifyHash(passwordInput, user.Salt, user.Password);
        }



    }
}
