namespace RsaSecureToken
{
    public interface IToken
    {
        string GetRandom(string account);
    }
}