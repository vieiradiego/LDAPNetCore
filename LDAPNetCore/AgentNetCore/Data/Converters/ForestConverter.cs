using AgentNetCore.Data.Converter;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Data.Converters
{
    public class ForestConverter : IParser<ForestVO, Forest>, IParser<Forest, ForestVO>
    {
        public Forest Parse(ForestVO origin)
        {
            if (origin == null) return null;
            return new Forest()
            {
                Name = origin.Name,
                DisplayName = origin.DisplayName,
                Description = origin.Description,
                ProtectDeletion = origin.ProtectDeletion,
                SamAccountName = origin.SamAccountName,
                Manager = origin.Manager,
                PathDomain = origin.PathDomain,
                Domain = origin.Domain,
                City = origin.City,
                State = origin.State,
                PostalCode = origin.PostalCode,
                Country = origin.Country,
                Email = origin.Email,
                WhenChanged = origin.WhenChanged,
                WhenCreated = origin.WhenCreated,
                Ou = origin.Ou,
                DistinguishedName = origin.DistinguishedName,
                Street = origin.Street,
                IsCriticalSystemObject = origin.IsCriticalSystemObject,
                CommonName = origin.CommonName
            };
        }

        public ForestVO Parse(Forest origin)
        {
            if (origin == null) return null;
            return new ForestVO()
            {
                Name = origin.Name,
                DisplayName = origin.DisplayName,
                Description = origin.Description,
                ProtectDeletion = origin.ProtectDeletion,
                SamAccountName = origin.SamAccountName,
                Manager = origin.Manager,
                PathDomain = origin.PathDomain,
                Domain = origin.Domain,
                City = origin.City,
                State = origin.State,
                PostalCode = origin.PostalCode,
                Country = origin.Country,
                Email = origin.Email,
                WhenChanged = origin.WhenChanged,
                WhenCreated = origin.WhenCreated,
                Ou = origin.Ou,
                DistinguishedName = origin.DistinguishedName,
                Street = origin.Street,
                IsCriticalSystemObject = origin.IsCriticalSystemObject,
                CommonName = origin.CommonName
            };
        }

        public List<Forest> ParseList(List<ForestVO> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<ForestVO> ParseList(List<Forest> origin)
        {
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}

