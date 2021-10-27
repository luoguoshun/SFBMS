create database SFBMS
use SFBMS
---------
create table AdminInfo(
AdminNo varchar(125) primary key not null,
[Name] varchar(125),
[Password] varchar(125),
Sex int,
)
insert into AdminInfo values('luo','罗先生','123456',1)
--日志表
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
ClientNo int primary key identity(1,1),
[Name] varchar(125),
[Password] varchar(125),
Sex int,
IdNumber varchar(225),--身份证
BirthDate datetime,
[Address] varchar(255),
Phone varchar(125),
Flag int,
)
insert into ClientInfo values('张三','123456',1,452723199904202457,'1999.4.20','广西罗城仫佬族自治县','1587291189',1)
insert into ClientInfo values('李四','123456',1,452723199008202457,'1900.8.20','广西罗城仫佬族自治县','1587291189',1)

----
create table RoleInfo(
Id int primary key identity(1,1),
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
Id int identity(1,1),
UserId varchar(125) not null,
RoleId int  not null
)
ALTER TABLE User_Role WITH NOCHECK ADD
CONSTRAINT[PK_User_Role] PRIMARY KEY NONCLUSTERED(UserId,RoleId)
insert into RoleInfo values('luo',1)

---书籍
create table BookInfo(
Id int primary key identity(1,1),
BookName varchar(125) not null,
TypeId int ,
Author varchar(125) ,
Press  varchar(125),--出版社
PublicationDate datetime,--出版年月
Price int not null,
Inventory int not null,--库存量
Descripcion text,--书籍描述
ImageSrc varchar(125),
CreateTime datetime ,--添加时间
)
insert into BookInfo values('西游记',4,'吴承恩','','2021-07-10',39,10,
'《西游记》是中国古代第一部浪漫主义章回体长篇神魔小说。现存明刊百回本《西游记》均无作者署名。清代学者吴玉搢等首先提出《西游记》作者是明代吴承恩。','',getdate())
insert into BookInfo values('红楼梦',4,'曹雪芹','','2021-07-10',39,5,
'《红楼梦》，中国古代章回体长篇小说，中国古典四大名著之一，一般认为是清代作家曹雪芹所著。
小说以贾、史、王、薛四大家族的兴衰为背景，以富贵公子贾宝玉为视角，以贾宝玉与林黛玉、薛宝钗的爱情婚姻悲剧为主线，
描绘了一批举止见识出于须眉之上的闺阁佳人的人生百态，展现了真正的人性美和悲剧美，
可以说是一部从各个角度展现女性美以及中国古代社会世态百相的史诗性著作。','',getdate())
insert into BookInfo values('水浒传',4,'施耐庵','','2021-07-10',39,22,
'《水浒传》是元末明初施耐庵（现存刊本署名大多有施耐庵、罗贯中两人中的一人，或两人皆有）编著的章回体长篇小说。','',getdate())
insert into BookInfo values('三国演义',4,'罗贯中','','2021-07-10',39,30,
'《三国演义》（全名为《三国志通俗演义》，又称《三国志演义》）是元末明初小说家罗贯中根据陈寿《三国志》和裴松之注解以及民间三国故事传说经过艺术加工创作而成的长篇章回体历史演义小说，
与《西游记》《水浒传》《红楼梦》并称为中国古典四大名著。该作品成书后有嘉靖壬午本等多个版本传于世，
到了明末清初，毛宗岗对《三国演义》整顿回目、修正文辞、改换诗文，该版本也成为诸多版本中水平最高、流传最广的版本。','',getdate())
select *from BookInfo

 create table BookType(
 TypeId int primary key identity(1,1),
 TypeName varchar(125) not null,
 )
 insert into BookType values('言情')
 insert into BookType values('悬疑')
 insert into BookType values('科技')
 insert into BookType values('文学')
 insert into BookType values('历史')
 insert into BookType values('军事')
 insert into BookType values('农业')
 insert into BookType values('植物')
 insert into BookType values('感情')
 insert into BookType values('励志')
 select *from BookType

 select b.Id,b.BookName,b.Author,b.Press,b.PublicationDate,b.Price,b.Inventory,b.Descripcion,
 t.TypeName
 from BookInfo as b
 left join BookType as t on b.TypeId=b.TypeId








