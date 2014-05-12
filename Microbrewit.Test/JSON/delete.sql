-- Deletes Database tables
DELETE ABVs
DELETE Beers
DELETE BeerStyles
DELETE BoilStepFermentables
DELETE BoilStepHops
DELETE BoilStepOthers
DELETE BoilSteps
DELETE Breweries
DELETE BreweryBeers
DELETE BreweryMembers
DELETE Fermentables
DELETE FermentationStepFermentables
DELETE FermentationStepHops
DELETE FermentationStepOthers
DELETE FermentationSteps
DELETE FermentationStepYeasts
DELETE Flavours
DELETE ForkedRecipe
DELETE HopFlavours
DELETE HopForms
DELETE Hops
DELETE IBUs
DELETE MashStepFermentables
DELETE MashStepHops
DELETE MashStepOthers
DELETE MashSteps
DELETE Other
DELETE Recipes
DELETE SRMs
DELETE Substitute
DELETE Yeasts
DELETE Origins
DELETE Suppliers
DELETE UserBeers
DELETE UserCredentials
DELETE Users

-- To reset identity
DBCC CHECKIDENT(Origins, RESEED, 0)
DBCC CHECKIDENT(Suppliers, RESEED, 0)
DBCC CHECKIDENT(Hops, RESEED, 0)
DBCC CHECKIDENT(Other, RESEED,0)
DBCC CHECKIDENT(Fermentables, RESEED,0)
DBCC CHECKIDENT(Yeasts, RESEED,0)
