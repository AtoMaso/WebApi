USE [master]
GO
/****** Object:  Database [iTradeDatabase]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE DATABASE [iTradeDatabase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'iTradeDatabase', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQL2012\MSSQL\DATA\iTradeDatabase.mdf' , SIZE = 4160KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'iTradeDatabase_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQL2012\MSSQL\DATA\iTradeDatabase_log.ldf' , SIZE = 1040KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
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
ALTER DATABASE [iTradeDatabase] SET AUTO_CLOSE OFF 
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
ALTER DATABASE [iTradeDatabase] SET RECOVERY FULL 
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
EXEC sys.sp_db_vardecimal_storage_format N'iTradeDatabase', N'ON'
GO
USE [iTradeDatabase]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 21/02/2018 7:08:26 PM ******/
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
/****** Object:  Table [dbo].[Addresses]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Addresses](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[number] [nvarchar](8) NOT NULL,
	[street] [nvarchar](30) NOT NULL,
	[suburb] [nvarchar](30) NOT NULL,
	[city] [nvarchar](20) NOT NULL,
	[postcode] [nvarchar](10) NOT NULL,
	[state] [nvarchar](20) NOT NULL,
	[country] [nvarchar](30) NOT NULL,
	[preferred] [nvarchar](10) NOT NULL,
	[typeId] [int] NOT NULL,
	[personalDetailsId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Addresses] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AddressTypes]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressTypes](
	[typeId] [int] IDENTITY(1,1) NOT NULL,
	[typeDescription] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.AddressTypes] PRIMARY KEY CLUSTERED 
(
	[typeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 21/02/2018 7:08:26 PM ******/
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
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 21/02/2018 7:08:26 PM ******/
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
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 21/02/2018 7:08:26 PM ******/
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
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 21/02/2018 7:08:26 PM ******/
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
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 21/02/2018 7:08:26 PM ******/
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
/****** Object:  Table [dbo].[Categories]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[categoryId] [int] IDENTITY(1,1) NOT NULL,
	[categoryDescription] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.Categories] PRIMARY KEY CLUSTERED 
(
	[categoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContactDetails]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactDetails](
	[contactDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[traderId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.ContactDetails] PRIMARY KEY CLUSTERED 
(
	[contactDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Correspondences]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Correspondences](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[subject] [nvarchar](20) NOT NULL,
	[message] [nvarchar](70) NOT NULL,
	[content] [nvarchar](500) NOT NULL,
	[status] [nvarchar](10) NOT NULL,
	[dateSent] [datetime] NOT NULL,
	[tradeId] [int] NOT NULL,
	[traderIdReciever] [nvarchar](max) NOT NULL,
	[traderIdSender] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.Correspondences] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Emails]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Emails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[account] [nvarchar](70) NOT NULL,
	[preferred] [nvarchar](10) NOT NULL,
	[typeId] [int] NOT NULL,
	[contactDetailsId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Emails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmailTypes]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTypes](
	[typeId] [int] IDENTITY(1,1) NOT NULL,
	[typeDescription] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.EmailTypes] PRIMARY KEY CLUSTERED 
(
	[typeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Images]    Script Date: 21/02/2018 7:08:26 PM ******/
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
/****** Object:  Table [dbo].[PersonalDetails]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonalDetails](
	[personalDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[firstName] [nvarchar](20) NOT NULL,
	[middleName] [nvarchar](20) NULL,
	[lastName] [nvarchar](30) NOT NULL,
	[dateOfBirth] [datetime] NOT NULL,
	[traderId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.PersonalDetails] PRIMARY KEY CLUSTERED 
(
	[personalDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Phones]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Phones](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[typeId] [int] NOT NULL,
	[number] [nvarchar](10) NOT NULL,
	[countryCode] [nvarchar](10) NOT NULL,
	[cityCode] [nvarchar](10) NOT NULL,
	[preferred] [nvarchar](10) NOT NULL,
	[contactDetailsId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Phones] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PhoneTypes]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhoneTypes](
	[typeId] [int] IDENTITY(1,1) NOT NULL,
	[typeDescription] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.PhoneTypes] PRIMARY KEY CLUSTERED 
(
	[typeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Places]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Places](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](25) NOT NULL,
	[stateId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Places] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProcessMessages]    Script Date: 21/02/2018 7:08:26 PM ******/
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
/****** Object:  Table [dbo].[ProcessMessageTypes]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcessMessageTypes](
	[messageTypeId] [int] IDENTITY(1,1) NOT NULL,
	[messageTypeDescription] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.ProcessMessageTypes] PRIMARY KEY CLUSTERED 
(
	[messageTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SecurityAnswers]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityAnswers](
	[answerId] [int] IDENTITY(1,1) NOT NULL,
	[questionId] [int] NOT NULL,
	[questionAnswer] [nvarchar](20) NOT NULL,
	[securityDetailsId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SecurityAnswers] PRIMARY KEY CLUSTERED 
(
	[answerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SecurityDetails]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityDetails](
	[securityDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[traderId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.SecurityDetails] PRIMARY KEY CLUSTERED 
(
	[securityDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SecurityQuestions]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityQuestions](
	[questionId] [int] IDENTITY(1,1) NOT NULL,
	[questionText] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_dbo.SecurityQuestions] PRIMARY KEY CLUSTERED 
(
	[questionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SocialNetworks]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SocialNetworks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[account] [nvarchar](70) NOT NULL,
	[preferred] [nvarchar](10) NOT NULL,
	[typeId] [int] NOT NULL,
	[contactDetailsId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SocialNetworks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SocialNetworkTypes]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SocialNetworkTypes](
	[typeId] [int] IDENTITY(1,1) NOT NULL,
	[typeDescription] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_dbo.SocialNetworkTypes] PRIMARY KEY CLUSTERED 
(
	[typeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[States]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[States](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_dbo.States] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Subcategories]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subcategories](
	[subcategoryId] [int] IDENTITY(1,1) NOT NULL,
	[subcategoryDescription] [nvarchar](30) NOT NULL,
	[categoryId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Subcategories] PRIMARY KEY CLUSTERED 
(
	[subcategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TradeHistories]    Script Date: 21/02/2018 7:08:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TradeHistories](
	[historyId] [int] IDENTITY(1,1) NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[status] [nvarchar](20) NOT NULL,
	[tradeId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TradeHistories] PRIMARY KEY CLUSTERED 
(
	[historyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Trades]    Script Date: 21/02/2018 7:08:26 PM ******/
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
	[status] [nvarchar](10) NOT NULL,
	[stateId] [int] NOT NULL,
	[placeId] [int] NOT NULL,
	[categoryId] [int] NOT NULL,
	[subcategoryId] [int] NOT NULL,
	[traderId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.Trades] PRIMARY KEY CLUSTERED 
