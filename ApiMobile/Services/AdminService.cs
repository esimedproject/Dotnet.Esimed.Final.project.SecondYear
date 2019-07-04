using System.Collections.Generic;
using System.Linq;
using ApiMobile.Models;
using ApiMobile.Helpers;

namespace ApiMobile.Services
{
    public interface IAdminService
    {
        Admins AuthenticateAdmin(string email, string password);
        Admins GetByAdminId(int id);
        void DeleteAdmin(int id);
        void UpdateAdmin(Admins admin, string password = null);
        Admins CreateAdmin(Admins admin, string password);
        IEnumerable<Admins> GetAllAdmins();
        Context GetContextAdmins();
    }
    public class AdminService : IAdminService
    {
        private Context _context;

        public AdminService(Context context)
        {
            _context = context;
        }
        public Admins AuthenticateAdmin(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var admin = _context.Admin.SingleOrDefault(x => x.Email == email);
            if (admin == null)
                return null;

            return admin;
        }
        public Context GetContextAdmins()
        {
            return _context;
        }
        public IEnumerable<Admins> GetAllAdmins()
        {
            return _context.Admin;
        }

        public void DeleteAdmin(int id)
        {
            var admin = _context.Admin.Find(id);
            if (admin != null)
            {
                _context.Admin.Remove(admin);
                _context.SaveChanges();
            }
        }

        public Admins GetByAdminId(int id)
        {
            return _context.Admin.Find(id);
        }

        public Admins CreateAdmin(Admins admin, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Mots de passe requis");

            if (_context.Admin.Any(x => x.Email == admin.Email))
                throw new AppException("Username \"" + admin.Email + "\" est déjà prit. ");

            _context.Admin.Add(admin);
            _context.SaveChanges();

            return admin;
        }

        public void UpdateAdmin(Admins adminParam, string password = null)
        {
            var admin = _context.Admin.Find(adminParam.Id);

            if (admin == null)
                throw new AppException("User not found");

            if (adminParam.Email != admin.Email)
            {
                if (_context.Admin.Any(x => x.Email == adminParam.Email))
                    throw new AppException("Email " + adminParam.Email + " is already taken");
            }

            admin.Email = adminParam.Email;

            _context.Admin.Update(admin);
            _context.SaveChanges();
        }
    }
}
