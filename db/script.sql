USE [master]
GO
/****** Object:  Database [warehouse]    Script Date: 6/15/2021 5:12:27 AM ******/
CREATE DATABASE [warehouse]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'warehouse', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\warehouse.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'warehouse_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\warehouse_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [warehouse].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [warehouse] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [warehouse] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [warehouse] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [warehouse] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [warehouse] SET ARITHABORT OFF 
GO
ALTER DATABASE [warehouse] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [warehouse] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [warehouse] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [warehouse] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [warehouse] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [warehouse] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [warehouse] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [warehouse] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [warehouse] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [warehouse] SET  DISABLE_BROKER 
GO
ALTER DATABASE [warehouse] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [warehouse] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [warehouse] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [warehouse] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [warehouse] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [warehouse] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [warehouse] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [warehouse] SET RECOVERY FULL 
GO
ALTER DATABASE [warehouse] SET  MULTI_USER 
GO
ALTER DATABASE [warehouse] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [warehouse] SET DB_CHAINING OFF 
GO
ALTER DATABASE [warehouse] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [warehouse] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'warehouse', N'ON'
GO
USE [warehouse]
GO
/****** Object:  Table [dbo].[IOInventory]    Script Date: 6/15/2021 5:12:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IOInventory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QRCode] [nvarchar](200) NOT NULL,
	[Quantity] [numeric](18, 0) NOT NULL,
	[Status] [smallint] NOT NULL,
	[WarehouseId] [int] NOT NULL,
	[WarehouseName] [nvarchar](200) NULL,
	[Username] [nvarchar](200) NOT NULL,
	[TransactionId] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_io_inventory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 6/15/2021 5:12:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](200) NOT NULL,
	[Usermobi] [int] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WHMobi]    Script Date: 6/15/2021 5:12:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WHMobi](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
 CONSTRAINT [PK_WHMobi] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[IOInventory] ON 

INSERT [dbo].[IOInventory] ([Id], [QRCode], [Quantity], [Status], [WarehouseId], [WarehouseName], [Username], [TransactionId]) VALUES (2, N'123', CAST(123 AS Numeric(18, 0)), 1, 1, N'abc', N'abc', N'20210615509_abc')
INSERT [dbo].[IOInventory] ([Id], [QRCode], [Quantity], [Status], [WarehouseId], [WarehouseName], [Username], [TransactionId]) VALUES (3, N'123', CAST(123 AS Numeric(18, 0)), 1, 1, N'abc', N'abc', N'20210615510_abc')
INSERT [dbo].[IOInventory] ([Id], [QRCode], [Quantity], [Status], [WarehouseId], [WarehouseName], [Username], [TransactionId]) VALUES (4, N'1234', CAST(123 AS Numeric(18, 0)), 1, 1, N'abc', N'abc', N'20210615510_abc')
SET IDENTITY_INSERT [dbo].[IOInventory] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [Username], [Usermobi], [Status]) VALUES (1, N'abc', 1, 1)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[WHMobi] ON 

INSERT [dbo].[WHMobi] ([Id], [Name]) VALUES (1, N'abc')
INSERT [dbo].[WHMobi] ([Id], [Name]) VALUES (2, N'dfe')
SET IDENTITY_INSERT [dbo].[WHMobi] OFF
GO
USE [master]
GO
ALTER DATABASE [warehouse] SET  READ_WRITE 
GO
