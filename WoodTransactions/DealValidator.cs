using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodTransactions
{
    public class DealValidator
    {
        List<DealParseData> _notValidateData;

        public Dictionary<string, DealValidateData> Result;

        public DealValidator()
        {
            Result = new Dictionary<string, DealValidateData>();
        }

        public void Validate(List<DealParseData> notValidateData)
        {
            _notValidateData = notValidateData;
            

            foreach (var data in notValidateData)
            {
                string key = data.DealNumber.Trim();
                string sellerInn = data.SellerInn.Trim();
                string buyerInn = data.BuyerInn.Trim();

                double woodVolumeSeller = 0.0d;
                double woodVolumeBuyer = 0.0d;

                DateTime date = DateTime.Now;

                if (Result.ContainsKey(key) /*&& 
                    Result[key].SellerInn.Equals(sellerInn) &&
                    Result[key].BuyerInn.Equals(buyerInn)*/)
                {
                    if (double.TryParse(data.WoodVolumeSeller, out woodVolumeSeller))
                        Result[key].WoodVolumeSeller += woodVolumeSeller;
                    
                    if (double.TryParse(data.WoodVolumeBuyer, out woodVolumeBuyer))
                        Result[key].WoodVolumeBuyer += woodVolumeBuyer;

                    if (DateTime.TryParse(data.DealDate, out date) && date.Date > Result[key].DealDate.Date)
                        Result[key].DealDate = date;

                    continue;
                }

                var validData = new DealValidateData();

                validData.DealNumber = key;
                validData.SellerName = data.SellerName.Trim().Replace("\'", "");
                validData.SellerInn = sellerInn;
                validData.BuyerName = data.BuyerName.Trim().Replace("\'", "");
                validData.BuyerInn = buyerInn;

                if (validData.SellerInn.Length != 10 && validData.SellerInn.Length != 12)
                    validData.SellerInn = "";

                if (validData.BuyerInn.Length != 10 && validData.BuyerInn.Length != 12)
                    validData.BuyerInn = "";

                if(double.TryParse(data.WoodVolumeSeller, out woodVolumeSeller))
                    validData.WoodVolumeSeller = woodVolumeSeller;

                if (double.TryParse(data.WoodVolumeBuyer, out woodVolumeBuyer))
                    validData.WoodVolumeBuyer = woodVolumeBuyer;

                if (DateTime.TryParse(data.DealDate, out date))
                    validData.DealDate = date;

                date = DateTime.Now;
                if (validData.DealDate.Date > date.Date || validData.DealDate.Date < date.Date.AddYears(-35))
                    validData.DealDate = DateTime.MinValue;

                Result.Add(key, validData);
            }
        }
    }
}
