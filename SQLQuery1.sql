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


-- User 1
INSERT INTO Users (Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc)
VALUES ('Maria Clara', 'maria@example.com', 'Female', 22, 'Quezon City', 'maria_clara', 'password123', GETUTCDATE());

-- User 2
INSERT INTO Users (Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc)
VALUES ('Jose Rizal', 'jose@example.com', 'Male', 35, 'Calamba, Laguna', 'jose_rizal', 'hero123', GETUTCDATE());

-- User 3
INSERT INTO Users (Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc)
VALUES ('Andres Bonifacio', 'andres@example.com', 'Male', 30, 'Tondo, Manila', 'andres_b', 'katipunan123', GETUTCDATE());

-- User 4
INSERT INTO Users (Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc)
VALUES ('Gabriela Silang', 'gabriela@example.com', 'Female', 28, 'Ilocos Sur', 'gabriela_s', 'ilocana123', GETUTCDATE());

-- User 5
INSERT INTO Users (Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc)
VALUES ('Apolinario Mabini', 'apolinario@example.com', 'Male', 40, 'Tanauan, Batangas', 'mabini', 'sublime123', GETUTCDATE());

-- User 6
INSERT INTO Users (Name, Email, Gender, Age, Address, Username, Password, CreatedAtUtc)
VALUES ('Melchora Aquino', 'melchora@example.com', 'Female', 50, 'Balintawak', 'melchora_a', 'ina_ng_bayan', GETUTCDATE());


SELECT * FROM Users;



USE RODRIGUEZ_MESSAGEBOXEDB;
GO


-- UPDATE USER 1: Maria Clara - Change password and move to Makati

UPDATE Users 
SET Password = 'new_secure_password', Address = 'Makati City' 
WHERE Username = 'maria_clara';




-- UPDATE USER 2: Jose Rizal - Change age, address, and password

UPDATE Users 
SET Age = 36, Address = 'Dapitan, Zamboanga', Password = 'rizal_new_pass' 
WHERE Username = 'jose_rizal';




-- UPDATE USER 3: Andres Bonifacio - Update email and gender

UPDATE Users 
SET Email = 'bonifacio_new@example.com', Gender = 'Male' 
WHERE Username = 'andres_b';




-- UPDATE USER 4: Gabriela Silang - Change her full Name and Password

UPDATE Users 
SET Name = 'Gabriela Silang-Diego', Password = 'gab_new_pass' 
WHERE Username = 'gabriela_s';
-- PREDICTION: (1 row affected)



-- UPDATE USER 5: Apolinario Mabini - Update Address only

UPDATE Users 
SET Address = 'Nagcarlan, Laguna' 
WHERE Username = 'mabini';




-- UPDATE USER 6: Melchora Aquino - Update Age only

UPDATE Users 
SET Age = 51 
WHERE Username = 'melchora_a';



SELECT * FROM Users WHERE Username = 'maria_clara';


DELETE FROM Users 
WHERE Username = 'maria_clara';


SELECT * FROM Users;

