﻿using AgentNetCore.Data.VO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapioca.HATEOAS;

namespace AgentNetCore.Hypermedia
{
    public class UserEnricher : ObjectContentResponseEnricher<UserVO>
    {
        private readonly object _lock = new object();
        protected override Task EnrichModel(UserVO content, IUrlHelper urlHelper)
        {
            var path = "v1/users/";
            string link = GetLink(content.EmailAddress, urlHelper, path);
            
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
        private string GetLink(string email, IUrlHelper urlHelper, string path)
        {
            lock (_lock)
            {
                var url = new { controller = path, EmailAddress = email };
                return new StringBuilder(urlHelper.Link("DefaultApi", url)).Replace("%2F", "/").ToString();
            };
        }
    }
}
