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
                search.Filter = ("ou=" + orgUnit.SamAccountName);
                SearchResult result = search.FindOne();
                if (result == null)
                {
                    DirectoryEntry newOrganizationalUnit = dirEntry.Children.Add("OU=" + orgUnit.SamAccountName, ObjectApplication.Category.organizationalUnit.ToString());
                    newOrganizationalUnit.Properties["description"].Value = orgUnit.Description;
                    newOrganizationalUnit.Properties["l"].Value = orgUnit.City; //"Caxias do Sul";
                    newOrganizationalUnit.Properties["st"].Value = orgUnit.State; //"Rio Grande do Sul";
                    newOrganizationalUnit.Properties["street"].Value = orgUnit.Street; //"Rua para Teste";
                    newOrganizationalUnit.Properties["postalcode"].Value = orgUnit.PostalCode; //"95095495";
                    newOrganizationalUnit.Properties["c"].Value = orgUnit.Country; //"US";
                    newOrganizationalUnit.Properties["managedBy"].Value = orgUnit.Manager; //"CN=" + "Ghost Rider" + "," + "cn=Users,dc=marveldomain,dc=local";
                    newOrganizationalUnit.CommitChanges();
                    dirEntry.Close();
                    newOrganizationalUnit.Close();
                    return orgUnit;
                }
                else
                {
                    Console.WriteLine("\r\nO Usuario ja existe no contexto:\r\n\t");
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
            return FindAll(config.GetConfiguration("DefaultDomain"));
        }
        public List<OrganizationalUnit> FindAll(string dn)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
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
        public OrganizationalUnit FindByName(string domain, string nameOU)
        {
            try
            {
                return FindOne(domain, "ou", nameOU);
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        private OrganizationalUnit FindOne(string dn, string campo, string valor)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                ServerRepository sr = new ServerRepository(_mySQLContext);
                OrganizationalUnit organizationalUnit = new OrganizationalUnit();
                credential.DN = dn;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = "(" + campo + "=" + valor + ")";
                search.SearchScope = SearchScope.Subtree;
                search.PropertiesToLoad.Add("name");
                search.PropertiesToLoad.Add("displayName");
                search.PropertiesToLoad.Add("description");
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("managedby");
                search.PropertiesToLoad.Add("adspath");
                search.PropertiesToLoad.Add("l");
                search.PropertiesToLoad.Add("st");
                search.PropertiesToLoad.Add("postalcode");
                search.PropertiesToLoad.Add("c");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("objectsid");
                search.PropertiesToLoad.Add("whenchanged");
                search.PropertiesToLoad.Add("whencreated");
                search.PropertiesToLoad.Add("distinguishedname");
                search.PropertiesToLoad.Add("street");
                search.PropertiesToLoad.Add("iscriticalsystemobject");
                search.PropertiesToLoad.Add("cn");
                organizationalUnit = GetResult(search.FindOne());
                return organizationalUnit;
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
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
                    newOrganizationalUnit.Properties["description"].Value = orgUnit.Description;
                    newOrganizationalUnit.Properties["l"].Value = orgUnit.City; //"Caxias do Sul";
                    newOrganizationalUnit.Properties["st"].Value = orgUnit.State; //"Rio Grande do Sul";
                    newOrganizationalUnit.Properties["street"].Value = orgUnit.Street; //"Rua para Teste";
                    newOrganizationalUnit.Properties["postalcode"].Value = orgUnit.PostalCode; //"95095495";
                    newOrganizationalUnit.Properties["c"].Value = orgUnit.Country; //"US";
                    newOrganizationalUnit.Properties["managedBy"].Value = orgUnit.Manager; //"CN=" + "Ghost Rider" + "," + "cn=Users,dc=marveldomain,dc=local";
                    newOrganizationalUnit.CommitChanges();
                    dirEntry.Close();
                    newOrganizationalUnit.Close();
                    return FindOne(orgUnit.DistinguishedName, "ou", orgUnit.SamAccountName);
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
        public void Delete(OrganizationalUnit orgUnit)
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
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
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
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Teste API", Description = "Descricao TesteAPI", ProtectDeletion = true, SamAccountName = "Teste API", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Domain Controllers", Description = "Descrição Domain Controllers", ProtectDeletion = true, SamAccountName = "Domain Controllers", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Domain Members", Description = "Descrição Domain Members", ProtectDeletion = true, SamAccountName = "Domain Members", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Company", Description = "Descrição Marvel Company", ProtectDeletion = true, SamAccountName = "Marvel Company", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "EXCH-PublicFolders", Description = "Descrição EXCH-PublicFolders", ProtectDeletion = true, SamAccountName = "EXCH-PublicFolders", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "IIFP Contatos", Description = "Descrição IIFP Contatos", ProtectDeletion = true, SamAccountName = "IIFP Contatos", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "IIFP Swap", Description = "Descrição IIFP Swap", ProtectDeletion = true, SamAccountName = "IIFP Swap", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "IIFP Usuario", Description = "Descrição IIFP Usuario", ProtectDeletion = true, SamAccountName = "IIFP Usuario", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Limpeza Computers", Description = "Descrição LimpezaComputers", ProtectDeletion = true, SamAccountName = "Limpeza Computers", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Microsoft Exchange Security Groups", Description = "Descrição Microsoft Exchange Security Groups", ProtectDeletion = true, SamAccountName = "Microsoft Exchange Security Groups", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Teste", Description = "Descrição Teste", ProtectDeletion = true, SamAccountName = "Teste", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Users", Description = "Descrição Users", ProtectDeletion = true, SamAccountName = "Users", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Usuarios Marvel Company", Description = "Descrição Usuarios Marvel Company", ProtectDeletion = true, SamAccountName = "UsuariosMarvelCompany", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });

            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Banco Marvel", Description = "BancoMarvel", ProtectDeletion = true, SamAccountName = "BancoMarvel", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Field Marvel", Description = "FieldMarvel", ProtectDeletion = true, SamAccountName = "FieldMarvel", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marveltech", Description = "Marveltech", ProtectDeletion = true, SamAccountName = "Marveltech", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvelcoop", Description = "Marvelcoop", ProtectDeletion = true, SamAccountName = "Marvelcoop", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Mar-vel", Description = "Mar-vel", ProtectDeletion = true, SamAccountName = "Mar-vel", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Brothers Holding", Description = "BrothersHolding", ProtectDeletion = true, SamAccountName = "BrothersHolding", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Flows Marvel", Description = "FlowsMarvel", ProtectDeletion = true, SamAccountName = "FlowsMarvel", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Holding", Description = "MarvelHolding", ProtectDeletion = true, SamAccountName = "MarvelHolding", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Instituto Marvel", Description = "InstitutoMarvel", ProtectDeletion = true, SamAccountName = "InstitutoMarvel", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Connect", Description = "MarvelConnect", ProtectDeletion = true, SamAccountName = "MarvelConnect", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Disney", Description = "Disney", ProtectDeletion = true, SamAccountName = "Disney", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "MasterTech", Description = "MasterTech", ProtectDeletion = true, SamAccountName = "MasterTech", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel", Description = "Marvel", ProtectDeletion = true, SamAccountName = "Marvel", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Consorcios", Description = "MarvelConsorcios", ProtectDeletion = true, SamAccountName = "MarvelConsorcios", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Prev", Description = "MarvelPrev", ProtectDeletion = true, SamAccountName = "MarvelPrev", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Mobile", Description = "MarvelMobile", ProtectDeletion = true, SamAccountName = "MarvelMobile", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "MVL", Description = "MVL", ProtectDeletion = true, SamAccountName = "MVL", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Apple", Description = "Apple", ProtectDeletion = true, SamAccountName = "Apple", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvinia", Description = "Marvinia", ProtectDeletion = true, SamAccountName = "Marvinia", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Ser Marvel", Description = "SerMarvel", ProtectDeletion = true, SamAccountName = "SerMarvel", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Shared", Description = "Shared", ProtectDeletion = true, SamAccountName = "Shared", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Saude", Description = "MarvelSaude", ProtectDeletion = true, SamAccountName = "MarvelSaude", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Heros", Description = "MarvelHeros", ProtectDeletion = true, SamAccountName = "MarvelHeros", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Third-Party", Description = "Third-Party", ProtectDeletion = true, SamAccountName = "Castertech", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });

            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "CPT-SERVER", Description = "Servers of Marvel Company", ProtectDeletion = true, SamAccountName = "CPT-SERVER", PathDomain = "LDAP://192.168.0.99:389/ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "HIPER-V", Description = "Virtual Hiper-V Servers of Marvel Company", ProtectDeletion = true, SamAccountName = "HIPER-V", PathDomain = "LDAP://192.168.0.99:389/ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "CPT-HOSTS", Description = "Hosts of Marvel Company", ProtectDeletion = true, SamAccountName = "CPT-HOSTS", PathDomain = "LDAP://192.168.0.99:389/ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "CPT-SGBD", Description = "SQL and MySql servers of Marvel Company", ProtectDeletion = true, SamAccountName = "CPT-SGBD", PathDomain = "LDAP://192.168.0.99:389/ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "VMWARE", Description = "Virtual VMWare Servers of Marvel Company", ProtectDeletion = true, SamAccountName = "VMWARE", PathDomain = "LDAP://192.168.0.99:389/ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "TERMINAL", Description = "Terminal servicer of Marvel Company", ProtectDeletion = true, SamAccountName = "TERMINAL", PathDomain = "LDAP://192.168.0.99:389/ou=Domain Members,dc=marveldomain,dc=local" });


            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "United States", Description = "United States unit", ProtectDeletion = true, SamAccountName = "UnitedStates", PathDomain = "LDAP://192.168.0.99:389/ou=Marvel,ou=Marvel Company,dc=marveldomain,dc=local" });

            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Computers", Description = "Computers", ProtectDeletion = true, SamAccountName = "Computers", PathDomain = "LDAP://192.168.0.99:389/ou=UnitedStates,ou=Marvel,ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Groups", Description = "Groups", ProtectDeletion = true, SamAccountName = "Groups", PathDomain = "LDAP://192.168.0.99:389/ou=UnitedStates,ou=Marvel,ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Users", Description = "Users", ProtectDeletion = true, SamAccountName = "Users", PathDomain = "LDAP://192.168.0.99:389/ou=UnitedStates,ou=Marvel,ou=Marvel Company,dc=marveldomain,dc=local" });

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
