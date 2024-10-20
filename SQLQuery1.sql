

/*CREATE TABLE Appointments (
    AppointmentID INT PRIMARY KEY ,  
    PatientName VARCHAR(100) NOT NULL,
    AppointmentTime DATETIME NOT NULL,
    IsCompleted BIT DEFAULT 0
);
*/
select * from Appointments
/*drop table Appointments */

/*INSERT INTO Appointments (PatientName, AppointmentTime, IsCompleted)
VALUES ('John Doe', '2024-10-17 09:00:00', 0);


INSERT INTO Appointments (PatientName, AppointmentTime, IsCompleted)
VALUES ('Jane Smith', '2024-10-17 10:30:00', 0);*/

/*select * from Appointments */



/*CREATE TABLE Clinics (
    ClinicID INT PRIMARY KEY ,  
    ClinicName VARCHAR(100) NOT NULL
);
*/

/*drop table Clinics */

/*CREATE TABLE ClinicianAppointments (
    ClinicianAppointmentID INT PRIMARY KEY ,
    AppointmentID INT NOT NULL,
    ClinicID INT NOT NULL,
    FOREIGN KEY (AppointmentID) REFERENCES Appointments(AppointmentID),
    FOREIGN KEY (ClinicID) REFERENCES Clinics(ClinicID)
);
*/

/*drop table ClinicianAppointments */