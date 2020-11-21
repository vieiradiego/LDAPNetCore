using AgentNetCore.Data.Converter;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Data.Converters
{
    public class GroupConverter : IParser<GroupVO, Group>, IParser<Group, GroupVO>
    {
        public Group Parse(GroupVO origin)
        {
            if (origin == null) return null;
            return new Group()
            {
                DisplayName = origin.DisplayName,
                Name = origin.Name,
                Description = origin.Description,
                EmailAddress = origin.EmailAddress,
                SamAccountName = origin.SamAccountName,
                ObjectSid = origin.ObjectSid,
                PathDomain = origin.PathDomain
            };
        }

        public GroupVO Parse(Group origin)
        {
            if (origin == null) return null;
            return new GroupVO()
            {
                DisplayName = origin.DisplayName,
                Name = origin.Name,
                Description = origin.Description,
                EmailAddress = origin.EmailAddress,
                SamAccountName = origin.SamAccountName,
                ObjectSid = origin.ObjectSid,
                PathDomain = origin.PathDomain
            };
        }

        public List<Group> ParseList(List<GroupVO> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<GroupVO> ParseList(List<Group> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}

