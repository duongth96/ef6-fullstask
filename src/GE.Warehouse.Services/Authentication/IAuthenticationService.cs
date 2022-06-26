namespace GE.Warehouse.Services.Authentication
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IAuthenticationService 
    {
        void SignIn(string username, bool createPersistentCookie);
        void SignOut();
    }
}