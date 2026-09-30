CREATE OR ALTER PROCEDURE dbo.ClearTravelTestData
AS
BEGIN
    SET NOCOUNT ON;
    DELETE dbo.AttractionCategories;
    DELETE dbo.Review;
    DELETE dbo.Attraction;
    DELETE dbo.City;
    DELETE dbo.Category;
    DELETE dbo.[User];
    DELETE dbo.Country;
END