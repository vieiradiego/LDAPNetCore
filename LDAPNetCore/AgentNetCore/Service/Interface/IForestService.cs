using AgentNetCore.Model;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IForestService
    {
        List<Forest> FindAll();
        List<Forest> FindAll(string domain);
    }
}
