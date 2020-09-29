using AgentNetCore.Context;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgentNetCore.Service
{
    public class GroupService : IGroupService
    {
        private MySQLContext _context;

        public GroupService(MySQLContext context)
        {
            _context = context;
        }

        public Group Create(Group group)
        {
            try
            {
                _context.Add(group);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return group;
        }

        public List<Group> FindAll()
        {
            return _context.Groups.ToList();
        }

        public Group FindById(long id)
        {
            return _context.Groups.SingleOrDefault(p => p.Id.Equals(id));
        }

        public Group Update(Group group)
        {
            if (!Exist(group.Id)) return new Group();

            var result = _context.Groups.SingleOrDefault(p => p.Id.Equals(group.Id));
            try
            {
                _context.Entry(result).CurrentValues.SetValues(group);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return group;
        }

        private bool Exist(long? id)
        {
            return _context.Groups.Any(p => p.Id.Equals(id));
        }
        public void Delete(long id)
        {
            var result = _context.Groups.SingleOrDefault(p => p.Id.Equals(id));
            try
            {
                if (result != null) _context.Groups.Remove(result);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
