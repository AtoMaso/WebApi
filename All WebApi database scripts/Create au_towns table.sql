USE [iTradeDatabase]
GO

/****** Object:  Table [dbo].[au_towns]    Script Date: 2/04/2018 3:06:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[au_towns](
	[id] [int] NOT NULL,
	[name] [varchar](37) NULL,
	[urban_area] [varchar](28) NULL,
	[state_code] [varchar](3) NULL,
	[state] [varchar](28) NULL,
	[postcode] [varchar](4) NULL,
	[type] [varchar](20) NULL,
	[latitude] [numeric](8, 5) NULL,
	[longitude] [numeric](8, 5) NULL,
	[elevation] [int] NULL,
	[population] [int] NULL,
	[area_sq_km] [numeric](9, 3) NULL,
	[local_government_area] [varchar](44) NULL,
	[time_zone] [varchar](21) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

