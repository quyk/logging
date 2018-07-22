using System.Collections.Generic;
using System.Security.Claims;
using System.Web;

namespace Flogging.Web
{
    public static class Helpers
    {
        public static IDictionary<string, object> GetWebFlooginData(out string userId, out string userName,
            out string location)
        {
            var data = new Dictionary<string, object>();
            GetRequestData(data, out location);
            GetUserData(data, out userId, out userName);
            GetSeesionData(data);

            //got cookies?

            return data;
        }

        private static void GetRequestData(IDictionary<string, object> data, out string location)
        {
            location = "";

            var request = HttpContext.Current.Request;
            //rich obj - you may want to explose this
            if (request != null)
            {
                location = request.Path;
                string type, version;

                //MS EDGE requires special detection logic
                GetBrowserInfo(request, out type, out version);
                data.Add("Browser: ", $"{type}{version}");
                data.Add("UserAgent: ", request.UserAgent);
                data.Add("Language: ", request.UserLanguages); // nom en-US preferences here??
                foreach (var queryStringKey in request.QueryString.Keys)
                {
                    data.Add($"QueryString-{queryStringKey}", request.QueryString[queryStringKey.ToString()]);
                }
            }
        }

        private static void GetBrowserInfo(HttpRequest request, out string type, out string version)
        {
            type = request.Browser.Type;
            if (type.StartsWith("Chrome") && request.UserAgent.Contains("Edge/"))
            {
                type = "Edge";
                version = $" (v{request.UserAgent.Substring(request.UserAgent.IndexOf("Edge/") + 5)})";
            }
            else
            {
                version = $" (v{request.Browser.MajorVersion}.{request.Browser.MinorVersion}";
            }
        }

        private static void GetUserData(IDictionary<string, object> data, out string userId, out string userName)
        {
            userId = "";
            userName = "";

            var user = ClaimsPrincipal.Current;
            if (user != null)
            {
                var i = 1; // i included in dictionary key to ensure uniqueness
                foreach (var claim in user.Claims)
                {
                    if (claim.Type == ClaimTypes.NameIdentifier)
                    {
                        userId = claim.Value;
                    }
                    else
                    {
                        if (claim.Type == ClaimTypes.Name)
                        {
                            userName = claim.Value;
                        }
                        else
                        {
                            // example dictionary key: UserClaim-4-role
                            data.Add($"UserClaim-{i++}-{claim.Type}", claim.Value);
                        }
                    }
                }
            }
        }

        private static void GetSeesionData(IDictionary<string, object> data)
        {
            if (HttpContext.Current.Session != null)
            {
                foreach (var sessionKey in HttpContext.Current.Session.Keys)
                {
                    var keyName = sessionKey.ToString();
                    if (HttpContext.Current.Session[keyName] != null)
                    {
                        data.Add($"Session-{keyName}", HttpContext.Current.Session[keyName].ToString());
                    }
                }
                data.Add("SessionId", HttpContext.Current.Session.SessionID);
            }
        }
    }
}

