create table if not exists "LocalizationDomain"
(
    id             serial
        constraint localizationdomain_pk
            primary key,
    name           varchar(500)                           not null,
    active         boolean                  default true  not null,
    created_date   timestamp with time zone default now() not null,
    created_by     integer                  default 1     not null,
    modified_date  timestamp with time zone default now() not null,
    modified_by    integer                  default 0     not null,
    deleted_date   timestamp with time zone default now() not null,
    deleted_by     integer                  default 0     not null,
    deleted_reason text                                   not null
);

alter table "LocalizationDomain"
    owner to trackdot;

create unique index if not exists localizationdomain_id_uindex
    on "LocalizationDomain" (id);

create unique index if not exists localizationdomain_name_uindex
    on "LocalizationDomain" (name);

create table if not exists "LocalizationLanguage"
(
    id             serial
        constraint localizationlanguage_pk
            primary key,
    name           varchar(500)                                                                         not null,
    "default"      boolean                  default true                                                not null,
    active         boolean                  default true                                                not null,
    created_date   timestamp with time zone default now()                                               not null,
    created_by     integer                  default 1                                                   not null,
    modified_date  timestamp with time zone default now()                                               not null,
    modified_by    integer                  default 0                                                   not null,
    deleted_date   timestamp with time zone default now()                                               not null,
    deleted_by     integer                  default 0                                                   not null,
    deleted_reason text
);

alter table "LocalizationLanguage"
    owner to trackdot;

create table if not exists "LocalizationValue"
(
    id             serial
        constraint localizationvalues_pk
            primary key,
    "domainId"     integer                                                                           not null
        constraint localizationvalue_localizationdomain_id_fk
            references "LocalizationDomain"
            on update cascade on delete cascade,
    "languageId"   integer                                                                           not null
        constraint localizationvalue_localizationlanguage_id_fk
            references "LocalizationLanguage"
            on update cascade on delete cascade,
    key            varchar(500)                                                                      not null,
    value          varchar(500)                                                                      not null,
    active         boolean                  default true                                             not null,
    created_date   timestamp with time zone default now()                                            not null,
    created_by     integer                  default 1                                                not null,
    modified_date  timestamp with time zone default now()                                            not null,
    modified_by    integer                  default 0                                                not null,
    deleted_date   timestamp with time zone default now()                                            not null,
    deleted_by     integer                  default 0                                                not null,
    deleted_reason text
);

alter table "LocalizationValue"
    owner to trackdot;

create unique index if not exists localizationvalues_id_uindex
    on "LocalizationValue" (id);

create unique index if not exists localizationvalue_key_uindex
    on "LocalizationValue" (key);

create unique index if not exists localizationvalue_value_uindex
    on "LocalizationValue" (value);

create unique index if not exists localizationlanguage_id_uindex
    on "LocalizationLanguage" (id);

create unique index if not exists localizationlanguage_name_uindex
    on "LocalizationLanguage" (name);

