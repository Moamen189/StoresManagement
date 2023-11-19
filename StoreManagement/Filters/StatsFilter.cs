using Microsoft.AspNetCore.Mvc.Filters;

namespace StoreManagement.Filters
{
    public class StatsFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            DateTime refrence = new DateTime(2020, 1, 1);
            TimeSpan timeSpan = DateTime.Now - refrence;
            Console.WriteLine("Excuted : " + timeSpan.Microseconds + "ms");
     
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            DateTime refrence = new DateTime(2020, 1, 1);
            TimeSpan timeSpan = DateTime.Now - refrence;
            Console.WriteLine("Excuting : " + timeSpan.Microseconds + "ms");
        }
    }
}
