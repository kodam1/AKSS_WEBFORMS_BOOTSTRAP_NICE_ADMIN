create database AKSS_Management
go
use AKSS_Management
go
create table EmpDemo
(
	id int primary key identity(1,1),
	name varchar(100) ,
	role varchar(100) ,
	salary int
)