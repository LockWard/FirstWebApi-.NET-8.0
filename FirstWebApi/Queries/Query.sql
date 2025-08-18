DECLARE @i INT = 1;
DECLARE @rowsToInsert INT = 500; -- Change this to the number of rows you want to insert

WHILE @i <= @rowsToInsert
BEGIN
    INSERT INTO dbo.Products (Name, Price, Stock)
    VALUES (
        'Product ' + CONVERT(NVARCHAR(50), @i), -- Simple way to make unique names
        CAST(RAND() * 1000 AS DECIMAL(10, 2)), -- Generates a price between 0.00 and 1000.00
        ABS(CHECKSUM(NEWID())) % 1000 -- Generates a stock value between 0 and 999
    );
    SET @i = @i + 1;
END;

select top 5 * from dbo.Products order by 1 asc;

-- Different methods

;WITH RandomNames (Name) AS
(
    SELECT 'Gizmo' UNION ALL
    SELECT 'Widget' UNION ALL
    SELECT 'Gadget' UNION ALL
    SELECT 'Thingamajig' UNION ALL
    SELECT 'Doodad' UNION ALL
    SELECT 'Contraption' UNION ALL
    SELECT 'Appliance' UNION ALL
    SELECT 'Device' UNION ALL
    SELECT 'Tool' UNION ALL
    SELECT 'Accessory'
)
INSERT INTO Products (Name, Price, Stock)
SELECT
    N.Name + ' ' + CONVERT(NVARCHAR(10), T.n), -- Combines a random name with a number
    CAST(ABS(CHECKSUM(NEWID())) % 10000 AS DECIMAL(10, 2)) / 100, -- Price between 0 and 100
    ABS(CHECKSUM(NEWID())) % 500 -- Stock between 0 and 499
FROM (
    SELECT TOP 1000 -- The number of rows you want to insert
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
    FROM sys.objects A, sys.objects B, sys.objects C
) AS T
JOIN RandomNames AS N ON N.Name = (
    SELECT TOP 1 Name FROM RandomNames ORDER BY NEWID()
);