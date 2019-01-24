
Create table Country(
Id bigint identity(1,1) not null,
Name nvarchar(255) not null,
primary key (Id)
);

Create table Region(
Id bigint identity(1,1) not null,
Name nvarchar(255) not null,
Country_Id bigint not null,
primary key (Id),
constraint FK_Region_Country foreign key (Country_Id) references Country(Id)
);

Create table CityType(
Id bigint identity(1,1) not null,
Name nvarchar(255) not null,
ShortName nvarchar(255) not null,
primary key (Id)
);

Create table City(
Id bigint identity(1,1) not null,
Name nvarchar(255) not null,
CityType_Id bigint not null,
Region_Id bigint not null,
primary key (Id),
constraint FK_City_CityType foreign key (CityType_Id) references CityType(Id),
constraint FK_City_Region foreign key (Region_Id) references Region(Id)
);

Create table Street(
Id bigint identity(1,1) not null,
Name nvarchar(255) not null,
City_Id bigint not null,
primary key (Id),
constraint FK_Street_City foreign key (City_Id) references City(Id)
);

Create table Address(
Id bigint identity(1,1) not null,
School_Id bigint,
House nvarchar(255) not null,
Street_Id bigint not null,
primary key (Id),
constraint FK_Address_School foreign key (School_Id) references School(Id),
constraint FK_Address_Street foreign key (Street_Id) references Street(Id)
);