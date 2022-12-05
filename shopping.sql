-- Store user infromation
create table if not exists AWSShopping.`users_information`
(
`Full name` varchar(256) not null comment 'user full name',
`email` varchar(256) not null comment 'email address of users',
`password` varchar(256) not null comment 'user acoount password' primary key
) comment 'Store user infromation';

