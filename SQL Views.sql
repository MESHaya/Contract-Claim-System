use claimdb;
CREATE OR REPLACE VIEW Users AS
SELECT 
    Id AS UserId, 
    Name, 
    Username, 
    Password, 
    'Lecturer' AS Role 
FROM Lecturers
UNION ALL
SELECT 
    MangerId AS UserId, 
    Name, 
    Username, 
    Password, 
    'Academic Manager' AS Role 
FROM AcademicManger
UNION ALL
SELECT 
    CoordinatorsId AS UserId, 
    Name, 
    Username, 
    Password, 
    'Programme Coordinator' AS Role 
FROM Coordinators;
