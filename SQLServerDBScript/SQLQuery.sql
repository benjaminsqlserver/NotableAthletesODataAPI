USE [master]
GO
/****** Object:  Database [NotableAthletesDB]    Script Date: 3/9/2023 5:14:03 AM ******/
CREATE DATABASE [NotableAthletesDB]
 
GO

USE [NotableAthletesDB]
GO
/****** Object:  Table [dbo].[Athletes]    Script Date: 3/9/2023 5:14:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Athletes](
	[AthleteID] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[CountryID] [int] NOT NULL,
 CONSTRAINT [PK_Athletes] PRIMARY KEY CLUSTERED 
(
	[AthleteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 3/9/2023 5:14:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Athletes] ON 
GO
INSERT [dbo].[Athletes] ([AthleteID], [FirstName], [LastName], [CountryID]) VALUES (1, N'Tobi', N'Amusan', 1)
GO
INSERT [dbo].[Athletes] ([AthleteID], [FirstName], [LastName], [CountryID]) VALUES (2, N'David', N'Beckham', 3)
GO
INSERT [dbo].[Athletes] ([AthleteID], [FirstName], [LastName], [CountryID]) VALUES (3, N'Roger', N'Milla', 4)
GO
INSERT [dbo].[Athletes] ([AthleteID], [FirstName], [LastName], [CountryID]) VALUES (4, N'Michael', N'Jordan', 2)
GO
INSERT [dbo].[Athletes] ([AthleteID], [FirstName], [LastName], [CountryID]) VALUES (5, N'Christiano', N'Ronaldo', 5)
GO
SET IDENTITY_INSERT [dbo].[Athletes] OFF
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 
GO
INSERT [dbo].[Countries] ([CountryID], [CountryName]) VALUES (1, N'Nigeria')
GO
INSERT [dbo].[Countries] ([CountryID], [CountryName]) VALUES (2, N'USA')
GO
INSERT [dbo].[Countries] ([CountryID], [CountryName]) VALUES (3, N'England')
GO
INSERT [dbo].[Countries] ([CountryID], [CountryName]) VALUES (4, N'Cameroun')
GO
INSERT [dbo].[Countries] ([CountryID], [CountryName]) VALUES (5, N'Portugal')
GO
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
ALTER TABLE [dbo].[Athletes]  WITH CHECK ADD  CONSTRAINT [FK_Athletes_Countries] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Countries] ([CountryID])
GO
ALTER TABLE [dbo].[Athletes] CHECK CONSTRAINT [FK_Athletes_Countries]
GO
USE [master]
GO
ALTER DATABASE [NotableAthletesDB] SET  READ_WRITE 
GO
