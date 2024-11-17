create database claimdb; 
use claimdb; 

CREATE TABLE Lecturers (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    ContactInfo VARCHAR(255) NOT NULL,
    DateOfHire DATE NOT NULL
);

CREATE TABLE Claims (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    StatusId INT NOT NULL,
    LecturerName VARCHAR(255) NOT NULL,
    HoursWorked DECIMAL(10, 2) NOT NULL,
    HourlyRate DECIMAL(10, 2) NOT NULL,
    Notes TEXT,
    Status VARCHAR(255) NOT NULL,
    FileName VARCHAR(255),
    DateSubmitted DATETIME NOT NULL,
    RejectionReason TEXT,
    FOREIGN KEY (StatusId) REFERENCES Status(Id) -- If you have a status table
);
