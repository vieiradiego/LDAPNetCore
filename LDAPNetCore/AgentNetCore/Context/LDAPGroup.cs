using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Context
{
    public class LDAPGroup
    {
        private LDAPConnect _connect;
        private DirectoryEntry _dirEntry;
        private GroupPrincipal _groupPrincipal;
        private DirectorySearcher _search;
        public LDAPGroup(string domain)
        {
            _connect = new LDAPConnect(domain, LDAPConnect.ObjectCategory.group);
            _dirEntry = new DirectoryEntry(_connect.Path, _connect.User, _connect.Pass);
            _search = new DirectorySearcher(_dirEntry);
        }

        #region CRUD
        public Group Create(Group group)
        {
            try
            {
                return SetProperties(group);
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                if (e.Message.Equals("The object already exists.\r\n"))
                {

                    Console.WriteLine("\r\nO Grupo já existe no contexto:\r\n\t" + e.GetType() + ":" + e.Message);
                }
                return group;
            }
        }

        public Group Create2(Group group)
        {
            try
            {
                _groupPrincipal = new GroupPrincipal(_connect.Context);
                _groupPrincipal.SamAccountName = group.SamAccountName;
                _groupPrincipal.Name = group.Name;
                _groupPrincipal.Description = group.Description;
                _groupPrincipal.DisplayName = group.DisplayName;
                _groupPrincipal.Save();
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
                _groupPrincipal = GroupPrincipal.FindByIdentity(_connect.Context, group.SamAccountName);
                if (_groupPrincipal != null)
                {
                    return UpdateGroup(group);
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

        private Group UpdateGroup(Group group)
        {
            // Alterar para receber um Directory Entry
            // Fazer função para alterar Login do Usuário 

            _search.Filter = ("SamAccountName=" + group.SamAccountName);
            DirectoryEntry newGroup = (_search.FindOne()).GetDirectoryEntry();
            //newGroup.Properties["cn"].Value = group.SamAccountName;
            newGroup.Properties["displayName"].Value = group.DisplayName;
            newGroup.Properties["description"].Value = group.Description;
            newGroup.Properties["mail"].Value = group.EmailAddress;
            newGroup.Properties["managedBy"].Value = "CN=" + group.Manager + "," + group.PathDomain;
            var groupType = unchecked((int)(GroupType.UNIVERSAL | GroupType.SECURITY));
            newGroup.Properties["groupType"].Value = groupType;
            newGroup.CommitChanges();
            newGroup.Rename("cn=" + group.SamAccountName);
            newGroup.CommitChanges();
            _dirEntry.Close();
            newGroup.Close();
            return FindOne("SamAccountName", group.SamAccountName);
        }

        public void Delete(Group group)
        {
            try
            {
                _groupPrincipal = GroupPrincipal.FindByIdentity(_connect.Context, group.SamAccountName);
                if (_groupPrincipal != null)
                {
                    _groupPrincipal.Delete();
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
                _groupPrincipal = GroupPrincipal.FindByIdentity(_connect.Context, group.EmailAddress);
                if (_groupPrincipal != null)
                {
                    _groupPrincipal.Delete();
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
        public List<Group> FindAll(string campo, string valor)
        {
            try
            {
                List<Group> groupList = new List<Group>();
                _search.Filter = "(&(objectClass=group))";
                var groupsResult = _search.FindAll();
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
                return new List<Group>();
            }

        }
        public Group FindBySamName(string samName)
        {
            try
            {
                return FindOne("samAccountName", samName);
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        private Group FindOne(string campo, string valor)
        {
            try
            {
                Group group = new Group();
                _search.Filter = "(" + campo + "=" + valor + ")";
                group = GetResult(_search.FindOne());
                return group;
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        public Group FindByEmail(string email)
        {
            try
            {
                return FindOne("mail", email);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        #endregion

        #region SET
        private Group SetProperties(Group group)
        {
            DirectoryEntry newGroup = _dirEntry.Children.Add("CN=" + group.SamAccountName, "group");
            newGroup.Properties["cn"].Value = group.SamAccountName;
            newGroup.Properties["samAccountName"].Value = group.SamAccountName;
            newGroup.Properties["displayName"].Value = group.DisplayName;
            newGroup.Properties["description"].Value = group.Description;
            newGroup.Properties["mail"].Value = group.EmailAddress;
            newGroup.Properties["managedBy"].Value = "CN=" + group.Manager + "," + group.PathDomain;
            var groupType = unchecked((int)(GroupType.UNIVERSAL | GroupType.SECURITY));
            newGroup.Properties["groupType"].Value = groupType;
            newGroup.CommitChanges();
            _dirEntry.Close();
            newGroup.Close();
            return FindOne("SamAccountName", group.SamAccountName);
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
                    ResultPropertyCollection fields = result.Properties;
                    return GetProperties(group, fields);
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
