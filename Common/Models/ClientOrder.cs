using QuikSharp.DataStructures;


namespace Common.Models
{
    public class ClientOrder
    {
        public string SecurityCode { get; set; }
        public string ClassCode { get; set; }
        public Operation Operation { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }    
        public bool MarketPrice { get; set; }

    }
}
