USE [master]
GO
/****** Object:  Database [ArtClubDb]    Script Date: 5/8/2026 1:16:22 AM ******/
CREATE DATABASE [ArtClubDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ArtClubDb', FILENAME = N'C:\Users\dobre\ArtClubDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ArtClubDb_log', FILENAME = N'C:\Users\dobre\ArtClubDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ArtClubDb] SET COMPATIBILITY_LEVEL = 170
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ArtClubDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ArtClubDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ArtClubDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ArtClubDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ArtClubDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ArtClubDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [ArtClubDb] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ArtClubDb] SET AUTO_SHRINK OFF 
GO 
ALTER DATABASE [ArtClubDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ArtClubDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ArtClubDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ArtClubDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ArtClubDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ArtClubDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ArtClubDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ArtClubDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ArtClubDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ArtClubDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ArtClubDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ArtClubDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ArtClubDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ArtClubDb] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [ArtClubDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ArtClubDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ArtClubDb] SET  MULTI_USER 
GO
ALTER DATABASE [ArtClubDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ArtClubDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ArtClubDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ArtClubDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ArtClubDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ArtClubDb] SET OPTIMIZED_LOCKING = OFF 
GO
ALTER DATABASE [ArtClubDb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ArtClubDb] SET QUERY_STORE = ON
GO
ALTER DATABASE [ArtClubDb] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ArtClubDb]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 5/8/2026 1:16:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArtPieces]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArtPieces](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Creator] [nvarchar](max) NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[IsPopular] [bit] NOT NULL,
 CONSTRAINT [PK_ArtPieces] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Role] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsMembershipActive] [bit] NOT NULL,
	[MembershipDate] [datetime2](7) NULL,
	[EventCreationLimit] [int] NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[IsBanned] [bit] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [int] NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventArtPieces]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventArtPieces](
	[EventId] [int] NOT NULL,
	[ArtPieceId] [int] NOT NULL,
 CONSTRAINT [PK_EventArtPieces] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[ArtPieceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](1000) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Status] [int] NOT NULL,
	[Budget] [decimal](18, 2) NOT NULL,
	[OrganizerId] [int] NOT NULL,
	[ResourceId] [int] NOT NULL,
	[IsPaid] [bit] NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invitations]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invitations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventId] [int] NOT NULL,
	[InviteeId] [int] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Invitations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Type] [int] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[IsIncome] [bit] NOT NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reservations]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ResourceId] [int] NOT NULL,
	[EventId] [int] NULL,
	[StartTime] [datetime2](7) NOT NULL,
	[EndTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Reservations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resources]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resources](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Capacity] [int] NOT NULL,
	[BasePrice] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Resources] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/8/2026 1:16:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[Role] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsBanned] [bit] NOT NULL,
	[IsMembershipActive] [bit] NOT NULL,
	[MembershipDate] [datetime2](7) NULL,
	[EventCreationLimit] [int] NOT NULL,
	[UserType] [nvarchar](50) NOT NULL,
	[AdminLevel] [int] NULL,
	[CanOverrideReservations] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260417121621_InitialCreate', N'10.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260504221054_actualizareevent', N'10.0.5')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260506170939_InitialCreate', N'10.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260507135335_AddAdminControlls', N'10.0.5')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260507142825_Admin', N'10.0.5')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260507200115_Initial1', N'10.0.5')
GO
SET IDENTITY_INSERT [dbo].[ArtPieces] ON 

INSERT [dbo].[ArtPieces] ([Id], [Title], [Creator], [ImageUrl], [IsPopular]) VALUES (1, N'Sunlit Studio', N'A. Ionescu', N'https://th.bing.com/th/id/OIP.3sfiAVXQMT9ttnyfU8Zn6wHaEJ?w=322&h=181&c=7&r=0&o=7&dpr=2.5&pid=1.7&rm=3', 1)
INSERT [dbo].[ArtPieces] ([Id], [Title], [Creator], [ImageUrl], [IsPopular]) VALUES (2, N'Modern Shapes', N'G. Pop', N'https://i.pinimg.com/originals/f5/37/fc/f537fcb465335414f09e8a7a2af4a5ca.jpg', 1)
INSERT [dbo].[ArtPieces] ([Id], [Title], [Creator], [ImageUrl], [IsPopular]) VALUES (3, N'Gallery Night', N'L. Marin', N'https://www.bing.com/th/id/OIP.z3PL5ZlMX7AZ9Em_lcjcWwHaFu?w=193&h=149&c=8&rs=1&qlt=90&o=6&dpr=2.5&pid=3.1&rm=2', 0)
INSERT [dbo].[ArtPieces] ([Id], [Title], [Creator], [ImageUrl], [IsPopular]) VALUES (4, N'Blue Abstract', N'R. Stan', N'https://th.bing.com/th/id/OIP.L886y88un-v9R16Tl4H9qQHaEU?w=327&h=190&c=7&r=0&o=7&dpr=2.5&pid=1.7&rm=3', 0)
INSERT [dbo].[ArtPieces] ([Id], [Title], [Creator], [ImageUrl], [IsPopular]) VALUES (5, N'Urban Lines', N'M. Enache', N'https://th.bing.com/th/id/OIP.zHdlwpcwDUY2Ma8FiKq2sgHaFS?w=287&h=205&c=7&r=0&o=7&dpr=2.5&pid=1.7&rm=3', 0)
SET IDENTITY_INSERT [dbo].[ArtPieces] OFF
GO
SET IDENTITY_INSERT [dbo].[AspNetUsers] ON 

