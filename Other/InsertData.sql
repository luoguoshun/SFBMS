use SFBMS
insert into BookInfo values('西游记',4,'吴承恩','未知','2021-07-10',39,10,
'《西游记》是中国古代第一部浪漫主义章回体长篇神魔小说。现存明刊百回本《西游记》均无作者署名。清代学者吴玉搢等首先提出《西游记》作者是明代吴承恩。','/src/Book/images/西游记.jpg',1,getdate())
insert into BookInfo values('红楼梦',4,'曹雪芹','未知','2021-07-10',39,5,
'《红楼梦》，中国古代章回体长篇小说，中国古典四大名著之一，一般认为是清代作家曹雪芹所著。
小说以贾、史、王、薛四大家族的兴衰为背景，以富贵公子贾宝玉为视角，以贾宝玉与林黛玉、薛宝钗的爱情婚姻悲剧为主线，
描绘了一批举止见识出于须眉之上的闺阁佳人的人生百态，展现了真正的人性美和悲剧美，
可以说是一部从各个角度展现女性美以及中国古代社会世态百相的史诗性著作。','/src/Book/images/红楼梦.jpg',1,getdate())
insert into BookInfo values('水浒传',4,'施耐庵','未知','2021-07-10',39,22,
'《水浒传》是元末明初施耐庵（现存刊本署名大多有施耐庵、罗贯中两人中的一人，或两人皆有）编著的章回体长篇小说。','/src/Book/images/水浒传.jpg',1,getdate())
insert into BookInfo values('三国演义',4,'罗贯中','未知','2021-07-10',39,30,
'《三国演义》（全名为《三国志通俗演义》，又称《三国志演义》）是元末明初小说家罗贯中根据陈寿《三国志》和裴松之注解以及民间三国故事传说经过艺术加工创作而成的长篇章回体历史演义小说，
与《西游记》《水浒传》《红楼梦》并称为中国古典四大名著。该作品成书后有嘉靖壬午本等多个版本传于世，
到了明末清初，毛宗岗对《三国演义》整顿回目、修正文辞、改换诗文，该版本也成为诸多版本中水平最高、流传最广的版本。','/src/Book/images/三国演义.jpg',1,getdate())
select * from BookInfo

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

insert into BorrowInfo values(5,'19300326',1,20,'2021-12-30','',getdate())
insert into BorrowInfo values(6,'19300326',1,20,'2021-12-30','',getdate())
select * from BorrowInfo

insert into ToExamine values('123','1','luo','19300326','2',getdate())
insert into ToExamine values('456','2','luo','19300326','2',getdate())
select * from ToExamine

insert into OrderInfo values('1','19300326',1,2,20,getdate())
insert into OrderInfo values('2','19300326',1,2,20,getdate())
select * from OrderInfo

select br.Id as 'BorrowId',br.Number,br.Deposit,br.ExpectedReturnTime,br.ActualReturnTime,
bk.BookName,bk.CoverImgSrc,
bt.TypeName, bt.TypeId,
te.State,te.CreateTime as 'ExamineDate',
cli.Name as 'ClientName',cli.IdNumber as 'ClientIdNumber',
ad.Name as 'ExamineName'
from BorrowInfo as br
left join BookInfo as bk on br.BookId=bk.Id
left join BookType as bt on bk.TypeId=bt.TypeId
left join ToExamine as te on br.Id=te.BorrowId
left join ClientInfo as cli on te.ClientNo=cli.ClientNo
left join AdminInfo as  ad on te.AdminNo=ad.AdminNo