using AgentNetCore.Data.Converter;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Data.Converters
{
    public class OrganizationalUnitConverter : IParser<OrganizationalUnitVO, OrganizationalUnit>, IParser<OrganizationalUnit, OrganizationalUnitVO>
    {
        public OrganizationalUnit Parse(OrganizationalUnitVO origin)
        {
            if (origin == null) return null;
            return new OrganizationalUnit()
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
                ObjectSid = origin.ObjectSid,
                WhenChanged = origin.WhenChanged,
                WhenCreated = origin.WhenCreated,
                Ou = origin.Ou,
                DistinguishedName = origin.DistinguishedName,
                Street = origin.Street,
                IsCriticalSystemObject = origin.IsCriticalSystemObject,
                CommonName = origin.CommonName

            };
        }

        public OrganizationalUnitVO Parse(OrganizationalUnit origin)
        {
            if (origin == null) return null;
            return new OrganizationalUnitVO()
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
                ObjectSid = origin.ObjectSid,
                WhenChanged = origin.WhenChanged,
                WhenCreated = origin.WhenCreated,
                Ou = origin.Ou,
                DistinguishedName = origin.DistinguishedName,
                Street = origin.Street,
                IsCriticalSystemObject = origin.IsCriticalSystemObject,
                CommonName = origin.CommonName
            };
        }

        public List<OrganizationalUnit> ParseList(List<OrganizationalUnitVO> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<OrganizationalUnitVO> ParseList(List<OrganizationalUnit> origin)
        {
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}

