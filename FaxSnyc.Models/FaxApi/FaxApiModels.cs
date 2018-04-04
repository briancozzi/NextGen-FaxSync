
namespace FaxSync.Models.FaxApi
{
    public class FaxUser
    {
        public int id { get; set; }
        public int enterprise_id { get; set; }
        public string email { get; set; }
        public string openid { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string role { get; set; }
        public string language { get; set; }
        public string time_zone { get; set; }
        public string job_title { get; set; }
        public string company_name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip_code { get; set; }
        public string phone_number { get; set; }
        public string mobile_number { get; set; }
        public int main_fax_number_id { get; set; }
        public int group_id { get; set; }
        public object external_id { get; set; }
        public string salutation { get; set; }
        public string main_fax_number { get; set; }
    }
    public class FaxApiNumber
    {
        public int FaxNumberId { get; set; }
        public string FaxNumber { get; set; }
        public bool Shared { get; set; }
    }
    public class FaxApiRootObject<T>
    {
        public bool result { get; set; }
        public T data { get; set; }
        public Errors errors { get; set; }
    }
    public class Errors
    {
        public string token { get; set; }
    }
}
