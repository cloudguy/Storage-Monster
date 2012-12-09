CREATE DATABASE storage_services  WITH ENCODING='UTF8' CONNECTION LIMIT=-1;
  
CREATE TABLE users
(
   id serial NOT NULL, 
   name character varying(100) NOT NULL, 
   email character varying(100) NOT NULL, 
   password character varying(200) NOT NULL, 
   locale character varying(10) NOT NULL DEFAULT 'en', 
   role integer NOT NULL DEFAULT 0,
   timezone integer NOT NULL DEFAULT 0, 
   version int8 NOT NULL DEFAULT 0,
   PRIMARY KEY (id), 
   CONSTRAINT un_email UNIQUE (email)
) WITH (OIDS = FALSE);
 
 
CREATE TABLE storage_plugins
(
   id serial NOT NULL, 
   classpath character varying(100) NOT NULL, 
   status integer NOT NULL, 
   version int8 NOT NULL DEFAULT 0,
   PRIMARY KEY (id), 
   CONSTRAINT un_classpath UNIQUE (classpath)
) WITH (OIDS = FALSE);


CREATE TABLE sessions 
(
	id serial NOT NULL,
	user_id integer NOT NULL,
    session_token character varying(32) NOT NULL,
	expires int8 NOT NULL, 
	PRIMARY KEY (id),
	CONSTRAINT fk_sessions_users 
		FOREIGN KEY (user_id) 
		REFERENCES users (id) 
		ON DELETE CASCADE 
		ON UPDATE CASCADE,
	CONSTRAINT un_session_token 
		UNIQUE (session_token)
) WITH (OIDS=FALSE);



CREATE  TABLE storage_accounts 
(
	id serial NOT NULL,
	user_id integer NOT NULL,
    storage_plugin_id integer NOT NULL,
    account_name character varying(100) NOT NULL, 
    version int8 NOT NULL DEFAULT 0,
  PRIMARY KEY (id),
  CONSTRAINT un_saccountlogin_spluginid 
		UNIQUE (storage_plugin_id, account_name, user_id),  
  CONSTRAINT fk_saccounts_splugins
    FOREIGN KEY (storage_plugin_id)
    REFERENCES storage_plugins (id)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT fk_saccounts_users
    FOREIGN KEY (user_id)
    REFERENCES users (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
) WITH (OIDS=FALSE);


CREATE  TABLE storage_accounts_settings 
(
  id serial NOT NULL,
  storage_account_id integer NOT NULL,
  setting_name character varying(45) NOT NULL,
  setting_value character varying(300) NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT un_saccountid_settingname 
		UNIQUE (storage_account_id, setting_name),  
  CONSTRAINT fk_saccounts_saccountssettings
    FOREIGN KEY (storage_account_id)
    REFERENCES storage_accounts (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
) WITH (OIDS=FALSE);

CREATE  TABLE reset_password_requests
(
  id serial NOT NULL,
  user_id integer NOT NULL,
  token character varying(100) NOT NULL,
  expires int8 NOT NULL,
  PRIMARY KEY (id),  
  CONSTRAINT fk_resetpasswd_users
    FOREIGN KEY (user_id)
    REFERENCES users (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
) WITH (OIDS=FALSE);
