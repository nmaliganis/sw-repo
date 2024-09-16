create sequence public.users_id_seq
    as integer;

alter sequence public.users_id_seq owner to trackdot;
------------------------------------------------------------------
create sequence public.roles_id_seq
    as integer;

alter sequence public.roles_id_seq owner to trackdot;
------------------------------------------------------------------
create sequence public.refreshtokens_id_seq
    as integer;

alter sequence public.refreshtokens_id_seq owner to trackdot;
------------------------------------------------------------------
create sequence public.members_id_seq
    as integer;

alter sequence public.members_id_seq owner to trackdot;
------------------------------------------------------------------
create sequence public.usersroles_id_seq
    as integer;

alter sequence public.usersroles_id_seq owner to trackdot;
------------------------------------------------------------------
create table if not exists "Users"
(
    id             integer                  default nextval('users_id_seq'::regclass) not null
        constraint user_pk
            primary key,
    login          varchar(512)                                                       not null,
    password_hash  varchar(512)                                                       not null,
    is_activated   boolean                  default false                             not null,
    created_by     integer                                                            not null,
    modified_by    integer                                                            not null,
    deleted_by     integer                                                            not null,
    deleted_reason text                                                               not null,
    deleted_date   timestamp with time zone default now()                             not null,
    created_date   timestamp with time zone default now()                             not null,
    modified_date  timestamp with time zone default now()                             not null,
    active         boolean                  default true                              not null,
    reset_key      uuid,
    activation_key uuid                                                               not null,
    reset_date     timestamp with time zone default now()                             not null,
    disabled       boolean                  default true                              not null,
    is_loggedin    boolean                  default false                             not null,
    last_login     timestamp with time zone default now()                             not null,
    image_path     varchar(128)
);

alter table "Users"
    owner to trackdot;

create unique index if not exists users_id_uindex
    on "Users" (id);

create unique index if not exists users_activation_key_uindex
    on "Users" (activation_key);

create unique index if not exists users_login_uindex
    on "Users" (login);

create unique index if not exists users_reset_key_uindex
    on "Users" (reset_key);

create index if not exists users_password_hash_index
    on "Users" (password_hash);

create index if not exists users_login_index
    on "Users" (login);

create table if not exists "Roles"
(
    id             integer                  default nextval('roles_id_seq'::regclass) not null
        constraint roles_pk
            primary key,
    name           varchar(128)                                                        not null,
    created_by     integer                                                             not null,
    modified_by    integer                                                             not null,
    deleted_by     integer                                                             not null,
    deleted_reason text                                                                not null,
    deleted_date   timestamp with time zone default now()                              not null,
    created_date   timestamp with time zone default now()                              not null,
    modified_date  timestamp with time zone default now()                              not null,
    active         boolean                  default true                               not null
);

alter table "Roles"
    owner to trackdot;

create unique index if not exists roles_id_uindex
    on "Roles" (id);

create unique index if not exists roles_name_uindex
    on "Roles" (name);

create table if not exists "RefreshTokens"
(
    id             integer                  default nextval('refreshtokens_id_seq'::regclass) not null
        constraint refreshtokens_pk
            primary key,
    user_id        integer                                                                    not null
        constraint refreshtokens_users_id_fk
            references "Users"
            on update cascade on delete cascade,
    token          uuid                                                                       not null,
    expired        boolean                  default false                                     not null,
    created_by     integer                                                                    not null,
    modified_by    integer                                                                    not null,
    deleted_by     integer                                                                    not null,
    deleted_reason text                                                                       not null,
    deleted_date   timestamp with time zone default now()                                     not null,
    created_date   timestamp with time zone default now()                                     not null,
    modified_date  timestamp with time zone default now()                                     not null,
    active         boolean                  default true                                      not null
);

alter table "RefreshTokens"
    owner to trackdot;

create unique index if not exists refreshtokens_id_uindex
    on "RefreshTokens" (id);

create unique index if not exists refreshtokens_token_uindex
    on "RefreshTokens" (token);

