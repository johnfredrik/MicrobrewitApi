-- Inserts test values
SET IDENTITY_INSERT Origins ON
INSERT INTO [dbo].[Origins] ([OriginId], [Name]) VALUES (1, N'United States')
INSERT INTO [dbo].[Origins] ([OriginId], [Name]) VALUES (2, N'United Kingdom')
INSERT INTO [dbo].[Origins] ([OriginId], [Name]) VALUES (3, N'Belgium')
INSERT INTO [dbo].[Origins] ([OriginId], [Name]) VALUES (4, N'Germany')
INSERT INTO [dbo].[Origins] ([OriginId], [Name]) VALUES (5, N'Norway')
SET IDENTITY_INSERT Origins OFF

SET IDENTITY_INSERT Suppliers ON
INSERT INTO [dbo].[Suppliers] ([SupplierId], [Name], [OriginId]) VALUES (1, N'Boortmalt', 3)
INSERT INTO [dbo].[Suppliers] ([SupplierId], [Name], [OriginId]) VALUES (2, N'White Labs',1)
INSERT INTO [dbo].[Suppliers] ([SupplierId], [Name], [OriginId]) VALUES (3, N'Fermentis',1)
INSERT INTO [dbo].[Suppliers] ([SupplierId], [Name], [OriginId]) VALUES (4, N'Thomas Fawcett',2)
INSERT INTO [dbo].[Suppliers] ([SupplierId], [Name], [OriginId]) VALUES (5, N'De Wolf-Cosyns',3)
INSERT INTO [dbo].[Suppliers] ([SupplierId], [Name], [OriginId]) VALUES (6, N'Bestmaltz',4)
INSERT INTO [dbo].[Suppliers] ([SupplierId], [Name], [OriginId]) VALUES (7, N'Delete this Bitch',4)
INSERT INTO [dbo].[Suppliers] ([SupplierId], [Name], [OriginId]) VALUES (8, N'Delete this Bitch2',4)
SET IDENTITY_INSERT Suppliers OFF

