USE [master]
GO
/****** Object:  Database [iTradeDatabase]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE DATABASE [iTradeDatabase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'iTradeDatabase', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQL2012PREFILL\MSSQL\DATA\iTradeDatabase.mdf' , SIZE = 5184KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'iTradeDatabase_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQL2012PREFILL\MSSQL\DATA\iTradeDatabase_log.ldf' , SIZE = 1088KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [iTradeDatabase] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [iTradeDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [iTradeDatabase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [iTradeDatabase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [iTradeDatabase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [iTradeDatabase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [iTradeDatabase] SET ARITHABORT OFF 
GO
ALTER DATABASE [iTradeDatabase] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [iTradeDatabase] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [iTradeDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [iTradeDatabase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [iTradeDatabase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [iTradeDatabase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [iTradeDatabase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [iTradeDatabase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [iTradeDatabase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [iTradeDatabase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [iTradeDatabase] SET  ENABLE_BROKER 
GO
ALTER DATABASE [iTradeDatabase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [iTradeDatabase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [iTradeDatabase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [iTradeDatabase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [iTradeDatabase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [iTradeDatabase] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [iTradeDatabase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [iTradeDatabase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [iTradeDatabase] SET  MULTI_USER 
GO
ALTER DATABASE [iTradeDatabase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [iTradeDatabase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [iTradeDatabase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [iTradeDatabase] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [iTradeDatabase]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Addresses]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Addresses](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[number] [nvarchar](8) NULL,
	[unit] [nvarchar](8) NULL,
	[pobox] [nvarchar](8) NULL,
	[street] [nvarchar](30) NOT NULL,
	[suburb] [nvarchar](30) NOT NULL,
	[place] [nvarchar](20) NOT NULL,
	[postcode] [nvarchar](10) NOT NULL,
	[state] [nvarchar](20) NOT NULL,
	[country] [nvarchar](30) NULL,
	[preferredFlag] [nvarchar](10) NOT NULL,
	[addressTypeId] [int] NOT NULL,
	[traderId] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.Addresses] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AddressTypes]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressTypes](
	[addressTypeId] [int] IDENTITY(1,1) NOT NULL,
	[addressType] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.AddressTypes] PRIMARY KEY CLUSTERED 
(
	[addressTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[categoryId] [int] IDENTITY(1,1) NOT NULL,
	[category] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.Categories] PRIMARY KEY CLUSTERED 
(
	[categoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Correspondences]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Correspondences](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[subject] [nvarchar](20) NOT NULL,
	[message] [nvarchar](70) NOT NULL,
	[content] [nvarchar](500) NOT NULL,
	[statusSender] [nvarchar](10) NOT NULL,
	[statusReceiver] [nvarchar](10) NOT NULL,
	[dateSent] [datetime] NOT NULL,
	[tradeId] [int] NOT NULL,
	[traderIdReceiver] [nvarchar](max) NOT NULL,
	[traderIdSender] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.Correspondences] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Emails]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Emails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[account] [nvarchar](70) NOT NULL,
	[preferredFlag] [nvarchar](5) NOT NULL,
	[emailTypeId] [int] NOT NULL,
	[traderId] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.Emails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmailTypes]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTypes](
	[emailTypeId] [int] IDENTITY(1,1) NOT NULL,
	[emailType] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.EmailTypes] PRIMARY KEY CLUSTERED 
(
	[emailTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ForgotPasswords]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ForgotPasswords](
	[userId] [nvarchar](128) NOT NULL,
	[createdDt] [datetime] NOT NULL,
	[attemptsCount] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ForgotPasswords] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GeoDatas]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeoDatas](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[state] [nvarchar](3) NOT NULL,
	[place] [nvarchar](45) NOT NULL,
	[postcode] [nvarchar](4) NOT NULL,
	[suburb] [nvarchar](45) NOT NULL,
 CONSTRAINT [PK_dbo.GeoDatas] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Images]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Images](
	[imageId] [int] IDENTITY(1,1) NOT NULL,
	[imageUrl] [nvarchar](70) NOT NULL,
	[imageTitle] [nvarchar](70) NOT NULL,
	[tradeId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Images] PRIMARY KEY CLUSTERED 
(
	[imageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PersonalDetails]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonalDetails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[firstName] [nvarchar](20) NOT NULL,
	[middleName] [nvarchar](20) NULL,
	[lastName] [nvarchar](30) NOT NULL,
	[dateOfBirth] [datetime] NOT NULL,
	[traderId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.PersonalDetails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Phones]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Phones](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[number] [nvarchar](10) NOT NULL,
	[countryCode] [nvarchar](10) NOT NULL,
	[cityCode] [nvarchar](10) NOT NULL,
	[preferredFlag] [nvarchar](10) NOT NULL,
	[phoneTypeId] [int] NOT NULL,
	[traderId] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.Phones] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PhoneTypes]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhoneTypes](
	[phoneTypeId] [int] IDENTITY(1,1) NOT NULL,
	[phoneType] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.PhoneTypes] PRIMARY KEY CLUSTERED 
(
	[phoneTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProcessMessages]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcessMessages](
	[messageId] [int] IDENTITY(1,1) NOT NULL,
	[messageCode] [nvarchar](10) NOT NULL,
	[messageText] [nvarchar](150) NOT NULL,
	[messageTypeId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ProcessMessages] PRIMARY KEY CLUSTERED 
(
	[messageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProcessMessageTypes]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcessMessageTypes](
	[messageTypeId] [int] IDENTITY(1,1) NOT NULL,
	[messageType] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.ProcessMessageTypes] PRIMARY KEY CLUSTERED 
(
	[messageTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SocialNetworks]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SocialNetworks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[account] [nvarchar](70) NOT NULL,
	[preferredFlag] [nvarchar](10) NOT NULL,
	[socialTypeId] [int] NOT NULL,
	[traderId] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.SocialNetworks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SocialNetworkTypes]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SocialNetworkTypes](
	[socialTypeId] [int] IDENTITY(1,1) NOT NULL,
	[socialType] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.SocialNetworkTypes] PRIMARY KEY CLUSTERED 
(
	[socialTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Subcategories]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subcategories](
	[subcategoryId] [int] IDENTITY(1,1) NOT NULL,
	[subcategory] [nvarchar](30) NOT NULL,
	[categoryId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Subcategories] PRIMARY KEY CLUSTERED 
(
	[subcategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TradeHistories]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TradeHistories](
	[historyId] [int] IDENTITY(1,1) NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[status] [nvarchar](20) NOT NULL,
	[tradeId] [int] NOT NULL,
	[viewer] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_dbo.TradeHistories] PRIMARY KEY CLUSTERED 
(
	[historyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Trades]    Script Date: 17/04/2018 2:10:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trades](
	[tradeId] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](20) NOT NULL,
	[description] [nvarchar](200) NOT NULL,
	[tradeFor] [nvarchar](20) NOT NULL,
	[datePublished] [datetime] NOT NULL,
	[status] [nvarchar](20) NOT NULL,
	[state] [nvarchar](3) NOT NULL,
	[place] [nvarchar](45) NOT NULL,
	[postcode] [nvarchar](4) NOT NULL,
	[suburb] [nvarchar](45) NOT NULL,
	[category] [nvarchar](45) NOT NULL,
	[subcategory] [nvarchar](45) NOT NULL,
	[traderId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.Trades] PRIMARY KEY CLUSTERED 
(
	[tradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_addressTypeId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_addressTypeId] ON [dbo].[Addresses]
(
	[addressTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_traderId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_traderId] ON [dbo].[Addresses]
(
	[traderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [RoleNameIndex]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_RoleId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UserNameIndex]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_tradeId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_tradeId] ON [dbo].[Correspondences]
(
	[tradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_emailTypeId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_emailTypeId] ON [dbo].[Emails]
(
	[emailTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_traderId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_traderId] ON [dbo].[Emails]
(
	[traderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_tradeId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_tradeId] ON [dbo].[Images]
(
	[tradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_traderId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_traderId] ON [dbo].[PersonalDetails]
(
	[traderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_phoneTypeId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_phoneTypeId] ON [dbo].[Phones]
(
	[phoneTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_traderId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_traderId] ON [dbo].[Phones]
(
	[traderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_messageTypeId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_messageTypeId] ON [dbo].[ProcessMessages]
(
	[messageTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_socialTypeId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_socialTypeId] ON [dbo].[SocialNetworks]
(
	[socialTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_traderId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_traderId] ON [dbo].[SocialNetworks]
(
	[traderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_categoryId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_categoryId] ON [dbo].[Subcategories]
(
	[categoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_tradeId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_tradeId] ON [dbo].[TradeHistories]
(
	[tradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_category]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_category] ON [dbo].[Trades]
(
	[category] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_place]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_place] ON [dbo].[Trades]
(
	[place] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_postcode]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_postcode] ON [dbo].[Trades]
(
	[postcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_state]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_state] ON [dbo].[Trades]
(
	[state] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_subcategory]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_subcategory] ON [dbo].[Trades]
(
	[subcategory] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_suburb]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_suburb] ON [dbo].[Trades]
(
	[suburb] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_traderId]    Script Date: 17/04/2018 2:10:19 PM ******/
