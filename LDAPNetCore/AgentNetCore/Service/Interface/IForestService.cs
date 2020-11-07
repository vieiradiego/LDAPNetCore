using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using AgentNetCore.Service.Interface;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IForestService : IService
    {
        List<ForestVO> FindAll();
        List<ForestVO> FindAll(string domain);
    }
}
