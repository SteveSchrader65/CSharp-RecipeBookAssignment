SELECT table1.Name AS 'Recipe',
	   table2.CatName AS 'Category',
	   table3.ProdName AS 'Ingredient',
	   table4.Quantity,
	   table5.Unit AS 'Measure'
FROM [RecipeBook].[dbo].Ingredients as table4 INNER JOIN
[RecipeBook].[dbo].Recipes as table1 ON table4.RecipeID = table1.RecipeID INNER JOIN 
[RecipeBook].[dbo].Categories as table2 ON table2.CatID = table1.Category INNER JOIN
[RecipeBook].[dbo].Products as table3 ON table3.ProdID = table4.ProdID INNER JOIN 
[RecipeBook].[dbo].Measures as table5 ON table5.MeasureID = table4.Measure
where table1.RecipeID = 1

/*
FROM [RecipeBook].[dbo].Recipes as table1, [RecipeBook].[dbo].Categories as table2, [RecipeBook].[dbo].Products as table3, [RecipeBook].[dbo].Ingredients as table4,
[RecipeBook].[dbo].Measures as table5
WHERE table1.RecipeID = table4.RecipeID
AND table1.Category = table2.CatID
AND table3.ProdID = table4.ProdID
AND table4.Unit = table5.MeasureID
*/