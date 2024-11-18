create database claimdb; 
use claimdb; 



CREATE TABLE Lecturers (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    ContactInfo VARCHAR(255) NOT NULL,
    DateOfHire DATE NOT NULL,
    username varchar(255),
password varchar(255)
);

alter table Lecturers add column Email varchar(255); 
alter table Lecturers add column Phone varchar(255); 

alter table Lecturers add column Department  varchar(255); 
alter table Lecturers drop column ContactInfo; 

create table AcademicManger(
MangerId int auto_increment primary key,
name varchar(255),
username varchar(255),
password varchar(255)
);

create table Coordinators(
CoordinatorsId int auto_increment primary key,
name varchar(255),
username varchar(255),
password varchar(255)
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

ALTER TABLE Lecturers CHANGE COLUMN Id LecturerID INT ;

ALTER TABLE Lecturers ADD Role VARCHAR(255) DEFAULT 'Lecturer';
ALTER TABLE AcademicManger ADD Role VARCHAR(255) DEFAULT 'Academic Manager';
ALTER TABLE Coordinators ADD Role VARCHAR(255) DEFAULT 'Programme Coordinator';

create table INVOICE(
InvoiceID	INT	Primary Key,
LecturerID	INT AUTO_INCREMENT,
GeneratedDate	DATETIME,
TotalAmount	DECIMAL	,
Status VARCHAR(255) NOT NULL,
FOREIGN KEY (LecturerID	) REFERENCES lecturer(LecturerID)

);

create table Reports(
ReportID int auto_increment primary key, 
ReportDate datetime,
TotalClaimsAmount decimal,
Summary varchar(500)
);

