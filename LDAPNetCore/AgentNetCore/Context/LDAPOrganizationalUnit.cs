using AgentNetCore.Model;
using AgentNetCore.Service;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AgentNetCore.Context
{
    public class LDAPOrganizationalUnit : IOrganizationalUnitService
    {
        private LDAPConnect _connect;
        private DirectoryEntry _dirEntry;
        private DirectorySearcher _search;
        public LDAPOrganizationalUnit(string domain)
        {
            _connect = new LDAPConnect(domain,LDAPConnect.ObjectCategory.organizationalUnit);
            _dirEntry = new DirectoryEntry(_connect.Path, _connect.User, _connect.Pass);
            _search = new DirectorySearcher(_dirEntry);
            CreateRandonStructure();
        }
        #region CRUD
        public OrganizationalUnit Create(OrganizationalUnit organizationalUnit)
        {
            try
            {
                return SetProperties(organizationalUnit);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                if (e.Message.Equals("The object already exists.\r\n"))
                {

                    Console.WriteLine("\r\nO Unidade Organizacional já existe no contexto:\r\n\t" + e.GetType() + ":" + e.Message);
                }
                return organizationalUnit;
            }
        }
        public void CreateRandonStructure()
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
        private OrganizationalUnit SetProperties(OrganizationalUnit organizationalUnit)
        {
            _dirEntry.Path = organizationalUnit.PathDomain;
            DirectoryEntry newOrganizationalUnit = _dirEntry.Children.Add("OU=" + organizationalUnit.SamAccountName, "organizationalUnit");
            newOrganizationalUnit.Properties["description"].Value = organizationalUnit.Description;
            newOrganizationalUnit.Properties["l"].Value = "Caxias do Sul";
            newOrganizationalUnit.Properties["st"].Value = "Rio Grande do Sul";
            newOrganizationalUnit.Properties["street"].Value = "Rua para Teste"; 
            newOrganizationalUnit.Properties["postalcode"].Value = "95095495";
            newOrganizationalUnit.Properties["c"].Value = "US";
            newOrganizationalUnit.Properties["managedBy"].Value = "CN=" + "Ghost Rider" + "," + "cn=Users,dc=marveldomain,dc=local";
            if (organizationalUnit.ProtectDeletion)
            { 
            
            }
            newOrganizationalUnit.CommitChanges();
            _dirEntry.Close();
            newOrganizationalUnit.Close();
            return organizationalUnit;
        }
        #endregion

        private List<OrganizationalUnit> SetOrganizarionUnitMarvel()
        {
            List<OrganizationalUnit> organizationalUnitMarvel = new List<OrganizationalUnit>();
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Teste API",                           Description = "Descricao TesteAPI",                           ProtectDeletion = true, SamAccountName = "Teste API",                          PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Domain Controllers",                  Description = "Descrição Domain Controllers",                 ProtectDeletion = true, SamAccountName = "Domain Controllers",                 PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Domain Members",                      Description = "Descrição Domain Members",                     ProtectDeletion = true, SamAccountName = "Domain Members",                     PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Company",                      Description = "Descrição Marvel Company",                     ProtectDeletion = true, SamAccountName = "Marvel Company",                     PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "EXCH-PublicFolders",                  Description = "Descrição EXCH-PublicFolders",                 ProtectDeletion = true, SamAccountName = "EXCH-PublicFolders",                 PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "IIFP Contatos",                       Description = "Descrição IIFP Contatos",                      ProtectDeletion = true, SamAccountName = "IIFP Contatos",                      PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "IIFP Swap",                           Description = "Descrição IIFP Swap",                          ProtectDeletion = true, SamAccountName = "IIFP Swap",                          PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "IIFP Usuario",                        Description = "Descrição IIFP Usuario",                       ProtectDeletion = true, SamAccountName = "IIFP Usuario",                       PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Limpeza Computers",                   Description = "Descrição LimpezaComputers",                   ProtectDeletion = true, SamAccountName = "Limpeza Computers",                  PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Microsoft Exchange Security Groups",  Description = "Descrição Microsoft Exchange Security Groups", ProtectDeletion = true, SamAccountName = "Microsoft Exchange Security Groups", PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Teste",                               Description = "Descrição Teste",                              ProtectDeletion = true, SamAccountName = "Teste",                              PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Users",                               Description = "Descrição Users",                              ProtectDeletion = true, SamAccountName = "Users",                              PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Usuarios Marvel Company",             Description = "Descrição Usuarios Marvel Company",            ProtectDeletion = true, SamAccountName = "UsuariosMarvelCompany",              PathDomain = "LDAP://192.168.0.99:389/dc=marveldomain,dc=local" });

            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Banco Marvel",                        Description = "BancoMarvel",                                  ProtectDeletion = true, SamAccountName = "BancoMarvel",                         PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Field Marvel",                        Description = "FieldMarvel",                                  ProtectDeletion = true, SamAccountName = "FieldMarvel",                         PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marveltech",                          Description = "Marveltech",                                   ProtectDeletion = true, SamAccountName = "Marveltech",                          PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvelcoop",                          Description = "Marvelcoop",                                   ProtectDeletion = true, SamAccountName = "Marvelcoop",                          PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Mar-vel",                             Description = "Mar-vel",                                      ProtectDeletion = true, SamAccountName = "Mar-vel",                             PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Brothers Holding",                    Description = "BrothersHolding",                              ProtectDeletion = true, SamAccountName = "BrothersHolding",                     PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Flows Marvel",                        Description = "FlowsMarvel",                                  ProtectDeletion = true, SamAccountName = "FlowsMarvel",                         PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Holding",                      Description = "MarvelHolding",                                ProtectDeletion = true, SamAccountName = "MarvelHolding",                       PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Instituto Marvel",                    Description = "InstitutoMarvel",                              ProtectDeletion = true, SamAccountName = "InstitutoMarvel",                     PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Connect",                      Description = "MarvelConnect",                                ProtectDeletion = true, SamAccountName = "MarvelConnect",                       PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Disney",                              Description = "Disney",                                       ProtectDeletion = true, SamAccountName = "Disney",                              PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "MasterTech",                          Description = "MasterTech",                                   ProtectDeletion = true, SamAccountName = "MasterTech",                          PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel",                              Description = "Marvel",                                       ProtectDeletion = true, SamAccountName = "Marvel",                              PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Consorcios",                   Description = "MarvelConsorcios",                             ProtectDeletion = true, SamAccountName = "MarvelConsorcios",                    PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Prev",                         Description = "MarvelPrev",                                   ProtectDeletion = true, SamAccountName = "MarvelPrev",                          PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Mobile",                       Description = "MarvelMobile",                                 ProtectDeletion = true, SamAccountName = "MarvelMobile",                        PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "MVL",                                 Description = "MVL",                                          ProtectDeletion = true, SamAccountName = "MVL",                                 PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Apple",                               Description = "Apple",                                        ProtectDeletion = true, SamAccountName = "Apple",                               PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvinia",                            Description = "Marvinia",                                     ProtectDeletion = true, SamAccountName = "Marvinia",                            PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Ser Marvel",                          Description = "SerMarvel",                                    ProtectDeletion = true, SamAccountName = "SerMarvel",                           PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Shared",                              Description = "Shared",                                       ProtectDeletion = true, SamAccountName = "Shared",                              PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Saude",                        Description = "MarvelSaude",                                  ProtectDeletion = true, SamAccountName = "MarvelSaude",                         PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Marvel Heros",                        Description = "MarvelHeros",                                  ProtectDeletion = true, SamAccountName = "MarvelHeros",                         PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Third-Party",                         Description = "Third-Party",                                  ProtectDeletion = true, SamAccountName = "Castertech",                          PathDomain = "LDAP://192.168.0.99:389/ou=Marvel Company,dc=marveldomain,dc=local" });
 
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "CPT-SERVER",                          Description = "Servers of Marvel Company",                     ProtectDeletion = true, SamAccountName = "CPT-SERVER",                         PathDomain = "LDAP://192.168.0.99:389/ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "HIPER-V",                             Description = "Virtual Hiper-V Servers of Marvel Company",     ProtectDeletion = true, SamAccountName = "HIPER-V",                            PathDomain = "LDAP://192.168.0.99:389/ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "CPT-HOSTS",                           Description = "Hosts of Marvel Company",                       ProtectDeletion = true, SamAccountName = "CPT-HOSTS",                          PathDomain = "LDAP://192.168.0.99:389/ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "CPT-SGBD",                            Description = "SQL and MySql servers of Marvel Company",       ProtectDeletion = true, SamAccountName = "CPT-SGBD",                           PathDomain = "LDAP://192.168.0.99:389/ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "VMWARE",                              Description = "Virtual VMWare Servers of Marvel Company",      ProtectDeletion = true, SamAccountName = "VMWARE",                             PathDomain = "LDAP://192.168.0.99:389/ou=Domain Members,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "TERMINAL",                            Description = "Terminal servicer of Marvel Company",           ProtectDeletion = true, SamAccountName = "TERMINAL",                           PathDomain = "LDAP://192.168.0.99:389/ou=Domain Members,dc=marveldomain,dc=local" });


            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "United States",                       Description = "United States unit",                            ProtectDeletion = true, SamAccountName = "UnitedStates",                       PathDomain = "LDAP://192.168.0.99:389/ou=Marvel,ou=Marvel Company,dc=marveldomain,dc=local" });

            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Computers",                           Description = "Computers",                                     ProtectDeletion = true, SamAccountName = "Computers",                          PathDomain = "LDAP://192.168.0.99:389/ou=UnitedStates,ou=Marvel,ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Groups",                              Description = "Groups",                                        ProtectDeletion = true, SamAccountName = "Groups",                             PathDomain = "LDAP://192.168.0.99:389/ou=UnitedStates,ou=Marvel,ou=Marvel Company,dc=marveldomain,dc=local" });
            organizationalUnitMarvel.Add(new OrganizationalUnit { DisplayName = "Users",                               Description = "Users",                                         ProtectDeletion = true, SamAccountName = "Users",                              PathDomain = "LDAP://192.168.0.99:389/ou=UnitedStates,ou=Marvel,ou=Marvel Company,dc=marveldomain,dc=local" });

            return organizationalUnitMarvel;
        }

        public OrganizationalUnit FindBySamName(string samName)
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
        private OrganizationalUnit FindOne(string campo, string valor)
        {
            try
            {
                OrganizationalUnit organizationalUnit = new OrganizationalUnit();
                _search.Filter = "(" + campo + "=" + valor + ")";
                organizationalUnit = GetResult(_search.FindOne());
                return organizationalUnit;
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        private OrganizationalUnit GetResult(SearchResult result)
        {
            OrganizationalUnit organizationalUnit = new OrganizationalUnit();
            try
            {
                if (result != null)
                {
                    ResultPropertyCollection fields = result.Properties;
                    return GetProperties(organizationalUnit, fields);
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
                foreach (String ldapField in fields.PropertyNames)
                {
                    foreach (Object myCollection in fields[ldapField])
                    {

                        switch (ldapField)
                        {
                            case "cn":
                                organizationalUnit.DisplayName = myCollection.ToString();
                                break;
                            case "samaccountname":
                                organizationalUnit.DisplayName = myCollection.ToString();
                                break;
                            case "displayName":
                                organizationalUnit.DisplayName = myCollection.ToString();
                                break;
                            case "description":
                                organizationalUnit.Description = myCollection.ToString();
                                break;
                            //case "mail":
                            //    organizationalUnit.EmailAddress = myCollection.ToString();
                            //    break;
                            //case "managedby":
                            //    organizationalUnit.Manager = myCollection.ToString();
                            //    break;
                            //case "objectsid":
                            //    organizationalUnit.ObjectSid = myCollection.ToString();
                            //    break;
                            case "adspath":
                                organizationalUnit.PathDomain = myCollection.ToString();
                                break;
                        }
                        Console.WriteLine(String.Format("{0,-20} : {1}", ldapField, myCollection.ToString()));
                    }
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return organizationalUnit;
            }
            return organizationalUnit;
        }
        public OrganizationalUnit Update(Group organizationalUnit)
        {
            throw new NotImplementedException();
        }

        public List<OrganizationalUnit> FindAll()
        {
            throw new NotImplementedException();
        }

        public void Delete(string samName)
        {
            throw new NotImplementedException();
        }
    }
}
