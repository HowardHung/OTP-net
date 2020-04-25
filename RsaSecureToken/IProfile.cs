namespace RsaSecureToken
{
    public interface IProfile
    {
        string GetPassword(string account);
    }
}