INSERT [dbo].[AspNetUsers] ([Id], [Role], [IsActive], [IsMembershipActive], [MembershipDate], [EventCreationLimit], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsBanned]) VALUES (1, 2, 1, 1, CAST(N'2026-05-06T20:10:54.3859085' AS DateTime2), 5, N'lorena@yahoo.com', N'LORENA@YAHOO.COM', N'lorena@yahoo.com', N'LORENA@YAHOO.COM', 0, N'AQAAAAIAAYagAAAAEGYKWaGLWDQBwwq4Y+VAde7ppgcICtwMGxlxIMnmbtfbpVKWz45h7/5LAQ/U8QNe5w==', N'H2RUDT4C47OUXBFYZPWHKJ6BFFAJNILA', N'c267e42c-2526-4062-a2ad-f86fc1014c68', NULL, 0, 0, NULL, 1, 0, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Role], [IsActive], [IsMembershipActive], [MembershipDate], [EventCreationLimit], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsBanned]) VALUES (2, 2, 1, 0, CAST(N'2026-05-07T02:26:56.7173322' AS DateTime2), 1, N'mihaipopescu@yahoo.com', N'MIHAIPOPESCU@YAHOO.COM', N'mihaipopescu@yahoo.com', N'MIHAIPOPESCU@YAHOO.COM', 0, N'AQAAAAIAAYagAAAAEPv/RmZZl1XhAPu6TJSgaadvytziOGRAaplopPhypEaZ4jD4LYxIy9LToyS9MKmb/Q==', N'SXB7735FSK3EILAJQ5TEMV6DQZVGFZIC', N'8a3335c3-7a58-4680-a9da-ef5a45790cb4', NULL, 0, 0, NULL, 1, 0, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Role], [IsActive], [IsMembershipActive], [MembershipDate], [EventCreationLimit], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsBanned]) VALUES (3, 2, 1, 0, CAST(N'2026-05-07T02:28:10.5135164' AS DateTime2), 1, N'ana.soara@gmail.com', N'ANA.SOARA@GMAIL.COM', N'ana.soara@gmail.com', N'ANA.SOARA@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEKW56sT5cCmq0VnLJjs70n2T/9wgU7YK9DHTNRy9+5kPZt9KE97WmMk8YI1Q2pDcVg==', N'G454RRA2JT2YS6HBZS5KCEXXLEFKP35O', N'ff97eb13-6990-4d1e-980b-451e219b93e4', NULL, 0, 0, NULL, 1, 0, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Role], [IsActive], [IsMembershipActive], [MembershipDate], [EventCreationLimit], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsBanned]) VALUES (4, 2, 1, 1, CAST(N'2026-05-07T02:30:15.5726304' AS DateTime2), 5, N'diaconualexandru@yahoo.com', N'DIACONUALEXANDRU@YAHOO.COM', N'diaconualexandru@yahoo.com', N'DIACONUALEXANDRU@YAHOO.COM', 0, N'AQAAAAIAAYagAAAAECaRK8KI0wUP3nKyfUgskYuDdrkS0C3Icbs+LUo8v+/lWGNh5LH7a03tiAW1UTmxqQ==', N'2WU7K6XEBEGY32GNLN44CZBUQAMNJXLU', N'5566f2c2-b6eb-4235-9572-a39120cd3e2f', NULL, 0, 0, NULL, 1, 0, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Role], [IsActive], [IsMembershipActive], [MembershipDate], [EventCreationLimit], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsBanned]) VALUES (5, 2, 1, 1, CAST(N'2026-05-07T12:08:31.0567524' AS DateTime2), 5, N'andrei@yahoo.com', N'ANDREI@YAHOO.COM', N'andrei@yahoo.com', N'ANDREI@YAHOO.COM', 0, N'AQAAAAIAAYagAAAAELdocdl+Ha4gzmi+5A/L5RYCpAqdq/e3Xg/WI68NfAlb7GroNJUsx+/FJU5z2DPk3w==', N'SP4LTQO3YF6Z3VAGP2JQ4WVYB6ASAYRO', N'2e3664f8-fa7d-4d68-b0be-df7d21ac359b', NULL, 0, 0, NULL, 1, 0, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Role], [IsActive], [IsMembershipActive], [MembershipDate], [EventCreationLimit], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsBanned]) VALUES (6, 2, 1, 0, CAST(N'2026-05-08T00:30:23.9212531' AS DateTime2), 1, N'anapopescu@yahoo.com', N'ANAPOPESCU@YAHOO.COM', N'anapopescu@yahoo.com', N'ANAPOPESCU@YAHOO.COM', 0, N'AQAAAAIAAYagAAAAEEthUph4A83TGE2j6Mxwp79NrFsx8Lc0ohk9R0GmzLT6ltLDyeg84ReF6wobm7gcOQ==', N'K55WLLBL5EIRHKS3WPA7ZVEV2V5JQGEC', N'10ff344d-0b77-467f-9431-cbfa9d15989d', NULL, 0, 0, NULL, 1, 0, 0)
SET IDENTITY_INSERT [dbo].[AspNetUsers] OFF
GO
INSERT [dbo].[EventArtPieces] ([EventId], [ArtPieceId]) VALUES (1, 1)
INSERT [dbo].[EventArtPieces] ([EventId], [ArtPieceId]) VALUES (3, 1)
INSERT [dbo].[EventArtPieces] ([EventId], [ArtPieceId]) VALUES (2, 2)
INSERT [dbo].[EventArtPieces] ([EventId], [ArtPieceId]) VALUES (1, 3)
INSERT [dbo].[EventArtPieces] ([EventId], [ArtPieceId]) VALUES (2, 4)
INSERT [dbo].[EventArtPieces] ([EventId], [ArtPieceId]) VALUES (3, 5)
GO
SET IDENTITY_INSERT [dbo].[Events] ON 

