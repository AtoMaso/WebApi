/****** Script for SelectTopNRows command from SSMS  ******/

use iTradeDatabase
Delete from Addresses

use iTradeDatabase
Delete from AddressTypes

use iTradeDatabase
Delete from AspNetRoles

use iTradeDatabase
Delete from AspNetUserClaims

use iTradeDatabase
Delete from AspNetUserLogins

use iTradeDatabase
Delete from AspNetUserRoles

use iTradeDatabase
Delete from AspNetUsers

use iTradeDatabase
Delete from au_towns

use iTradeDatabase
Delete from Categories

use iTradeDatabase
Delete from Correspondences

use iTradeDatabase
Delete from Emails

use iTradeDatabase
Delete from EmailTypes

use iTradeDatabase
Delete from Images

use iTradeDatabase
Delete from PersonalDetails

use iTradeDatabase
Delete from Phones

use iTradeDatabase
Delete from PhoneTypes

--use iTradeDatabase
--Delete from ProcessMessages

use iTradeDatabase
Delete from ProcessMessageTypes

use iTradeDatabase
Delete from SocialNetworks

use iTradeDatabase
Delete from SocialNetworkTypes

use iTradeDatabase
Delete from StatePlacePostcodeSuburbs

use iTradeDatabase
Delete from Subcategories

use iTradeDatabase
Delete from TradeHistories

use iTradeDatabase
Delete from Trades


use iTradeDatabase
select count(*) from dbo.StatePlacePostcodeSuburbs 

use iTradeDatabase
select count(*) from dbo.ProcessMessages 

use iTradeDatabase
select count(*) from dbo.Correspondences

use iTradeDatabase
select count(*) from dbo.Trades  