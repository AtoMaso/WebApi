use iTradeDatabase  
DECLARE @State as varchar(3)
DECLARE @Place AS VARCHAR(44) 
DECLARE @Postcode AS varchar(4) 
DECLARE @Suburb as varchar(37)   
 
DECLARE @PlaceId as int    
DECLARE @PostcodeResult as int
                      
DECLARE @Temp table ( [state] varchar(3), [place] varchar(44), [postcode] varchar(4),[suburb] varchar(37) )	
	
INSERT INTO @Temp 
SELECT [state], [place], [postcode], [suburb] from dbo.StatePlacePostcodeSuburbs

DECLARE TempCursor CURSOR LOCAL FOR SELECT [state], [place], [postcode], [suburb] FROM @Temp 	

OPEN TempCursor
	FETCH NEXT FROM TempCursor into @State, @Place, @Postcode, @Suburb
	WHILE @@FETCH_STATUS = 0
	BEGIN 							
		Set @PostcodeResult  = (Select Count(*) From Postcodes Where number = @Postcode)
		if(@PostcodeResult = 0)
		BEGIN
				SET @PlaceId = 	(Select id From Places Where name = @Place)																									
				Insert Into dbo.Postcodes ( [number], [placeId])  Values(@Postcode, @PlaceId );	
		END																																			
	FETCH NEXT FROM TempCursor into  @State, @Place, @Postcode, @Suburb
	END

CLOSE TempCursor
DEALLOCATE TempCursor


