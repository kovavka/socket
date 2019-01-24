Create table Event(
Id bigint identity(1,1) not null,
Name nvarchar(255) not null,
Info nvarchar(255) not null,
Execution date not null,
Comment nvarchar(255),
Address_Id bigint not null,
constraint FK_EventExecution_Address foreign key (Address_Id) references Address(Id),


primary key (Id)
);