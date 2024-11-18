use claimdb; 

-- Inserting data into the Lecturers table
INSERT INTO Lecturers (LecturerID,Name,  DateOfHire, username, password,Email,Phone,Department)
VALUES
(4,'John Doe', '2020-08-15', 'johndoe', 'password123', 'john.doe@example.com','089564785','Science'),
(5,'Jane Smith', '2021-01-10', 'janesmith', 'password456', 'jane.smith@example.com','0781239847','Law'),
(6,'Mark Taylor', '2019-07-22', 'marktaylor', 'password789', 'mark.taylor@example.com','0948521456','Education');

-- Inserting data into the AcademicManager table
INSERT INTO AcademicManger (name, username, password)
VALUES
('Alice Brown', 'alicebrown', 'managerpass123'),
('Bob White', 'bobwhite', 'managerpass456');

-- Inserting data into the Coordinators table
INSERT INTO Coordinators (name, username, password)
VALUES
('Carol Green', 'carolgreen', 'coordpass123'),
('David Black', 'davidblack', 'coordpass456');




 