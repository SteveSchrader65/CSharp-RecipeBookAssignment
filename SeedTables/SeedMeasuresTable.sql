DROP TABLE IF EXISTS [RecipeBook].[dbo].Measures

GO
CREATE TABLE [RecipeBook].[dbo].Measures (
MeasureID int identity (1,1),
Unit varchar(20),
PRIMARY KEY(MeasureID)
);

INSERT [RecipeBook].[dbo].[Measures] ([Unit]) VALUES ('tsp')
INSERT [RecipeBook].[dbo].[Measures] ([Unit]) VALUES ('tbsp')
INSERT [RecipeBook].[dbo].[Measures] ([Unit]) VALUES ('cup')
INSERT [RecipeBook].[dbo].[Measures] ([Unit]) VALUES ('oz')
INSERT [RecipeBook].[dbo].[Measures] ([Unit]) VALUES ('lb')
INSERT [RecipeBook].[dbo].[Measures] ([Unit]) VALUES ('kg')
INSERT [RecipeBook].[dbo].[Measures] ([Unit]) VALUES ('pc')
INSERT [RecipeBook].[dbo].[Measures] ([Unit]) VALUES ('pinch')
INSERT [RecipeBook].[dbo].[Measures] ([Unit]) VALUES ('gm')
INSERT [RecipeBook].[dbo].[Measures] ([Unit]) VALUES ('sprinkle')