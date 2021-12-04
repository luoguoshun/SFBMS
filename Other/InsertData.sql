use SFBMS
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
select * from BookInfo

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