CREATE NONCLUSTERED INDEX [IX_traderId] ON [dbo].[Trades]
(
	[traderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Addresses_dbo.AddressTypes_addressTypeId] FOREIGN KEY([addressTypeId])
REFERENCES [dbo].[AddressTypes] ([addressTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_dbo.Addresses_dbo.AddressTypes_addressTypeId]
GO
ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Addresses_dbo.AspNetUsers_traderId] FOREIGN KEY([traderId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_dbo.Addresses_dbo.AspNetUsers_traderId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Correspondences]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Correspondences_dbo.Trades_tradeId] FOREIGN KEY([tradeId])
REFERENCES [dbo].[Trades] ([tradeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Correspondences] CHECK CONSTRAINT [FK_dbo.Correspondences_dbo.Trades_tradeId]
GO
ALTER TABLE [dbo].[Emails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Emails_dbo.AspNetUsers_traderId] FOREIGN KEY([traderId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Emails] CHECK CONSTRAINT [FK_dbo.Emails_dbo.AspNetUsers_traderId]
GO
ALTER TABLE [dbo].[Emails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Emails_dbo.EmailTypes_emailTypeId] FOREIGN KEY([emailTypeId])
REFERENCES [dbo].[EmailTypes] ([emailTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Emails] CHECK CONSTRAINT [FK_dbo.Emails_dbo.EmailTypes_emailTypeId]
GO
ALTER TABLE [dbo].[Images]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Images_dbo.Trades_tradeId] FOREIGN KEY([tradeId])
REFERENCES [dbo].[Trades] ([tradeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Images] CHECK CONSTRAINT [FK_dbo.Images_dbo.Trades_tradeId]
GO
ALTER TABLE [dbo].[PersonalDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PersonalDetails_dbo.AspNetUsers_traderId] FOREIGN KEY([traderId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PersonalDetails] CHECK CONSTRAINT [FK_dbo.PersonalDetails_dbo.AspNetUsers_traderId]
GO
ALTER TABLE [dbo].[Phones]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Phones_dbo.AspNetUsers_traderId] FOREIGN KEY([traderId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Phones] CHECK CONSTRAINT [FK_dbo.Phones_dbo.AspNetUsers_traderId]
GO
ALTER TABLE [dbo].[Phones]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Phones_dbo.PhoneTypes_phoneTypeId] FOREIGN KEY([phoneTypeId])
REFERENCES [dbo].[PhoneTypes] ([phoneTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Phones] CHECK CONSTRAINT [FK_dbo.Phones_dbo.PhoneTypes_phoneTypeId]
GO
ALTER TABLE [dbo].[ProcessMessages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ProcessMessages_dbo.ProcessMessageTypes_messageTypeId] FOREIGN KEY([messageTypeId])
REFERENCES [dbo].[ProcessMessageTypes] ([messageTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProcessMessages] CHECK CONSTRAINT [FK_dbo.ProcessMessages_dbo.ProcessMessageTypes_messageTypeId]
GO
ALTER TABLE [dbo].[SocialNetworks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SocialNetworks_dbo.AspNetUsers_traderId] FOREIGN KEY([traderId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[SocialNetworks] CHECK CONSTRAINT [FK_dbo.SocialNetworks_dbo.AspNetUsers_traderId]
GO
ALTER TABLE [dbo].[SocialNetworks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SocialNetworks_dbo.SocialNetworkTypes_socialTypeId] FOREIGN KEY([socialTypeId])
REFERENCES [dbo].[SocialNetworkTypes] ([socialTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SocialNetworks] CHECK CONSTRAINT [FK_dbo.SocialNetworks_dbo.SocialNetworkTypes_socialTypeId]
GO
ALTER TABLE [dbo].[Subcategories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Subcategories_dbo.Categories_categoryId] FOREIGN KEY([categoryId])
REFERENCES [dbo].[Categories] ([categoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subcategories] CHECK CONSTRAINT [FK_dbo.Subcategories_dbo.Categories_categoryId]
GO
ALTER TABLE [dbo].[TradeHistories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TradeHistories_dbo.Trades_tradeId] FOREIGN KEY([tradeId])
REFERENCES [dbo].[Trades] ([tradeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TradeHistories] CHECK CONSTRAINT [FK_dbo.TradeHistories_dbo.Trades_tradeId]
GO
ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Trades_dbo.AspNetUsers_traderId] FOREIGN KEY([traderId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [FK_dbo.Trades_dbo.AspNetUsers_traderId]
GO
USE [master]
GO
ALTER DATABASE [iTradeDatabase] SET  READ_WRITE 
GO
