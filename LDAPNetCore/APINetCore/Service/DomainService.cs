using APINetCore.Context;
using APINetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace APINetCore.Service
{ 

    public class DomainService : IDomainService
    {
        private MySQLContext _context;

        public DomainService(MySQLContext context)
        {
            _context = context;
        }

        public Domain Create(Domain domain)
        {
            try
            {
                _context.Add(domain);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return domain;
        }

        public List<Domain> FindAll()
        {
            return _context.Domains.ToList();
        }

        public Domain FindById(long id)
        {
            return _context.Domains.SingleOrDefault(p => p.Id.Equals(id));
        }

        public Domain Update(Domain domain)
        {
            if (!Exist(domain.Id)) return new Domain();

            var result = _context.Domains.SingleOrDefault(p => p.Id.Equals(domain.Id));
            try
            {
                _context.Entry(result).CurrentValues.SetValues(domain);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return domain;
        }

        private bool Exist(long? id)
        {
            return _context.Domains.Any(p => p.Id.Equals(id));
        }
        public void Delete(long id)
        {
            var result = _context.Domains.SingleOrDefault(p => p.Id.Equals(id));
            try
            {
                if (result != null) _context.Domains.Remove(result);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
