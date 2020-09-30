using APINetCore.Context;
using APINetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace APINetCore.Service
{
    public class UserService : IUserService
    {
        private MySQLContext _context;

        public UserService(MySQLContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            try
            {
                // Implementar o uso de classe para consumir a API de Agente.
                // Passar o user por parâmetro
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return user;
        }
                
        public List<User> FindAll()
        {
            return _context.Users.ToList();
        }

        public User FindById(long id)
        {
            return _context.Users.SingleOrDefault(p => p.Id.Equals(id));
        }

        public User Update(User user)
        {
            if (!Exist(user.Id)) return new User();

            var result = _context.Users.SingleOrDefault(p => p.Id.Equals(user.Id));
            try
            {
                _context.Entry(result).CurrentValues.SetValues(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;
        }

        private bool Exist(long? id)
        {
            return _context.Users.Any(p => p.Id.Equals(id));
        }
        public void Delete(long id)
        {
            var result = _context.Users.SingleOrDefault(p => p.Id.Equals(id));
            try
            {
                if (result != null) _context.Users.Remove(result);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
