create database SFBMS
use SFBMS
---------
create table AdminInfo(
AdminNo varchar(20) primary key not null,
[Name] varchar(20),
[Password] varchar(125),
Sex int,
)
insert into AdminInfo values('luo','������','123456',1)
--��־��
CREATE TABLE [dbo].[NLogInfo](
	[LogId] int primary key identity(1,1) NOT NULL,
	[MachineId] nvarchar(120) NOT NULL,	
	[Origin] nvarchar(100) NULL,--������Դ
	[RouteInfo] varchar(100) NULL,--�ӿ���Ϣ
	[Level] varchar(50) NULL,
	[Message] varchar(max) NULL,
	[Detail] varchar(max) NULL,
	[Date] datetime NOT NULL,
)
insert into NLogInfo values ('::2','','','','','',getdate())
select * from [NLogInfo]
delete from [NLogInfo]where [MachineId]='::2'

create table ClientInfo(
ClientNo varchar(20) primary key ,
[Name] varchar(20),
[Password] varchar(125),
Sex int,
IdNumber varchar(225),--���֤
BirthDate datetime,
[Address] varchar(255),
Phone varchar(125),
[State] int,--�Ƿ����û� 1���� 0�ر�
HeaderImgSrc varchar(125),
CreateTime datetime ,--���ʱ��
)
insert into ClientInfo values('19300326','����','123456',1,452723199904202457,'1999.4.20','�����޳�������������','1587291189',1,'/src/Client/HeaderImg/����ͷ��1.jpg',getdate())
insert into ClientInfo values('19300126','����','123456',1,452723199008202457,'1900.8.20','�����޳�������������','1587291189',1,'/src/Client/HeaderImg/����ͷ��2.jpg',getdate())
insert into ClientInfo values('19300111','С��','123456',1,452723199008202457,'1900.8.20','�����޳�������������','15872891189',1,'/src/Client/HeaderImg/����ͷ��2.jpg',getdate())
insert into ClientInfo values('19300112','С��','123456',1,452723199008202457,'1900.8.20','�����޳�������������','12345678911',1,'/src/Client/HeaderImg/����ͷ��2.jpg',getdate())

select *from ClientInfo
delete from ClientInfo
----
create table RoleInfo(
RoleId int primary key identity(1,1),
[Name] varchar(125) not null,
Descripcion varchar(125)
)
insert into RoleInfo values('��������Ա','')
insert into RoleInfo values('��ͨ����Ա','')
insert into RoleInfo values('VIP�û�','')
insert into RoleInfo values('SVIP�û�','')
insert into RoleInfo values('��ͨ�û�','')
select *from RoleInfo
------------
create table User_Role(
UserId varchar(20) not null,
RoleId int  not null
)
ALTER TABLE User_Role WITH NOCHECK ADD
CONSTRAINT[PK_User_Role] PRIMARY KEY NONCLUSTERED(UserId,RoleId)
insert into User_Role values('luo',1)
insert into User_Role values('19300326',3)
insert into User_Role values('19300326',4)
insert into User_Role values('19300126',5)
select *from User_Role
---�鼮��Ϣ��
create table BookInfo(
Id int primary key identity(1,1),
BookName varchar(125) not null,
TypeId int ,
Author varchar(125) ,
Press  varchar(125),--������
PublicationDate datetime,--��������
Price float not null,
Inventory int not null,--�����
Descripcion text,--�鼮����
CoverImgSrc varchar(125),
[State] int,--�Ƿ����鼮 1���� 0�ر�
CreateTime datetime ,--���ʱ��
)
select *from BookInfo
---�鼮���ͱ�
 create table BookType(
 TypeId int primary key identity(1,1),
 TypeName varchar(125) not null,
 )
 --�鼮���ı�
create table Subscribe(
 Id int identity(1,1),
 BookId int not null,
 ClientNo varchar(20) not null,--������
 CreateTime datetime ,--����ʱ��
 )
 ALTER TABLE Subscribe WITH NOCHECK ADD