(
	[tradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_personalDetailsId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_personalDetailsId] ON [dbo].[Addresses]
(
	[personalDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_typeId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_typeId] ON [dbo].[Addresses]
(
	[typeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [RoleNameIndex]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_RoleId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UserNameIndex]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_traderId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_traderId] ON [dbo].[ContactDetails]
(
	[traderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_tradeId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_tradeId] ON [dbo].[Correspondences]
(
	[tradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_contactDetailsId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_contactDetailsId] ON [dbo].[Emails]
(
	[contactDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_typeId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_typeId] ON [dbo].[Emails]
(
	[typeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_tradeId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_tradeId] ON [dbo].[Images]
(
	[tradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_traderId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_traderId] ON [dbo].[PersonalDetails]
(
	[traderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_contactDetailsId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_contactDetailsId] ON [dbo].[Phones]
(
	[contactDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_typeId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_typeId] ON [dbo].[Phones]
(
	[typeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_messageTypeId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_messageTypeId] ON [dbo].[ProcessMessages]
(
	[messageTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_securityDetailsId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_securityDetailsId] ON [dbo].[SecurityAnswers]
(
	[securityDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_traderId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_traderId] ON [dbo].[SecurityDetails]
(
	[traderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_contactDetailsId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_contactDetailsId] ON [dbo].[SocialNetworks]
(
	[contactDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_typeId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_typeId] ON [dbo].[SocialNetworks]
(
	[typeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_tradeId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_tradeId] ON [dbo].[TradeHistories]
(
	[tradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_categoryId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_categoryId] ON [dbo].[Trades]
(
	[categoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_placeId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_placeId] ON [dbo].[Trades]
(
	[placeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_stateId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_stateId] ON [dbo].[Trades]
(
	[stateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_subcategoryId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_subcategoryId] ON [dbo].[Trades]
(
	[subcategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_traderId]    Script Date: 21/02/2018 7:08:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_traderId] ON [dbo].[Trades]
(
	[traderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Addresses_dbo.AddressTypes_typeId] FOREIGN KEY([typeId])
REFERENCES [dbo].[AddressTypes] ([typeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_dbo.Addresses_dbo.AddressTypes_typeId]
GO
ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Addresses_dbo.PersonalDetails_personalDetailsId] FOREIGN KEY([personalDetailsId])
REFERENCES [dbo].[PersonalDetails] ([personalDetailsId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_dbo.Addresses_dbo.PersonalDetails_personalDetailsId]
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
ALTER TABLE [dbo].[ContactDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ContactDetails_dbo.AspNetUsers_traderId] FOREIGN KEY([traderId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContactDetails] CHECK CONSTRAINT [FK_dbo.ContactDetails_dbo.AspNetUsers_traderId]
GO
ALTER TABLE [dbo].[Correspondences]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Correspondences_dbo.Trades_tradeId] FOREIGN KEY([tradeId])
REFERENCES [dbo].[Trades] ([tradeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Correspondences] CHECK CONSTRAINT [FK_dbo.Correspondences_dbo.Trades_tradeId]
GO
ALTER TABLE [dbo].[Emails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Emails_dbo.ContactDetails_contactDetailsId] FOREIGN KEY([contactDetailsId])
REFERENCES [dbo].[ContactDetails] ([contactDetailsId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Emails] CHECK CONSTRAINT [FK_dbo.Emails_dbo.ContactDetails_contactDetailsId]
GO
ALTER TABLE [dbo].[Emails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Emails_dbo.EmailTypes_typeId] FOREIGN KEY([typeId])
REFERENCES [dbo].[EmailTypes] ([typeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Emails] CHECK CONSTRAINT [FK_dbo.Emails_dbo.EmailTypes_typeId]
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
ALTER TABLE [dbo].[Phones]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Phones_dbo.ContactDetails_contactDetailsId] FOREIGN KEY([contactDetailsId])
REFERENCES [dbo].[ContactDetails] ([contactDetailsId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Phones] CHECK CONSTRAINT [FK_dbo.Phones_dbo.ContactDetails_contactDetailsId]
GO
ALTER TABLE [dbo].[Phones]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Phones_dbo.PhoneTypes_typeId] FOREIGN KEY([typeId])
REFERENCES [dbo].[PhoneTypes] ([typeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Phones] CHECK CONSTRAINT [FK_dbo.Phones_dbo.PhoneTypes_typeId]
GO
ALTER TABLE [dbo].[ProcessMessages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ProcessMessages_dbo.ProcessMessageTypes_messageTypeId] FOREIGN KEY([messageTypeId])
REFERENCES [dbo].[ProcessMessageTypes] ([messageTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProcessMessages] CHECK CONSTRAINT [FK_dbo.ProcessMessages_dbo.ProcessMessageTypes_messageTypeId]
GO
ALTER TABLE [dbo].[SecurityAnswers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SecurityAnswers_dbo.SecurityDetails_securityDetailsId] FOREIGN KEY([securityDetailsId])
REFERENCES [dbo].[SecurityDetails] ([securityDetailsId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SecurityAnswers] CHECK CONSTRAINT [FK_dbo.SecurityAnswers_dbo.SecurityDetails_securityDetailsId]
GO
ALTER TABLE [dbo].[SecurityDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SecurityDetails_dbo.AspNetUsers_traderId] FOREIGN KEY([traderId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SecurityDetails] CHECK CONSTRAINT [FK_dbo.SecurityDetails_dbo.AspNetUsers_traderId]
GO
ALTER TABLE [dbo].[SocialNetworks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SocialNetworks_dbo.ContactDetails_contactDetailsId] FOREIGN KEY([contactDetailsId])
REFERENCES [dbo].[ContactDetails] ([contactDetailsId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SocialNetworks] CHECK CONSTRAINT [FK_dbo.SocialNetworks_dbo.ContactDetails_contactDetailsId]
GO
ALTER TABLE [dbo].[SocialNetworks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SocialNetworks_dbo.SocialNetworkTypes_typeId] FOREIGN KEY([typeId])
REFERENCES [dbo].[SocialNetworkTypes] ([typeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SocialNetworks] CHECK CONSTRAINT [FK_dbo.SocialNetworks_dbo.SocialNetworkTypes_typeId]
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
ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Trades_dbo.Categories_categoryId] FOREIGN KEY([categoryId])
REFERENCES [dbo].[Categories] ([categoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [FK_dbo.Trades_dbo.Categories_categoryId]
GO
ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Trades_dbo.Places_placeId] FOREIGN KEY([placeId])
REFERENCES [dbo].[Places] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [FK_dbo.Trades_dbo.Places_placeId]
GO
ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Trades_dbo.States_stateId] FOREIGN KEY([stateId])
REFERENCES [dbo].[States] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [FK_dbo.Trades_dbo.States_stateId]
GO
ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Trades_dbo.Subcategories_subcategoryId] FOREIGN KEY([subcategoryId])
REFERENCES [dbo].[Subcategories] ([subcategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [FK_dbo.Trades_dbo.Subcategories_subcategoryId]
GO
USE [master]
GO
ALTER DATABASE [iTradeDatabase] SET  READ_WRITE 
GO
