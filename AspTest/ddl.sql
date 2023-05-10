create table user_state
(
    id          serial       not null,
    code        varchar(32)  not null,
    description varchar(256) null,
    constraint user_state_id primary key (id),
    constraint user_state_code_uq unique (code)
);

create table user_group
(
    id          serial       not null,
    code        varchar(32)  not null,
    description varchar(256) null,
    constraint user_group_id primary key (id),
    constraint user_group_code_uq unique (code)
);

create table "user"
(
    id            uuid                     not null,
    login         varchar(32)              not null,
    password      varchar(128)             not null,
    created_date  timestamp with time zone not null,
    user_group_id int                      not null,
    user_state_id int                      not null,
    constraint user_id primary key (id),
    constraint user_login_uq unique (login),
    constraint user_user_state_user_state_id foreign key (user_state_id)
        references user_state (id),
    constraint user_user_group_user_group_id foreign key (user_group_id)
        references user_group (id)
)