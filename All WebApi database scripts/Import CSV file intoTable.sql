use iTradeDatabase
bulk insert [dbo].StatesPostcodesSuburbs
from 'C:\Development\Angular 2\StatesCitiesSuburbsPostcodes\Australian_StatesPostcodesSuburbs.csv'
with (FIRSTROW = 2, fieldterminator = ',', rowterminator = '\n')
go