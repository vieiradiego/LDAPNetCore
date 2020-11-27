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
                credential.DN = group.DistinguishedName;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + group.SamAccountName);
                SearchResult result = search.FindOne();
                if (result == null)
                {
                    DirectoryEntry newGroup = dirEntry.Children.Add("CN=" + group.SamAccountName, "group");
                    
                    if (!String.IsNullOrWhiteSpace(group.SamAccountName))
                    {
                        newGroup.Properties["samAccountName"].Value = group.SamAccountName;
                        newGroup.Properties["info"].Value = "Grupo criado pela API RESTful  - " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    }
                    if (!String.IsNullOrWhiteSpace(group.Name))
                    {
                        newGroup.Properties["name"].Value = group.Name;
                    }
                    if (!String.IsNullOrWhiteSpace(group.DistinguishedName) && (!String.IsNullOrWhiteSpace(group.DisplayName)))
                    {
                        newGroup.Properties["distinguishedName"].Value = "CN=" + group.DisplayName + "," + group.DistinguishedName;
                        newGroup.Properties["displayName"].Value = group.DisplayName;
                    }
                    if (!String.IsNullOrWhiteSpace(group.EmailAddress))
                    {
                        newGroup.Properties["mail"].Value = group.EmailAddress;
                    }
                    if (!String.IsNullOrWhiteSpace(group.Manager))
                    {
                        newGroup.Properties["managedby"].Value = group.Manager;
                    }
                    if (!String.IsNullOrWhiteSpace(group.Description))
                    {
                        newGroup.Properties["description"].Value = group.Description;
                    }
                    if ((group.System) && (group.Global))
                    {//SYSTEM = 0x00000001 |//GLOBAL = 0x00000002,// 1 + 2
                        var groupType = unchecked((int)(GroupType.SYSTEM | GroupType.GLOBAL));
                        newGroup.Properties["groupType"].Value = groupType;
                    }
                    else if ((group.System) && (group.DomainLocal))
                    {//SYSTEM = 0x00000001 | DOMAIN_LOCAL = 0x00000004 // 1 + 4
                        var groupType = unchecked((int)(GroupType.SYSTEM | GroupType.DOMAIN_LOCAL));
                        newGroup.Properties["groupType"].Value = groupType;
                    }
                    else if ((group.System) && (group.Universal))
                    {//SYSTEM = 0x00000001 | UNIVERSAL = 0x00000008 // 1 + 8
                        var groupType = unchecked((int)(GroupType.SYSTEM | GroupType.UNIVERSAL));
                        newGroup.Properties["groupType"].Value = groupType;
                    }
                    else if ((group.Security) && (group.Global))
                    {//SECURITY = 2147483648 | GLOBAL = 0x00000002,// 2147483648 + 2
                        var groupType = unchecked((int)(GroupType.SECURITY | GroupType.GLOBAL));
                        newGroup.Properties["groupType"].Value = groupType;
                    }
                    else if ((group.Security) && (group.DomainLocal))
                    {//SECURITY = 2147483648 | DOMAIN_LOCAL = 0x00000004 // 2147483648 + 4
                        var groupType = unchecked((int)(GroupType.SECURITY | GroupType.DOMAIN_LOCAL));
                        newGroup.Properties["groupType"].Value = groupType; //
                    }
                    else if ((group.Security) && (group.Universal))
                    {//SECURITY = 2147483648 | UNIVERSAL = 0x00000008 // 2147483648 + 8
                        var groupType = unchecked((int)(GroupType.UNIVERSAL | GroupType.SECURITY));
                        newGroup.Properties["groupType"].Value = groupType;//-2147483640
                    }
                    newGroup.CommitChanges();
                    dirEntry.Close();
                    newGroup.Close();
                    return FindOne(credential, "SamAccountName", group.SamAccountName);
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
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            credential.DN = config.GetConfiguration("DefaultDN");
            return FindAll(credential);
        }
        private List<Group> FindAll(CredentialRepository credential)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
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

        private Group FindOne(CredentialRepository credential, string campo, string valor)
        {
            try
            {
                ServerRepository sr = new ServerRepository(_mySQLContext);
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
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

        public Group FindByEmail(string dn, string email)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                return FindOne(credential, "mail", email);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        public Group FindBySamName(CredentialRepository credential, string samName)
        {
            try
            {
                return FindOne(credential, "samAccountName", samName);
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        public Group FindBySamName(string dn, string samName)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                return FindOne(credential, "samAccountName", samName);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        public List<Group> FindByDn(string dn)
        {
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            credential.DN = dn;
            return FindAll(credential);
        }
        public Group Update(Group group)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = group.DistinguishedName;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + group.SamAccountName);
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    DirectoryEntry newGroup = result.GetDirectoryEntry();
                    newGroup.Properties["info"].Value = "Grupo Editado pela API RESTful  - " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    if (!String.IsNullOrWhiteSpace(group.Name))
                    {
                        newGroup.Rename("cn=" + group.Name);
                    }
                    if (!String.IsNullOrWhiteSpace(group.DistinguishedName) && (!String.IsNullOrWhiteSpace(group.DisplayName)))
                    {
                        newGroup.Properties["displayName"].Value = group.DisplayName;
                    }
                    if (!String.IsNullOrWhiteSpace(group.EmailAddress))
                    {
                        newGroup.Properties["mail"].Value = group.EmailAddress;
                    }
                    if (!String.IsNullOrWhiteSpace(group.Manager))
                    {
                        newGroup.Properties["managedby"].Value = group.Manager;
                    }
                    if (!String.IsNullOrWhiteSpace(group.Description))
                    {
                        newGroup.Properties["description"].Value = group.Description;
                    }
                    if ((group.System) && (group.Global))
                    {//SYSTEM = 0x00000001 |//GLOBAL = 0x00000002,// 1 + 2
                        var groupType = unchecked((int)(GroupType.SYSTEM | GroupType.GLOBAL));
                        newGroup.Properties["groupType"].Value = groupType;
                    }
                    else if ((group.System) && (group.DomainLocal))
                    {//SYSTEM = 0x00000001 | DOMAIN_LOCAL = 0x00000004 // 1 + 4
                        var groupType = unchecked((int)(GroupType.SYSTEM | GroupType.DOMAIN_LOCAL));
                        newGroup.Properties["groupType"].Value = groupType;
                    }
                    else if ((group.System) && (group.Universal))
                    {//SYSTEM = 0x00000001 | UNIVERSAL = 0x00000008 // 1 + 8
                        var groupType = unchecked((int)(GroupType.SYSTEM | GroupType.UNIVERSAL));
                        newGroup.Properties["groupType"].Value = groupType;
                    }
                    else if ((group.Security) && (group.Global))
                    {//SECURITY = 2147483648 | GLOBAL = 0x00000002,// 2147483648 + 2
                        var groupType = unchecked((int)(GroupType.SECURITY | GroupType.GLOBAL));
                        newGroup.Properties["groupType"].Value = groupType;
                    }
                    else if ((group.Security) && (group.DomainLocal))
                    {//SECURITY = 2147483648 | DOMAIN_LOCAL = 0x00000004 // 2147483648 + 4
                        var groupType = unchecked((int)(GroupType.SECURITY | GroupType.DOMAIN_LOCAL));
                        newGroup.Properties["groupType"].Value = groupType; //
                    }
                    else if ((group.Security) && (group.Universal))
                    {//SECURITY = 2147483648 | UNIVERSAL = 0x00000008 // 2147483648 + 8
                        var groupType = unchecked((int)(GroupType.UNIVERSAL | GroupType.SECURITY));
                        newGroup.Properties["groupType"].Value = groupType;//-2147483640
                    }
                    newGroup.CommitChanges();
                    dirEntry.Close();
                    newGroup.Close();
                    return FindOne(credential, "SamAccountName", group.SamAccountName);
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
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = group.DistinguishedName;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
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
                    Console.WriteLine("User not found!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);

            }
            return group;
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
                            case "adspath":
                                group.PathDomain = myCollection.ToString();
                                break;
                        }
                        Console.WriteLine(String.Format("{0,-20} : {1}", ldapField, myCollection.ToString()));
                    }
                }
                return group;
            }
            catch (DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
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
