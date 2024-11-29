using System.DirectoryServices.AccountManagement;

public class LdapAuthenticationService
{
    private readonly string _domain;
    private readonly string _container;

    public LdapAuthenticationService(string domain, string container)
    {
        _domain = domain;
        _container = container;
    }

    public bool Authenticate(string username, string password)
    {
        using (var context = new PrincipalContext(ContextType.Domain, _domain, _container))
        {
            return context.ValidateCredentials(username, password);
        }
    }
}
