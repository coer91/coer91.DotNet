using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http; 
using System.Xml;

namespace coer91.Tools
{
    public static class Conversions
    {

        #region ToJson

        public static string ToJson<T>(T obj, bool nesting = false)
        {
            if (!nesting)
                obj = Clean.NoNesting(obj);

            return ToJson([obj], nesting);
        }


        public static string ToJson<T>(List<T> objectList, bool nesting = false)
        {
            if (!nesting)
                objectList = Clean.NoNesting(objectList);

            return JsonConvert.SerializeObject(objectList);
        }


        public static string ToJson(Dictionary<string, string> dictionary)
            => JsonConvert.SerializeObject(dictionary);


        public static string ToJson(Dictionary<string, int> dictionary)
            => JsonConvert.SerializeObject(dictionary);

        public static string ToJson(Dictionary<string, float> dictionary)
            => JsonConvert.SerializeObject(dictionary);

        public static string ToJson(Dictionary<string, double> dictionary)
            => JsonConvert.SerializeObject(dictionary);

        public static string ToJson(Dictionary<string, decimal> dictionary)
            => JsonConvert.SerializeObject(dictionary);


        public static string ToJson(string xml)
        {
            XmlDocument doc = new();
            doc.LoadXml(xml);
            string json = JsonConvert.SerializeXmlNode(doc);

            if (json.StartsWith("{\"Document\":{\"Nodo\":") && json.EndsWith("}}"))
            {
                json = json[20..];
                json = json[..^2];
            }

            return json;
        }

        #endregion


        #region ToXML

        public static string ToXML<T>(T obj, bool nesting = false)
        {
            if (!nesting)
                obj = Clean.NoNesting(obj);

            return ToXML([obj], nesting);
        }


        public static string ToXML<T>(List<T> objectList, bool nesting = false)
        {
            if (!nesting)
                objectList = Clean.NoNesting(objectList);

            return ToXML(JsonConvert.SerializeObject(objectList), nesting);
        }


        public static string ToXML(string json, bool nesting = false)
        {
            object obj = JsonConvert.DeserializeObject(json);

            if (Validations.IsCollection(obj))
            {
                if (json.StartsWith("[[")) json = json[1..];
                if (json.EndsWith("]]")) json = json[..^1];
                return ToXMLDocument(json);
            }

            json = ToJson(obj, nesting);
            obj = JsonConvert.DeserializeObject(json);
            return ToXML(obj, false);
        }


        private static string ToXMLDocument(string json)
        {
            DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(json);
            dataTable.TableName = "Nodo";

            DataSet dataSet = new("Document");
            dataSet.Tables.Add(dataTable);

            StringWriter stringWriter = new();
            dataSet.WriteXml(stringWriter);
            return stringWriter.ToString();
        }

        #endregion 


        #region ToHttpRequest   

        public static HttpRequestDTO ToHttpRequest(ActionExecutingContext context)
            => ToHttpRequest(context?.HttpContext);


        public static HttpRequestDTO ToHttpRequest(ActionExecutedContext context)
            => ToHttpRequest(context?.HttpContext);


        public static HttpRequestDTO ToHttpRequest(ExceptionContext context)
            => ToHttpRequest(context?.HttpContext);


        public static HttpRequestDTO ToHttpRequest(IHttpContextAccessor httpContextAccessor)
            => ToHttpRequest(httpContextAccessor?.HttpContext);


        public static HttpRequestDTO ToHttpRequest(HttpContext context)
        {
            try
            {
                return context is not null ? new HttpRequestDTO()
                {
                    Project    = Security.ProjectName,
                    Controller = context.Request.RouteValues.TryGetValue("controller", out var controller) ? controller.ToString() : string.Empty,
                    Method     = context.Request.RouteValues.TryGetValue("action", out var action) ? action.ToString() : string.Empty,
                    User       = context.Request.Headers.TryGetValue("Clien-User", out var user) ? user : JWT.GetClaim("User", context),
                    Role       = context.Request.Headers.TryGetValue("User-Role", out var role) ? role : JWT.GetClaim("Role", context),
                    UtcOffset  = context.Request.Headers.TryGetValue("Utc-Offset", out var utcOffset) && int.TryParse(utcOffset, out int utcOffsetInteger) ? utcOffsetInteger : 0
                } : null;
            }

            catch
            {
                return null;
            }
        }

        #endregion 
    }
}