using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using FacebookLeadAdsWebhooks.Model;
using Newtonsoft.Json;

namespace FacebookLeadAdsWebhooks.Controller
{
    public class WebhooksController : ApiController
    {
        #region Get Request
        [HttpGet]
        public HttpResponseMessage Get()
        {
            //var response = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new StringContent(HttpContext.Current.Request.QueryString["hub.challenge"])
            //};
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");


            //return response;


            var querystrings = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            if (querystrings["hub.verify_token"] == "CrmMedya_Alipasa_Karatas")
            {
                LogService.Save("Get işlemi yapıldı", JsonConvert.SerializeObject(querystrings), LogService.ItemTypes.Exception);


                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(querystrings["hub.challenge"], Encoding.UTF8, "text/plain")
                };
            }
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);


        }
        #endregion Get Request

        #region Post Request

        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] JsonData data)
        {
            try
            {
                LogService.Save("Post Datası", JsonConvert.SerializeObject(data), LogService.ItemTypes.Exception);

                var entry = data.Entry.FirstOrDefault();
                var change = entry?.Changes.FirstOrDefault();
                if (change == null) return new HttpResponseMessage(HttpStatusCode.BadRequest);

                string token = WebConfigurationManager.AppSettings["ACCESS_TOKEN"].ToString();

                var leadUrl = $"https://graph.facebook.com/v2.10/{change.Value.LeadGenId}?access_token={token}";
                var formUrl = $"https://graph.facebook.com/v2.10/{change.Value.FormId}?access_token={token}";

                using (var httpClientLead = new HttpClient())
                {


                    try
                    {
                        var response = await httpClientLead.GetStringAsync(formUrl);

                        if (!string.IsNullOrEmpty(response))
                        {
                            var jsonObjLead = JsonConvert.DeserializeObject<LeadFormData>(response);
                            //jsonObjLead.Name contains the lead ad name

                            LogService.Save("jsonObjLead", JsonConvert.SerializeObject(jsonObjLead), LogService.ItemTypes.Information);


                            //If response is valid get the field data
                            using (var httpClientFields = new HttpClient())
                            {
                                var responseFields = await httpClientFields.GetStringAsync(leadUrl);
                                if (!string.IsNullOrEmpty(responseFields))
                                {
                                    var jsonObjFields = JsonConvert.DeserializeObject<LeadData>(responseFields);

                                    LogService.Save("İşlem tamamlandı", JsonConvert.SerializeObject(jsonObjFields), LogService.ItemTypes.Information);

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogService.Save("İç Hata", "", ex, LogService.ItemTypes.Exception);
                    }
                }



                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                LogService.Save("Hata", "", ex, LogService.ItemTypes.Exception);
                return new HttpResponseMessage(HttpStatusCode.BadGateway);
            }
        }

        #endregion Post Request
    }
}
