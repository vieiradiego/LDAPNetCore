using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IForestService
    {
        List<ForestVO> FindAll();
        List<ForestVO> FindAll(string domain);
    }
}
