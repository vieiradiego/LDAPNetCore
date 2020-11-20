using AgentNetCore.Application;
using AgentNetCore.Model;
using AgentNetCore.Repository;
using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace AgentNetCore.Context
{
    public class ComputerRepository : IComputerRepository
    {
        private readonly MySQLContext _mySQLContext;
        public ComputerRepository(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }

        #region CRUD
        public Computer Create(Computer computer)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = computer.DistinguishedName; 
                PrincipalContext pc = new PrincipalContext(ContextType.Machine,computer.SamAccountName, credential.Path);
                ComputerPrincipal computerPrincipal = new ComputerPrincipal(pc);
                computerPrincipal = ComputerPrincipal.FindByIdentity(pc, computer.SamAccountName);
                
                if (computerPrincipal != null)
                {
                    ServerRepository sr = new ServerRepository(_mySQLContext);
                    DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                    
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
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = computer.DistinguishedName;
                PrincipalContext pc = new PrincipalContext(ContextType.Machine, computer.SamAccountName, credential.Path);
                ComputerPrincipal computerPrincipal = new ComputerPrincipal(pc);
                computerPrincipal = ComputerPrincipal.FindByIdentity(pc, computer.SamAccountName);
                if (computerPrincipal != null)
                {
                    ServerRepository sr = new ServerRepository(_mySQLContext);
                    DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
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
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = computer.DistinguishedName;
                PrincipalContext pc = new PrincipalContext(ContextType.Machine, computer.SamAccountName, credential.Path);
                ComputerPrincipal computerPrincipal = new ComputerPrincipal(pc);
                computerPrincipal = ComputerPrincipal.FindByIdentity(pc, computer.SamAccountName);
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
        private Computer FindOne(string dn, string campo, string valor)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);//domain, ObjectApplication.Category.user);
                credential.DN = dn;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
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
                    return GetProperties(computer, result.Properties);
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
