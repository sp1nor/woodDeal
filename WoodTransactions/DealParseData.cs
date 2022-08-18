using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodTransactions
{
    public class DealParseData
    {
        public string SellerName { get; set; }

        public string SellerInn { get; set; }

        public string BuyerName { get; set; }

        public string BuyerInn { get; set; }

        public string WoodVolumeBuyer { get; set; }

        public string WoodVolumeSeller { get; set; }

        public string DealDate { get; set; }

        public string DealNumber { get; set; }

        public void SetParamsBuyName(string name, string value)
        {
            if (name.Equals(DealParser.sellerName))
                SellerName = value;

            switch(name)
            {
                case DealParser.sellerName:
                    SellerName = value;
                    break;
                case DealParser.sellerInn:
                    SellerInn = value;
                    break;
                case DealParser.buyerName:
                    BuyerName = value;
                    break;
                case DealParser.buyerInn:
                    BuyerInn = value;
                    break;
                case DealParser.woodVolumeBuyer:
                    WoodVolumeBuyer = value;
                    break;
                case DealParser.woodVolumeSeller:
                    WoodVolumeSeller = value;
                    break;
                case DealParser.dealDate:
                    DealDate = value;
                    break;
                case DealParser.dealNumber:
                    DealNumber = value;
                    break;
                default: throw new ArgumentException("error");
            };
        }
    }
}
