create database SFBMS
use SFBMS
---------
create table AdminInfo(
AdminNo varchar(20) primary key not null,
[Name] varchar(20),
[Password] varchar(125),
Sex int,
)
insert into AdminInfo values('luo','罗先生','123456',1)
--日志表
CREATE TABLE [dbo].[NLogInfo](
	[LogId] int primary key identity(1,1) NOT NULL,
	[MachineId] nvarchar(120) NOT NULL,	
	[Origin] nvarchar(100) NULL,--错误来源
	[RouteInfo] varchar(100) NULL,--接口信息
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
IdNumber varchar(225),--身份证
BirthDate datetime,
[Address] varchar(255),
Phone varchar(125),
[State] int,--是否开启用户 1开启 0关闭
HeaderImgSrc varchar(125),
CreateTime datetime ,--添加时间
)
insert into ClientInfo values('19300326','张三','123456',1,452723199904202457,'1999.4.20','广西罗城仫佬族自治县','1587291189',1,'/src/Client/HeaderImg/高清头像1.jpg',getdate())
insert into ClientInfo values('19300126','李四','123456',1,452723199008202457,'1900.8.20','广西罗城仫佬族自治县','1587291189',1,'/src/Client/HeaderImg/高清头像2.jpg',getdate())
insert into ClientInfo values('19300111','小罗','123456',1,452723199008202457,'1900.8.20','广西罗城仫佬族自治县','15872891189',1,'/src/Client/HeaderImg/高清头像2.jpg',getdate())
insert into ClientInfo values('19300112','小五','123456',1,452723199008202457,'1900.8.20','广西罗城仫佬族自治县','12345678911',1,'/src/Client/HeaderImg/高清头像2.jpg',getdate())

select *from ClientInfo
delete from ClientInfo
----
create table RoleInfo(
RoleId int primary key identity(1,1),
[Name] varchar(125) not null,
Descripcion varchar(125)
)
insert into RoleInfo values('超级管理员','')
insert into RoleInfo values('普通管理员','')
insert into RoleInfo values('VIP用户','')
insert into RoleInfo values('SVIP用户','')
insert into RoleInfo values('普通用户','')
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
---书籍信息表
create table BookInfo(
Id int primary key identity(1,1),
BookName varchar(125) not null,
TypeId int ,
Author varchar(125) ,
Press  varchar(125),--出版社
PublicationDate datetime,--出版年月
Price float not null,
Inventory int not null,--库存量
Descripcion text,--书籍描述
CoverImgSrc varchar(125),
[State] int,--是否开启书籍 1开启 0关闭
CreateTime datetime ,--添加时间
)
select *from BookInfo
---书籍类型表
 create table BookType(
 TypeId int primary key identity(1,1),
 TypeName varchar(125) not null,
 )
 --书籍订阅表
create table Subscribe(
 Id int identity(1,1),
 BookId int not null,
 ClientNo varchar(20) not null,--订阅者
 CreateTime datetime ,--订阅时间
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

  --借阅明细表
 create table BorrowInfo(
 Id int primary key identity(1,1),--借阅号
 BookId int not null,
 ClientNo varchar(20) not null,
 Number int not null,--借阅数量
 Deposit int not null,--押金
 ExpectedReturnTime datetime,--预计归还日期
 ActualReturnTime datetime,--实际归还日期
 CreateTime datetime ,--申请时间
 )
 create table ToExamine(
 Id varchar(20) primary key,--审核号
 BorrowId varchar(20) not null,--借阅号
 AdminNo varchar(20) not null,--审核人
 ClientNo varchar(20) not null,
 [State] int not null, --1 审核中 2 已通过 3 驳回 4 用户取消 5 已逾期 6 已归还 7 已支付
 CreateTime datetime ,--审核时间
 )
 create table OrderInfo(
 Id int primary key identity(1,1),
 BorrowId varchar(20),--借阅号
 ClientNo varchar(20) not null,--支付者
 OrderType int ,--订单类型 借阅 购买 在线
 PayType int ,--支付方式 1支付宝 2 微信 3现金
 [Money] int ,--金额
 CreateTime datetime ,--创建时间
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