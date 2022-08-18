using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodTransactions
{
    public class DealValidateData
    {
        public string DealNumber { get; set; }

        public string SellerName { get; set; }

        public string SellerInn { get; set; }

        public string BuyerName { get; set; }

        public string BuyerInn { get; set; }

        public double WoodVolumeBuyer { get; set; }

        public double WoodVolumeSeller { get; set; }

        public DateTime DealDate { get; set; }
    }
}