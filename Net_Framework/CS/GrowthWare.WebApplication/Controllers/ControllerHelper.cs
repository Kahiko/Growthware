using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace GrowthWare.WebApplication.Controllers
{
    public class ControllerHelper
    {

        // Extracts Request FormatData as a strongly typed model
        public static object GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unEscapedFormData = Uri.UnescapeDataString(result.FormData.GetValues(0).FirstOrDefault() ?? String.Empty);
                if (!String.IsNullOrEmpty(unEscapedFormData))
                    return JsonConvert.DeserializeObject<T>(unEscapedFormData);
            }
            //UploadFileSetting uploadFileSetting = GetFormData<UploadFileSetting>(result) as UploadFileSetting;
            return null;
        }
    }
}