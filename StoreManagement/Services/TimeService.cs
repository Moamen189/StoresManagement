namespace StoreManagement.Services
{
    public class TimeService
    {
        public string GetDate()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            return date;
        }

        public string GetTime() { 
            string time = DateTime.Now.ToString("h:mm:ss tt");
            return time;
        }
    }
}
