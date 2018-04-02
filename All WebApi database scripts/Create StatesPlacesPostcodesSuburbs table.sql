USE [iTradeDatabase]
GO

/****** Object:  Table [dbo].[StatesPlacesPostcodesSuburbs]    Script Date: 2/04/2018 3:06:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[StatesPlacesPostcodesSuburbs](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[postcode] [varchar](4) NOT NULL,
	[suburb] [varchar](37) NOT NULL,
	[state] [varchar](3) NOT NULL,
	[place] [varchar](44) NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

