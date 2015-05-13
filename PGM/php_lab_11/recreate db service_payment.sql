drop database if exists services_payment;
create database services_payment;
use services_payment;
create table Mounthes(
			 num       int     not null,
             full_name char(8) not null,
             primary key (num));
                      
insert into Mounthes values (1,'Январь');
insert into Mounthes values (2,'Февраль');
insert into Mounthes values (3,'Март');
insert into Mounthes values (4,'Апрель');
insert into Mounthes values (5,'Май');
insert into Mounthes values (6,'Июнь');
insert into Mounthes values (7,'Июль');
insert into Mounthes values (8,'Август');
insert into Mounthes values (9,'Сентябрь');
insert into Mounthes values (10,'Октябрь');
insert into Mounthes values (11,'Ноябрь');
insert into Mounthes values (12,'Декабрь');

create table Payment_types(
			 id    int      auto_increment,
			 title char(14) not null,
             primary key (id),
			 index (title) );
             
insert into Payment_types (title) values('авансовый');
insert into Payment_types (title) values('платёж диллеру');
insert into Payment_types (title) values('скретч карта');

create table Categories(
			 id    int    auto_increment,
             title char(30) not null,
             primary key (id),
             index (title) );
             
insert into Categories (title) values('входящие');
insert into Categories (title) values('абонентская плата');
insert into Categories (title) values('исходящие внутри сети');
insert into Categories (title) values('исходящие вне сети');
insert into Categories (title) values('исходящие международные звонки');
insert into Categories (title) values('входящие международные звонки');
insert into Categories (title) values('услуги SMS');

create table Services(
			 service_code int      not null,
             title        char(50) not null,
             category     char(50) not null,
             primary key (service_code),
             index (title),
             foreign key (category) references Categories (title) );
                    
create table Clients(
			 bill         int       not null,
             tariff_plan  char(50)  not null,
             connect_date datetime  not null,
             phone_number char(12)  not null,
             full_name    char(50)  not null,
             adress       char(100) not null,
             region       char(50)  not null,
             primary key (bill),
             foreign key (tariff_plan) references services (title) );
             
create table Clients_balanses(
		     bill    int not null,
             mounth  int not null,
             year_   int not null,
             balance double not null,
             primary key (bill),
             index (bill),
             foreign key (mounth) references mounthes (num),
             foreign key (bill) references clients (bill) );
create table Payments(
			 bill         int      not null,
             mounth       int      not null,
             year_        int      not null,
             payment_type char(14) not null,
             amount       double   not null,
             primary key (bill),
             foreign key (payment_type) references Payment_types (title));
             
create table Debts(
			 bill         int    not null,
             mounth       int    not null,
             year_        int    not null,
             service_code int    not null,
             amount       double not null,
             primary key (bill) );