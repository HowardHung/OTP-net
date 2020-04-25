using System;
using System.Collections.Generic;

namespace RsaSecureToken
{
    public class AuthenticationService
    {
        private readonly IProfile _profile;
        private readonly IToken _rsaTokenDao;
        private readonly ILogger _logger;

        public AuthenticationService()
        {
            _logger = new Logger();
            _profile = new Profile();
            _rsaTokenDao = new RsaTokenDao();
        }

        public AuthenticationService(IProfile profile, IToken rsaTokenDao, ILogger logger)
        {
            _profile = profile;
            _rsaTokenDao = rsaTokenDao;
            _logger = logger;
        }

        public bool IsValid(string account, string password)
        {
            // 根據 account 取得自訂密碼
            var passwordFromDao = _profile.GetPassword(account);

            // 根據 account 取得 RSA token 目前的亂數
            var randomCode = _rsaTokenDao.GetRandom(account);

            // 驗證傳入的 password 是否等於自訂密碼 + RSA token亂數
            var validPassword = passwordFromDao + randomCode;
            var isValid = password == validPassword;

            if (isValid)
            {
                return true;
            }
            else
            {
                _logger.Save(account);
                return false;
            }
        }
    }

    public class Profile : IProfile
    {
        public string GetPassword(string account)
        {
            return Context.GetPassword(account);
        }
    }

    public static class Context
    {
        public static Dictionary<string, string> profiles;

        static Context()
        {
            profiles = new Dictionary<string, string>();
            profiles.Add("joey", "91");
            profiles.Add("mei", "99");
        }

        public static string GetPassword(string key)
        {
            return profiles[key];
        }
    }

    public class RsaTokenDao : IToken
    {
        public string GetRandom(string account)
        {
            var seed = new Random((int) DateTime.Now.Ticks & 0x0000FFFF);
            var result = seed.Next(0, 999999).ToString("000000");
            Console.WriteLine("randomCode:{0}", result);

            return result;
        }
    }
}