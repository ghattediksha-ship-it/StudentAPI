
----------------------## Students ##------------------

-----------------------------------------------1
CREATE PROCEDURE GetAllStudents
AS
BEGIN
    SELECT Id,Name,CGPA,Age,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM Students1
    WHERE IsActive = 1;
END

EXEC GetAllStudents;

update Students1 set Name = 'shruti' where Id = 9

------------------------------------------------2

CREATE PROCEDURE GetStudentById
    @Id INT
AS
BEGIN
    SELECT Id,Name,CGPA,Age,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM Students1
    WHERE Id = @Id
      AND IsActive = 1;
END

EXEC GetStudentById @Id = 1;


-------------------------------------------------3

CREATE PROCEDURE AddStudent
    @Name VARCHAR(50),
    @CGPA FLOAT,
    @Age INT,
    @CreatedBy VARCHAR(100)
AS
BEGIN
    INSERT INTO Students1
    (Name,CGPA,Age,CreatedBy,CreatedOn,IsActive)
    VALUES
    (@Name,@CGPA,@Age,@CreatedBy,GETDATE(),1);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END


--------------------------------------------------4

CREATE PROCEDURE UpdateStudent
    @Id INT,
    @Name VARCHAR(50),
    @CGPA FLOAT,
    @Age INT,
    @ModifiedBy VARCHAR(100)
AS
BEGIN
    UPDATE Students1
    SET
        Name = @Name,CGPA = @CGPA,Age = @Age,ModifiedBy = @ModifiedBy,ModifiedOn = GETDATE()
    WHERE Id = @Id
      AND IsActive = 1;
END


--------------------------------------------------5

CREATE PROCEDURE DeleteStudent
    @Id INT
AS
BEGIN
    UPDATE Students1
    SET
        IsActive = 0,
        ModifiedOn = GETDATE()
    WHERE Id = @Id
      AND IsActive = 1;

    SELECT @@ROWCOUNT AS RowsAffected;
END


EXEC DeleteStudent @Id = 5;




----------------------Department---------------------------------------------------------------------------------------------------

CREATE TABLE Department1(
    DepartmentID INT Primary key identity(1,1),
    DepartmentName varchar(20) not null,
    CreatedBy VARCHAR(100) NULL,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedBy VARCHAR(100) NULL,
    ModifiedOn DATETIME NULL ,
    IsActive INT NOT NULL DEFAULT 1
);

INSERT INTO Department1(DepartmentName)
VALUES
('CSE'),
('AIML');

select * from Department1



-----------------------------------------------1

CREATE PROCEDURE GetAllDepartment
AS
BEGIN
    SELECT DepartmentID,DepartmentName,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM Department1
    WHERE IsActive = 1;
END
Go

EXEC GetAllDepartment;



------------------------------------------------2

CREATE PROCEDURE GetDepartmentById
    @DepartmentID INT
AS
BEGIN
    SELECT DepartmentID, DepartmentName,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM Department1
    WHERE DepartmentID = @DepartmentID
      AND IsActive = 1;
END
GO


EXEC GetDepartmentById @DepartmentID = 1;


-------------------------------------------------3

CREATE PROCEDURE AddDepartment
    @DepartmentName NVARCHAR(100),
    @CreatedBy VARCHAR(100)
