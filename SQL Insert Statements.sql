use claimdb; 

-- Inserting data into the Lecturers table
INSERT INTO Lecturers (Name, ContactInfo, DateOfHire, username, password)
VALUES
('John Doe', 'john.doe@example.com', '2020-08-15', 'johndoe', 'password123'),
('Jane Smith', 'jane.smith@example.com', '2021-01-10', 'janesmith', 'password456'),
('Mark Taylor', 'mark.taylor@example.com', '2019-07-22', 'marktaylor', 'password789');

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




 