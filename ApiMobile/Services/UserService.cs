using System.Collections.Generic;
using System.Linq;
using ApiMobile.Models;
using ApiMobile.Helpers;

namespace ApiMobile.Services
{
    public interface IUserService
    {
        Users Authenticate( string email, string password);
        Users GetById(int id);
        void Delete(int id);
        void Update(Users user, string password = null);
        Users Create(Users user, string password);
        IEnumerable<Users> GetAll();
        Context GetContext(); 
    }

    public class UserService : IUserService
    {
        private Context _context;

        public UserService( Context context)
        {
            _context = context;
        }
        public Users Authenticate( string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.User.SingleOrDefault(x => x.Email == email);
            if (user == null)
                return null;
            
            return user;
        }
        public Context GetContext()
        {
            return _context; 
        }
        public IEnumerable<Users> GetAll()
        {
            return _context.User;
        }

        public void Delete(int id)
        {
            var user = _context.User.Find(id);
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }

        public Users GetById(int id)
        {
            return _context.User.Find(id);
        }

        public Users Create(Users user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Mots de passe requis");

            if (_context.User.Any(x => x.Email == user.Email))
                throw new AppException("Username \"" + user.Email + "\" est déjà prit. ");

            _context.User.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(Users userParam, string password = null)
        {
            var user = _context.User.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Email != user.Email)
            {
                // username has changed so check if the new username is already taken
                if (_context.User.Any(x => x.Email == userParam.Email))
                    throw new AppException("Email " + userParam.Email + " is already taken");
            }

            // update user properties
            user.Firstname = userParam.Firstname;
            user.Lastname = userParam.Lastname;
            user.Email = userParam.Email;

            _context.User.Update(user);
            _context.SaveChanges();
        }
    }
}
