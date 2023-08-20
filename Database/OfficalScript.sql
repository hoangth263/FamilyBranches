
Create database Interactive_Family_TreeOffical;
go
use Interactive_Family_TreeOffical;
go 
 create table Members(
Id int primary key,
FullName Nvarchar(100) not null,
Image Nvarchar(100),
Phone nvarchar(11) ,
Email Nvarchar(50) unique not null,
Gender bit ,
Birthday Datetime not null,
Password Nvarchar(50) not null default '1',
Status bit default 1
 );

 go

 go
create table FamilyTrees(
Id int primary key identity(1,1),
FirstName nvarchar(100) not null,
CreateDate datetime not null,
ModifyDate datetime,
TotalGeneration int default 1,
Status bit default 1
);

create table FamilyMember(
ID int primary key identity(1,1),
TreeID int foreign key references FamilyTrees(Id) ,
MemberID int foreign key references Members(Id) default null,
FullName nvarchar(100) not null,
Gender bit default 1,
Generation int not null,
Birthday datetime not null,
CreateDate datetime,
UpdateDate datetime,
Role nvarchar(50) default 'member',
Status bit default 1,
unique(TreeID, MemberID),
StatusHealth bit default 1
);
go
create table ChildAndParentsRelationShip(
Id int primary key identity (1,1),
ParentID int foreign key references FamilyMember(Id),
ChildID int foreign key references FamilyMember(Id)
);

go
create table CoupleRelationship(
Id int primary key identity(1,1),
HusbandID int foreign key references FamilyMember(Id),
WifeID int foreign key references FamilyMember(Id),
CreateDate datetime not null,
UpdateDate datetime not null,
MarrageStatus nvarchar(20)
);
go

create table Career(
Id int primary key identity(1,1),
FamilyMemberID int foreign key references FamilyMember(Id),
Detail nvarchar(100) not null,
StartDate datetime,
EndDate datetime,
Status bit default 1
);
go
create table FamilyEvent(
Id int primary key identity(1,1),
[Name] nvarchar(100) ,
[Description] nvarchar(max),
TreeId int foreign key references FamilyTrees(Id),
Type bit default 1,
Date datetime,
Status bit default 1
);
go
Create table EventParticipant (
Id int primary key identity(1,1),
EventId int foreign key references FamilyEvent(Id),
FamilyMemberID int foreign key references FamilyMember(Id),
Status bit default 1
);