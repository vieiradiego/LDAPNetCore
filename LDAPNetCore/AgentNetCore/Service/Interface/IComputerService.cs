using AgentNetCore.Data.VO;
using AgentNetCore.Service.Interface;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IComputerService : IService<IComputerService>
    {
        ComputerVO Create(ComputerVO computer);
        List<ComputerVO> FindAll();
        List<ComputerVO> FindByDn(string dn);
        ComputerVO FindBySamName(string dn, string samName);
        ComputerVO Update(ComputerVO computer);
        bool Delete(string dn, string samName);
        
    }
}
