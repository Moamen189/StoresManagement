namespace StoreManagement.Services
{
    public class OrderHelper
    {

        public static decimal ShippingFee { get; } = 5;

        public static Dictionary<string, string> PaymentMethods { get; } = new()
        {
            { "Cash", "Cash on Delivery" },
            { "Paypal", "Paypal" },
            { "Credit Card", "Credit Card" }
        };

        public static List<string> PaymentStatuses { get; } = new()
        {
            "Pending", "Accepted", "Canceled"
        };

        public static List<string> OrderStatuses { get; } = new()
        {
            "Created", "Accepted", "Canceled", "Shipped", "Delivered", "Returned"
        };
        public static Dictionary<int, int> GetProductDictionary(string ProductIdentifier)
        {
            var Productdictionary = new Dictionary<int, int>();
            if(ProductIdentifier.Length > 0) {
                string[] productArray = ProductIdentifier.Split('-');
                foreach(var productId in productArray)
                {
                    try
                    {
                        int id = int.Parse(productId);
                        if (Productdictionary.ContainsKey(id))
                        {
                            Productdictionary[id] += 1;
                        }else
                        {
                            Productdictionary.Add(id, 1);
                        }
                    }catch(Exception) { }

                }
            
            
            }




            return Productdictionary;
        }
    }
}
