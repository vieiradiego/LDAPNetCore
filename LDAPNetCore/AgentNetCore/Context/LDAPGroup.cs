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
        public LDAPGroup()
        {
            _connect = new LDAPConnect("group", "marveldomain.local", "192.168.0.99", false);
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
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                if (e.Message.Equals("The object already exists.\r\n"))
                {

                    Console.WriteLine("\r\nO Usuario ja existe no contexto:\r\n\t" + e.GetType() + ":" + e.Message);
                }
                return group;
            }
        }
        public Group Update(Group group)
        {
            try
            {
                _groupPrincipal = GroupPrincipal.FindByIdentity(_connect.Context, group.DisplayName);
                if (_groupPrincipal != null)
                {
                    return SetProperties(group);
                }
                else
                {
                    Console.WriteLine("\r\nUser not identify:\r\n\t");
                    return group;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return group;
            }
        }

        public void Delete(Group group)
        {
            if (DirectoryEntry.Exists(_connect.Path + group.DisplayName))
            {
                try
                {
                    DirectoryEntry entry = new DirectoryEntry(_connect.Path + group.DisplayName);
                    DirectoryEntry groupEntry = new DirectoryEntry(_connect.Path + group.DisplayName);
                    entry.Children.Remove(groupEntry);
                    groupEntry.CommitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                }
            }
            else
            {
                Console.WriteLine(_connect.Path + group.DisplayName + " doesn't exist");
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
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);

                if (e.Message == "The server is not operational")
                {//The server is not operational.

                }
            }
        }
        public Group FindByName(string name)
        {
            try
            {
                return FindOne("cn", name);
            }
            catch (Exception e)
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
            DirectoryEntry newGroup = _dirEntry.Children.Add("CN=" + group.DisplayName, "group");
            newGroup.Properties["samAccountName"].Value = group.SamAccountName;
            newGroup.CommitChanges();
            _dirEntry.Close();
            newGroup.Close();
            return FindByName(group.DisplayName);
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
                            case "samaccountname":
                                group.SamAccountName = myCollection.ToString();
                                break;
                            case "givenname":
                                group.DisplayName = myCollection.ToString();
                                break;
                        }
                        Console.WriteLine(String.Format("{0,-20} : {1}", ldapField, myCollection.ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return group;
            }
            return group;
        }
        #endregion
    }
}
