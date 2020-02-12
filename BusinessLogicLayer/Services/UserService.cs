using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer;
using DataAccessLayer.IRepositories;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// Service for User, include all methods needed to work with User storage.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ApplicationDbContext Database;
        public UserService(IUserRepository _userRepository, ApplicationDbContext _Database)
        {
            userRepository = _userRepository;
            Database = _Database;
        }

        /// <summary>
        /// Check the user permission
        /// </summary>
        /// <param name="email">Should not be null</param>
        /// <param name="password">Should not be null</param>
        /// <returns> User model</returns>
        public User Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = Database.Users.FirstOrDefault(x => x.Email == email);

            // check if email exists     
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        /// <summary>
        /// Check the user permission and if It's correct add to DB
        /// </summary>
        /// <param name="user">Should not be null</param>
        /// <param name="password">Should not be null</param>
        /// <returns> User model</returns>
        public User CreateUser (User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (Database.Users.Any(x => x.Name == user.Name))
                throw new AppException($"Username { user.Name } is already taken");

            if (Database.Users.Any(x => x.Email == user.Email))
                throw new AppException($"Username {user.Email}  is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            userRepository.Insert(user);
            return user;
        }
        /// <summary>
        /// Check the user permission and if It's correct Update in DB
        /// </summary>
        /// <param name="userParam">Should not be null, if It's null we return exception</param>
        /// <param name="password">Should not be null</param>
        /// <returns> User model</returns>
        public void UpdateUser(User userParam, string password = null)
        {
            var user = userRepository.GetById(userParam.Id);

            if (user == null)
                throw new Exception("User not found");

            // update Email if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Email) && userParam.Email != user.Email)
            {
                // throw error if the new Email is already taken
                if (Database.Users.Any(x => x.Email == userParam.Email))
                    throw new AppException($"Username {userParam.Email} is already taken");

                user.Email = userParam.Email;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.Name))
                user.Name = userParam.Name;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            userRepository.Update(user);

        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        public bool Exist(string email) 
        {
           var result = userRepository.Exist(email);
           return result;
        }
    }
}
