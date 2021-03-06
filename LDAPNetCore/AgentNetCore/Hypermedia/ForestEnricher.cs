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
    public class ForestEnricher : ObjectContentResponseEnricher<ForestVO>
    {
        private readonly object _lock = new object();
        protected override Task EnrichModel(ForestVO content, IUrlHelper urlHelper)
        {
            var path = "v1/forests/";
            string link = GetLink(content.SamAccountName, urlHelper, path);
            
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultGet
            });
            return null;
        }
        private string GetLink(string samAccountName, IUrlHelper urlHelper, string path)
        {
            lock (_lock)
            {
                var url = new { controller = path, SamAccountName = samAccountName };
                return new StringBuilder(urlHelper.Link("DefaultApi", url)).Replace("%2F", "/").ToString();
            };
        }
    }
}
