use iTradeDatabase  
DECLARE @State as varchar(3)
DECLARE @Place AS VARCHAR(44) 
DECLARE @Postcode AS varchar(4) 
DECLARE @Suburb as varchar(37)   
 
DECLARE @PostcodeId as int    
DECLARE @SuburbResult as int
                      
DECLARE @Temp table ( [state] varchar(3), [place] varchar(44), [postcode] varchar(4),[suburb] varchar(37) )	
	
INSERT INTO @Temp 
SELECT [state], [place], [postcode], [suburb] from dbo.StatePlacePostcodeSuburbs

DECLARE TempCursor CURSOR LOCAL FOR SELECT [state], [place], [postcode], [suburb] FROM @Temp 	

OPEN TempCursor
	FETCH NEXT FROM TempCursor into @State, @Place, @Postcode, @Suburb
	WHILE @@FETCH_STATUS = 0
	BEGIN 					
	    Set @SuburbResult  = (Select Count(*) From Suburbs where name = @Suburb)
		if(@SuburbResult = 0)
		BEGIN
				SET @PostcodeId = 	(Select id From Postcodes Where number = @Postcode)																									
				Insert Into dbo.Suburbs ( [name], [postcodeId])  Values(@Suburb, @PostcodeId );	
		END																																			
	FETCH NEXT FROM TempCursor into  @State, @Place, @Postcode, @Suburb
	END

CLOSE TempCursor
DEALLOCATE TempCursor


