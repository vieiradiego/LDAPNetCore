using AgentNetCore.Application;
using AgentNetCore.Model;
using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace AgentNetCore.Context
{
    public class ComputerRepository
    {
        public ComputerRepository()
        {
            
        }

        #region CRUD
        public Computer Create(Computer computer)
        {
            try
            {
                ConnectRepository connect = new ConnectRepository();
                ComputerPrincipal computerPrincipal = new ComputerPrincipal(connect.Context);
                computerPrincipal = ComputerPrincipal.FindByIdentity(connect.Context, computer.SamAccountName);
                if (computerPrincipal != null)
                {
                    DirectoryEntry dirEntry = new DirectoryEntry(connect.Path, connect.User, connect.Pass);
                    DirectoryEntry newComputer = dirEntry.Children.Add("CN=" + computerPrincipal.SamAccountName, "computer");
                    newComputer.Properties["samAccountName"].Value = computer.SamAccountName;
                    newComputer.CommitChanges();
                    dirEntry.Close();
                    newComputer.Close();
                    return FindByName(computer.PathDomain, computer.SamAccountName);
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
                ConnectRepository connect = new ConnectRepository();
                ComputerPrincipal computerPrincipal = new ComputerPrincipal(connect.Context);
                computerPrincipal = ComputerPrincipal.FindByIdentity(connect.Context, computer.SamAccountName);
                if (computerPrincipal != null)
                {
                    DirectoryEntry dirEntry = new DirectoryEntry(connect.Path, connect.User, connect.Pass);
                    DirectoryEntry newComputer = dirEntry.Children.Add("CN=" + computerPrincipal.SamAccountName, "computer");
                    newComputer.Properties["samAccountName"].Value = computer.SamAccountName;
                    newComputer.CommitChanges();
                    dirEntry.Close();
                    newComputer.Close();
                    return FindByName(computer.PathDomain, computer.SamAccountName);
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
                ConnectRepository connect = new ConnectRepository(computer.PathDomain, ObjectApplication.Category.user);
                UserPrincipal computerPrincipal = new UserPrincipal(connect.Context);
                computerPrincipal = ComputerPrincipal.FindByIdentity(connect.Context, computer.SamAccountName);
                if (computerPrincipal != null)
                {
                    computerPrincipal.Delete();
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
        public Computer FindByName(string domain, string name)
        {
            try
            {
                return FindOne(domain, "SamAccountName", name);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        private Computer FindOne(string domain, string campo, string valor)
        {
            try
            {
                ConnectRepository connect = new ConnectRepository(domain, ObjectApplication.Category.user);
                DirectoryEntry dirEntry = new DirectoryEntry(connect.Path, connect.User, connect.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                Computer computer = new Computer();
                search.Filter = "(" + campo + "=" + valor + ")";
                computer = GetResult(search.FindOne());
                return computer;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

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
