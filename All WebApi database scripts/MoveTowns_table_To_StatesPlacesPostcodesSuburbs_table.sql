use iTradeDatabase  
DECLARE @State as varchar(3)
DECLARE @Place AS VARCHAR(44) 
DECLARE @Postcode AS varchar(4) 
DECLARE @Suburb as varchar(37)                              
DECLARE @Temp table ( [state_code] varchar(3), [local_government_area] varchar(44), [postcode] varchar(4),[name] varchar(37) )	
	
INSERT INTO @Temp 
SELECT [state_code],[local_government_area], [postcode], [name] from dbo.au_towns 

DECLARE PlaceCursor CURSOR LOCAL FOR SELECT state_code, local_government_area, postcode, name FROM @Temp 	

OPEN PlaceCursor
	FETCH NEXT FROM PlaceCursor into @State, @Place, @Postcode, @Suburb
	WHILE @@FETCH_STATUS = 0
	BEGIN 																															
		Insert Into dbo.StatePlacePostcodeSuburbs ( [state], [place], [postcode], [suburb])  Values(@State, @Place, @Postcode, @Suburb );	
																																						
	FETCH NEXT FROM PlaceCursor into  @State, @Place, @Postcode, @Suburb
	END

CLOSE PlaceCursor
DEALLOCATE PlaceCursor


