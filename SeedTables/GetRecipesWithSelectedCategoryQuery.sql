SELECT table1.RecipeID, table1.Name
FROM [RecipeBook].[dbo].Recipes AS table1, [RecipeBook].[dbo].Categories AS table2
WHERE table1.Category = table2.CatID
AND table2.CatID = 9
