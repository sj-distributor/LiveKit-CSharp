using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LiveKit_CSharp.Auth
{
    public class AccessToken
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;

        private ClaimGrants Grant { get; set; }

        private TimeSpan TTL { get; set; } = TimeSpan.FromHours(12);

        public AccessToken(string apiKey, string apiSecret)
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                throw new Exception("api-key and api-secret must be set");
            }

            _apiKey = apiKey;
            _apiSecret = apiSecret;
            Grant = new ClaimGrants();
        }

        public AccessToken AddGrant(VideoGrant grant)
        {
            Grant.Video = grant;
            return this;
        }

        public AccessToken SetTTL(TimeSpan timeSpan)
        {
            TTL = timeSpan;
            return this;
        }

        public AccessToken SetIdentity(string identity)
        {
            Grant.Identity = identity;
            return this;
        }

        public AccessToken SetName(string name)
        {
            Grant.Name = name;
            return this;
        }

        public AccessToken Metadata(string metadata)
        {
            Grant.Metadata = metadata;
            return this;
        }

        public string ToJwt()
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSecret));

            var signingCredentials = new SigningCredentials(secretKey, Grant.Sha256);


            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(Grant.Identity))
            {
                claims.Add(new Claim("identity", Grant.Identity));
                claims.Add(new Claim("sub", Grant.Identity));
            }

            if (!string.IsNullOrEmpty(Grant.Metadata))
            {
                claims.Add(new Claim("metadata", Grant.Metadata));
            }

            if (!string.IsNullOrEmpty(Grant.Name))
            {
                claims.Add(new Claim("name", Grant.Name));
            }

            var jwtPayload = new JwtPayload(
                _apiKey,
                null,
                claims,
                DateTime.Now,
                DateTime.Now.Add(TTL));
            
            jwtPayload.Add("video", Grant.Video.ToDictionary());

            var token =  new JwtSecurityToken(
                new JwtHeader(signingCredentials),
                jwtPayload
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}