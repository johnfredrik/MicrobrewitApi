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
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (2, N'Mixed', 2)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (3, N'Speciality Beer',null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (4, N'Golden Ale', 1)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (5, N'Chocolate/Cocoa-Flavored Beer', 3)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (6, N'Pale Ale',1)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (7, N'Stout', null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (8, N'English-Style Pale Ale', 6)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (9, N'Irish-Style Dry Stout', 7)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (10, N'Coffee-Flavored Beer', 3)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (11, N'Wheat Beer', null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (12, N'Dark American Wheat or Lager with Yeast', 11)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (13, N'Golden Lager', null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (14, N'Dortmunder/European Style Export', 13)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (15, N'Barley Wine Ale', null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (16, N'English-Style Barley Wine Ale', 15)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (17, N'Brown Ale', null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (18, N'English-Style Brown Ale', 17)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (19, N'English-Style Dark Mild Ale', 17)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (20, N'Indian Pale Ale', null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (21, N'English-Style Indian Pale Ale', 20)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (22, N'English Summer', 4)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (23, N'English-Style Pale Ale', 6)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (24, N'Lager', null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (25, N'Dark Lager', 24)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (26, N'English-Style Dark/Münchner Dunkel', 25)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (27, N'European Low-Alcohol Lager/German Leicht', 4)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (28, N'Extra Special Bitter or Strong Bitter', 6)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (29, N'Foreign-Style Stout', 7)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (30, N'French Ale', 1)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (31, N'French-Style Bière de Garde', 30)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (32, N'Garden Beer', 3)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (33, N'Hybrid Beer', null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (34, N'German-Style Brown Ale/Düsseldorf-Style Altbier', 33)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (35, N'Bock', null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (36, N'German-Style Doppelbock', 35)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (37, N'German-Style Heller Bock/Maibock', 35)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (38, N'German-Style Kölsch/Köln-Style Kölsch', 33)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (39, N'German-Style Leichtes Weizen Weissbier', 11)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (40, N'Amber Lager', 24)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (41, N'German-Style Märzen', 40)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (42, N'German-Style Oktoberfest/Wiesen', 40)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (43, N'German-Style Pilsener', 13)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (44, N'German-Style Schwarzbier', 25)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (45, N'German-Style Strong Eisbock', 35)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (46, N'Gluten Free Beer', 3)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (47, N'Golden or Blonde Ale', 4)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (48, N'Herb and Spice Beer', 3)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (49, N'Imperial or Double India Pale Ale', 20)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (50, N'Amber Ale', 1)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (51, N'Imperial or Double Red Ale', 50)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (52, N'International-Style Pale Ale', 6)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (53, N'Irish ALe', 1)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (54, N'Irish-Style Red Ale', 53)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (55, N'International-Style Pilsner', 13)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (56, N'Japanese Sake Yeast Beer', 3)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (57, N'Kellerbier (Cellar beer) or Zwickelbier', 33)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (58, N'Sour Ale', 1)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (59, N'Leipzig-Style Gose',58)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (60, N'Light American Ale or Lager with Yeast', 11)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (61, N'Light American Ale or Lager without Yeast', 11)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (62, N'Münchner-Style Helles', 13)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (63, N'Non-Alcoholic Beer', null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (64, N'Non-Alcoholic Malt Beverages', 33)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (65, N'Oatmeal Sout', 7)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (66, N'Old Ale', 50)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (67, N'Ordinary Bitter', 4)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (68, N'Belgian Ale', 33)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (69, N'Pumpkin Beer', 3)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (70, N'Porter', 3)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (71, N'Robust Porter', 70)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (72, N'Scottish Ale', 1)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (73, N'Scottish-Style Export Ale', 72)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (74, N'Scottish-Style Heavy Ale', 72)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (75, N'Scottish-Style Light Ale', 72)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (76, N'Smoke Beer', null)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (77, N'Smoke-Flavored Beer', 76)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (78, N'South German-Style Bernsteinfarbenes Weizen Weissbier', 11)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (79, N'South German-Style Dunkel Weizen Dunkel Weissbier', 11)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (80, N'South German-Style Hefeweizen Hefeweissbier', 11)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (81, N'South German-Style Kristal Weizen Kristal Weissbier', 11)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (82, N'South German-Style Weizenbock Weissbock', 11)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (83, N'Special Bitter or Best Bitter', 50)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (84, N'Specialty Honey Lager or Ale', 3)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (85, N'Specialty Stout', 7)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (86, N'Strong Ale', 50)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (87, N'Strong Scotch Aler', 72)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (88, N'Sweet Stout', 7)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (89, N'Traditional German-Style Bock', 35)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (90, N'Vienna-Style Lager', 40)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (91, N'Wood-Aged Beer', 2)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (92, N'Wood Aged Sour Beer', 91)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (93, N'Wood and Barrel Aged Dark Beer', 91)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (94, N'Wood and Barrel Aged Pale or Amber Beer', 91)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (95, N'Wood and Barrel Aged Strong Beer', 91)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (96, N'Belgian-Style Flanders/Oud Bruin or Oud Red Ales', 58)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (97, N'Rye Beer', 2)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (98, N'German-Style Rye Ale (Roggenbier) with or without Yeast', 97)
INSERT INTO [dbo].BeerStyles (BeerStyleId,[Name],[SuperStyleId]) VALUES (99, N'American Wheat Ale or Lager with Yeast', 11)
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