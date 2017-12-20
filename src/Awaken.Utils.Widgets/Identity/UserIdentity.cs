using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Awaken.Utils.Widgets
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo()
        {
        }
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="claims"></param>
        public UserInfo(IEnumerable<Claim> claims)
        {
            if (claims != null && claims.Any())
            {
                Id = Convert(claims, "sub");
                UserName = Convert(claims, "name");
                Phone = Convert(claims, "phone_number");
                NickName = Convert(claims, "nickname");
                Roles = ConvertList(claims, "role");

                //Scopes = ConvertList(claims, "scope");

                //if (long.TryParse(Convert(claims, "nbf"), out long nbfUnix)) {
                //    NotUseBefore = TimeConvert.UnixTimeToTime(nbfUnix);
                //}

                //if (long.TryParse(Convert(claims, "exp"), out long expUnix))
                //{
                //    ExpireTime = TimeConvert.UnixTimeToTime(expUnix);
                //}
            }

        }

        public string Id { get; private set; }
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string NickName { get; private set; }

        public IList<string> Roles { get; private set; }

        //public IList<string> Scopes { get; private set; }

        /// <summary>
        /// 认证生效时间
        /// </summary>
        //public DateTime NotUseBefore { get; private set; }

        /// <summary>
        /// 认证过期时间
        /// </summary>
        //public DateTime ExpireTime { get; private set; }



        private string Convert(IEnumerable<Claim> claims, string type)
        {
            var claim = claims.FirstOrDefault(p => p.Type == type);
            
            return (claim != null)? claim.Value:string.Empty;
        }

        private List<string> ConvertList(IEnumerable<Claim> claims, string type)
        {
            return claims.Where(p => p.Type == type)
                         .Select(p => p.Value)
                         .ToList() ;
        }
    }
    
}