CONSTRAINT[PK_Subscribe] PRIMARY KEY NONCLUSTERED(BookId,ClientNo)
 insert into Subscribe values(5,'19300111',GETDATE())
 insert into Subscribe values(6,'19300111',GETDATE())
 insert into Subscribe values(7,'19300111',GETDATE())
 insert into Subscribe values(8,'19300111',GETDATE())
 insert into Subscribe values(9,'19300111',GETDATE())
 insert into Subscribe values(5,'19300326',GETDATE())
 insert into Subscribe values(5,'19300126',GETDATE())
 insert into Subscribe values(5,'19300112',GETDATE())
 insert into Subscribe values(6,'19300112',GETDATE())
 insert into Subscribe values(7,'19300112',GETDATE())
 insert into Subscribe values(9,'19300112',GETDATE())
 insert into Subscribe values(12,'19300112',GETDATE())
 insert into Subscribe values(12,'19300126',GETDATE())
 insert into Subscribe values(13,'19300112',GETDATE())
 insert into Subscribe values(13,'19300126',GETDATE())
 insert into Subscribe values(13,'19300326',GETDATE())
 select bu.BookId,bu.ClientNo,bu.CreateTime as 'SubscribeTime',
 cli.[Name],bk.BookName,bt.TypeName
 from Subscribe as bu
 left join ClientInfo as cli on bu.ClientNo=cli.ClientNo
 left join BookInfo as bk on bu.BookId=bk.Id
 left join BookType as bt on bk.TypeId=bt.TypeId

  --������ϸ��
 create table BorrowInfo(
 Id int primary key identity(1,1),--���ĺ�
 BookId int not null,
 ClientNo varchar(20) not null,
 Number int not null,--��������
 Deposit int not null,--Ѻ��
 ExpectedReturnTime datetime,--Ԥ�ƹ黹����
 ActualReturnTime datetime,--ʵ�ʹ黹����
 CreateTime datetime ,--����ʱ��
 )
 create table ToExamine(
 Id varchar(20) primary key,--��˺�
 BorrowId varchar(20) not null,--���ĺ�
 AdminNo varchar(20) not null,--�����
 ClientNo varchar(20) not null,
 [State] int not null, --1 ����� 2 ��ͨ�� 3 ���� 4 �û�ȡ�� 5 ������ 6 �ѹ黹 7 ��֧��
 CreateTime datetime ,--���ʱ��
 )
 create table OrderInfo(
 Id int primary key identity(1,1),
 BorrowId varchar(20),--���ĺ�
 ClientNo varchar(20) not null,--֧����
 OrderType int ,--�������� ���� ���� ����
 PayType int ,--֧����ʽ 1֧���� 2 ΢�� 3�ֽ�
 [Money] int ,--���
 CreateTime datetime ,--����ʱ��
 )
 select b.Id,b.BookName,b.Author,b.Press,b.PublicationDate,b.Price,b.Inventory,b.Descripcion,
 t.TypeName
 from BookInfo as b
 left join BookType as t on b.TypeId=t.TypeId

 select c.ClientNo,c.Name,c.Sex,c.IdNumber,c.BirthDate,c.Address,c.Phone,c.State,c.CreateTime,
 STRING_AGG(r.Name, '|') as RoleNames
 from ClientInfo as c 
 left join User_Role as ur on c.ClientNo = ur.UserId
 left join RoleInfo as r on r.RoleId = ur.RoleId
 GROUP BY c.ClientNo,c.Name,c.Sex,c.IdNumber,c.BirthDate,c.Address,c.Phone,c.State,c.CreateTime

 select count(bt.TypeName) as value, bt.TypeName as [Name]
 from Subscribe as bu
 left join BookInfo as bk on bu.BookId = bk.Id
 left join BookType as bt on bk.TypeId = bt.TypeId
 group by bt.TypeName
 order by value desc