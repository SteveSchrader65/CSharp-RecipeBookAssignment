DROP TABLE IF EXISTS [RecipeBook].[dbo].Categories

CREATE TABLE [RecipeBook].[dbo].Categories (
CatID int identity (1,1),
CatName varchar(20),
PRIMARY KEY(CatID)
);

INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Meat')
INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Vegetarian')
INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Poultry')
INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Cakes')
INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Dessert')
INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Seafood')
INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Pasta')
INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Salad')
INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Biscuits')
INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Eggs')
INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Drinks')
INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Pies')
INSERT [RecipeBook].[dbo].[Categories] ([CatName]) VALUES ('Savouries')