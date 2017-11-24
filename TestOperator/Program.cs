using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NRA.Common.ViewModels.Common;
using NRA.Common.Message;
using NRA.Common.ViewModels.PRA.ARBC;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace TestOperator
{
    class Program
    {
        static void Main(string[] args)
        {
           var a= post();
            //string b= JsonConvert.SerializeObject((a);
            string url = "http://localhost:58322/api/ARBCD001";
            string b = GetWebApi(url);
            var c = get<Response<List<ARBCD001TableViewModel>>>(b);

        }
        public static bool FuzzySearch(string input,string pattern)
        {
            //"*"=任一文字或數字不含空白，"@"=任一文字或數字含空白，"-"=略=不限字數文字或數字，"$"=任一文字，"#"=任一數字)
            //先把*改變成任一文字或數字不含空白
            pattern = pattern.Replace("*","\\w{1}");
            //先把@改變成任一文字或數字含空白
            pattern = pattern.Replace("@", ".{1}");
            //先把-改變成不限字數文字或數字
            pattern = pattern.Replace("-", "\\w+");
            //先把$改變成任一文字
            pattern = pattern.Replace("$", @"[^ 0-9]{1}");
            //先把#改變成任一數字
            pattern = pattern.Replace("#", "\\d{1}");
            //判斷是否符合條件
            return Regex.IsMatch(input, $"^{pattern}$");
        }

        public static Response<IEnumerable<ARBCD001TableViewModel>> post()
        {
            List<ARBCD001TableViewModel> a = new List<ARBCD001TableViewModel>();

            a.Add(new ARBCD001TableViewModel()
            {
                Class_Type = "a",
                Create_Date = DateTime.Now,
                Create_User = "JJ"
            } );

            a.Add(new ARBCD001TableViewModel()
            {
                Class_Type = "B",
                Create_Date = DateTime.Now,
                Create_User = "DD"
            });

            var response = new Response<IEnumerable<ARBCD001TableViewModel>>()
            {
                Success = false,
                Data = a.AsEnumerable()
            };
            
            response.Success = true;
            response.Code = ResponseCode.Success;
            response.Message = "";

            return response;
        }
        public static TResult get<TResult>( string jsonStr)
        {
            TResult result = JsonConvert.DeserializeObject<TResult>(jsonStr);
            
            return result;
        }

private static string GetWebApi(string url)
{
    var request = HttpWebRequest.Create(url) as HttpWebRequest;
    request.Method = "Get";
    request.ContentType = "application/json; charset=utf-8";
    request.Timeout = 30000;
    request.Headers.Add("Authorization", "Good");
    string result = "";
    // 取得回應資料
    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
    {
        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        {
            result = sr.ReadToEnd();
        }
    }
    return result;
}
    }


    public class Response<T>
    {
        public bool Success { get; set; } = false;
        public ResponseCode Code { get; set; } = ResponseCode.Success;
        public string Message { get; set; }
        public T Data { get; set; }
        public int TotalCount { get; set; }
        public List<ErrorMessageViewModel> ErrorMsg { get; set; } = new List<ErrorMessageViewModel>();
    }

    public enum ResponseCode
    {
        Success = 0,
        Verification_Fail = 1,
        Data_Not_Found = 2,
        Data_Duplicate = 3,
        Operation_Fail = 4
    }

}
