/****** Script for SelectTopNRows command from SSMS  ******/
--use iTradeDatabase
--Delete from [StatesPlacesPostcodesSuburbs]

use iTradeDatabase
select count(*) from dbo.Places 


use iTradeDatabase
select count(*) from dbo.Postcodes 

use iTradeDatabase
select count(*) from dbo.StatePlacePostcodeSuburbs 


use iTradeDatabase
Delete from Postcodes


use iTradeDatabase
Delete from Places


