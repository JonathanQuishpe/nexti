CREATE TABLE Events (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Date DATE,
    Location VARCHAR(100),
    Description VARCHAR(100),
    Price DECIMAL(10,2)
);