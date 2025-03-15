CREATE TABLE Donors (
    DonorID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    BloodGroup NVARCHAR(5) NOT NULL,
    Contact NVARCHAR(15) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    LastDonationDate DATE NULL
);


INSERT INTO Donors (Name, Age, BloodGroup, Contact, City, LastDonationDate) 
VALUES 
('Anushmita Sen', 22, 'O+', '9876543210', 'Siliguri', '2024-02-10'),
('Rahul Das', 25, 'A-', '9876543211', 'Bangalore', '2023-11-15'),
('Meera Iyer', 30, 'B+', '9876543212', 'Chennai', '2024-01-05'),
('Vikram Singh', 27, 'AB-', '9876543213', 'Delhi', NULL);

SELECT * FROM DONORS;