INSERT [dbo].[Events] ([Id], [Title], [Description], [Date], [Status], [Budget], [OrganizerId], [ResourceId], [IsPaid]) VALUES (1, N'Spring Exhibition', N'Exhibition dedicated to spring-inspired artworks created by club members. The event presents paintings, sketches and mixed-media pieces focused on nature, color and renewal.', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 0, CAST(700.00 AS Decimal(18, 2)), 1, 1, 1)
INSERT [dbo].[Events] ([Id], [Title], [Description], [Date], [Status], [Budget], [OrganizerId], [ResourceId], [IsPaid]) VALUES (2, N'Modern Art Workshop', N'Interactive workshop where participants explore modern composition techniques, abstract forms and creative visual expression. The event is designed for members interested in practical artistic exercises.', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 0, CAST(700.00 AS Decimal(18, 2)), 1, 4, 0)
INSERT [dbo].[Events] ([Id], [Title], [Description], [Date], [Status], [Budget], [OrganizerId], [ResourceId], [IsPaid]) VALUES (3, N'Watercolor Evening', N'Creative evening focused on watercolor painting techniques. Participants practice color blending, light effects and landscape-inspired compositions in a relaxed club environment.', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 0, CAST(700.00 AS Decimal(18, 2)), 4, 1, 1)
SET IDENTITY_INSERT [dbo].[Events] OFF
GO
SET IDENTITY_INSERT [dbo].[Invitations] ON 

INSERT [dbo].[Invitations] ([Id], [EventId], [InviteeId], [Status]) VALUES (1, 1, 3, 0)
INSERT [dbo].[Invitations] ([Id], [EventId], [InviteeId], [Status]) VALUES (2, 1, 2, 0)
INSERT [dbo].[Invitations] ([Id], [EventId], [InviteeId], [Status]) VALUES (3, 2, 3, 0)
INSERT [dbo].[Invitations] ([Id], [EventId], [InviteeId], [Status]) VALUES (4, 3, 1, 1)
INSERT [dbo].[Invitations] ([Id], [EventId], [InviteeId], [Status]) VALUES (5, 3, 2, 0)
INSERT [dbo].[Invitations] ([Id], [EventId], [InviteeId], [Status]) VALUES (6, 2, 4, 0)
INSERT [dbo].[Invitations] ([Id], [EventId], [InviteeId], [Status]) VALUES (8, 1, 5, 0)
INSERT [dbo].[Invitations] ([Id], [EventId], [InviteeId], [Status]) VALUES (9, 1, 6, 1)
SET IDENTITY_INSERT [dbo].[Invitations] OFF
GO
SET IDENTITY_INSERT [dbo].[Payments] ON 

