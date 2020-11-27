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
                SamAccountName = origin.SamAccountName,
                DistinguishedName = origin.DistinguishedName,
                DisplayName = origin.DisplayName,
                Name = origin.Name,
                Description = origin.Description,
                EmailAddress = origin.EmailAddress,
                PathDomain = origin.PathDomain,
                Manager = origin.Manager,
                Domain = origin.Domain,
                DomainLocal = origin.DomainLocal,
                Global = origin.Global,
                AppBasic = origin.AppBasic,
                AppQuery = origin.AppQuery,
                Security = origin.Security,
                System = origin.System,
                Universal = origin.Universal,
            };
        }

        public GroupVO Parse(Group origin)
        {
            if (origin == null) return null;
            return new GroupVO()
            {
                SamAccountName = origin.SamAccountName,
                DistinguishedName = origin.DistinguishedName,
                DisplayName = origin.DisplayName,
                Name = origin.Name,
                Description = origin.Description,
                EmailAddress = origin.EmailAddress,
                PathDomain = origin.PathDomain,
                Manager = origin.Manager,
                Domain = origin.Domain,
                DomainLocal = origin.DomainLocal,
                Global = origin.Global,
                AppBasic = origin.AppBasic,
                AppQuery = origin.AppQuery,
                Security = origin.Security,
                System = origin.System,
                Universal = origin.Universal,
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

