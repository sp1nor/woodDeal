CREATE TABLE [dbo].[WoodDeal]
(
	[DealNumber] NVARCHAR(30) NOT NULL PRIMARY KEY,
	[SellerName] NVARCHAR(500) NOT NULL,
	[SellerInn] NVARCHAR(12) NULL,
	[BuyerName] NVARCHAR(500) NOT NULL,
	[BuyerInn] NVARCHAR(12) NULL,
	[WoodVolumeBuyer] FLOAT(53),
	[WoodVolumeSeller] FLOAT(53),
	[DealDate] DATE NOT NULL
)