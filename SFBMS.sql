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
	[Date] datetime NOT NULL,
	[Origin] nvarchar(100) NULL,
	[Level] nvarchar(50) NULL,
	[Message] nvarchar(max) NULL,
	[Detail] nvarchar(max) NULL
)
insert into NLogInfo([Date],[origin],[Level],[Message],[Detail]) values (getdate(),'', 'Information', 'Test','')
select * from [NLogInfo]

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
insert into ClientInfo values('13900326','����','123456',1,452723199904202457,'1999.4.20','�����޳�������������','1587291189',1,'/src/Client/HeaderImg/����ͷ��1.jpg',getdate())
insert into ClientInfo values('19300126','����','123456',1,452723199008202457,'1900.8.20','�����޳�������������','1587291189',1,'/src/Client/HeaderImg/����ͷ��2.jpg',getdate())
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
insert into User_Role values('13900326',3)
insert into User_Role values('13900326',4)
insert into User_Role values('19300126',5)
select *from User_Role
---�鼮
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
insert into BookInfo values('���μ�',4,'��ж�','δ֪','2021-07-10',39,10,
'�����μǡ����й��Ŵ���һ�����������»��峤ƪ��ħС˵���ִ������ٻر������μǡ������������������ѧ������|��������������μǡ�������������ж���','/src/Book/images/���μ�.jpg',1,getdate())
insert into BookInfo values('��¥��',4,'��ѩ��','δ֪','2021-07-10',39,5,
'����¥�Ρ����й��Ŵ��»��峤ƪС˵���й��ŵ��Ĵ�����֮һ��һ����Ϊ��������Ҳ�ѩ��������
С˵�Լ֡�ʷ������Ѧ�Ĵ�������˥Ϊ�������Ը����Ӽֱ���Ϊ�ӽǣ��Լֱ�����������Ѧ���εİ����������Ϊ���ߣ�
�����һ����ֹ��ʶ������ü֮�ϵĹ����˵�������̬��չ�����������������ͱ�������
����˵��һ���Ӹ����Ƕ�չ��Ů�����Լ��й��Ŵ������̬�����ʷʫ��������','/src/Book/images/��¥��.jpg',1,getdate())
insert into BookInfo values('ˮ䰴�',4,'ʩ����','δ֪','2021-07-10',39,22,
'��ˮ䰴�����Ԫĩ����ʩ���֣��ִ濯�����������ʩ���֡��޹��������е�һ�ˣ������˽��У��������»��峤ƪС˵��','/src/Book/images/ˮ䰴�.jpg',1,getdate())
insert into BookInfo values('��������',4,'�޹���','δ֪','2021-07-10',39,30,
'���������塷��ȫ��Ϊ������־ͨ�����塷���ֳơ�����־���塷����Ԫĩ����С˵���޹��и��ݳ��١�����־��������֮ע���Լ�����������´�˵���������ӹ��������ɵĳ�ƪ�»�����ʷ����С˵��
�롶���μǡ���ˮ䰴�������¥�Ρ�����Ϊ�й��ŵ��Ĵ�����������Ʒ������мξ����籾�ȶ���汾��������
������ĩ�����ë�ڸڶԡ��������塷���ٻ�Ŀ�������Ĵǡ��Ļ�ʫ�ģ��ð汾Ҳ��Ϊ���汾��ˮƽ��ߡ��������İ汾��','/src/Book/images/��������.jpg',1,getdate())
select *from BookInfo

 create table BookType(
 TypeId int primary key identity(1,1),
 TypeName varchar(125) not null,
 )
 insert into BookType values('����')
 insert into BookType values('����')
 insert into BookType values('�Ƽ�')
 insert into BookType values('��ѧ')
 insert into BookType values('��ʷ')
 insert into BookType values('����')
 insert into BookType values('ũҵ')
 insert into BookType values('ֲ��')
 insert into BookType values('����')
 insert into BookType values('��־')
 select *from BookType

 select b.Id,b.BookName,b.Author,b.Press,b.PublicationDate,b.Price,b.Inventory,b.Descripcion,
 t.TypeName
 from BookInfo as b
 left join BookType as t on b.TypeId=b.TypeId

 select c.ClientNo,c.Name,c.Sex,c.IdNumber,c.BirthDate,c.Address,c.Phone,c.State,c.CreateTime,
 STRING_AGG(r.Name, '��') as RoleNames
 from ClientInfo as c 
 left join User_Role as ur on c.ClientNo = ur.UserId
 left join RoleInfo as r on r.RoleId = ur.RoleId
 GROUP BY c.ClientNo,c.Name,c.Sex,c.IdNumber,c.BirthDate,c.Address,c.Phone,c.State,c.CreateTime