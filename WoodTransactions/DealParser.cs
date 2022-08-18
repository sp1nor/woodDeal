using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodTransactions
{
    public class DealParser
    {
        public const string sellerName = "sellerName";
        public const string sellerInn = "sellerInn";
        public const string buyerName = "buyerName";
        public const string buyerInn = "buyerInn";
        public const string woodVolumeBuyer = "woodVolumeBuyer";
        public const string woodVolumeSeller = "woodVolumeSeller";
        public const string dealDate = "dealDate";
        public const string dealNumber = "dealNumber";
        public const string typename = "__typename";

        static readonly int offsetForNumbers = 2;
        static readonly int offsetForText = 3;

        List<ParseParams> currentAndNextDealAttributes;

        List<ParseParams> CurrentAndNextDealAttributes
        {
            get 
            { 
                if (currentAndNextDealAttributes != null)
                    return currentAndNextDealAttributes;

                currentAndNextDealAttributes = new List<ParseParams>();
                currentAndNextDealAttributes.Add(new ParseParams { Attribute = sellerName, NextAttribute = sellerInn, Offset = offsetForText });
                currentAndNextDealAttributes.Add(new ParseParams { Attribute = sellerInn, NextAttribute = buyerName, Offset = offsetForText });
                currentAndNextDealAttributes.Add(new ParseParams { Attribute = buyerName, NextAttribute = buyerInn, Offset = offsetForText });
                currentAndNextDealAttributes.Add(new ParseParams { Attribute = buyerInn, NextAttribute = woodVolumeBuyer, Offset = offsetForText });
                currentAndNextDealAttributes.Add(new ParseParams { Attribute = woodVolumeBuyer, NextAttribute = woodVolumeSeller, Offset = offsetForNumbers });
                currentAndNextDealAttributes.Add(new ParseParams { Attribute = woodVolumeSeller, NextAttribute = dealDate, Offset = offsetForNumbers });
                currentAndNextDealAttributes.Add(new ParseParams { Attribute = dealDate, NextAttribute = dealNumber, Offset = offsetForText });
                currentAndNextDealAttributes.Add(new ParseParams { Attribute = dealNumber, NextAttribute = typename, Offset = offsetForText });

                return currentAndNextDealAttributes;
            }
        }

        public List<DealParseData> Result;

        string _notParsedData;

        public DealParser()
        {
        }

        public void Parse(string notParsedData)
        {
            _notParsedData = notParsedData;
            Result = new List<DealParseData>();

            int dataLength = _notParsedData.Length;
            int currentPosition = 0;

            while (true)
            {
                var data = new DealParseData();

                foreach (var val in CurrentAndNextDealAttributes)
                {
                    int findPosiiton = _notParsedData.IndexOf(val.Attribute, currentPosition);

                    if (findPosiiton == -1) //end of data
                        return;

                    int startIndex = findPosiiton + val.Offset + val.Attribute.Length;
                    var endIndex = _notParsedData.IndexOf(val.NextAttribute, startIndex) - val.Offset;
                    
                    data.SetParamsBuyName(val.Attribute, _notParsedData.Substring(startIndex, endIndex - startIndex));

                    currentPosition = endIndex;
                }

                Result.Add(data);
            }
        }

        class ParseParams
        {
            public string Attribute { get; set; }

            public string NextAttribute { get; set; }

            public int Offset { get; set; }
        }
    }
}