using AgentNetCore.Data.VO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapioca.HATEOAS;

namespace AgentNetCore.Hypermedia
{
    public class ClientEnricher : ObjectContentResponseEnricher<ClientVO>
    {
        private readonly object _lock = new object();
        protected override Task EnrichModel(ClientVO content, IUrlHelper urlHelper)
        {
            var path = "v1/clients/";
            string link = GetLink(content.UserName, urlHelper, path);
            
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.POST,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPost
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PUT,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPost
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.DELETE,
                Href = link,
                Rel = RelationType.self,
                Type = "string"
            });
            return null;
        }
        private string GetLink(string userName, IUrlHelper urlHelper, string path)
        {
            lock (_lock)
            {
                var url = new { controller = path, UserName = userName };
                return new StringBuilder(urlHelper.Link("DefaultApi", url)).Replace("%2F", "/").ToString();
            };
        }
    }
}