AS
BEGIN
    INSERT INTO Department1 (DepartmentName, CreatedBy, CreatedOn, IsActive)
    VALUES (@DepartmentName,@CreatedBy,GETDATE(),1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS DepartmentID;
END
GO

EXEC AddDepartment
    @DepartmentName = 'AIDS ADDED' , @CreatedBy = 'Admin';


--------------------------------------------------4

CREATE PROCEDURE UpdateDepartment
    @DepartmentID INT,
    @DepartmentName NVARCHAR(100),
    @ModifiedBy VARCHAR(100)
AS
BEGIN
    UPDATE Department1
    SET DepartmentName = @DepartmentName,
    ModifiedBy = @ModifiedBy,
    ModifiedOn = GETDATE()
    WHERE DepartmentID = @DepartmentID;
END
GO

EXEC UpdateDepartment
    @DepartmentID = 1,
    @DepartmentName = 'CSE UPDATED';


--------------------------------------------------5

CREATE PROCEDURE DeleteDepartment
    @DepartmentID INT
AS
BEGIN
    UPDATE Department1
    SET
        IsActive = 0,
        ModifiedOn = GETDATE()
    WHERE DepartmentID = @DepartmentID
       AND isActive = 1;
       
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO



EXEC DeleteDepartment @deptId = 3;




----------------------Attendance------------------

CREATE TABLE Attendence2
(
    StudentID INT NOT NULL,
    SubjectID INT NOT NULL,
    AttendanceDate DATETIME NOT NULL,
    Status VARCHAR(20) NOT NULL,
    CreatedBy VARCHAR(100) NULL,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedBy VARCHAR(100) NULL,
    ModifiedOn DATETIME NULL,
    IsActive INT NOT NULL DEFAULT 1
);

drop table Attendence2


INSERT INTO Attendence2
    (StudentID, SubjectID, AttendanceDate, Status)
VALUES
    (1, 22, '2026-06-17', 'P');

    select * from Attendence2


-------------------------------------------------------------------------1

CREATE PROCEDURE GetAllAttendance
AS
BEGIN
    SELECT StudentID,SubjectID,AttendanceDate,Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, IsActive
    FROM Attendence2
    WHERE IsActive = 1;
END;
Go


EXEC GetAllAttendance


----------------------------------------------------------------------------2

CREATE PROCEDURE GetByStudentId
    @StudentID INT
AS
BEGIN
    SELECT StudentID,SubjectID,AttendanceDate,Status,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM Attendence2
    WHERE StudentID = @StudentID
      AND IsActive = 1
    ORDER BY AttendanceDate DESC;
END;



-----------------------------------------------------------------------------3

CREATE PROCEDURE AddAttendance
    
    @StudentID INT,
    @SubjectID INT,
    @AttendanceDate DATE,
    @Status VARCHAR(10)
AS
BEGIN

    INSERT INTO Attendence2
    (StudentID,SubjectID,AttendanceDate,Status,CreatedOn,IsActive)
    VALUES
    (@StudentID,@SubjectID,@AttendanceDate,@Status,GETDATE(),1);
END;



------------------------------------------------------------------------------4

CREATE PROCEDURE UpdateAttendance
    @StudentID INT,
    @SubjectID INT,
    @AttendanceDate DATE,
    @Status VARCHAR(10),
    @ModifiedBy VARCHAR(100)
AS
BEGIN
    UPDATE Attendence2
    SET
        SubjectID = @SubjectID,
        AttendanceDate = @AttendanceDate,
        Status = @Status,
        ModifiedBy = @ModifiedBy,
        ModifiedOn = GETDATE()
    WHERE StudentID = @StudentID 
         AND IsActive = 1 ;

         SELECT @@ROWCOUNT;
END;

Drop procedure UpdateAttendance

-------------------------------------------------------------------------------5

CREATE PROCEDURE DeleteAttendance
    @StudentID INT
AS
BEGIN

    UPDATE Attendence2
    SET
        IsActive = 0,
        ModifiedOn = GETDATE()
    WHERE StudentID = @StudentID
      AND IsActive = 1;

    SELECT @@ROWCOUNT
END;

drop procedure DeleteAttendance




----------------------Faculty--------------------

CREATE TABLE Faculty2(
    FacultyID INT primary key identity (101,1),
    FacultyName VARCHAR(100),
    Email VARCHAR(100),
    Salary DECIMAL(10,2) CHECK (Salary > 0),
    DepartmentName VARCHAR(50),
    Phone VARCHAR(10) UNIQUE,
    CreatedBy VARCHAR(100) NULL,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedBy VARCHAR(100) NULL,
    ModifiedOn DATETIME NULL ,
    IsActive INT NOT NULL DEFAULT 1
);

INSERT INTO Faculty2(FacultyName, Email, Salary, DepartmentName , Phone)
VALUES
('Dr. Sharma','drsharma@college.com',65000,'CSE', '9876543210'),
('Dr. Patil','drpatil@college.com',70000, 'AIML', '9876543211');

SELECT * FROM Faculty2



--------------------------------------------------------------------1

CREATE PROCEDURE GetAllFaculty
AS
BEGIN
    SELECT FacultyId, FacultyName, Email, Salary, DepartmentName, Phone,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive

    FROM Faculty2
    WHERE IsActive = 1;
END;
GO

EXEC GetAllFaculty;

drop procedure GetFacultyById

---------------------------------------------------------------------2

CREATE PROCEDURE GetFacultyById
    @FacultyId INT
AS
BEGIN
    SELECT FacultyId, FacultyName, Email, Salary,DepartmentName,Phone,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM Faculty2
    WHERE FacultyId = @FacultyId
        AND IsActive =1;
END;
GO


---------------------------------------------------------------------3

CREATE PROCEDURE AddFaculty
    @FacultyName VARCHAR(100),
    @Email VARCHAR(150),
    @Salary DECIMAL(10,2),
    @DepartmentName VARCHAR(50),
    @Phone VARCHAR(10),
    @CreatedBy VARCHAR(100) = NULL
AS
BEGIN
    INSERT INTO Faculty2
    (FacultyName,Email,Salary,DepartmentName,Phone,CreatedBy)
    VALUES
    (@FacultyName,@Email,@Salary,@DepartmentName,@Phone,@CreatedBy); 
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;

drop procedure AddFaculty
------------------------------------------------------------------------4

CREATE PROCEDURE UpdateFaculty
    @FacultyId INT,
    @FacultyName VARCHAR(100),
    @Email VARCHAR(150),
    @Salary DECIMAL(10,2),
    @DepartmentName VARCHAR(50),
    @Phone VARCHAR(10),
    @ModifiedBy VARCHAR(100) = NULL
AS
BEGIN
    UPDATE Faculty2
    SET
        FacultyName = @FacultyName,
        Email = @Email,
        Salary = @Salary,
        DepartmentName = @DepartmentName,
        Phone = @Phone,
        ModifiedBy = @ModifiedBy,
        ModifiedOn = GETDATE()
    WHERE FacultyID = @FacultyId
        AND IsActive = 1;
END;
GO

drop procedure UpdateFaculty


------------------------------------------------------------------------5

CREATE PROCEDURE DeleteFaculty
    @FacultyId INT
AS
BEGIN
    UPDATE Faculty2
    SET
        IsActive = 0,
        ModifiedOn = GETDATE()
    WHERE FacultyId = @FacultyId
      AND IsActive = 1;

    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

EXEC DeleteFaculty @FacultyId = 105;



--------------------Subject-----------------------


CREATE TABLE Subject2
(
    SubjectCode INT IDENTITY(11,1) PRIMARY KEY,
    SubjectName VARCHAR(100) NOT NULL,
    Credits INT NOT NULL,
    FacultyID INT NOT NULL,
    DepartmentID INT NOT NULL,
    CreatedBy VARCHAR(100) NULL,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedBy VARCHAR(100) NULL,
    ModifiedOn DATETIME NULL,
    IsActive INT NOT NULL DEFAULT 1
);

INSERT INTO Subject2(SubjectName,Credits,FacultyID,DepartmentID)
VALUES
('Database',4,1,1);

select*from Subject2



-------------------------------------------------------------------------1

CREATE PROCEDURE GetAllSubjects
AS
BEGIN
    SELECT SubjectCode,SubjectName,Credits,FacultyID,DepartmentID,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM Subject2
    WHERE IsActive = 1; 
END;
GO


-------------------------------------------------------------------------2

CREATE PROCEDURE GetSubjectByCode
    @SubjectCode INT
AS
BEGIN
    SELECT SubjectCode,SubjectName,Credits,FacultyID,DepartmentID,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM Subject2
    WHERE SubjectCode = @SubjectCode
        AND IsActive=1;
END;
GO


-------------------------------------------------------------------------3

CREATE PROCEDURE AddSubject
    @SubjectName VARCHAR(100),
    @Credits INT,
    @FacultyID INT,
    @DepartmentID INT,
    @CreatedBy VARCHAR(50)
AS
BEGIN
    INSERT INTO Subject2
    (SubjectName,Credits,FacultyID,DepartmentID,CreatedBy,CreatedOn,IsActive)
    VALUES
    (@SubjectName,@Credits,@FacultyID,@DepartmentID,@CreatedBy,GETDATE(),1);

    SELECT SubjectCode,SubjectName,Credits,FacultyID,DepartmentID,CreatedBy,CreatedOn
    FROM Subject2
    WHERE SubjectCode = SCOPE_IDENTITY();
END;
GO


-------------------------------------------------------------------------4

CREATE PROCEDURE UpdateSubject
    @SubjectCode INT,
    @SubjectName VARCHAR(100),
    @Credits INT,
    @FacultyID INT,
    @DepartmentID INT,
    @ModifiedBy VARCHAR(50)
AS
BEGIN
    UPDATE Subject2
    SET
        SubjectName = @SubjectName,
        Credits = @Credits,
        FacultyID = @FacultyID,
        DepartmentID = @DepartmentID,
        ModifiedBy =@ModifiedBy,
        ModifiedOn = GETDATE()
    WHERE SubjectCode = @SubjectCode
        AND IsActive=1;

    SELECT SubjectCode,SubjectName,Credits,FacultyID,DepartmentID,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM Subject2
    WHERE SubjectCode = @SubjectCode
        AND IsActive =1;
END;
GO


-------------------------------------------------------------------------5

CREATE PROCEDURE DeleteSubject
    @SubjectCode INT
AS
BEGIN
    UPDATE Subject2
    SET
        IsActive = 0,
        ModifiedOn = GETDATE()
    WHERE SubjectCode = @SubjectCode
      AND IsActive = 1;

    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO



----------------------User-----------------------

CREATE TABLE User1
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    CreatedBy VARCHAR(100) NULL,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedBy VARCHAR(100) NULL,
    ModifiedOn DATETIME NULL,
    IsActive INT NOT NULL DEFAULT 1
);

