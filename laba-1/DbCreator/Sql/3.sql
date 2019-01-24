Create table Attendee(
Id bigint identity(1,1) not null,
FullName nvarchar(255) not null,
PhoneNumber nvarchar(255),
Email nvarchar(255),
Type int not null,

primary key (Id)
);

Create table AcademicProgram(
Id bigint identity(1,1) not null,
Name nvarchar(255) not null,
primary key (Id)
);	

Create table Pupil(
Sex int not null,
YearOfGraduation int not null,
EnterProgram_Id bigint,
School_Id bigint not null,
Attendee_Id bigint not null,

constraint FK_Pupil_EnterProgram foreign key (EnterProgram_Id) references AcademicProgram(Id),
constraint FK_Pupil_School foreign key (School_Id) references School(Id),
constraint FK_Pupil_Attendee foreign key (Attendee_Id) references Attendee(Id),
constraint AK_AttendeeId unique(Attendee_Id)
);

Create table IntrestingProgram(
AcademicProgram_Id bigint not null,
Pupil_Id bigint not null,

constraint FK_IntrestingProgram_AcademicProgram foreign key (AcademicProgram_Id) references AcademicProgram(Id),
constraint FK_IntrestingProgram_Pupil foreign key (Pupil_Id) references Pupil(Attendee_Id)

);	

Create table RegistrarionProgram(
AcademicProgram_Id bigint not null,
Pupil_Id bigint not null,

constraint FK_RegistrarionProgram_AcademicProgram foreign key (AcademicProgram_Id) references AcademicProgram(Id),
constraint FK_RegistrarionProgram_Pupil foreign key (Pupil_Id) references Pupil(Attendee_Id)
);