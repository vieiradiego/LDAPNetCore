using AgentNetCore.Application;
using AgentNetCore.Context;
using AgentNetCore.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace AgentNetCore.Repository
{
    public class ForestRepository
    {
        private readonly MySQLContext _mySQLContext;
        public ForestRepository(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }
        public List<Forest> FindAll()
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                ServerRepository serverRepo = new ServerRepository(_mySQLContext);
                ConfigurationRepository config = new ConfigurationRepository(_mySQLContext);
                string domain = config.GetConfiguration("DefaultDomain");
                credential.Domain = domain;
                string pathDomain = serverRepo.GetPathByServer(domain);
                DirectoryEntry dirEntry = new DirectoryEntry(pathDomain, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = String.Format("(&(objectCategory={0})(name={1}))", "container", ObjectApplication.Category.system);
                SearchResultCollection forestsResult = search.FindAll();
                List<Forest> forestList = new List<Forest>();
                foreach (SearchResult forestResult in forestsResult)
                {
                    forestList.Add(GetResult(forestResult));
                }
                return forestList;
            }
            catch (Exception e)
            {

                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        private Forest GetResult(SearchResult result)
        {
            Forest forest = new Forest();
            try
            {
                if (result != null)
                {
                    return GetProperties(forest, result.Properties);
                }
                else
                {
                    Console.WriteLine("Forest not found!");
                    return null;
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        private Forest GetProperties(Forest forest, ResultPropertyCollection fields)
        {
            try
            {
                ServerRepository sr = new ServerRepository(_mySQLContext);
                foreach (String ldapField in fields.PropertyNames)
                {
                    foreach (Object myCollection in fields[ldapField])
                    {

                        switch (ldapField)
                        {
                            case "domain":
                                forest.Domain = myCollection.ToString();
                                break;
                            case "name":
                                forest.Name = myCollection.ToString();
                                break;
                            case "displayName":
                                forest.DisplayName = myCollection.ToString();
                                break;
                            case "description":
                                forest.Description = myCollection.ToString();
                                break;
                            case "samaccountname":
                                forest.SamAccountName = myCollection.ToString();
                                break;
                            case "managedby":
                                forest.Manager = myCollection.ToString();
                                break;
                            case "adspath":
                                forest.PathDomain = myCollection.ToString();
                                forest.Domain = sr.ConvertToDomain(myCollection.ToString());
                                break;
                            case "l":
                                forest.City = myCollection.ToString();
                                break;
                            case "st":
                                forest.State = myCollection.ToString();
                                break;
                            case "postalcode":
                                forest.PostalCode = myCollection.ToString();
                                break;
                            case "c":
                                forest.Country = myCollection.ToString();
                                break;
                            case "mail":
                                forest.Email = myCollection.ToString();
                                break;
                            case "objectsid":
                                forest.ObjectSid = myCollection.ToString();
                                break;
                            case "whenchanged":
                                forest.WhenChanged = myCollection.ToString();
                                break;
                            case "whencreated":
                                forest.WhenCreated = myCollection.ToString();
                                break;
                            case "ou":
                                forest.Ou = myCollection.ToString();
                                break;
                            case "distinguishedname":
                                forest.DistinguishedName = myCollection.ToString();
                                break;
                            case "street":
                                forest.Street = myCollection.ToString();
                                break;
                            case "iscriticalsystemobject":
                                forest.IsCriticalSystemObject = (bool)myCollection;
                                break;
                            case "cn":
                                forest.CommonName = myCollection.ToString();
                                break;
                        }
                        Console.WriteLine(String.Format("{0,-20} : {1}", ldapField, myCollection.ToString()));
                    }
                }
                return forest;
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        public List<Forest> FindAll(string domain)
        {
            return null;
        }
    }
}
