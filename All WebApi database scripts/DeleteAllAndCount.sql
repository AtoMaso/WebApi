/****** Script for SelectTopNRows command from SSMS  ******/
--use iTradeDatabase
--Delete from [StatesPlacesPostcodesSuburbs]

use iTradeDatabase
select count(*) from dbo.Places 

use iTradeDatabase
Delete from Places


use iTradeDatabase
select count(*) from dbo.StatesPlacesPostcodesSuburbs 


use iTradeDatabase
Delete from Postcodes

use iTradeDatabase
select count(*) from dbo.Postcodes 