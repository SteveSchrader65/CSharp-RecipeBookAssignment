SELECT DISTINCT table1.RecipeID, table1.Name 
FROM [RecipeBook].[dbo].Recipes AS table1, [RecipeBook].[dbo].Products AS table2, [RecipeBook].[dbo].Ingredients AS table3  
WHERE table1.RecipeID = table3.RecipeID
AND table2.ProdID = table3.ProdID 
AND table2.ProdID = 45
