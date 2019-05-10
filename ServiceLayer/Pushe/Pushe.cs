using System;
using System.Net.Http;
using System.Threading.Tasks;
using ViewModel.Areas;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Text;

namespace ServiceLayer.Pushe
{
    public class Pushe
    {
        static readonly string token = "70a5a995751eaf06f7ff366107c98226115e024c";
        static readonly string appId = "co.ronash.pushesample";
        static string temp = null;

        public static async Task<string> SendNotifiction(string pusheId, Factor.FactorPreview factorPreview)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("authorization", "Token " + token);

            HttpResponseMessage response = await client.PostAsync(
               "https://api.pushe.co/v2/messaging/notifications/",
               GetNotificationData(pusheId, factorPreview)
            );

            int status_code = (int)response.StatusCode;
            JObject reponse_json = JObject.Parse(await response.Content.ReadAsStringAsync());

            if (status_code == 201)
            {
                temp = "Success";

                string hashed_id = reponse_json.GetValue("hashed_id").ToString(),
                       report_url;

                if (string.IsNullOrEmpty(hashed_id))
                {
                    report_url = "no report url for your plan";
                }
                else
                {
                    report_url = "https://pushe.co/report?id=" + hashed_id;
                }

                string notif_id = reponse_json.GetValue("wrapper_id").ToString();
            }
            else
            {
                temp = null;
            }
            return temp;
        }

        private static StringContent GetNotificationData(string pusheId, Factor.FactorPreview factorPreview)
        {
            string data = new JavaScriptSerializer().Serialize(factorPreview);
            JObject request_data = new JObject
            {
                { "app_ids", new JArray(new string[] { appId }) },
                //{ "filters",
                //    new JObject
                //    {
                //        { "pushe_id",new JArray(new string[] { pusheId }) }
                //    }
                //},
                {"platform" , 2 },
                { "data",
                    new JObject
                    {
                        {"show_app", false }
                    }
                },
                {"custom_content", data }
            };

            return new StringContent(request_data.ToString(), Encoding.UTF8, "application/json");
        }
    }
}
