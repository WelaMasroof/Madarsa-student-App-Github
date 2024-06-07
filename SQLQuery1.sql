CREATE TABLE class (
    id INT IDENTITY(1,1) PRIMARY KEY,
    class NVARCHAR(50) NOT NULL,
    subject1 NVARCHAR(50) NOT NULL,
    subject2 NVARCHAR(50),
    subject3 NVARCHAR(50),
    subject4 NVARCHAR(50),
    subject5 NVARCHAR(50),
    subject6 NVARCHAR(50),
    subject7 NVARCHAR(50)
);

CREATE TABLE student_admission (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    fathername NVARCHAR(100) NOT NULL,
    address NVARCHAR(255) NOT NULL,
    contact_no NVARCHAR(15) NOT NULL,
    alternate_no NVARCHAR(15),
    guardian_no NVARCHAR(15) NOT NULL,
    relation_with_guardian NVARCHAR(50) NOT NULL,
    previous_qualification NVARCHAR(100) NOT NULL,
    cnic NVARCHAR(15) NOT NULL,
    date_of_admission DATE NOT NULL,
    class_id INT NOT NULL,
    FOREIGN KEY (class_id) REFERENCES class(id)
);

CREATE TABLE result (
    id INT IDENTITY(1,1) PRIMARY KEY,
    student_id INT NOT NULL,
    class_id INT NOT NULL,
    student_name NVARCHAR(100) NOT NULL,
    class_name NVARCHAR(50) NOT NULL,
    examname NVARCHAR(100) NOT NULL,
    s1marks INT not null,
    s2marks INT,
    s3marks INT,
    s4marks INT,
    s5marks INT,
    s6marks INT,
    s7marks INT,
    FOREIGN KEY (student_id) REFERENCES student_admission(id),
    FOREIGN KEY (class_id) REFERENCES class(id)
);


CREATE TABLE student_left(
    id INT identity(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    fathername NVARCHAR(100) NOT NULL,
    address NVARCHAR(255) NOT NULL,
    contact_no NVARCHAR(15) NOT NULL,
    alternate_no NVARCHAR(15),
    guardian_no NVARCHAR(15) not null,
    relation_with_guardian NVARCHAR(50) not null,
    previous_qualification NVARCHAR(100) not null,
    cnic NVARCHAR(15) NOT NULL,
    date_of_admission DATE NOT NULL,
	class nvarchar(50) not null,
	duration_of_study nvarchar(50) not null,
	reason_to_leave nvarchar(50) not null,
	date_of_left nvarchar(50) not null,
);

INSERT INTO class (class, subject1, subject2, subject3, subject4, subject5, subject6, subject7)
VALUES 
('Class 1', 'Subject 1', 'Subject 2', 'Subject 3', NULL, NULL, NULL, NULL),
('Class 2', 'Subject A', 'Subject B', NULL, NULL, NULL, NULL, NULL),
('Class 3', 'Topic X', 'Topic Y', 'Topic Z', 'Topic W', NULL, NULL, NULL),
('Class 4', 'Physics', 'Chemistry', 'Biology', 'Mathematics', NULL, NULL, NULL),
('Class 5', 'History', 'Geography', 'Civics', 'Economics', 'Political Science', NULL, NULL);

INSERT INTO student_admission (name, fathername, address, contact_no, alternate_no, guardian_no, relation_with_guardian, previous_qualification, cnic, date_of_admission, class_id)
VALUES 
('John Doe', 'Michael Doe', '123 Main Street, City', '1234567890', '9876543210', '9998887776', 'Father', 'High School Diploma', '12345-6789123-4', '2024-01-01', 1),
('Jane Smith', 'David Smith', '456 Elm Street, Town', '9876543210', '1234567890', '9997778886', 'Father', 'GED', '98765-4321890-1', '2023-09-15', 2),
('Alice Johnson', 'Robert Johnson', '789 Oak Street, Village', '5553334444', '1112223333', '9996665558', 'Mother', 'Associate Degree', '44433-2221110-5', '2022-07-01', 3),
('Bob Brown', 'William Brown', '101 Pine Street, County', '7778889999', '4445556666', '9993332220', 'Guardian', 'Some College', '77722-8881113-6', '2023-03-20', 1),
('Eva Garcia', 'Jose Garcia', '202 Maple Street, State', '2229998888', '8889990000', '9990001112', 'Guardian', 'None', '66699-1112223-4', '2021-12-10', 2);
