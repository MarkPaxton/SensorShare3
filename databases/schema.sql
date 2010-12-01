CREATE TABLE [log] (
    [time] datetime NOT NULL ON CONFLICT REPLACE DEFAULT (datetime('now')),
    [message] nvarchar(511) NOT NULL
);
CREATE TABLE [servers] (
    [server_id] guid PRIMARY KEY NOT NULL,
    [name] nvarchar(50) NOT NULL,
    [description] nvarchar(255) NOT NULL,
    [location] nvarchar(127) NOT NULL,
    [image] image,
    [sensor_1_name] nvarchar(50) NOT NULL ON CONFLICT REPLACE DEFAULT "",
    [sensor_2_name] nvarchar(50) NOT NULL ON CONFLICT REPLACE DEFAULT "",
    [sensor_3_name] nvarchar(50) NOT NULL ON CONFLICT REPLACE DEFAULT "",
    [sensor_4_name] nvarchar(50) NOT NULL ON CONFLICT REPLACE DEFAULT "",
    [sensor_1_unit] nvarchar(50) NOT NULL ON CONFLICT REPLACE DEFAULT "",
    [sensor_2_unit] nvarchar(50) NOT NULL ON CONFLICT REPLACE DEFAULT "",
    [sensor_3_unit] nvarchar(50) NOT NULL ON CONFLICT REPLACE DEFAULT "",
    [sensor_4_unit] nvarchar(50) NOT NULL ON CONFLICT REPLACE DEFAULT "",
    [sensor_1_range] integer NOT NULL ON CONFLICT REPLACE DEFAULT 0,
    [sensor_2_range] integer NOT NULL ON CONFLICT REPLACE DEFAULT 0,
    [sensor_3_range] integer NOT NULL ON CONFLICT REPLACE DEFAULT 0,
    [sensor_4_range] integer NOT NULL ON CONFLICT REPLACE DEFAULT 0,
    [sensor_1_id] integer NOT NULL ON CONFLICT REPLACE DEFAULT 0,
    [sensor_2_id] integer NOT NULL ON CONFLICT REPLACE DEFAULT 0,
    [sensor_3_id] integer NOT NULL ON CONFLICT REPLACE DEFAULT 0,
    [sensor_4_id] integer NOT NULL ON CONFLICT REPLACE DEFAULT 0
);
CREATE TABLE [readings] (
    [server_id] guid NOT NULL,
    [reading_1] real,
    [reading_2] real,
    [reading_3] real,
    [reading_4] real,
    [reading_time] datetime NOT NULL ON CONFLICT REPLACE DEFAULT (datetime('now'))
);