-- Beer styles
SET IDENTITY_INSERT BeerStyles ON
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (1, N'Ale',null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (2, N'Golden Ale', 1)
SET IDENTITY_INSERT BeerStyles OFF

-- Breweries
SET IDENTITY_INSERT Breweries ON
INSERT INTO [dbo].[Breweries] ([BreweryId],[Name],[Description],[Type]) VALUES (1, N'Hummlepung bryggeri', N'Best kitchen brewed beer around',N'Home Brewery')
INSERT INTO [dbo].[Breweries] ([BreweryId],[Name],[Description],[Type]) VALUES (2, N'Ægir Bryggeri ', N'Flåmsbrygga er eit tun ved Aurlandsfjorden som omfattar Flåmsbrygga Hotell med konferansefasillitetar, Ægir Bryggeri & Pub, Flåmstova Restaurant og Furukroa Kafé.', N'Microbrewery')
INSERT INTO [dbo].[Breweries] ([BreweryId],[Name],[Description],[Type]) VALUES (3, N'Gromle bryggeri', N'Best kitchen brewed beer around',N'Home Brewery')
INSERT INTO [dbo].[Breweries] ([BreweryId],[Name],[Description],[Type]) VALUES (4, N'Fomle bryggeri', N'Best kitchen brewed beer around',N'Home Brewery')
INSERT INTO [dbo].[Breweries] ([BreweryId],[Name],[Description],[Type]) VALUES (5, N'Best bryggeri', N'Best kitchen brewed beer around',N'Home Brewery')
SET IDENTITY_INSERT Breweries OFF

-- Brewery Members
INSERT INTO [dbo].[Users] ([Username],[Email],[Settings]) VALUES (N'johnfredrik',N'john-f@online.no',N'{something}')
INSERT INTO [dbo].[Users] ([Username],[Email],[Settings]) VALUES (N'torstein',N'torstein@gmail.com',N'{something}')
INSERT INTO [dbo].[Users] ([Username],[Email],[Settings]) VALUES (N'thedude','thedude@dude.no',N'{something}')

INSERT INTO [dbo].[BreweryMembers] ([BreweryId],[MemberUsername],[Role]) VALUES (1,N'torstein',N'Bossman')
INSERT INTO [dbo].[BreweryMembers] ([BreweryId],[MemberUsername],[Role]) VALUES (1,N'thedude',N'Slave')
INSERT INTO [dbo].[BreweryMembers] ([BreweryId],[MemberUsername],[Role]) VALUES (2,N'torstein',N'Bossman')
INSERT INTO [dbo].[BreweryMembers] ([BreweryId],[MemberUsername],[Role]) VALUES (2,N'thedude',N'Slave')
INSERT INTO [dbo].[BreweryMembers] ([BreweryId],[MemberUsername],[Role]) VALUES (4,N'torstein',N'Bossman')

SET IDENTITY_INSERT UserCredentials ON
INSERT INTO [dbo].[UserCredentials] ([UserCredentialsId],[Password],[SharedSecret],[Username]) VALUES (1,N'EAAAAA2i7rB183t/vrZ62ahBVELmFmmO9B5Fzz4xz9F57tya',N'test',N'johnfredrik')
INSERT INTO [dbo].[UserCredentials] ([UserCredentialsId],[Password],[SharedSecret],[Username]) VALUES (2,N'EAAAAA2i7rB183t/vrZ62ahBVELmFmmO9B5Fzz4xz9F57tya',N'test',N'torstein')
SET IDENTITY_INSERT UserCredentials OFF

SET IDENTITY_INSERT Hops ON
INSERT INTO [dbo].[Hops] ([HopId], [Name], [AAlow], [AAHigh],[BetaLow],[BetaHigh],[Notes],[FlavourDescription],[OriginId]) VALUES (1, N'Challanger', 6, 8, 4, 4.5, '', '',2)
INSERT INTO [dbo].[Hops] ([HopId], [Name], [AAlow], [AAHigh],[BetaLow],[BetaHigh],[Notes],[FlavourDescription],[OriginId]) VALUES (2, N'Ahtanum', 5.7, 6.3, 4, 6.3, 'Ahtanum is an aroma-type cultivar bred by yakima chief ranches. its name is derived from the area near yakima where the first hop farm was established in 1869 by charles carpenter.', 'Distinctive aromatic hops with moderate bittering power from washington.',1)
INSERT INTO [dbo].[Hops] ([HopId], [Name], [AAlow], [AAHigh],[BetaLow],[BetaHigh],[Notes],[FlavourDescription],[OriginId]) VALUES (3, N'Amarillo', 8, 9, 6, 7, 'Amarillo is an aroma-type cultivar of recent origin, discovered and introduced by virgil gamache farms inc.', '',1)
INSERT INTO [dbo].[Hops] ([HopId], [Name], [AAlow], [AAHigh],[BetaLow],[BetaHigh],[Notes],[FlavourDescription],[OriginId]) VALUES (4, N'Centennial', 8, 11, 3.5, 4.5, 'Centennial is an aroma-type cultivar, bred in 1974 and released in 1990. the genetic composition is 3/4 brewers gold, 3/32 fuggle, 1/16 east kent golding, 1/32 bavarian and 1/16 unknown.', '',1)
INSERT INTO [dbo].[Hops] ([HopId], [Name], [AAlow], [AAHigh],[BetaLow],[BetaHigh],[Notes],[FlavourDescription],[OriginId]) VALUES (5, N'Target', 10, 12, 0, 0, '', '',2)
INSERT INTO [dbo].[Hops] ([HopId], [Name], [AAlow], [AAHigh],[BetaLow],[BetaHigh],[Notes],[FlavourDescription],[OriginId]) VALUES (6, N'Admiral', 9, 15, 0, 0, '', '',2)
INSERT INTO [dbo].[Hops] ([HopId], [Name], [AAlow], [AAHigh],[BetaLow],[BetaHigh],[Notes],[FlavourDescription],[OriginId]) VALUES (7, N'East Kent Goldings', 4, 7, 0, 0, '', '',2)
SET IDENTITY_INSERT Hops OFF

--INSERT INTO [dbo].[Fermentables] ([Name],[EBC],[Lovibond],[PPG],[SupplierId],[Type]) VALUES ()
SET IDENTITY_INSERT Fermentables ON
INSERT INTO [dbo].[Fermentables] ([FermentableId],[Name],[EBC],[Lovibond],[PPG],[SupplierId],[Type]) VALUES ( 1,'Pale Malt',0,6,34,null,'Grain')
INSERT INTO [dbo].[Fermentables] ([FermentableId],[Name],[EBC],[Lovibond],[PPG],[SupplierId],[Type]) VALUES ( 2,'Pale Ale Malt',0,2,37,null,'Grain')
INSERT INTO [dbo].[Fermentables] ([FermentableId],[Name],[EBC],[Lovibond],[PPG],[SupplierId],[Type]) VALUES ( 3,'Plain Light DME',0,4,43,null,'DryExtract')
INSERT INTO [dbo].[Fermentables] ([FermentableId],[Name],[EBC],[Lovibond],[PPG],[SupplierId],[Type]) VALUES ( 4,'Plain Light DME',0,4,43,null,'LiquidExtract')
INSERT INTO [dbo].[Fermentables] ([FermentableId],[Name],[EBC],[Lovibond],[PPG],[SupplierId],[Type]) VALUES ( 5,'Amber Malt',0,20,34,1,'Grain')
INSERT INTO [dbo].[Fermentables] ([FermentableId],[Name],[EBC],[Lovibond],[PPG],[SupplierId],[Type]) VALUES ( 6,'Maris Otter Pale Malt',0,7,36,4,'Grain')
INSERT INTO [dbo].[Fermentables] ([FermentableId],[Name],[EBC],[Lovibond],[PPG],[SupplierId],[Type]) VALUES ( 7,'Pale Crystal',0,75,34,4,'Grain')
INSERT INTO [dbo].[Fermentables] ([FermentableId],[Name],[EBC],[Lovibond],[PPG],[SupplierId],[Type]) VALUES ( 8,'Chocolate Malt',0,1175,30,4,'Grain')
INSERT INTO [dbo].[Fermentables] ([FermentableId],[Name],[EBC],[Lovibond],[PPG],[SupplierId],[Type]) VALUES ( 9,'Biscute Malt',0,23,35,5,'Grain')
SET IDENTITY_INSERT Fermentables OFF

-- Gets Added in the test
SET IDENTITY_INSERT Other ON
INSERT INTO [dbo].[Other] ([OtherId],[Name],[Type]) VALUES (1,'Strawberry','Fruit')
INSERT INTO [dbo].[Other] ([OtherId],[Name],[Type]) VALUES (2,'Honey','NoneFermentableSugar')
INSERT INTO [dbo].[Other] ([OtherId],[Name],[Type]) VALUES (3,'Koriander','Spice')
INSERT INTO [dbo].[Other] ([OtherId],[Name],[Type]) VALUES (4,'For Deleting','Spice')
INSERT INTO [dbo].[Other] ([OtherId],[Name],[Type]) VALUES (5,'For Deleting','Fruit')
SET IDENTITY_INSERT Other OFF

SET IDENTITY_INSERT Yeasts ON
INSERT INTO [dbo].[Yeasts] ([YeastId],
							[Name],
							[TemperatureHigh],
							[TemperatureLow],
							[Flocculation],
							[AlcoholTolerance],
							[ProductCode],
							[Comment],
							[Type],
							[SupplierId]) 
					VALUES (
							1,
							'California Ale Yeast',
							21,
							24,
							null,
							null,
							'WLP001',
							'This yeast is famous for its clean flavors, balance and ability to be used in almost any style ale. It accentuates the hop flavors and is extremely versatile',
							'LiquidYeast',
							2)

INSERT INTO [dbo].[Yeasts] ([YeastId],
							[Name],
							[TemperatureHigh],
							[TemperatureLow],
							[Flocculation],
							[AlcoholTolerance],
							[ProductCode],
							[Comment],
							[Type],
							[SupplierId]) 
					VALUES (2,
							'Safale US 05',
							null,
							null,
							null,
							null,
							null,
							'Ready-to-pitch American ale yeast for well balanced beers with low diacetyl and a very crisp end palate.',
							'Dry Yeast',
							3)
INSERT INTO [dbo].[Yeasts] ([YeastId],
							[Name],
							[TemperatureHigh],
							[TemperatureLow],
							[Flocculation],
							[AlcoholTolerance],
							[ProductCode],
							[Comment],
							[Type],
							[SupplierId]) 
					VALUES (3,
							'English Ale Yeast',
							18.3,
							20,
							'Medium',
							'Very High',
							'WLP008',
							'A classic ESB strain from one of England''s largest independent breweries. This yeast is best suited for English style ales including milds, bitters, porters, and English style stouts. This yeast will leave a beer very clear, and will leave some residual sweetness.',
							'LiquidYeast',
							2)
SET IDENTITY_INSERT Yeasts OFF