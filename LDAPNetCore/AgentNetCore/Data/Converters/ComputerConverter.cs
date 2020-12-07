using AgentNetCore.Data.Converter;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Data.Converters
{
    public class ComputerConverter : IParser<ComputerVO, Computer>, IParser<Computer, ComputerVO>
    {
        public Computer Parse(ComputerVO origin)
        {
            if (origin == null) return null;
            return new Computer()
            {
                SamAccountName = origin.SamAccountName,
                DistinguishedName = origin.DistinguishedName,
                DisplayName = origin.DisplayName,
                Name = origin.Name,
                PathDomain = origin.PathDomain,
                Domain = origin.Domain,
                Description = origin.Description,
                DnsHostName = origin.DnsHostName,
                Location = origin.Location
            };
        }

        public ComputerVO Parse(Computer origin)
        {
            if (origin == null) return null;
            return new ComputerVO()
            {
                SamAccountName = origin.SamAccountName,
                DistinguishedName = origin.DistinguishedName,
                DisplayName = origin.DisplayName,
                Name = origin.Name,
                PathDomain = origin.PathDomain,
                Domain = origin.Domain,
                Description = origin.Description,
                DnsHostName = origin.DnsHostName,
                Location = origin.Location
            };
        }

        public List<Computer> ParseList(List<ComputerVO> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<ComputerVO> ParseList(List<Computer> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}

