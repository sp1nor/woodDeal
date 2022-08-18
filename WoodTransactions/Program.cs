using System.Net;
using WoodTransactions;

while (true)
{
    var startDateTime = DateTime.Now;

    Console.WriteLine(@"Start parser for https://www.lesegais.ru/open-area/deal");
    Console.WriteLine(@"Start date time: " + startDateTime);

    var cookieContainer = new CookieContainer();

    var requestPost = new PostRequest("https://www.lesegais.ru/open-area/graphql ");
    requestPost.Referer = "https://www.lesegais.ru/open-area/deal";
    requestPost.Host = "www.lesegais.ru";
    requestPost.Accept = "*/*";
    requestPost.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36";
    requestPost.ContentType = "application/json";

    requestPost.Headers.Add("Origin", "https://www.lesegais.ru");
    requestPost.Headers.Add(@"sec-ch-ua", "\"Chromium\";v=\"104\", \" Not A; Brand \";v=\"99\", \"Google Chrome\";v=\"104\"");
    requestPost.Headers.Add("sec-ch-ua-mobile", "?0");
    requestPost.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
    requestPost.Headers.Add("Sec-Fetch-Dest", "empty");
    requestPost.Headers.Add("Sec-Fetch-Mode", "cors");
    requestPost.Headers.Add("Sec-Fetch-Site", "same-origin");

    var loadPacketSize = 2500;

    requestPost.Data = "{ \"query\":\"query SearchReportWoodDealCount($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {\\n  searchReportWoodDeal(filter: $filter, pageable: {number: $number, size: $size}, orders: $orders) {\\n    total\\n    number\\n    size\\n    overallBuyerVolume\\n    overallSellerVolume\\n    __typename\\n  }\\n}\\n\",\"variables\":{ \"size\":1,\"number\":0,\"filter\":null},\"operationName\":\"SearchReportWoodDealCount\"}";

    requestPost.Run(cookieContainer);

    var pattern = "\"total\":";
    var indexStart = requestPost.Response.IndexOf(pattern) + pattern.Length;
    var indexEnd = requestPost.Response.IndexOf(',', indexStart);
    var countsOfRecords = int.Parse(requestPost.Response.Substring(indexStart, indexEnd - indexStart));

    var numberOfLoad = countsOfRecords / loadPacketSize;

    if (countsOfRecords % loadPacketSize != 0)
        numberOfLoad += 1;

    var parser = new DealParser();
    var validator = new DealValidator();
    var dataContext = new DataContext();

    for (int i = 0; i < numberOfLoad; i++)
    {
        requestPost.Data = "{\"query\":\"query SearchReportWoodDeal($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {\\n searchReportWoodDeal(filter: $filter, pageable: { number: $number, size: $size}, orders: $orders) {\\n content {\\n sellerName\\n sellerInn\\n buyerName\\n buyerInn\\n woodVolumeBuyer\\n woodVolumeSeller\\n dealDate\\n dealNumber\\n __typename\\n    }\\n __typename\\n  }\\n}\\n\",\"variables\":{\"size\":" + loadPacketSize + ",\"number\":" + i + ",\"filter\":null,\"orders\":null},\"operationName\":\"SearchReportWoodDeal\"}";

        requestPost.Run(cookieContainer);

        parser.Parse(requestPost.Response);
        validator.Validate(parser.Result);

        int loadRecords = loadPacketSize;
        if (i == numberOfLoad - 1 && countsOfRecords % loadPacketSize != 0)
            loadRecords = countsOfRecords % loadPacketSize;

        Console.WriteLine($"Packet number: {i + 1} finished, count records in packet: {loadRecords} ");

    /*#if RELEASE
            Thread.Sleep(500);
    #endif*/
    
    }

    GC.Collect();

    dataContext.HandleData(new List<DealValidateData>(validator.Result.Values));

    GC.Collect();

    Console.WriteLine($"Merge count records for check/load in base: {validator.Result.Values.Count} ");

    var endDateTime = DateTime.Now;

    Console.WriteLine(@"End date time: " + endDateTime);
    Console.WriteLine(@"Time span: " + (endDateTime - startDateTime));

#if RELEASE
        Thread.Sleep(10 * 60 * 1000);
#endif
}