create table if not exists "Members"
(
    id                    integer                  default nextval('members_id_seq'::regclass) not null
        constraint members_pk
            primary key,
    firstname             varchar(128)                                                         not null,
    lastname              varchar(128)                                                         not null,
    gender                integer                  default 1                                   not null,
    phone                 varchar(10),
    extphone              varchar(5),
    notes                 text,
    email                 varchar(128)                                                         not null,
    user_id               integer                                                              not null
        constraint users_users_id_fk
            references "Users"
            on update cascade on delete cascade,
    address_street        varchar(128),
    address_street_number varchar(128),
    address_postcode      varchar(8),
    address_city          varchar(64),
    address_region        varchar(64),
    mobile                varchar(10),
    extmobile             varchar(5),
    created_by            integer                                                              not null,
    modified_by           integer                                                              not null,
    deleted_by            integer                                                              not null,
    deleted_reason        text                                                                 not null,
    deleted_date          timestamp with time zone default now()                               not null,
    created_date          timestamp with time zone default now()                               not null,
    modified_date         timestamp with time zone default now()                               not null,
    active                boolean                  default true                                not null
);

alter table "Members"
    owner to trackdot;

create unique index if not exists members_id_uindex
    on "Members" (id);

create unique index if not exists members_email_uindex
    on "Members" (email);

create unique index if not exists members_user_id_uindex
    on "Members" (user_id);

create table if not exists "Companies"
(
    id             serial
        constraint companies_pk
            primary key,
    name           varchar(200)                           not null,
    description    text,
    "codeErp"      varchar(150)                           not null,
    created_date   timestamp with time zone default now() not null,
    created_by     integer                                not null,
    modified_date  timestamp with time zone default now() not null,
    modified_by    integer                                not null,
    deleted_date   timestamp with time zone default now() not null,
    deleted_reason text,
    active         boolean                  default true  not null,
    deleted_by     integer                                not null
);

alter table "Companies"
    owner to trackdot;

create unique index if not exists companies_id_uindex
    on "Companies" (id);

create table if not exists "Departments"
(
    id             serial
        constraint departments_pk
            primary key,
    company_id     integer                                not null
        constraint departments_companies_id_fk
            references "Companies"
            on update cascade on delete cascade,
    name           varchar(250)                           not null,
    notes          text,
    "codeErp"      varchar(150)                           not null,
    created_date   timestamp with time zone default now() not null,
    created_by     integer                                not null,
    modified_date  timestamp with time zone default now() not null,
    modified_by    integer                                not null,
    deleted_date   timestamp with time zone default now() not null,
    deleted_reason text,
    deleted_by     integer                                not null,
    active         boolean                  default true  not null
);

alter table "Departments"
    owner to trackdot;

create table if not exists "DepartmentsRoles"
(
    id             integer                  default nextval('usersroles_id_seq'::regclass) not null
        constraint departmentsroles_pk
            primary key,
    department_id  integer                                                                 not null
        constraint departmentsroles_department_id_fk
            references "Departments"
            on update cascade on delete cascade,
    role_id        integer                                                                 not null
        constraint departmentsroles_roles_id_fk
            references "Roles"
            on update cascade on delete cascade,
    created_by     integer                                                                 not null,
    modified_by    integer                                                                 not null,
    deleted_by     integer                                                                 not null,
    deleted_reason text                                                                    not null,
    deleted_date   timestamp with time zone default now()                                  not null,
    created_date   timestamp with time zone default now()                                  not null,
    modified_date  timestamp with time zone default now()                                  not null,
    active         boolean                  default true                                   not null
);

alter table "DepartmentsRoles"
    owner to trackdot;

create unique index if not exists departmentsroles_id_uindex
    on "DepartmentsRoles" (id);

create unique index if not exists departments_id_uindex
    on "Departments" (id);

create table if not exists "MembersDepartments"
(
    id             serial
        constraint membersdepartments_pk
            primary key,
    department_id  integer                                not null
        constraint membersdepartments_departments_id_fk
            references "Departments"
            on update cascade on delete cascade,
    member_id      integer                                not null
        constraint membersdepartments_members_id_fk
            references "Members"
            on update cascade on delete cascade,
    notes          text,
    "codeErp"      varchar(150)                           not null,
    created_date   timestamp with time zone default now() not null,
    created_by     integer                                not null,
    modified_date  timestamp with time zone default now() not null,
    modified_by    integer                                not null,
    deleted_date   timestamp with time zone default now() not null,
    deleted_reason text,
    deleted_by     integer                                not null,
    active         boolean                  default true  not null
);

alter table "MembersDepartments"
    owner to trackdot;

create unique index if not exists membersdepartments_id_uindex
    on "MembersDepartments" (id);

