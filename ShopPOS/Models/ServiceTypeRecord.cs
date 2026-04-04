namespace ShopPOS.Models
{
    public class ServiceTypeRecord
    {
        public int ServiceTypeId { get; set; }

        public string ServiceName { get; set; }

        public string ProviderName { get; set; }

        public decimal DefaultCharge { get; set; }

        public string CommissionType { get; set; }

        public decimal CommissionValue { get; set; }

        public bool IsActive { get; set; }

        public override string ToString()
        {
            return ServiceName;
        }
    }
}
