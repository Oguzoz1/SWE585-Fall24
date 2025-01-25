namespace Game.Authentication
{
    public interface IAuthenticatorService
    {
        public bool Authenticate(string token);
    }
}