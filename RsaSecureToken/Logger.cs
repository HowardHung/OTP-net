namespace RsaSecureToken
{
    internal class Logger : ILogger
    {
        public string Save(string account)
        {
            return $"account: {account} try to log fail";
        }
    }
}