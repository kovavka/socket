Create table "Group"(
Id bigint identity(1,1) not null,
Name nvarchar(255) not null,
primary key (Id)
);	

Create table Volunteer(
Id bigint identity(1,1) not null,
FullName nvarchar(255) not null,
Group_Id bigint,

primary key (Id),
constraint FK_Volunteer_Group foreign key (Group_Id) references "Group"(Id)
);

Create table Department(
Id bigint identity(1,1) not null,
Name nvarchar(255) not null,
Color nvarchar(255) not null,
primary key (Id)
);	

Create table Employee(
Id bigint identity(1,1) not null,
FullName nvarchar(255) not null,
PhoneNumber nvarchar(255),
Email nvarchar(255),
Appointment nvarchar(255),
primary key (Id)
);

Create table "User"(
Login nvarchar(255) not null,
Password nvarchar(255) not null,
IsAdmin bit not null,
Checked bit not null,
Employee_Id bigint not null,

constraint FK_User_Employee foreign key (Employee_Id) references Employee(Id),
constraint AK_EmployeeId unique(Employee_Id)
);
