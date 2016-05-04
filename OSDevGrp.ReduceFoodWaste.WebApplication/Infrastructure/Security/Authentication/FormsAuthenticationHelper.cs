using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using DotNetOpenAuth.AspNet;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Authentication
{
    public static class FormsAuthenticationHelper
    {
        #region Private constants

        private const string IdentityProviderClaimType = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";

        #endregion

        public static ClaimsIdentity ToClaimsIdentity(this AuthenticationResult authenticationResult)
        {
            if (authenticationResult == null)
            {
                throw new ArgumentNullException("authenticationResult");
            }

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, authenticationResult.ProviderUserId, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider),
                new Claim(ClaimTypes.Name, authenticationResult.UserName, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider),
                new Claim(IdentityProviderClaimType, authenticationResult.Provider, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider)
            };
            foreach (var extraData in authenticationResult.ExtraData.Where(m => string.IsNullOrWhiteSpace(m.Value) == false))
            {
                switch (extraData.Key)
                {
                    case "id":
                        // This has already been handled.
                        break;

                    case "name":
                        switch (authenticationResult.Provider)
                        {
                            case "facebook":
                                // Remove the existing claim for the name (it contains the mail address).
                                var nameClaim = claimCollection.Single(claim => string.Compare(claim.Type, ClaimTypes.Name, StringComparison.Ordinal) == 0);
                                claimCollection.Remove(nameClaim);
                                // Add new claim for the name containing the users name.
                                claimCollection.Add(new Claim(ClaimTypes.Name, extraData.Value, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                                break;
                        }
                        break;

                    case "link":
                        claimCollection.Add(new Claim(ClaimTypes.Webpage, extraData.Value, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                        break;

                    case "gender":
                        claimCollection.Add(new Claim(ClaimTypes.Gender, extraData.Value, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                        break;

                    case "username":
                        switch (authenticationResult.Provider)
                        {
                            case "facebook":
                                claimCollection.Add(new Claim(ClaimTypes.Email, extraData.Value, ClaimValueTypes.Email, authenticationResult.Provider, authenticationResult.Provider));
                                break;
                        }
                        break;

                    case "firstname":
                    case "first_name":
                    case "given_name":
                        claimCollection.Add(new Claim(ClaimTypes.GivenName, extraData.Value, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                        break;

                    case "lastname":
                    case "last_name":
                    case "family_name":
                        claimCollection.Add(new Claim(ClaimTypes.Surname, extraData.Value, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                        break;

                    case "birthday":
                        claimCollection.Add(new Claim(ClaimTypes.DateOfBirth, extraData.Value, ClaimValueTypes.Date, authenticationResult.Provider, authenticationResult.Provider));
                        break;

                    case "country":
                        claimCollection.Add(new Claim(ClaimTypes.Country, extraData.Value, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                        break;

                    case "email":
                    case "emails.preferred":
                    case "emails.account":
                    case "emails.personal":
                    case "emails.business":
                        claimCollection.Add(new Claim(ClaimTypes.Email, extraData.Value, ClaimValueTypes.Email, authenticationResult.Provider, authenticationResult.Provider));
                        break;
                }
            }
            return new ClaimsIdentity(claimCollection);
        }

        public static HttpCookie ToAuthenticationTicket(this IEnumerable<Claim> claimCollection, string mailAddress, int version = 1)
        {
            if (claimCollection == null)
            {
                throw new ArgumentNullException("claimCollection");
            }
            if (string.IsNullOrWhiteSpace(mailAddress))
            {
                throw new ArgumentNullException("mailAddress");
            }

            var userdata = Protect(Compress(Serialize(claimCollection)), mailAddress);

            var timeOut = FormsAuthentication.Timeout;
            var formsAuthenticationTicket = new FormsAuthenticationTicket(version, mailAddress, DateTime.Now, DateTime.Now.Add(timeOut), false, userdata);

            return new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(formsAuthenticationTicket));
        }

        public static FormsAuthenticationTicket ToFormsAuthenticationTicket(this HttpRequest httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException("httpRequest");
            }

            var formsAuthenticationCookie = httpRequest.Cookies[FormsAuthentication.FormsCookieName];
            return formsAuthenticationCookie == null ? null : FormsAuthentication.Decrypt(formsAuthenticationCookie.Value);
        }

        public static ClaimsPrincipal ToClaimsPrincipal(this FormsAuthenticationTicket formsAuthenticationTicket)
        {
            if (formsAuthenticationTicket == null)
            {
                throw new ArgumentNullException("formsAuthenticationTicket");
            }

            if (string.IsNullOrWhiteSpace(formsAuthenticationTicket.UserData) || string.Compare(formsAuthenticationTicket.UserData, "OAuth", StringComparison.Ordinal) == 0)
            {
                return null;
            }

            var claimCollection = Deserialize(Decompress(Unprotect(formsAuthenticationTicket.UserData, formsAuthenticationTicket.Name)));
            var claimIdentity = new ClaimsIdentity(claimCollection, "Forms");

            return new ClaimsPrincipal(claimIdentity);
        }

        private static byte[] Serialize(IEnumerable<Claim> claimCollection)
        {
            if (claimCollection == null)
            {
                throw new ArgumentNullException("claimCollection");
            }
            using (var memoryStream = new MemoryStream())
            {
                var serializer = CreateSerializer(typeof (IEnumerable<Claim>));
                serializer.WriteObject(memoryStream, claimCollection);
                return memoryStream.ToArray();
            }
        }

        private static IEnumerable<Claim> Deserialize(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            using (var memoryStream = new MemoryStream(data))
            {
                var serializer = CreateSerializer(typeof (IEnumerable<Claim>));
                return (IEnumerable<Claim>) serializer.ReadObject(memoryStream);
            }
        }

        private static XmlObjectSerializer CreateSerializer(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return new DataContractJsonSerializer(type);
        }

        private static byte[] Compress(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            using (var targetMemoryStream = new MemoryStream())
            {
                using (var sourceMemoryStream = new MemoryStream(data))
                {
                    using (var deflateStream = new DeflateStream(targetMemoryStream, CompressionMode.Compress))
                    {
                        sourceMemoryStream.CopyTo(deflateStream);
                        deflateStream.Flush();
                        deflateStream.Close();
                    }
                    sourceMemoryStream.Close();
                }
                return targetMemoryStream.ToArray();
            }
        }

        private static byte[] Decompress(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            using (var targetMemoryStream = new MemoryStream())
            {
                using (var sourceMemoryStream = new MemoryStream(data))
                {
                    using (var deflateStream = new DeflateStream(sourceMemoryStream, CompressionMode.Decompress))
                    {
                        deflateStream.CopyTo(targetMemoryStream);
                        deflateStream.Close();
                    }
                    sourceMemoryStream.Close();
                }
                return targetMemoryStream.ToArray();
            }
        }

        private static string Protect(byte[] data, string mailAddress)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (string.IsNullOrWhiteSpace(mailAddress))
            {
                throw new ArgumentNullException("mailAddress");
            }
            var purpose = GetMachineKeyPurpose(mailAddress);
            return Convert.ToBase64String(MachineKey.Protect(data, purpose));
        }

        private static byte[] Unprotect(string data, string mailAddress)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException("data");
            }
            if (string.IsNullOrWhiteSpace(mailAddress))
            {
                throw new ArgumentNullException("mailAddress");
            }
            var purpose = GetMachineKeyPurpose(mailAddress);
            return MachineKey.Unprotect(Convert.FromBase64String(data), purpose);
        }

        private static string GetMachineKeyPurpose(string mailAddress)
        {
            if (string.IsNullOrWhiteSpace(mailAddress))
            {
                throw new ArgumentNullException("mailAddress");
            }
            return string.Format("{0}:{1}", FormsAuthentication.FormsCookieName, mailAddress);
        }
    }
}