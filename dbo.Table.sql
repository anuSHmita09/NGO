CREATE TABLE Donors (
    DonorID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    BloodGroup NVARCHAR(5) NOT NULL,
    Contact NVARCHAR(15) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    LastDonationDate DATE NULL
);
