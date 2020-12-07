using AgentNetCore.Application;
using AgentNetCore.Model;
using AgentNetCore.Repository;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace AgentNetCore.Context
{
    public class ComputerRepository
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
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + computer.SamAccountName);
                SearchResult result = search.FindOne();
                if (result == null)
                {
                    DirectoryEntry newComputer = dirEntry.Children.Add("CN=" + computer.SamAccountName, ObjectApplication.Category.computer.ToString());
                    if (!String.IsNullOrWhiteSpace(computer.SamAccountName))
                    {
                        newComputer.Properties["samAccountName"].Value = computer.SamAccountName.ToUpper() + "$";
                    }
                    if (!String.IsNullOrWhiteSpace(computer.Description))
                    {
                        newComputer.Properties["description"].Value = computer.Description;
                    }
                    if (!String.IsNullOrWhiteSpace(computer.DnsHostName))
                    {
                        newComputer.Properties["dnshostname"].Value = computer.DnsHostName;
                    }
                    if (!String.IsNullOrWhiteSpace(computer.Location))
                    {
                        newComputer.Properties["location"].Value = computer.Location;
                    }
                    if (true)
                    {
                        var computerType = unchecked((int)(ComputerType.PASSWD_NOTREQD | ComputerType.WORKSTATION_TRUST_ACCOUNT));
                        newComputer.Properties["userAccountControl"].Value = (ComputerType.PASSWD_NOTREQD | ComputerType.WORKSTATION_TRUST_ACCOUNT);
                    }
                    newComputer.CommitChanges();
                    dirEntry.Close();
                    newComputer.Close();
                    return FindBySamName(credential, computer.Name);
                }
                else
                {
                    Console.WriteLine("\r\nUser not identify:\r\n\t");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                if (e.Message.Equals("The object already exists.\r\n"))
                {

                    Console.WriteLine("\r\nO Usuario ja existe no contexto:\r\n\t" + e.GetType() + ":" + e.Message);
                }
                return null;
            }
        }
        public List<Computer> FindAll()
        {
            ConfigurationRepository config = new ConfigurationRepository(_mySQLContext);
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            credential.DN = config.GetConfiguration("DefaultDN");
            return FindAll(credential);
        }
        private List<Computer> FindAll(CredentialRepository credential)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                List<Computer> computerList = new List<Computer>();
                search.Filter = "(&(objectClass=computer))";
                var computersResult = search.FindAll();
                List<SearchResult> results = new List<SearchResult>();
                foreach (SearchResult computerResult in computersResult)
                {
                    computerList.Add(GetResult(computerResult));
                }
                return computerList;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        private Computer FindOne(CredentialRepository credential, string campo, string valor)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = "(" + campo + "=" + valor + ")";
                var computer = GetResult(search.FindOne());
                if ((computer.DistinguishedName == null) && (computer.SamAccountName == null)) return null;
                return computer;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        public Computer FindBySamName(string dn, string samName)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                return FindOne(credential, "SamAccountName", samName);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public List<Computer> FindByDn(string dn)
        {
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            credential.DN = dn;
            return FindAll(credential);
        }
        public Computer Update(Computer computer)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = computer.DistinguishedName;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + computer.SamAccountName);
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    DirectoryEntry newComputer = result.GetDirectoryEntry();
                    if (!String.IsNullOrWhiteSpace(newComputer.Name))
                    {
                        newComputer.Rename("cn=" + newComputer.Name);
                    }
                    if (!String.IsNullOrWhiteSpace(computer.Description))
                    {
                        newComputer.Properties["description"].Value = computer.Description;
                    }
                    if (!String.IsNullOrWhiteSpace(computer.DnsHostName))
                    {
                        newComputer.Properties["dnshostname"].Value = computer.DnsHostName;
                    }
                    if (!String.IsNullOrWhiteSpace(computer.Location))
                    {
                        newComputer.Properties["location"].Value = computer.Location;
                    }
                    newComputer.CommitChanges();
                    dirEntry.Close();
                    newComputer.Close();
                    return FindBySamName(credential, computer.SamAccountName);
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
        public bool Delete(CredentialRepository credential, Computer computer)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("SamAccountName=" + computer.SamAccountName);
                DirectoryEntry computerFind = (search.FindOne()).GetDirectoryEntry();
                if (computerFind != null)
                {
                    computerFind.DeleteTree();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return false;
            }
        }
        public Computer FindBySamName(CredentialRepository credential, string samName)
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
                    Console.WriteLine("Computer not found!");
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
                            case "description":
                                computer.Description = myCollection.ToString();
                                break;
                            case "dnshostname":
                                computer.DnsHostName = myCollection.ToString();
                                break;
                            case "location":
                                computer.Location = myCollection.ToString();
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
        private enum ComputerType : uint
        {//https://docs.microsoft.com/en-us/windows/win32/adschema/c-group
            SCRIPT = 0x00000001,                          //1
            ACCOUNTDISABLE = 0x00000002,                  //2	
            HOMEDIR_REQUIRED = 0x00000008,                //8	
            LOCKOUT = 0x00000010,                         //16
            PASSWD_NOTREQD = 0x00000020,                  //32
            PASSWD_CANT_CHANGE = 0x00000040,              //64
            ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0x00000080, //128
            TEMP_DUPLICATE_ACCOUNT = 0x00000100,          //256
            NORMAL_ACCOUNT = 0x00000200,                  //512
            INTERDOMAIN_TRUST_ACCOUNT = 0x00000800,       //2048
            WORKSTATION_TRUST_ACCOUNT = 0x00001000,       //4096
            SERVER_TRUST_ACCOUNT = 0x00002000,	          //8192
            DONT_EXPIRE_PASSWORD = 0x00010000,	          //65536
            MNS_LOGON_ACCOUNT = 0x00020000,               //131072
            SMARTCARD_REQUIRED = 0x00040000,	          //262144
            TRUSTED_FOR_DELEGATION = 0x00080000,          //524288
            NOT_DELEGATED = 0x00100000                    //1048576
        }
    }
}