INSERT [dbo].[Payments] ([Id], [UserId], [Amount], [Type], [Date], [Description], [IsIncome]) VALUES (1, 1, CAST(300.00 AS Decimal(18, 2)), 0, CAST(N'2026-05-07T00:00:00.0000000' AS DateTime2), N'Venit nou', 1)
INSERT [dbo].[Payments] ([Id], [UserId], [Amount], [Type], [Date], [Description], [IsIncome]) VALUES (2, 1, CAST(150.00 AS Decimal(18, 2)), 0, CAST(N'2026-05-07T00:00:00.0000000' AS DateTime2), N'Venit nou', 1)
INSERT [dbo].[Payments] ([Id], [UserId], [Amount], [Type], [Date], [Description], [IsIncome]) VALUES (3, 1, CAST(80.00 AS Decimal(18, 2)), 2, CAST(N'2026-05-05T00:00:00.0000000' AS DateTime2), N'Cheltuială nouă', 0)
INSERT [dbo].[Payments] ([Id], [UserId], [Amount], [Type], [Date], [Description], [IsIncome]) VALUES (4, 5, CAST(200.00 AS Decimal(18, 2)), 0, CAST(N'2026-05-07T00:00:00.0000000' AS DateTime2), N'Venit nou', 1)
INSERT [dbo].[Payments] ([Id], [UserId], [Amount], [Type], [Date], [Description], [IsIncome]) VALUES (8, 1, CAST(30.00 AS Decimal(18, 2)), 0, CAST(N'2026-05-08T00:00:00.0000000' AS DateTime2), N'Venit nou', 1)
INSERT [dbo].[Payments] ([Id], [UserId], [Amount], [Type], [Date], [Description], [IsIncome]) VALUES (9, 1, CAST(100.00 AS Decimal(18, 2)), 0, CAST(N'2026-05-08T00:00:00.0000000' AS DateTime2), N'Venit nou', 1)
INSERT [dbo].[Payments] ([Id], [UserId], [Amount], [Type], [Date], [Description], [IsIncome]) VALUES (10, 1, CAST(700.00 AS Decimal(18, 2)), 0, CAST(N'2026-05-08T00:13:51.4914512' AS DateTime2), N'Încasare finală eveniment: Spring Exhibition', 1)
INSERT [dbo].[Payments] ([Id], [UserId], [Amount], [Type], [Date], [Description], [IsIncome]) VALUES (11, 4, CAST(700.00 AS Decimal(18, 2)), 0, CAST(N'2026-05-08T00:23:12.6763935' AS DateTime2), N'Încasare finală eveniment: Watercolor Evening', 1)
SET IDENTITY_INSERT [dbo].[Payments] OFF
GO
SET IDENTITY_INSERT [dbo].[Reservations] ON 

