using AgentNetCore.Application;
using AgentNetCore.Model;
using AgentNetCore.Repository;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace AgentNetCore.Context
{
    public class GroupRepository
    {
        private readonly MySQLContext _mySQLContext;
        public GroupRepository(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }

        #region CRUD
        public Group Create(Group group)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                ServerRepository serverRepo = new ServerRepository(_mySQLContext);
                string domain = serverRepo.ConvertToDomain(group.PathDomain);
                credential.Domain = domain;
                string pathDomain = serverRepo.GetPathByDN(group.PathDomain);
                DirectoryEntry dirEntry = new DirectoryEntry(pathDomain, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + group.SamAccountName);
                SearchResult result = search.FindOne();
                if (result == null)
                {
                    DirectoryEntry newGroup = dirEntry.Children.Add("CN=" + group.SamAccountName, "group");
                    newGroup.Properties["cn"].Value = group.SamAccountName;
                    newGroup.Properties["samAccountName"].Value = group.SamAccountName;
                    newGroup.Properties["displayName"].Value = group.DisplayName;
                    newGroup.Properties["description"].Value = group.Description;
                    newGroup.Properties["mail"].Value = group.EmailAddress;
                    newGroup.Properties["managedBy"].Value = group.Manager;
                    var groupType = unchecked((int)(GroupType.UNIVERSAL | GroupType.SECURITY));
                    newGroup.Properties["groupType"].Value = groupType;
                    newGroup.CommitChanges();
                    dirEntry.Close();
                    newGroup.Close();
                    return FindOne(domain, "SamAccountName", group.SamAccountName);
                }
                else
                {
                    return null;
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                if (e.Message.Equals("The object already exists.\r\n"))
                {

                    Console.WriteLine("\r\nO Grupo já existe no contexto:\r\n\t" + e.GetType() + ":" + e.Message);
                }
                return null;
            }
        }

        public List<Group> FindAll()
        {
            ConfigurationRepository config = new ConfigurationRepository(_mySQLContext);
            return FindAll(config.GetConfiguration("DefaultDomain"));
        }

        public List<Group> FindAll(string domain)
        {
            try
            {
                CredentialRepository connect = new CredentialRepository(_mySQLContext);
                connect.Domain = domain;
                ServerRepository sr = new ServerRepository(_mySQLContext);
                DirectoryEntry dirEntry = new DirectoryEntry(sr.GetPathByServer(domain), connect.User, connect.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                List<Group> groupList = new List<Group>();
                search.Filter = "(&(objectClass=group))";
                var groupsResult = search.FindAll();
                List<SearchResult> results = new List<SearchResult>();
                foreach (SearchResult groupResult in groupsResult)
                {
                    groupList.Add(GetResult(groupResult));
                }
                return groupList;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }

        public Group Create2(Group group)
        {
            try
            {
                CredentialRepository connect = new CredentialRepository(_mySQLContext);//group.PathDomain, ObjectApplication.Category.group);
                connect.Domain = group.PathDomain;
                PrincipalContext pc = new PrincipalContext(ContextType.Domain, group.SamAccountName, group.PathDomain);
                GroupPrincipal groupPrincipal = new GroupPrincipal(pc);
                groupPrincipal.SamAccountName = group.SamAccountName;
                groupPrincipal.Name = group.Name;
                groupPrincipal.Description = group.Description;
                groupPrincipal.DisplayName = group.DisplayName;
                groupPrincipal.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return group;
        }
        public Group Update(Group group)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                ServerRepository serverRepo = new ServerRepository(_mySQLContext);
                string domain = serverRepo.ConvertToDomain(group.PathDomain);
                credential.Domain = domain;
                string pathDomain = serverRepo.GetPathByDN(group.PathDomain);
                DirectoryEntry dirEntry = new DirectoryEntry(pathDomain, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + group.SamAccountName);
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    DirectoryEntry newGroup = result.GetDirectoryEntry();
                    newGroup.Properties["displayName"].Value = group.DisplayName;
                    newGroup.Properties["description"].Value = group.Description;
                    newGroup.Properties["mail"].Value = group.EmailAddress;
                    newGroup.Properties["managedBy"].Value = group.Manager;
                    var groupType = unchecked((int)(GroupType.UNIVERSAL | GroupType.SECURITY));
                    newGroup.Properties["groupType"].Value = groupType;
                    newGroup.CommitChanges();
                    newGroup.Rename("cn=" + group.SamAccountName);
                    newGroup.CommitChanges();
                    dirEntry.Close();
                    newGroup.Close();
                    return FindOne(domain, "SamAccountName", group.SamAccountName);
                }
                else
                {
                    Console.WriteLine("\r\nUser not identify:\r\n\t");
                    return null;
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        public void Delete(Group group)
        {
            try
            {
                ServerRepository serverRepo = new ServerRepository(_mySQLContext);
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                string domain = serverRepo.ConvertToDomain(group.PathDomain);
                credential.Domain = domain;
                string pathDomain = group.PathDomain;
                DirectoryEntry dirEntry = new DirectoryEntry(pathDomain, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("SamAccountName=" + group.SamAccountName);
                DirectoryEntry groupFind = (search.FindOne()).GetDirectoryEntry();
                if (groupFind != null)
                {
                    groupFind.DeleteTree();
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
        }

        public void Delete2(Group group)
        {
            try
            {
                CredentialRepository connect = new CredentialRepository(_mySQLContext);
                connect.Domain = group.PathDomain;
                PrincipalContext pc = new PrincipalContext(ContextType.Domain, group.SamAccountName, group.PathDomain);
                GroupPrincipal groupPrincipal = new GroupPrincipal(pc);
                groupPrincipal = GroupPrincipal.FindByIdentity(pc, group.EmailAddress);
                if (groupPrincipal != null)
                {
                    groupPrincipal.Delete();
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);

                if (e.Message == "The server is not operational")
                {//The server is not operational.

                }
            }
        }

        public Group FindBySamName(string domain, string samName)
        {
            try
            {
                return FindOne(domain, "samAccountName", samName);
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        private Group FindOne(string domain, string campo, string valor)
        {
            try
            {

                CredentialRepository connect = new CredentialRepository(_mySQLContext);//domain, ObjectApplication.Category.user);
                connect.Domain = domain;
                ServerRepository sr = new ServerRepository(_mySQLContext);
                DirectoryEntry dirEntry = new DirectoryEntry(sr.GetPathByServer(domain), connect.User, connect.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                Group group = new Group();
                search.Filter = "(" + campo + "=" + valor + ")";
                group = GetResult(search.FindOne());
                return group;
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        public Group FindByEmail(string domain, string email)
        {
            try
            {
                return FindOne(domain, "mail", email);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        #endregion


        #region GET
        private Group GetResult(SearchResult result)
        {
            Group group = new Group();
            try
            {
                if (result != null)
                {
                    return GetProperties(group, result.Properties);
                }
                else
                {
                    Console.WriteLine("Group not found!");
                    return null;
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        private Group GetProperties(Group group, ResultPropertyCollection fields)
        {
            try
            {
                foreach (String ldapField in fields.PropertyNames)
                {
                    foreach (Object myCollection in fields[ldapField])
                    {

                        switch (ldapField)
                        {
                            case "cn":
                                group.DisplayName = myCollection.ToString();
                                break;
                            case "name":
                                group.Name = myCollection.ToString();
                                break;
                            case "samaccountname":
                                group.SamAccountName = myCollection.ToString();
                                break;
                            case "displayName":
                                group.DisplayName = myCollection.ToString();
                                break;
                            case "description":
                                group.Description = myCollection.ToString();
                                break;
                            case "mail":
                                group.EmailAddress = myCollection.ToString();
                                break;
                            case "managedby":
                                group.Manager = myCollection.ToString();
                                break;
                            case "objectsid":
                                group.ObjectSid = myCollection.ToString();
                                break;
                            case "adspath":
                                group.PathDomain = myCollection.ToString();
                                break;
                        }
                        Console.WriteLine(String.Format("{0,-20} : {1}", ldapField, myCollection.ToString()));
                    }
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return group;
            }
            return group;
        }
        #endregion



        public enum GroupType : uint
        {//https://docs.microsoft.com/en-us/windows/win32/adschema/c-group
            SYSTEM = 0x00000001,//1
            GLOBAL = 0x00000002,//2
            DOMAIN_LOCAL = 0x00000004,//4
            UNIVERSAL = 0x00000008,//8
            APP_BASIC = 0x00000010,//16
            APP_QUERY = 0x00000020,//32
            SECURITY = 0x80000000//2147483648
        }
    }
}
