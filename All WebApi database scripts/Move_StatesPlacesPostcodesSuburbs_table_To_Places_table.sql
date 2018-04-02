use iTradeDatabase  
DECLARE @State as varchar(3)
DECLARE @Place AS VARCHAR(44) 
DECLARE @Postcode AS varchar(4) 
DECLARE @Suburb as varchar(37)   
 
DECLARE @StateId as int    
DECLARE @PlaceResult as int
                      
DECLARE @Temp table ( [state] varchar(3), [place] varchar(44), [postcode] varchar(4),[suburb] varchar(37) )	
	
INSERT INTO @Temp 
SELECT [state], [place], [postcode], [suburb] from dbo.StatePlacePostcodeSuburbs

DECLARE PlaceCursor CURSOR LOCAL FOR SELECT [state], [place], [postcode], [suburb] FROM @Temp 	

OPEN PlaceCursor
	FETCH NEXT FROM PlaceCursor into @State, @Place, @Postcode, @Suburb
	WHILE @@FETCH_STATUS = 0
	BEGIN 					
	    Set @PlaceResult  = (Select Count(*) From Places where name = @Place)
		if(@PlaceResult = 0)
		BEGIN
				SET @StateId = 	(Select Distinct id From States Where name = @State)																									
				Insert Into dbo.Places ( [name], [stateId])  Values(@Place, @StateId );	
		END																																			
	FETCH NEXT FROM PlaceCursor into  @State, @Place, @Postcode, @Suburb
	END

CLOSE PlaceCursor
DEALLOCATE PlaceCursor


