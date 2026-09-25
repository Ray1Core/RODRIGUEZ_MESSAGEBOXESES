USE RODRIGUEZ_MESSAGEBOXEDB;
GO

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Gender NVARCHAR(50),
    Age INT,
    Address NVARCHAR(200),
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    CreatedAtUtc DATETIME NOT NULL
);
GO


INSERT INTO Users (Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc)
VALUES ('Maria Clara', 'maria@example.com', 'Female', 22, 'Quezon City', 'maria_clara', 'password123', GETUTCDATE());


SELECT * FROM Users;



UPDATE Users 
SET Password = 'new_secure_password', Address = 'Makati City' 
WHERE Username = 'maria_clara';


SELECT * FROM Users WHERE Username = 'maria_clara';


DELETE FROM Users 
WHERE Username = 'maria_clara';


SELECT * FROM Users;

