Create table Event(
Id bigint identity(1,1) not null,
Name nvarchar(255) not null,
Info nvarchar(255) not null,
Comment nvarchar(255),
Type int not null,

primary key (Id)
);

Create table Subject(
Id bigint identity(1,1) not null,
Name nvarchar(255) not null,
primary key (Id)
);

Create table Course(
Price money,
Duration int,
Subject_Id bigint not null,
Event_Id bigint not null,

constraint FK_Course_Subject foreign key (Subject_Id) references Subject(Id),
constraint FK_Course_Event foreign key (Event_Id) references Event(Id),
constraint AK_Course_EventId unique(Event_Id)   
);

Create table AcademicCompetition(
Subject_Id bigint not null,
Event_Id bigint not null,

constraint FK_AcademicCompetition_Subject foreign key (Subject_Id) references Subject(Id),
constraint FK_AcademicCompetition_Event foreign key (Event_Id) references Event(Id),
constraint AK_AcademicCompetition_EventId unique(Event_Id)   
);


Create table SchoolWork(
Program text,
Event_Id bigint not null,

constraint FK_SchoolWork_Event foreign key (Event_Id) references Event(Id),
constraint AK_SchoolWork_EventId unique(Event_Id)   
);

Create table EventExecution(
Id bigint identity(1,1) not null,
Address_Id bigint not null,
Event_Id bigint,

primary key (Id),
constraint FK_EventExecution_Address foreign key (Address_Id) references Address(Id),
constraint FK_EventExecution_Event foreign key (Event_Id) references Event(Id)
);

Create table EventDate(
Id bigint identity(1,1) not null,
Date date not null,
StartTime time,
EndTime time,
EventExecution_Id bigint,

primary key (Id),
constraint FK_EventDate_EventExecution foreign key (EventExecution_Id) references EventExecution(Id)
);

Create table Purchase(
Id bigint identity(1,1) not null,
Name nvarchar(255) not null,
Price money not null,
Description text,
Event_Id bigint,

primary key (Id),
constraint FK_Purchase_Event foreign key (Event_Id) references Event(Id)
);
