using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using GE.Warehouse.Core.Infrastructure;

namespace GE.Warehouse.Web.Framework.UI.Captcha
{
    public static class HtmlExtensions
    {
        public static string GenerateCaptcha(this HtmlHelper helper)
        {
            var captchaSettings = EngineContext.Current.Resolve<CaptchaSettings>();
            var captchaControl = new Recaptcha.RecaptchaControl
            {
                ID = "recaptcha",
                Theme = "blackglass",
                PublicKey = captchaSettings.ReCaptchaPublicKey,
                PrivateKey = captchaSettings.ReCaptchaPrivateKey
            };

            var htmlWriter = new HtmlTextWriter(new StringWriter());

            captchaControl.RenderControl(htmlWriter);

            return htmlWriter.InnerWriter.ToString();
        }
    }
}
