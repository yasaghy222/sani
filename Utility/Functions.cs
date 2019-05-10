using System;
using System.Text;
using System.Security.Cryptography;
using System.Device.Location;

namespace Utility
{
    public static class Functions
    {
        /// <summary>
        /// تولید شناسه جدید
        /// </summary>
        /// <returns></returns>
        public static Guid GenerateNewId()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// تولید کد ترکیبی کوتاه غیر تکراری
        /// </summary>
        /// <returns></returns>
        public static string GenerateNewCode()
        {
            string temp = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return temp.Substring(0, 4) + DateTime.Now.Millisecond.ToString();
        }

        /// <summary>
        /// تولید کد ترکیبی عددی غیر تکراری
        /// </summary>
        /// <returns></returns>
        public static string GenerateNumCode()
        {
            string temp = $"{DateTime.Now.GetShortPersianDate()}{DateTime.Now.Millisecond}";
            return temp;
        }

        /// <summary>
        /// تولید توکن جدید
        /// </summary>
        /// <returns></returns>
        public static string GenerateNewToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        /// <summary>
        /// تولید کد عددی برای اعتبار سنجی
        /// </summary>
        /// <returns></returns>
        public static string GenerateValidationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        /// <summary>
        /// هش کردن پارامتر ورودی
        /// </summary>
        /// <param name="code">پارامتر ورودی جهت هش کردن</param>
        /// <returns></returns>
        public static string GenerateHash(string code)
        {
            UTF8Encoding ue = new UTF8Encoding();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            //first hash
            byte[] firsthash = ue.GetBytes(code);
            byte[] firsthashing = md5.ComputeHash(firsthash);

            string first = System.Text.RegularExpressions.Regex.Replace(BitConverter.ToString(firsthashing), "-", "").ToLower();

            //second hash
            byte[] secondhash = ue.GetBytes(first);
            byte[] secondhashing = md5.ComputeHash(secondhash);

            string second = System.Text.RegularExpressions.Regex.Replace(BitConverter.ToString(secondhashing), "-", "").ToLower();

            return second;
        }

        /// <summary>
        /// جدا کردن 3 نقطه از عدد
        /// </summary>
        /// <param name="InputText"></param>
        /// <returns></returns>
        public static string SetCama(object InputText)
        {
            string num = "0";
            try
            {
                if (InputText == null || string.IsNullOrEmpty(InputText.ToString()))
                    return "0";
                num = InputText.ToString();
                double number = 0;
                double.TryParse(InputText.ToString(), out number);
                string res = string.Format("{0:###,###.####}", number);
                if (string.IsNullOrEmpty(res)) return "0";
                else
                    return res;
            }
            catch
            {
                return num;
            }
        }

        /// <summary>
        /// محاسبه فاطله بین دو نقطه جغرافیایی
        /// </summary>
        /// <param name="customerLocation"></param>
        /// <param name="forceLocation"></param>
        /// <returns></returns>
        public static double CalcDistance(double cLat, double cLng, double fLat, double fLng)
        {
            GeoCoordinate cCoordinate = new GeoCoordinate(cLat, cLng);
            GeoCoordinate fCoordinate = new GeoCoordinate(fLat, fLng);
            return cCoordinate.GetDistanceTo(fCoordinate);
        }

        /// <summary>
        /// یافتن ثبت کننده سفارش
        /// </summary>
        /// <param name="EShopId"></param>
        /// <param name="OpratorId"></param>
        /// <returns></returns>
        public static string GetByWho(Guid? EShopId, Guid? OpratorId)
        {
            string temp = string.Empty;

            if (EShopId != null)
            {
                temp = "الکتریکی";
            }
            else if (OpratorId != null)
            {
                temp = "اپراتور";
            }
            else
            {
                temp = "مشتری";
            }

            return temp;
        }
    }
}
