using AgentNetCore.Application;
using AgentNetCore.Model;
using AgentNetCore.Repository;
using AgentNetCore.Service;
using System;
using System.Collections.Generic;
using System.DirectoryServices;

namespace AgentNetCore.Context
{
    public class OrganizationalUnitRepository
    {
        private readonly MySQLContext _mySQLContext;
        public OrganizationalUnitRepository(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
            //CreateMarvelStructure(); //Cria Mock
        }
        #region CRUD
        public OrganizationalUnit Create(OrganizationalUnit orgUnit)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = orgUnit.DistinguishedName;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("OU=" + orgUnit.Ou);
                SearchResult result = search.FindOne();
                if (result == null)
                {
                    DirectoryEntry newOrganizationalUnit = dirEntry.Children.Add("OU=" + orgUnit.Ou, ObjectApplication.Category.organizationalUnit.ToString());
                    if (!String.IsNullOrWhiteSpace(orgUnit.Description))
                    {
                        newOrganizationalUnit.Properties["description"].Value = orgUnit.Description;
                    }
                    if (!String.IsNullOrWhiteSpace(orgUnit.City))
                    {
                        newOrganizationalUnit.Properties["l"].Value = orgUnit.City;
                    }
                    if (!String.IsNullOrWhiteSpace(orgUnit.State))
                    {
                        newOrganizationalUnit.Properties["st"].Value = orgUnit.State;
                    }
                    if (!String.IsNullOrWhiteSpace(orgUnit.Street))
                    {
                        newOrganizationalUnit.Properties["street"].Value = orgUnit.Street;
                    }
                    if (!String.IsNullOrWhiteSpace(orgUnit.PostalCode))
                    {
                        newOrganizationalUnit.Properties["postalcode"].Value = orgUnit.PostalCode;
                    }
                    if (!String.IsNullOrWhiteSpace(orgUnit.Country))
                    {
                        newOrganizationalUnit.Properties["c"].Value = orgUnit.Country;
                    }
                    if (!String.IsNullOrWhiteSpace(orgUnit.Manager))
                    {
                        newOrganizationalUnit.Properties["managedBy"].Value = orgUnit.Manager;
                    }
                    newOrganizationalUnit.CommitChanges();
                    dirEntry.Close();
                    newOrganizationalUnit.Close();
                    return FindByName(credential, orgUnit.Name);
                }
                else
                {
                    Console.WriteLine("\r\nO Unidade Organizacional já existe no contexto:\r\n\t");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                if (e.Message.Equals("The object already exists.\r\n"))
                {

                    Console.WriteLine("\r\nO Unidade Organizacional já existe no contexto:\r\n\t" + e.GetType() + ":" + e.Message);
                }
                return null;
            }
        }
        public List<OrganizationalUnit> FindAll()
        {
            ConfigurationRepository config = new ConfigurationRepository(_mySQLContext);
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            credential.DN = config.GetConfiguration("DefaultDN");
            return FindAll(credential);
        }
        private List<OrganizationalUnit> FindAll(CredentialRepository credential)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                List<OrganizationalUnit> orgList = new List<OrganizationalUnit>();
                search.Filter = "(objectCategory=organizationalUnit)";
                var orgUnitsResult = search.FindAll();
                List<SearchResult> results = new List<SearchResult>();
                foreach (SearchResult orgUnitResult in orgUnitsResult)
                {
                    orgList.Add(GetResult(orgUnitResult));
                }
                return orgList;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        private OrganizationalUnit FindOne(CredentialRepository credential, string campo, string valor)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = "(" + campo + "=" + valor + ")";
                var orgUnit = GetResult(search.FindOne());
                if ((orgUnit.DistinguishedName == null) && (orgUnit.SamAccountName == null)) return null;
                return orgUnit;
                //CredentialRepository credential = new CredentialRepository(_mySQLContext);
                //ServerRepository sr = new ServerRepository(_mySQLContext);
                //OrganizationalUnit organizationalUnit = new OrganizationalUnit();
                //credential.DN = dn;
                //DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                //DirectorySearcher search = new DirectorySearcher(dirEntry);
                //search.Filter = "(" + campo + "=" + valor + ")";
                //search.SearchScope = SearchScope.Subtree;
                //search.PropertiesToLoad.Add("name");
                //search.PropertiesToLoad.Add("displayName");
                //search.PropertiesToLoad.Add("description");
                //search.PropertiesToLoad.Add("samaccountname");
                //search.PropertiesToLoad.Add("managedby");
                //search.PropertiesToLoad.Add("adspath");
                //search.PropertiesToLoad.Add("l");
                //search.PropertiesToLoad.Add("st");
                //search.PropertiesToLoad.Add("postalcode");
                //search.PropertiesToLoad.Add("c");
                //search.PropertiesToLoad.Add("mail");
                //search.PropertiesToLoad.Add("objectsid");
                //search.PropertiesToLoad.Add("whenchanged");
                //search.PropertiesToLoad.Add("whencreated");
                //search.PropertiesToLoad.Add("distinguishedname");
                //search.PropertiesToLoad.Add("street");
                //search.PropertiesToLoad.Add("iscriticalsystemobject");
                //search.PropertiesToLoad.Add("cn");
                //organizationalUnit = GetResult(search.FindOne());
                //return organizationalUnit;
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public OrganizationalUnit FindByName(string dn, string name)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                return FindOne(credential, "name", name);
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public OrganizationalUnit FindByName(CredentialRepository credential, string name)
        {
            try
            {
                return FindOne(credential, "name", name);
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public OrganizationalUnit FindByOu(string dn, string ou)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                return FindOne(credential, "ou", ou);
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        public List<OrganizationalUnit> FindByDn(string dn)
        {
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            credential.DN = dn;
            return FindAll(credential);
        }
        public OrganizationalUnit Update(OrganizationalUnit orgUnit)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = orgUnit.DistinguishedName;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("ou=" + orgUnit.SamAccountName);
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    DirectoryEntry newOrganizationalUnit = result.GetDirectoryEntry();
                    if (!String.IsNullOrWhiteSpace(orgUnit.Description))
                    {
                        newOrganizationalUnit.Properties["description"].Value = orgUnit.Description;
                    }
                    if (!String.IsNullOrWhiteSpace(orgUnit.City))
                    {
                        newOrganizationalUnit.Properties["l"].Value = orgUnit.City;
                    }
                    if (!String.IsNullOrWhiteSpace(orgUnit.State))
                    {
                        newOrganizationalUnit.Properties["st"].Value = orgUnit.State;
                    }
                    if (!String.IsNullOrWhiteSpace(orgUnit.State))
                    {
                        newOrganizationalUnit.Properties["street"].Value = orgUnit.Street;
                    }
                    if (!String.IsNullOrWhiteSpace(orgUnit.State))
                    {
                        newOrganizationalUnit.Properties["postalcode"].Value = orgUnit.PostalCode;
                    }
                    if (!String.IsNullOrWhiteSpace(orgUnit.State))
                    {
                        newOrganizationalUnit.Properties["c"].Value = orgUnit.Country;
                    }
                    if (!String.IsNullOrWhiteSpace(orgUnit.State))
                    {
                        newOrganizationalUnit.Properties["managedBy"].Value = orgUnit.Manager;
                    }
                    newOrganizationalUnit.CommitChanges();
                    dirEntry.Close();
                    newOrganizationalUnit.Close();
                    return FindByName(credential, orgUnit.Name);
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
        public bool Delete(OrganizationalUnit orgUnit)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = orgUnit.DistinguishedName;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("distinguishedname=" + orgUnit.DistinguishedName);
                DirectoryEntry orgUnitFind = (search.FindOne()).GetDirectoryEntry();
                if (orgUnitFind != null)
                {
                    orgUnitFind.DeleteTree();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return false;
            }
        }
        public void CreateMarvelStructure()
        {
            try
            {
                List<OrganizationalUnit> organizationalUnitList = new List<OrganizationalUnit>(SetOrganizarionUnitMarvel());
                foreach (var organizarionUnit in organizationalUnitList)
                {
                    Create(organizarionUnit);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
        }
        #endregion
        private List<OrganizationalUnit> SetOrganizarionUnitMarvel()
        {
            List<OrganizationalUnit> organizationalUnitMarvel = new List<OrganizationalUnit>();
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Teste API", Description = "Descricao TesteAPI", ProtectDeletion = true, SamAccountName = "Teste API", DistinguishedName = "dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Domain Controllers", Description = "Descrição Domain Controllers", ProtectDeletion = true, SamAccountName = "Domain Controllers", DistinguishedName = "dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Domain Members", Description = "Descrição Domain Members", ProtectDeletion = true, SamAccountName = "Domain Members", DistinguishedName = "dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Company", Description = "Descrição Marvel Company", ProtectDeletion = true, SamAccountName = "Marvel Company", DistinguishedName = "dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "EXCH-PublicFolders", Description = "Descrição EXCH-PublicFolders", ProtectDeletion = true, SamAccountName = "EXCH-PublicFolders", DistinguishedName = "dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "IIFP Contatos", Description = "Descrição IIFP Contatos", ProtectDeletion = true, SamAccountName = "IIFP Contatos", DistinguishedName = "dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "IIFP Swap", Description = "Descrição IIFP Swap", ProtectDeletion = true, SamAccountName = "IIFP Swap", DistinguishedName = "dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "IIFP Usuario", Description = "Descrição IIFP Usuario", ProtectDeletion = true, SamAccountName = "IIFP Usuario", DistinguishedName = "dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Limpeza Computers", Description = "Descrição LimpezaComputers", ProtectDeletion = true, SamAccountName = "Limpeza Computers", DistinguishedName = "dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Microsoft Exchange Security Groups", Description = "Descrição Microsoft Exchange Security Groups", ProtectDeletion = true, SamAccountName = "Microsoft Exchange Security Groups", DistinguishedName = "dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Teste", Description = "Descrição Teste", ProtectDeletion = true, SamAccountName = "Teste", DistinguishedName = "dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Users", Description = "Descrição Users", ProtectDeletion = true, SamAccountName = "Users", DistinguishedName = "dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Usuarios Marvel Company", Description = "Descrição Usuarios Marvel Company", ProtectDeletion = true, SamAccountName = "UsuariosMarvelCompany", DistinguishedName = "dc=marveldomain,dc=local" });

            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Banco Marvel", Description = "BancoMarvel", ProtectDeletion = true, SamAccountName = "BancoMarvel", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Field Marvel", Description = "FieldMarvel", ProtectDeletion = true, SamAccountName = "FieldMarvel", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marveltech", Description = "Marveltech", ProtectDeletion = true, SamAccountName = "Marveltech", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvelcoop", Description = "Marvelcoop", ProtectDeletion = true, SamAccountName = "Marvelcoop", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Mar-vel", Description = "Mar-vel", ProtectDeletion = true, SamAccountName = "Mar-vel", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Brothers Holding", Description = "BrothersHolding", ProtectDeletion = true, SamAccountName = "BrothersHolding", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Flows Marvel", Description = "FlowsMarvel", ProtectDeletion = true, SamAccountName = "FlowsMarvel", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Holding", Description = "MarvelHolding", ProtectDeletion = true, SamAccountName = "MarvelHolding", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Instituto Marvel", Description = "InstitutoMarvel", ProtectDeletion = true, SamAccountName = "InstitutoMarvel", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Connect", Description = "MarvelConnect", ProtectDeletion = true, SamAccountName = "MarvelConnect", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Disney", Description = "Disney", ProtectDeletion = true, SamAccountName = "Disney", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "MarvelTech", Description = "MarvelTech", ProtectDeletion = true, SamAccountName = "MarvelTech", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel", Description = "Marvel", ProtectDeletion = true, SamAccountName = "Marvel", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Consorcios", Description = "MarvelConsorcios", ProtectDeletion = true, SamAccountName = "MarvelConsorcios", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Prev", Description = "MarvelPrev", ProtectDeletion = true, SamAccountName = "MarvelPrev", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Mobile", Description = "MarvelMobile", ProtectDeletion = true, SamAccountName = "MarvelMobile", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "MVL", Description = "MVL", ProtectDeletion = true, SamAccountName = "MVL", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Apple", Description = "Apple", ProtectDeletion = true, SamAccountName = "Apple", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvinia", Description = "Marvinia", ProtectDeletion = true, SamAccountName = "Marvinia", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Ser Marvel", Description = "SerMarvel", ProtectDeletion = true, SamAccountName = "SerMarvel", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Shared", Description = "Shared", ProtectDeletion = true, SamAccountName = "Shared", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Saude", Description = "MarvelSaude", ProtectDeletion = true, SamAccountName = "MarvelSaude", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Heros", Description = "MarvelHeros", ProtectDeletion = true, SamAccountName = "MarvelHeros", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Third-Party", Description = "Third-Party", ProtectDeletion = true, SamAccountName = "Third-Party", DistinguishedName = "ou=Marvel Company,dc=marveldomain,dc=local" });

            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "CPT-SERVER", Description = "Servers of Marvel Company", ProtectDeletion = true, SamAccountName = "CPT-SERVER", DistinguishedName = "ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "HIPER-V", Description = "Virtual Hiper-V Servers of Marvel Company", ProtectDeletion = true, SamAccountName = "HIPER-V", DistinguishedName = "ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "CPT-HOSTS", Description = "Hosts of Marvel Company", ProtectDeletion = true, SamAccountName = "CPT-HOSTS", DistinguishedName = "ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "CPT-SGBD", Description = "SQL and MySql servers of Marvel Company", ProtectDeletion = true, SamAccountName = "CPT-SGBD", DistinguishedName = "ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "VMWARE", Description = "Virtual VMWare Servers of Marvel Company", ProtectDeletion = true, SamAccountName = "VMWARE", DistinguishedName = "ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "TERMINAL", Description = "Terminal servicer of Marvel Company", ProtectDeletion = true, SamAccountName = "TERMINAL", DistinguishedName = "ou=Domain Members,dc=marveldomain,dc=local" });


            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "United States", Description = "United States unit", ProtectDeletion = true, SamAccountName = "UnitedStates", DistinguishedName = "ou=Marvel,ou=Marvel Company,dc=marveldomain,dc=local" });

            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Computers", Description = "Computers", ProtectDeletion = true, SamAccountName = "Computers", DistinguishedName = "ou=UnitedStates,ou=Marvel,ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Groups", Description = "Groups", ProtectDeletion = true, SamAccountName = "Groups", DistinguishedName = "ou=UnitedStates,ou=Marvel,ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Users", Description = "Users", ProtectDeletion = true, SamAccountName = "Users", DistinguishedName = "ou=UnitedStates,ou=Marvel,ou=Marvel Company,dc=marveldomain,dc=local" });