INSERT [dbo].[Reservations] ([Id], [ResourceId], [EventId], [StartTime], [EndTime]) VALUES (1, 1, 1, CAST(N'2026-05-08T10:00:00.0000000' AS DateTime2), CAST(N'2026-05-09T14:00:00.0000000' AS DateTime2))
INSERT [dbo].[Reservations] ([Id], [ResourceId], [EventId], [StartTime], [EndTime]) VALUES (2, 4, 2, CAST(N'2026-05-10T00:00:00.0000000' AS DateTime2), CAST(N'2026-05-10T04:00:00.0000000' AS DateTime2))
INSERT [dbo].[Reservations] ([Id], [ResourceId], [EventId], [StartTime], [EndTime]) VALUES (3, 1, 3, CAST(N'2026-05-10T10:00:00.0000000' AS DateTime2), CAST(N'2026-05-11T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Reservations] OFF
GO
SET IDENTITY_INSERT [dbo].[Resources] ON 

INSERT [dbo].[Resources] ([Id], [Name], [Description], [Capacity], [BasePrice]) VALUES (1, N'Art Studio 1', N'Painting studio', 20, CAST(100.00 AS Decimal(18, 2)))
INSERT [dbo].[Resources] ([Id], [Name], [Description], [Capacity], [BasePrice]) VALUES (2, N'Main Hall', N'Exhibition hall', 120, CAST(300.00 AS Decimal(18, 2)))
INSERT [dbo].[Resources] ([Id], [Name], [Description], [Capacity], [BasePrice]) VALUES (3, N'Conference Room', N'Meeting room', 40, CAST(200.00 AS Decimal(18, 2)))
INSERT [dbo].[Resources] ([Id], [Name], [Description], [Capacity], [BasePrice]) VALUES (4, N'Workshop Room', N'Studio', 50, CAST(200.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Resources] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [UserName], [Email], [PasswordHash], [Role], [IsActive], [IsBanned], [IsMembershipActive], [MembershipDate], [EventCreationLimit], [UserType], [AdminLevel], [CanOverrideReservations]) VALUES (1, N'Lorena Dobre', N'lorena@yahoo.com', N'Lorena1!', 1, 1, 0, 0, CAST(N'2026-05-07T23:31:21.7684077' AS DateTime2), 5, N'Member', NULL, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [PasswordHash], [Role], [IsActive], [IsBanned], [IsMembershipActive], [MembershipDate], [EventCreationLimit], [UserType], [AdminLevel], [CanOverrideReservations]) VALUES (2, N'Ana  Popescu', N'ana@yahoo.com', N'Lorena1!', 1, 1, 0, 0, CAST(N'2026-05-07T23:47:39.2039323' AS DateTime2), 5, N'Member', NULL, 0)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EventArtPieces_ArtPieceId]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_EventArtPieces_ArtPieceId] ON [dbo].[EventArtPieces]
(
	[ArtPieceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Events_OrganizerId]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_Events_OrganizerId] ON [dbo].[Events]
(
	[OrganizerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Events_ResourceId]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_Events_ResourceId] ON [dbo].[Events]
(
	[ResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Invitations_EventId]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_Invitations_EventId] ON [dbo].[Invitations]
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Invitations_InviteeId]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_Invitations_InviteeId] ON [dbo].[Invitations]
(
	[InviteeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Payments_UserId]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_Payments_UserId] ON [dbo].[Payments]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Reservations_EventId]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Reservations_EventId] ON [dbo].[Reservations]
(
	[EventId] ASC
)
WHERE ([EventId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Reservations_ResourceId]    Script Date: 5/8/2026 1:16:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_Reservations_ResourceId] ON [dbo].[Reservations]
(
	[ResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsMembershipActive]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((1)) FOR [EventCreationLimit]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsBanned]
GO
ALTER TABLE [dbo].[Events] ADD  DEFAULT ((0)) FOR [IsPaid]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((2)) FOR [Role]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsBanned]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsMembershipActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [EventCreationLimit]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ('Member') FOR [UserType]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [CanOverrideReservations]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[EventArtPieces]  WITH CHECK ADD  CONSTRAINT [FK_EventArtPieces_ArtPieces_ArtPieceId] FOREIGN KEY([ArtPieceId])
REFERENCES [dbo].[ArtPieces] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventArtPieces] CHECK CONSTRAINT [FK_EventArtPieces_ArtPieces_ArtPieceId]
GO
ALTER TABLE [dbo].[EventArtPieces]  WITH CHECK ADD  CONSTRAINT [FK_EventArtPieces_Events_EventId] FOREIGN KEY([EventId])
REFERENCES [dbo].[Events] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventArtPieces] CHECK CONSTRAINT [FK_EventArtPieces_Events_EventId]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_AspNetUsers_OrganizerId] FOREIGN KEY([OrganizerId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_AspNetUsers_OrganizerId]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Resources_ResourceId] FOREIGN KEY([ResourceId])
REFERENCES [dbo].[Resources] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Resources_ResourceId]
GO
ALTER TABLE [dbo].[Invitations]  WITH CHECK ADD  CONSTRAINT [FK_Invitations_AspNetUsers_InviteeId] FOREIGN KEY([InviteeId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Invitations] CHECK CONSTRAINT [FK_Invitations_AspNetUsers_InviteeId]
GO
ALTER TABLE [dbo].[Invitations]  WITH CHECK ADD  CONSTRAINT [FK_Invitations_Events_EventId] FOREIGN KEY([EventId])
REFERENCES [dbo].[Events] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Invitations] CHECK CONSTRAINT [FK_Invitations_Events_EventId]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Events_EventId] FOREIGN KEY([EventId])
REFERENCES [dbo].[Events] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Events_EventId]
GO
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Resources_ResourceId] FOREIGN KEY([ResourceId])
REFERENCES [dbo].[Resources] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Resources_ResourceId]
GO
USE [master]
GO
ALTER DATABASE [ArtClubDb] SET  READ_WRITE 
GO
