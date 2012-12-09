CREATE DATABASE storage_services
GO

USE storage_services;
GO


CREATE TABLE users (
    id INT IDENTITY NOT NULL,
    version BIGINT default 0  NOT NULL,
    name NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL,
    password NVARCHAR(200) NOT NULL,
    locale NVARCHAR(10) DEFAULT 'en'  NOT NULL,
    timezone INT DEFAULT 0  NOT NULL,
    role INT default 0  NOT NULL,
    PRIMARY KEY (id),
    UNIQUE (email)
)
GO

CREATE TABLE storage_plugins (
    id INT IDENTITY NOT NULL,
    version BIGINT DEFAULT 0  NOT NULL,
    classpath NVARCHAR(100) NOT NULL,
    status INT DEFAULT 0  NOT NULL,
    PRIMARY KEY (id),
    UNIQUE (classpath)
)
GO


CREATE TABLE sessions (
    id INT IDENTITY NOT NULL,
    session_token NVARCHAR(32) NOT NULL,
    expires BIGINT NOT NULL,
    user_id INT NOT NULL,
    PRIMARY KEY (id),
    UNIQUE (session_token),
	CONSTRAINT fk_sessions_users
        FOREIGN KEY (user_id)
        REFERENCES users
		ON DELETE CASCADE
		ON UPDATE CASCADE
)
GO


CREATE TABLE reset_password_requests (
    id INT IDENTITY NOT NULL,
    token NVARCHAR(100) NOT NULL,
    expires BIGINT NOT NULL,
    user_id INT NOT NULL,
    PRIMARY KEY (id),
	UNIQUE(token),
	CONSTRAINT fk_resetpasswd_users
        FOREIGN KEY (user_id)
        REFERENCES users
		ON DELETE CASCADE
		ON UPDATE CASCADE
)
GO


CREATE TABLE storage_accounts (
    id INT IDENTITY NOT NULL,
    version BIGINT default 0  NOT NULL,
    account_name NVARCHAR(100) NOT NULL,
    user_id INT NOT NULL,
    storage_plugin_id INT NOT NULL,
    PRIMARY KEY (id),
    UNIQUE (account_name, user_id, storage_plugin_id),
	CONSTRAINT fk_saccounts_splugins
        FOREIGN KEY (storage_plugin_id)
        REFERENCES storage_plugins
		ON DELETE NO ACTION
		ON UPDATE CASCADE,
	CONSTRAINT fk_saccounts_users
        FOREIGN KEY (user_id)
        REFERENCES users
		ON DELETE CASCADE
		ON UPDATE CASCADE
)
GO

CREATE TABLE storage_accounts_settings (
    id INT IDENTITY NOT NULL,
    setting_name NVARCHAR(45) NOT NULL,
    setting_value NVARCHAR(300) NOT NULL,
    storage_account_id INT NOT NULL,
    PRIMARY KEY (id),
    UNIQUE (setting_name, storage_account_id),
	CONSTRAINT fk_saccounts_saccountssettings
        FOREIGN KEY (storage_account_id)
        REFERENCES storage_accounts
		ON DELETE CASCADE
		ON UPDATE CASCADE
)
GO
