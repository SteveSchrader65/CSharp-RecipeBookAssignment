USE [master]
GO
/****** Object:  Database [RecipeBook]    Script Date: 25/05/2021 4:39:20 AM ******/
CREATE DATABASE [RecipeBook]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RecipeBook', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER02\MSSQL\DATA\RecipeBook.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RecipeBook_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER02\MSSQL\DATA\RecipeBook_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [RecipeBook] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RecipeBook].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RecipeBook] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RecipeBook] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RecipeBook] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RecipeBook] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RecipeBook] SET ARITHABORT OFF 
GO
ALTER DATABASE [RecipeBook] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RecipeBook] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RecipeBook] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RecipeBook] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RecipeBook] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RecipeBook] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RecipeBook] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RecipeBook] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RecipeBook] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RecipeBook] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RecipeBook] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RecipeBook] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RecipeBook] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RecipeBook] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RecipeBook] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RecipeBook] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RecipeBook] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RecipeBook] SET RECOVERY FULL 
GO
ALTER DATABASE [RecipeBook] SET  MULTI_USER 
GO
ALTER DATABASE [RecipeBook] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RecipeBook] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RecipeBook] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RecipeBook] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RecipeBook] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'RecipeBook', N'ON'
GO
ALTER DATABASE [RecipeBook] SET QUERY_STORE = OFF
GO
USE [RecipeBook]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 25/05/2021 4:39:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CatID] [int] IDENTITY(1,1) NOT NULL,
	[CatName] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[CatID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ingredients]    Script Date: 25/05/2021 4:39:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingredients](
	[RecipeID] [smallint] NULL,
	[ProdID] [smallint] NULL,
	[Measure] [int] NULL,
	[Quantity] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Measures]    Script Date: 25/05/2021 4:39:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Measures](
	[MeasureID] [int] IDENTITY(1,1) NOT NULL,
	[Unit] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[MeasureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 25/05/2021 4:39:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProdID] [int] IDENTITY(1,1) NOT NULL,
	[ProdName] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProdID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recipes]    Script Date: 25/05/2021 4:39:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recipes](
	[RecipeID] [int] NULL,
	[Name] [varchar](30) NULL,
	[Method] [text] NULL,
	[CookTime] [float] NULL,
	[CookTemp] [int] NULL,
	[Category] [smallint] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (1, N'Meat')
INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (2, N'Vegetarian')
INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (3, N'Poultry')
INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (4, N'Cakes')
INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (5, N'Dessert')
INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (6, N'Seafood')
INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (7, N'Pasta')
INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (8, N'Salad')
INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (9, N'Biscuits')
INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (10, N'Eggs')
INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (11, N'Drinks')
INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (12, N'Pies')
INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (13, N'Savouries')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (1, 12, 9, 125)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (1, 9, 3, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (1, 23, 3, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (1, 41, 1, 1.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (1, 40, 2, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (1, 51, 3, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (1, 43, 3, 0.75)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (1, 28, 2, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (2, 12, 9, 125)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (2, 45, 7, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (2, 11, 3, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (2, 26, 3, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (2, 43, 3, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (2, 17, 3, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (3, 10, 3, 0.75)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (3, 42, 2, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (3, 26, 3, 1.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (3, 40, 2, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (3, 17, 3, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (3, 43, 3, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (3, 12, 4, 6)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (4, 12, 2, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (4, 11, 3, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (4, 42, 2, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (4, 26, 3, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (4, 45, 7, 3)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (4, 28, 3, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (5, 26, 3, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (5, 1, 1, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (5, 47, 1, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (5, 9, 3, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (5, 8, 1, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (5, 12, 9, 60)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (5, 45, 7, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (5, 13, 3, 0.75)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (5, 46, 2, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (5, 10, 2, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (6, 48, 6, 3)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (6, 12, 9, 100)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (6, 8, 10, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (6, 26, 3, 1.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (6, 9, 3, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (6, 47, 10, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (6, 10, 2, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (6, 47, 1, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (7, 45, 7, 3)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (7, 13, 3, 3)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (7, 12, 9, 50)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (7, 9, 2, 3)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (7, 43, 3, 0.25)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (7, 49, 10, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (8, 44, 7, 6)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (8, 46, 10, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (8, 10, 3, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (8, 45, 7, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (8, 11, 2, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (8, 42, 1, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (8, 13, 3, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (9, 12, 9, 120)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (9, 13, 3, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (9, 2, 8, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (9, 50, 9, 440)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (9, 23, 9, 60)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (9, 1, 1, 0.25)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (9, 18, 9, 440)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (9, 17, 9, 60)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (10, 26, 2, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (10, 31, 3, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (10, 13, 3, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (10, 37, 7, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (10, 18, 9, 440)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (10, 45, 7, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (10, 1, 8, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (11, 22, 6, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (11, 29, 1, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (11, 28, 3, 1.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (11, 1, 1, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (11, 52, 2, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (11, 23, 2, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (12, 45, 7, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (12, 12, 1, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (12, 1, 8, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (12, 2, 8, 1)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (12, 13, 2, 4)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (13, 53, 5, 0.5)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (13, 54, 7, 2)
INSERT [dbo].[Ingredients] ([RecipeID], [ProdID], [Measure], [Quantity]) VALUES (13, 45, 7, 4)
GO
SET IDENTITY_INSERT [dbo].[Measures] ON 

INSERT [dbo].[Measures] ([MeasureID], [Unit]) VALUES (1, N'tsp')
INSERT [dbo].[Measures] ([MeasureID], [Unit]) VALUES (2, N'tbsp')
INSERT [dbo].[Measures] ([MeasureID], [Unit]) VALUES (3, N'cup')
INSERT [dbo].[Measures] ([MeasureID], [Unit]) VALUES (4, N'oz')
INSERT [dbo].[Measures] ([MeasureID], [Unit]) VALUES (5, N'lb')
INSERT [dbo].[Measures] ([MeasureID], [Unit]) VALUES (6, N'kg')
INSERT [dbo].[Measures] ([MeasureID], [Unit]) VALUES (7, N'pc')
INSERT [dbo].[Measures] ([MeasureID], [Unit]) VALUES (8, N'pinch')
INSERT [dbo].[Measures] ([MeasureID], [Unit]) VALUES (9, N'gm')
INSERT [dbo].[Measures] ([MeasureID], [Unit]) VALUES (10, N'sprinkle')
SET IDENTITY_INSERT [dbo].[Measures] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (1, N'Salt')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (2, N'Black Pepper')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (3, N'Rosemary')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (4, N'Sage')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (5, N'Thyme')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (6, N'Oregano')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (7, N'Bay Leaves')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (8, N'Nutmeg')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (9, N'Brown Sugar')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (10, N'White Sugar')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (11, N'Castor Sugar')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (12, N'Butter')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (13, N'Milk')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (14, N'Cream')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (15, N'Buttermilk')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (16, N'Rice Bubbles')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (17, N'Cornflakes')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (18, N'Tinned Salmon')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (19, N'Fresh Fish')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (20, N'Fresh Chicken')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (21, N'Turkey')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (22, N'Chuck Steak')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (23, N'Plain Flour')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (24, N'Wholemeal Flour')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (25, N'Cornflour')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (26, N'Self-Raising Flour')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (27, N'Breadcrumbs')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (28, N'Water')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (29, N'Mustard')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (30, N'Olive Oil')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (31, N'Cooking Oil')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (32, N'Potatoes')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (33, N'Carrots')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (34, N'Pumpkin')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (35, N'Beets')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (36, N'Peas')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (37, N'Onions')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (38, N'Truffles')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (39, N'Spaghetti')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (40, N'Syrup')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (41, N'Bi-Carb Soda')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (42, N'Cocoa')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (43, N'Dried Coconut')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (44, N'Bananas')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (45, N'Eggs')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (46, N'Lemon Juice')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (47, N'Cinnamon')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (48, N'Apples')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (49, N'Vanilla Essence')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (50, N'Pineapple Chunks')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (51, N'Rolled Oats')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (52, N'Tomato Sauce')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (53, N'Bacon')
INSERT [dbo].[Products] ([ProdID], [ProdName]) VALUES (54, N'Tomatoes')
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (1, N'Anzac Cookies', N'Melt butter and golden syrup together. Boil water and mix in bi-carbonate soda. Add to butter mixture and stir. Place all dry ingredients into a large bowl and then stir through the butter and syrup mixture. Measure teaspoonfuls of biscuit dough and place onto a greased baking tray. Leave room between them. Allow to cool when done, then place on wire racks.', 20, 150, 9)
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (2, N'Cornflake Biscuits', N'Cream butter and sugar, then mix through egg. Stir in cornflakes, coconut and flour. Spoon onto biscuit tray and place on greased baking tray. Allow to cool when done, then place on wire racks.', 15, 180, 9)
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (3, N'Chocolate Slice', N'Mix sugar, cornflakes, cocoa, coconut and flour together. Melt butter into syrup and pour over dry ingredient mix. Press into a medium-depth baking tray. When cooked, slice while still warm.', 30, 180, 9)
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (4, N'Chocolate Sponge Cake', N'Separate eggs. Beat egg-whites until stiff, then mix in sugar. Next, gradually add egg yolks one at a time while mixing slowly. Sift flour two or three times, and fold into egg/sugar mixture. Boil water, then dissolve butter and cocoa into it. Stir into cake mix. Place mixture into cake tins and bake.', 20, 180, 4)
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (5, N'Tea Cake', N'Mix flour, salt, brown sugar, cinnamon and nutmeg together in a bowl. Melt butter and mix with beaten egg, milk and lemon juice. Stir into dry ingredients and beat until smooth. Pour mixture into a greaded and floured loaf tin. Mix white sugar and extra 1/2 teaspoon cinnamon and sprinkle over top of cake mixture. Bake until golden brown. Serve with butter.', 35, 180, 4)
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (6, N'Apple Crumble', N'Cover apples with water and stew for 30 minutes. Rub butter into flour and sugar until mixture has texture of breadcrumbs. Add nutmeg and cinnamon and mix through. Remove apples from water and mash with potato-masher. Mix apples through dry mixture and spread onto a baking tray. Bake until golden brown. Serve with whipped cream.', 15, 180, 5)
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (7, N'Baked Custard', N'Beat eggs and sugar until fluffy, but not stiff. Stir in milk and a few drops of vanilla essence. Pour mixture into a cassserole dish and place a few small chunks of butter on top. Stand dish in larger pan of water and bake.', 75, 180, 5)
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (8, N'Chocolate Banana Pudding', N'Cream butter and sugar together, then add cocoa, milk and flour and mix through. Slice bananas and layer over bottom of a greased pie dish. Sprinkle with castor sugar and lemon juice. Pour cake mixture over bananas and bake until set.', 30, 180, 5)
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (9, N'Salmon Pineapple', N'Make a white sauce with flour, milk and half of the butter. Drain salmon making sure to remove bones. Add to white sauce and season with salt and pepper. Drain the pineapple chunks. Layer one third of the salmon mixture into a baking dish, followed by a layer of pineapple. Repeat with a second layer of the salmon mixture, then another of pineapple, then remaining salmon mixture. Melt remaining butter in a frying pan and toast cornflakes until well coated. Spread cornflakes over salmon and pineapple mix. Cook until golden brown.', 20, 175, 6)
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (10, N'Salmon Patties', N'Make a batter using flour, egg, salt and milk. Grate onion and mix through. Drain salmon and add. Mix all ingredients and form into patties. Shallow fry in hot oil. Serve with potato chips and/or grilled tomatoes.', 5, 250, 6)
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (11, N'Chuck Steak Casserole', N'Cut rolled beef into cubes approximately 1.5cm. Place in casserole dish. Mix mustard, water, salt, pepper and flour together and pour over cubed meat. Cook until meat is tender.', 120, 180, 1)
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (12, N'Scrambled Eggs', N'Place milk and butter into a saucepan and bring to boil on stove-top. Crack eggs into a bowl and whisk through with salt and pepper to taste. Mix eggs into milk and butter and stir over a low heat until set. Serve with toast, bacon and grilled tomatoes.', 10, 150, 10)
INSERT [dbo].[Recipes] ([RecipeID], [Name], [Method], [CookTime], [CookTemp], [Category]) VALUES (13, N'Eggs a la Steve', N'Cook eggs in a pan. Grill bacon and tomatoes. Serve with a glass of bourbon.', 15, 120, 13)
GO
USE [master]
GO
ALTER DATABASE [RecipeBook] SET  READ_WRITE 
GO
