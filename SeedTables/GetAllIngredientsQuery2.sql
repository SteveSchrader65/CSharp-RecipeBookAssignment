SELECT table1.ProdName AS 'Ingredient',
	   table2.Quantity,
	   table3.MeasureAbbrev AS 'Unit'
FROM [RecipeBook].[dbo].Ingredients as table2 INNER JOIN
[RecipeBook].[dbo].Recipes as table4 ON table2.RecipeID = table4.RecipeID INNER JOIN 
[RecipeBook].[dbo].Products as table1 ON table1.ProdID = table2.ProdID INNER JOIN 
[RecipeBook].[dbo].Measures as table3 ON table3.MeasureID = table2.Measure

where table4.RecipeID = 1 /* current recipe */