create table if not exists "Company"
(
    id             serial
        constraint company_pk
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

alter table "Company"
    owner to trackdot;

create unique index if not exists company_id_uindex
    on "Company" (id);

create unique index if not exists company_name_uindex
    on "Company" (name);

create table if not exists "Department"
(
    id             serial
        constraint "department _pk"
            primary key,
    name           varchar(250)                                                               not null,
    notes          text,
    "companyId"    integer                                                                    not null
        constraint "department _company_id_fk"
            references "Company"
            on update cascade on delete cascade,
    "codeErp"      varchar(150)                                                               not null,
    created_date   timestamp with time zone default now()                                     not null,
    created_by     integer                                                                    not null,
    modified_date  timestamp with time zone default now()                                     not null,
    modified_by    integer                                                                    not null,
    deleted_date   timestamp with time zone default now()                                     not null,
    deleted_reason text,
    active         boolean                  default true                                      not null,
    deleted_by     integer                                                                    not null
);

alter table "Department"
    owner to trackdot;

create unique index if not exists "department _id_uindex"
    on "Department" (id);

create table if not exists spatial_ref_sys
(
    srid      integer not null
        primary key
        constraint spatial_ref_sys_srid_check
            check ((srid > 0) AND (srid <= 998999)),
    auth_name varchar(256),
    auth_srid integer,
    srtext    varchar(2048),
    proj4text varchar(2048)
);

alter table spatial_ref_sys
    owner to trackdot;

grant select on spatial_ref_sys to public;

create table if not exists us_lex
(
    id        serial
        constraint pk_us_lex
            primary key,
    seq       integer,
    word      text,
    stdword   text,
    token     integer,
    is_custom boolean default true not null
);

alter table us_lex
    owner to trackdot;

create table if not exists us_gaz
(
    id        serial
        constraint pk_us_gaz
            primary key,
    seq       integer,
    word      text,
    stdword   text,
    token     integer,
    is_custom boolean default true not null
);

alter table us_gaz
    owner to trackdot;

create table if not exists us_rules
(
    id        serial
        constraint pk_us_rules
            primary key,
    rule      text,
    is_custom boolean default true not null
);

alter table us_rules
    owner to trackdot;

create table if not exists "Person"
(
    id             serial
        constraint person_pk
            primary key,
    firstname      varchar(250)                                                        not null,
    lastname       varchar(250)                                                        not null,
    gender         integer                  default 1                                  not null,
    phone          varchar(10),
    extphone       varchar(5),
    notes          text,
    email          varchar(128)                                                        not null,
    mobile         varchar(10),
    extmobile      varchar(5),
    status         integer                  default 1                                  not null,
    created_date   timestamp with time zone default now()                              not null,
    created_by     integer                  default 1                                  not null,
    modified_date  timestamp with time zone default now()                              not null,
    modified_by    integer                  default 0                                  not null,
    deleted_date   timestamp with time zone default now()                              not null,
    deleted_reason text,
    active         boolean                  default true                               not null,
    deleted_by     integer                  default 0                                  not null,
    username       varchar(128)                                                        not null
);

alter table "Person"
    owner to trackdot;

create unique index if not exists person_id_uindex
    on "Person" (id);

create unique index if not exists person_email_uindex
    on "Person" (email);

create unique index if not exists person_username_uindex
    on "Person" (username);

create table if not exists "DepartmentPersonRole"
(
    id             serial
        constraint personrole_pk
            primary key,
    name           varchar(250)                                                              not null,
    notes          text,
    "departmentId" integer
        constraint "personrole_department _id_fk"
            references "Department"
            on update cascade on delete cascade,
    "codeErp"      varchar(150)                                                              not null,
    created_date   timestamp with time zone default now()                                    not null,
    created_by     integer                  default 1                                        not null,
    modified_date  timestamp with time zone default now()                                    not null,
    modified_by    integer                  default 0                                        not null,
    deleted_date   timestamp with time zone default now()                                    not null,
    deleted_reason text,
    active         boolean                  default true                                     not null,
    deleted_by     integer                  default 0                                        not null,
    "personId"     integer                                                                   not null
        constraint departmentpersonrole_person_id_fk
            references "Person"
            on update cascade on delete cascade
);

alter table "DepartmentPersonRole"
    owner to trackdot;

create unique index if not exists personrole_id_uindex
    on "DepartmentPersonRole" (id);

create table if not exists "AssetCategory"
(
    id             serial
        constraint assetcategory_pk
            primary key,
    name           varchar(250)                           not null,
    "codeErp"      varchar(150)                           not null,
    params         jsonb                                  not null,
    created_date   timestamp with time zone default now() not null,
    created_by     integer                                not null,
    modified_date  timestamp with time zone default now() not null,
    modified_by    integer                                not null,
    deleted_date   timestamp with time zone default now() not null,
    deleted_reason text,
    active         boolean                  default true  not null,
    deleted_by     integer                                not null
);

alter table "AssetCategory"
    owner to trackdot;

create table if not exists "Asset"
(
    id               serial
        constraint asset_pk
            primary key,
    "companyId"      integer                                not null
        constraint asset_company_id_fk
            references "Company"
            on update cascade on delete cascade,
    name             varchar(250)                           not null,
    image            varchar(250),
    codeerp          varchar(150)                           not null,
    description      text,
    assetcategory_id integer                                not null
        constraint asset_assetcategory_id_fk
            references "AssetCategory"
            on update cascade on delete cascade,
    created_date     timestamp with time zone default now() not null,
    created_by       integer                                not null,
    modified_date    timestamp with time zone default now() not null,
    modified_by      integer                                not null,
    deleted_date     timestamp with time zone default now() not null,
    deleted_reason   text,
    active           boolean                  default true  not null,
    deleted_by       integer                                not null
);

alter table "Asset"
    owner to trackdot;

create unique index if not exists asset_id_uindex
    on "Asset" (id);

create table if not exists "Container"
(
    id                    serial
        constraint container_pk
            primary key
        constraint container_asset_id_fk
            references "Asset"
            on update cascade on delete cascade,
    isvisible             boolean                  default true  not null,
    level                 integer                  default 0     not null,
    point                 geometry,
    timetofull            double precision         default 0.0   not null,
    lastserviceddate      timestamp with time zone default now() not null,
    status                integer                  default 1     not null,
    mandatorypickupdate   timestamp with time zone default now() not null,
    mandatorypickupactive boolean                  default false not null,
    capacity              integer                  default 0,
    wastetype             integer                  default 1,
    material              integer                  default 1,
    created_date          timestamp with time zone default now() not null,
    created_by            integer                  default 1     not null,
    modified_date         timestamp with time zone default now() not null,
    modified_by           integer                  default 0     not null,
    deleted_date          timestamp with time zone default now() not null,
    deleted_reason        text,
    active                boolean                  default true  not null,
    deleted_by            integer                  default 0     not null,
    binstatus             integer                  default 0     not null
);

alter table "Container"
    owner to trackdot;

create unique index if not exists container_id_uindex
    on "Container" (id);

create table if not exists "Vehicle"
(
    id              serial
        constraint vehicle_pk
            primary key
        constraint vehicle_asset_id_fk
            references "Asset"
            on update cascade on delete cascade,
    numplate        varchar(16)                            not null,
    brand           varchar(32)                            not null,
    registereddate  timestamp with time zone default now() not null,
    type            integer                  default 1     not null,
    status          integer                  default 1     not null,
    gas             integer                  default 1     not null,
    height          double precision         default 0.0,
    width           double precision         default 0.0,
    axels           double precision         default 0.0,
    min_turn_radius double precision         default 0.0,
    length          double precision         default 0.0,
    created_date    timestamp with time zone default now() not null,
    created_by      integer                  default 1     not null,
    modified_date   timestamp with time zone default now() not null,
    modified_by     integer                  default 0     not null,
    deleted_date    timestamp with time zone default now() not null,
    deleted_reason  text,
    active          boolean                  default true  not null,
    deleted_by      integer                  default 0     not null
);

alter table "Vehicle"
    owner to trackdot;

create unique index if not exists vehicle_id_uindex
    on "Vehicle" (id);

create unique index if not exists assetcategory_id_uindex
    on "AssetCategory" (id);

create unique index if not exists assetcategory_name_uindex
    on "AssetCategory" (name);

create table if not exists "DeviceModel"
(
    id             serial
        constraint devicemodel_pk
            primary key,
    name           varchar(250)                           not null,
    codename       varchar(250)                           not null,
    enabled        boolean                  default true  not null,
    "codeErp"      varchar(150)                           not null,
    created_date   timestamp with time zone default now() not null,
    created_by     integer                  default 1     not null,
    modified_date  timestamp with time zone default now() not null,
    modified_by    integer                  default 0     not null,
    deleted_date   timestamp with time zone default now() not null,
    deleted_reason text,
    active         boolean                  default true  not null,
    deleted_by     integer                  default 0     not null
);

alter table "DeviceModel"
    owner to trackdot;

create table if not exists "Device"
(
    id               serial
        constraint device_pk
            primary key,
    "deviceModelId"  integer                                not null
        constraint device_devicemodel_id_fk
            references "DeviceModel"
            on update cascade on delete cascade,
    imei             varchar(250)                           not null,
    serialnumber     varchar(250)                           not null,
    activationcode   varchar(100)                           not null,
    activationdate   timestamp with time zone default now() not null,
    activationby     integer                                not null,
    provisioningcode varchar(100)                           not null,
    provisioningby   integer                                not null,
    provisioningdate timestamp with time zone default now() not null,
    resetcode        varchar(100)                           not null,
    resetby          integer                                not null,
    resetdate        timestamp with time zone default now() not null,
    activated        boolean                  default false not null,
    enabled          boolean                  default false not null,
    ipaddress        inet                                   not null,
    lastrecordeddate timestamp with time zone default now() not null,
    lastreceiveddate timestamp with time zone default now() not null,
    "codeErp"        varchar(150),
    created_date     timestamp with time zone default now() not null,
    created_by       integer                  default 1     not null,
    modified_date    timestamp with time zone default now() not null,
    modified_by      integer                  default 0     not null,
    deleted_date     timestamp with time zone default now() not null,
    deleted_reason   text,
    active           boolean                  default true  not null,
    deleted_by       integer                  default 0     not null
);

alter table "Device"
    owner to trackdot;

create unique index if not exists device_id_uindex
    on "Device" (id);

create unique index if not exists device_ipaddress_uindex
    on "Device" (ipaddress);

create unique index if not exists devicemodel_id_uindex
    on "DeviceModel" (id);

create table if not exists "SensorType"
(
    id                  serial
        constraint sensortype_pk
            primary key,
    name                varchar(100)                           not null,
    showatstatus        boolean                  default false not null,
    statusexpiryminutes integer                  default 0     not null,
    showonmap           boolean                  default false not null,
    showatreport        boolean                  default false not null,
    showatchart         boolean                  default false not null,
    resetvalues         boolean                  default false not null,
    sumvalues           boolean                  default false not null,
    precision           integer                  default 0     not null,
    tunit               varchar(100),
    calcposition        boolean                  default false not null,
    "codeErp"           varchar(150)                           not null,
    created_date        timestamp with time zone default now() not null,
    created_by          integer                  default 1     not null,
    modified_date       timestamp with time zone default now() not null,
    modified_by         integer                  default 0     not null,
    deleted_date        timestamp with time zone default now() not null,
    deleted_reason      text,
    active              boolean                  default true  not null,
    deleted_by          integer                  default 0     not null,
    sensortypeindex     integer                  default 0     not null
);

alter table "SensorType"
    owner to trackdot;

create table if not exists "Sensor"
(
    id                  serial
        constraint sensor_pk
            primary key,
    "assetId"           integer
        constraint sensor_asset_id_fk
            references "Asset"
            on update cascade on delete cascade,
    "sensorTypeId"      integer                                not null
        constraint sensor_sensortype_id_fk
            references "SensorType"
            on update cascade on delete cascade,
    params              jsonb,
    name                varchar(250)                           not null,
    "codeErp"           varchar(150)                           not null,
    "isActive"          boolean                  default true  not null,
    "isVisible"         boolean                  default true  not null,
    "order"             integer                  default 1     not null,
    "minValue"          double precision         default 0.0   not null,
    "maxValue"          double precision         default 0.0,
    "minNotifyValue"    double precision         default 0.0,
    "maxNotifyValue"    double precision         default 0.0,
    "lastValue"         double precision         default 0.0,
    "lastRecordedDate"  timestamp with time zone default now() not null,
    "lastReceivedDate"  timestamp with time zone default now() not null,
    "highThreshold"     double precision         default 0.0,
    "lowThreshold"      double precision         default 0.0,
    "samplingInterval"  integer                  default 15,
    "reportingInterval" integer                  default 60,
    created_date        timestamp with time zone default now() not null,
    created_by          integer                  default 1     not null,
    modified_date       timestamp with time zone default now() not null,
    modified_by         integer                  default 0     not null,
    deleted_date        timestamp with time zone default now() not null,
    deleted_reason      text,
    active              boolean                  default true  not null,
    deleted_by          integer                  default 0     not null,
    "deviceId"          integer
        constraint sensor_device_id_fk
            references "Device"
            on update cascade on delete cascade
);

alter table "Sensor"
    owner to trackdot;

create unique index if not exists sensor_id_uindex
    on "Sensor" (id);

create unique index if not exists sensortype_id_uindex
    on "SensorType" (id);

create unique index if not exists sensortype_sensortypeindex_uindex
    on "SensorType" (sensortypeindex);

create unique index if not exists sensortype_name_uindex
    on "SensorType" (name);

create table if not exists "Calibration"
(
    id             serial
        constraint calibration_pk
            primary key,
    "deviceId"     integer                                not null
        constraint calibration_device_id_fk
            references "Device"
            on update cascade on delete cascade,
    recorded       timestamp with time zone default now() not null,
    received       timestamp with time zone default now() not null,
    valuejson      jsonb                                  not null,
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

alter table "Calibration"
    owner to trackdot;

create unique index if not exists calibration_id_uindex
    on "Calibration" (id);

create table if not exists "SimCard"
(
    id             serial
        constraint simcard_pk
            primary key,
    "deviceId"     integer
        constraint simcard_device_id_fk
            references "Device"
            on update cascade on delete cascade,
    iccid          varchar(150)                           not null,
    imsi           varchar(150)                           not null,
    country_iso    varchar(150)                           not null,
    number         varchar(150)                           not null,
    purchase_date  timestamp with time zone default now() not null,
    simcardtype    integer                  default 1     not null,
    simnetworktype integer                  default 1     not null,
    enable         boolean                  default true  not null,
    "codeErp"      varchar(150),
    created_date   timestamp with time zone default now() not null,
    created_by     integer                  default 1     not null,
    modified_date  timestamp with time zone default now() not null,
    modified_by    integer                  default 0     not null,
    deleted_date   timestamp with time zone default now() not null,
    deleted_reason text,
    active         boolean                  default true  not null,
    deleted_by     integer                  default 0     not null
);

alter table "SimCard"
    owner to trackdot;

create unique index if not exists simcard_id_uindex
    on "SimCard" (id);

create unique index if not exists simcard_number_uindex
    on "SimCard" (number);

create unique index if not exists simcard_deviceid_uindex
    on "SimCard" ("deviceId");

create table if not exists "Firmware"
(
    id             serial
        constraint firmware_pk
            primary key,
    code           varchar(100)                           not null,
    data           varchar(500)                           not null,
    valuejson      jsonb,
    created_date   timestamp with time zone default now() not null,
    created_by     integer                  default 1     not null,
    modified_date  timestamp with time zone default now() not null,
    modified_by    integer                  default 0     not null,
    deleted_date   timestamp with time zone default now() not null,
    deleted_reason text,
    active         boolean                  default true  not null,
    deleted_by     integer                  default 0     not null
);

alter table "Firmware"
    owner to trackdot;

create unique index if not exists firmware_id_uindex
    on "Firmware" (id);

create unique index if not exists firmware_code_uindex
    on "Firmware" (code);

create table if not exists "DeviceFirmware"
(
    id             serial
        constraint devicefirmware_pk
            primary key,
    "firmwareId"   integer                                                                     not null
        constraint devicefirmware_firmware_id_fk
            references "Firmware"
            on update cascade on delete cascade,
    "deviceId"     integer                                                                     not null
        constraint devicefirmware_device_id_fk
            references "Device"
            on update cascade on delete cascade,
    requested      timestamp with time zone default now()                                      not null,
    executed       timestamp with time zone default now()                                      not null,
    status         integer                  default 1                                          not null,
    created_date   timestamp with time zone default now()                                      not null,
    created_by     integer                  default 1                                          not null,
    modified_date  timestamp with time zone default now()                                      not null,
    modified_by    integer                  default 0                                          not null,
    deleted_date   timestamp with time zone default now()                                      not null,
    deleted_reason text,
    active         boolean                  default true                                       not null,
    deleted_by     integer                  default 0                                          not null
);

alter table "DeviceFirmware"
    owner to trackdot;

create unique index if not exists devicefirmware_id_uindex
    on "DeviceFirmware" (id);

create table if not exists "ConnectionLog"
(
    id             serial
        constraint connectionlog_pk
            primary key,
    "deviceId"     integer                                                                    not null
        constraint connectionlog_device_id_fk
            references "Device"
            on update cascade on delete cascade,
    established    timestamp with time zone default now()                                     not null,
    created_date   timestamp with time zone default now()                                     not null,
    created_by     integer                                                                    not null,
    modified_date  timestamp with time zone default now()                                     not null,
    modified_by    integer                                                                    not null,
    deleted_date   timestamp with time zone default now()                                     not null,
    deleted_reason text,
    active         boolean                  default true                                      not null,
    deleted_by     integer                                                                    not null
);

alter table "ConnectionLog"
    owner to trackdot;

create unique index if not exists connectionlog_id_uindex
    on "ConnectionLog" (id);

create table if not exists "LandmarkCategory"
(
    id             serial
        constraint landmarkcategory_pk
            primary key,
    name           varchar(100)                           not null,
    description    text,
    codeerp        varchar(100)                           not null,
    params         jsonb                                  not null,
    created_date   timestamp with time zone default now() not null,
    created_by     integer                  default 1     not null,
    modified_date  timestamp with time zone default now() not null,
    modified_by    integer                  default 0     not null,
    deleted_date   timestamp with time zone default now() not null,
    deleted_reason text,
    active         boolean                  default true  not null,
    deleted_by     integer                  default 0     not null
);

alter table "LandmarkCategory"
    owner to trackdot;

create table if not exists "Landmark"
(
    id                   serial
        constraint landmark_pk
            primary key,
    name                 varchar(500)                           not null,
    description          text,
    codeerp              varchar(500)                           not null,
    street               varchar(300),
    number               varchar(100),
    crossstreet          varchar(100),
    city                 varchar(100),
    prefecture           varchar(100),
    country              varchar(100),
    zipcode              varchar(100),
    phonenumber          varchar(100),
    phonenumber2         varchar(100),
    email                varchar(300),
    fax                  varchar(100),
    url                  varchar(100),
    personincharge       varchar(100),
    vat                  varchar(100),
    image                varchar(500),
    isbase               boolean                  default true  not null,
    excludefromspace     boolean                  default true  not null,
    hasspacepriority     boolean                  default true  not null,
    speedlimit           integer                  default 0     not null,
    expired              timestamp with time zone default now() not null,
    "rootId"             integer                                not null
        constraint landmark_landmark_id_fk
            references "Landmark"
            on update cascade on delete cascade,
    "parentId"           integer                                not null
        constraint landmark_landmark_id_fk_2
            references "Landmark"
            on update cascade on delete cascade,
    "landmarkCategoryId" integer                                not null
        constraint landmark_landmarkcategory_id_fk
            references "LandmarkCategory"
            on update cascade on delete cascade,
    "codeErp"            varchar(150)                           not null,
    created_date         timestamp with time zone default now() not null,
    created_by           integer                  default 1     not null,
    modified_date        timestamp with time zone default now() not null,
    modified_by          integer                  default 0     not null,
    deleted_date         timestamp with time zone default now() not null,
    deleted_reason       text,
    active               boolean                  default true  not null,
    deleted_by           integer                  default 0     not null
);

alter table "Landmark"
    owner to trackdot;

create unique index if not exists landmark_id_uindex
    on "Landmark" (id);

create unique index if not exists landmarkcategory_id_uindex
    on "LandmarkCategory" (id);

create unique index if not exists landmarkcategory_name_uindex
    on "LandmarkCategory" (name);

create table if not exists "DepartmentLandmark"
(
    id             serial
        constraint departmentlandmark_pk
            primary key,
    "departmentId" integer                                not null
        constraint departmentlandmark_department_id_fk
            references "Department"
            on update cascade on delete cascade,
    "landmarkId"   integer                                not null
        constraint departmentlandmark_landmark_id_fk
            references "Landmark"
            on update cascade on delete cascade,
    created_date   timestamp with time zone default now() not null,
    created_by     integer                  default 1     not null,
    modified_date  timestamp with time zone default now() not null,
    modified_by    integer                  default 0     not null,
    deleted_date   timestamp with time zone default now() not null,
    deleted_reason text,
    active         boolean                  default true  not null,
    deleted_by     integer                  default 0     not null
);

alter table "DepartmentLandmark"
    owner to trackdot;

create unique index if not exists departmentlandmark_id_uindex
    on "DepartmentLandmark" (id);

create table if not exists "GeocoderProfile"
(
    id             serial
        constraint geocoderprofile_pk
            primary key,
    name           varchar(100)                           not null,
    "sourceName"   varchar(100)                           not null,
    params         jsonb                                  not null,
    created_date   timestamp with time zone default now() not null,
    created_by     integer                  default 1     not null,
    modified_date  timestamp with time zone default now() not null,
    modified_by    integer                  default 0     not null,
    deleted_date   timestamp with time zone default now() not null,
    deleted_reason text,
    active         boolean                  default true  not null,
    deleted_by     integer                  default 0     not null
);

alter table "GeocoderProfile"
    owner to trackdot;

create table if not exists "GeocodedPosition"
(
    id                  serial
        constraint geocodedposition_pk
            primary key,
    position            geometry                               not null,
    street              varchar(300),
    number              varchar(100),
    crossstreet         varchar(100),
    city                varchar(100),
    prefecture          varchar(100),
    country             varchar(100),
    zipcode             varchar(100),
    "geocoderProfileId" integer                                not null
        constraint geocodedposition_geocoderprofile_id_fk
            references "GeocoderProfile"
            on update cascade on delete cascade,
    "lastGeocoded"      timestamp with time zone default now() not null
);

alter table "GeocodedPosition"
    owner to trackdot;

create unique index if not exists geocodedposition_id_uindex
    on "GeocodedPosition" (id);

create table if not exists "EventPosition"
(
    id                   serial
        constraint eventposition_pk
            primary key,
    "geocodedPositionId" integer                                not null
        constraint eventposition_geocodedposition_id_fk
            references "GeocodedPosition"
            on update cascade on delete cascade,
    position             geometry                               not null,
    created_date         timestamp with time zone default now() not null,
    created_by           integer                  default 1     not null,
    modified_date        timestamp with time zone default now() not null,
    modified_by          integer                  default 0     not null,
    deleted_date         timestamp with time zone default now() not null,
    deleted_reason       text,
    active               boolean                  default true  not null,
    deleted_by           integer                  default 0     not null
);

alter table "EventPosition"
    owner to trackdot;

create table if not exists "EventHistory"
(
    id                integer                  default nextval('"Event_id_seq"'::regclass) not null,
    "sensorId"        integer                                                              not null
        constraint event_sensor_id_fk
            references "Sensor",
    recorded          timestamp with time zone default now()                               not null,
    received          timestamp with time zone default now()                               not null,
    eventvalue        double precision         default 0.0,
    eventvaluejson    jsonb                                                                not null,
    "eventPositionId" integer
        constraint eventhistory_eventposition_id_fk
            references "EventPosition"
            on update cascade on delete cascade
);

alter table "EventHistory"
    owner to trackdot;

create unique index if not exists event_id_uindex
    on "EventHistory" (id);

create unique index if not exists event_sensorid_uindex
    on "EventHistory" ("sensorId");

create trigger ts_insert_blocker
    before insert
    on "EventHistory"
    for each row
execute procedure _timescaledb_internal.insert_blocker();

create unique index if not exists eventposition_id_uindex
    on "EventPosition" (id);

create unique index if not exists geocoderprofile_id_uindex
    on "GeocoderProfile" (id);

create unique index if not exists geocoderprofile_name_uindex
    on "GeocoderProfile" (name);

create table if not exists "Role"
(
    id             serial
        constraint roles_pk
            primary key,
    name           varchar(128)                                                         not null,
    codeerp        varchar(100),
    created_date   timestamp with time zone default now()                               not null,
    created_by     integer                                                              not null,
    modified_date  timestamp with time zone default now()                               not null,
    modified_by    integer                                                              not null,
    deleted_date   timestamp with time zone default now()                               not null,
    deleted_reason text,
    active         boolean                  default true                                not null,
    deleted_by     integer                                                              not null
);

alter table "Role"
    owner to trackdot;

create table if not exists "RoleSensorType"
(
    id             serial
        constraint rolesensortype_pk
            primary key,
    "roleId"       integer                                not null
        constraint rolesensortype_role_id_fk
            references "Role"
            on update cascade on delete cascade,
    "sensorTypeId" integer                                not null
        constraint rolesensortype_sensortype_id_fk
            references "SensorType"
            on update cascade on delete cascade,
    showatstatus   boolean                  default true  not null,
    statusexpiry   integer                  default 0     not null,
    showonmap      boolean                  default true  not null,
    showatreport   boolean                  default true  not null,
    showatchart    boolean                  default true  not null,
    resetvalues    boolean                  default true  not null,
    sumvalues      boolean                  default true  not null,
    precision      integer                  default 0     not null,
    unit           varchar(100)                           not null,
    calcposition   boolean                  default true  not null,
    created_date   timestamp with time zone default now() not null,
    created_by     integer                  default 1     not null,
    modified_date  timestamp with time zone default now() not null,
    modified_by    integer                  default 0     not null,
    deleted_date   timestamp with time zone default now() not null,
    deleted_reason text,
    active         boolean                  default true  not null,
    deleted_by     integer                  default 0     not null
);

alter table "RoleSensorType"
    owner to trackdot;

create unique index if not exists rolesensortype_id_uindex
    on "RoleSensorType" (id);

create table if not exists "RoleAssetCategory"
(
    id                serial
        constraint roleassetcategory_pk
            primary key,
    "roleId"          integer                                not null
        constraint roleassetcategory_role_id_fk
            references "Role"
            on update cascade on delete cascade,
    "assetCategoryId" integer                                not null
        constraint roleassetcategory_assetcategory_id_fk
            references "AssetCategory"
            on update cascade on delete cascade,
    created_date      timestamp with time zone default now() not null,
    created_by        integer                  default 1     not null,
    modified_date     timestamp with time zone default now() not null,
    modified_by       integer                  default 0     not null,
    deleted_date      timestamp with time zone default now() not null,
    deleted_reason    text,
    active            boolean                  default true  not null,
    deleted_by        integer                  default 0     not null
);

alter table "RoleAssetCategory"
    owner to trackdot;

create unique index if not exists roleassetcategory_id_uindex
    on "RoleAssetCategory" (id);

create table if not exists "RoleLandmarkCategory"
(
    id                   serial
        constraint rolelandmarkcategory_pk
            primary key,
    "roleId"             integer                                not null
        constraint rolelandmarkcategory_role_id_fk
            references "Role",
    "landmarkCategoryId" integer                                not null
        constraint rolelandmarkcategory_landmarkcategory_id_fk
            references "LandmarkCategory"
            on update cascade on delete cascade,
    created_date         timestamp with time zone default now() not null,
    created_by           integer                  default 1     not null,
    modified_date        timestamp with time zone default now() not null,
    modified_by          integer                  default 0     not null,
    deleted_date         timestamp with time zone default now() not null,
    deleted_reason       text,
    active               boolean                  default true  not null,
    deleted_by           integer                  default 0     not null
);

alter table "RoleLandmarkCategory"
    owner to trackdot;

create unique index if not exists rolelandmarkcategory_id_uindex
    on "RoleLandmarkCategory" (id);

create unique index if not exists roles_codeerp_uindex
    on "Role" (codeerp);

create unique index if not exists roles_id_uindex
    on "Role" (id);

create unique index if not exists roles_name_uindex
    on "Role" (name);

