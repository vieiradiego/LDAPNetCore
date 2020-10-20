using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Context
{
    public class LDAPComputer
    {
        private LDAPConnect _connect;
        private DirectoryEntry _dirEntry;
        private ComputerPrincipal _computerPrincipal;
        private DirectorySearcher _search;

        public LDAPComputer()
        {
            _connect = new LDAPConnect("computer", "marveldomain.local", "192.168.0.99", false);
            _dirEntry = new DirectoryEntry(_connect.Path, _connect.User, _connect.Pass);
            _search = new DirectorySearcher(_dirEntry);
        }
        #region CRUD
        public Computer Create(Computer computer)
        {
            try
            {
                return SetProperties(computer);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                if (e.Message.Equals("The object already exists.\r\n"))
                {

                    Console.WriteLine("\r\nO Usuario ja existe no contexto:\r\n\t" + e.GetType() + ":" + e.Message);
                }
                return computer;
            }
        }
        public Computer Update(Computer computer)
        {
            try
            {
                _computerPrincipal = ComputerPrincipal.FindByIdentity(_connect.Context, computer.SamAccountName);
                if (_computerPrincipal != null)
                {
                    return SetProperties(computer);
                }
                else
                {
                    Console.WriteLine("\r\nUser not identify:\r\n\t");
                    return computer;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return computer;
            }
        }
        public void Delete(Computer computer)
        {
            try
            {
                _computerPrincipal = ComputerPrincipal.FindByIdentity(_connect.Context, computer.SamAccountName);
                if (_computerPrincipal != null)
                {
                    _computerPrincipal.Delete();
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
        public Computer FindByName(string name)
        {
            try
            {
                return FindOne("SamAccountName", name);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        private Computer FindOne(string campo, string valor)
        {
            try
            {
                Computer computer = new Computer();
                _search.Filter = "(" + campo + "=" + valor + ")";
                computer = GetResult(_search.FindOne());
                return computer;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        #endregion

        #region SET
        private Computer SetProperties(Computer computer)
        {
            
            DirectoryEntry newComputer = _dirEntry.Children.Add("CN=" + computer.SamAccountName, "computer");
            newComputer.Properties["samAccountName"].Value = computer.SamAccountName;
            newComputer.CommitChanges();
            _dirEntry.Close();
            newComputer.Close();
            return FindByName(computer.SamAccountName);
        }
        #endregion

        #region GET
        private Computer GetResult(SearchResult result)
        {
            Computer computer = new Computer();
            try
            {
                if (result != null)
                {
                    ResultPropertyCollection fields = result.Properties;
                    return GetProperties(computer, fields);
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
            return computer;
        }
        private Computer GetProperties(Computer computer, ResultPropertyCollection fields)
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
                                computer.SamAccountName = myCollection.ToString();
                                break;
                            case "distinguishedname":
                                computer.DistinguishedName = myCollection.ToString();
                                break;
                        }
                        Console.WriteLine(String.Format("{0,-20} : {1}", ldapField, myCollection.ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return computer;
            }
            return computer;
        }
        #endregion
    }
}