            return organizationalUnitMarvel;
        }
        private OrganizationalUnit GetResult(SearchResult result)
        {
            OrganizationalUnit organizationalUnit = new OrganizationalUnit();
            try
            {
                if (result != null)
                {
                    return GetProperties(organizationalUnit, result.Properties);
                }
                else
                {
                    Console.WriteLine("Organizational Unit not found!");
                    return null;
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        private OrganizationalUnit GetProperties(OrganizationalUnit organizationalUnit, ResultPropertyCollection fields)
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
                            case "name":
                                organizationalUnit.Name = myCollection.ToString();
                                break;
                            case "displayName":
                                organizationalUnit.DisplayName = myCollection.ToString();
                                break;
                            case "description":
                                organizationalUnit.Description = myCollection.ToString();
                                break;
                            case "samaccountname":
                                organizationalUnit.SamAccountName = myCollection.ToString();
                                break;
                            case "managedby":
                                organizationalUnit.Manager = myCollection.ToString();
                                break;
                            case "adspath":
                                organizationalUnit.PathDomain = myCollection.ToString();
                                break;
                            case "l":
                                organizationalUnit.City = myCollection.ToString();
                                break;
                            case "st":
                                organizationalUnit.State = myCollection.ToString();
                                break;
                            case "postalcode":
                                organizationalUnit.PostalCode = myCollection.ToString();
                                break;
                            case "c":
                                organizationalUnit.Country = myCollection.ToString();
                                break;
                            case "mail":
                                organizationalUnit.Email = myCollection.ToString();
                                break;
                            case "objectsid":
                                organizationalUnit.ObjectSid = myCollection.ToString();
                                break;
                            case "whenchanged":
                                organizationalUnit.WhenChanged = myCollection.ToString();
                                break;
                            case "whencreated":
                                organizationalUnit.WhenCreated = myCollection.ToString();
                                break;
                            case "ou":
                                organizationalUnit.Ou = myCollection.ToString();
                                break;
                            case "distinguishedname":
                                organizationalUnit.DistinguishedName = myCollection.ToString();
                                break;
                            case "street":
                                organizationalUnit.Street = myCollection.ToString();
                                break;
                            case "iscriticalsystemobject":
                                organizationalUnit.IsCriticalSystemObject = (bool)myCollection;
                                break;
                            case "cn":
                                organizationalUnit.CommonName = myCollection.ToString();
                                break;
                        }
                        Console.WriteLine(String.Format("{0,-20} : {1}", ldapField, myCollection.ToString()));
                    }
                }
                return organizationalUnit;
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
    }
}