INSERT INTO User1(Username,Email,Password)
VALUES
('Diksha','diksha@gmail.com','12345');

SELECT * FROM User1



------------------------------------------------------------------------1

CREATE PROCEDURE GetAllUsers
AS
BEGIN
    SELECT UserID,Username,Email,Password,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM User1
    WHERE IsActive = 1;
END;
GO



-------------------------------------------------------------------------2

CREATE PROCEDURE GetUserById
    @UserID INT
AS
BEGIN
    SELECT UserID,Username,Email,Password,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM User1
    WHERE UserID = @UserID
      AND IsActive = 1;
END;
GO


-------------------------------------------------------------------------3

CREATE PROCEDURE AddUser
    @Username VARCHAR(100),
    @Email VARCHAR(150),
    @Password VARCHAR(255),
    @CreatedBy VARCHAR(50)
AS
BEGIN
    INSERT INTO User1(Username,Email,Password,CreatedBy,CreatedOn,IsActive)
    VALUES
    (@Username,@Email,@Password,@CreatedBy,GETDATE(),1);

    SELECT
        UserID,Username,Email,Password,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM User1
    WHERE UserID = SCOPE_IDENTITY();
END;
GO

---------------------------------------------------------------------------4

CREATE PROCEDURE UpdateUser
    @UserID INT,
    @Username VARCHAR(100),
    @Email VARCHAR(150),
    @Password VARCHAR(255),
    @ModifiedBy VARCHAR(50)
AS
BEGIN
    UPDATE User1
    SET
        Username = @Username,
        Email = @Email,
        Password = @Password,
        ModifiedBy = @ModifiedBy,
        ModifiedOn = GETDATE()
    WHERE UserID = @UserID;

    SELECT UserID,Username,Email,Password,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive
    FROM User1
    WHERE UserID = @UserID
          AND IsActive = 1;
END;
GO



------------------------------------------------------------------------------5
CREATE PROCEDURE DeleteUser
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE User1
    SET
        IsActive = 0,
        ModifiedOn = GETDATE()
    WHERE UserID = @UserID
      AND IsActive = 1;

    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

EXEC GetAllUsers

