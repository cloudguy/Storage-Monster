CREATE SCHEMA `storage_services` DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci ;

USE `storage_services`;

CREATE  TABLE `storages` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `classpath` VARCHAR(100) NOT NULL ,
  `status` INT NOT NULL DEFAULT 0,
  `stamp`  TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP  ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) ,
  UNIQUE INDEX `un_classpath` (`classpath` ASC) )
ENGINE = InnoDB;

CREATE  TABLE `users` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `name` VARCHAR(100) NOT NULL ,
  `email` VARCHAR(100) NOT NULL,
  `password` VARCHAR(200) NOT NULL ,  
  `locale` VARCHAR(10) NOT NULL DEFAULT 'en',
  `timezone` INT NOT NULL DEFAULT 0,
  `stamp`  TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP  ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) ,  
  UNIQUE INDEX `un_email` (`email` ASC))
ENGINE = InnoDB;

CREATE  TABLE `user_roles` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `user_id` INT NOT NULL ,
  `role` VARCHAR(45) NOT NULL ,
  PRIMARY KEY (`id`) ,
  INDEX `fk_userroles_users` (`id` ASC) ,
  UNIQUE INDEX `un_userroles` (`user_id` ASC, `role` ASC) ,
  CONSTRAINT `fk_userroles_users`
    FOREIGN KEY (`user_id` )
    REFERENCES `users` (`id` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;

CREATE  TABLE `sessions` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `user_id` INT NOT NULL ,
  `session_token` VARCHAR(32) NOT NULL ,
  `session_antiforgery_token` VARCHAR(32) NOT NULL ,
  `expiration_date` DATETIME NULL ,
  PRIMARY KEY (`id`) ,
  INDEX `fk_sessions_users` (`user_id` ASC) ,
  UNIQUE INDEX `un_session_token` (`session_token` ASC) ,
  CONSTRAINT `fk_sessions_users`
    FOREIGN KEY (`user_id` )
    REFERENCES `users` (`id` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;

CREATE  TABLE `accounts` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `user_id` INT NOT NULL ,
  `storage_id` INT NOT NULL ,
  `account_server` VARCHAR(100) NOT NULL ,
  `account_login` VARCHAR(100) NOT NULL ,
  `stamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP  ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`id`) ,
  INDEX `fk_accounts_storages` (`storage_id` ASC) ,
  UNIQUE INDEX `un_accountlogin_storageid` (`storage_id` ASC, `account_server` ASC, `account_login` ASC, `user_id` ASC) ,
  CONSTRAINT `fk_accounts_storages`
    FOREIGN KEY (`storage_id` )
    REFERENCES `storages` (`id` )
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `fk_accounts_users`
    FOREIGN KEY (`user_id` )
    REFERENCES `users` (`id` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


CREATE  TABLE `accounts_settings` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `account_id` INT NOT NULL ,
  `setting_name` VARCHAR(45) NOT NULL ,
  `setting_value` VARCHAR(300) NOT NULL ,
  `stamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP  ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`id`) ,
  INDEX `fk_accounts_accountssettings` (`account_id` ASC) ,
  UNIQUE INDEX `un_accountid_settingname` (`account_id` ASC, `setting_name` ASC) ,
  CONSTRAINT `fk_accounts_accountssettings`
    FOREIGN KEY (`account_id` )
    REFERENCES `accounts` (`id` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;



DELIMITER //
CREATE FUNCTION create_role(email_in VARCHAR(100), role_in VARCHAR(45),stamp_in TIMESTAMP)
RETURNS INT
BEGIN
	DECLARE role_exists_local INT;
	DECLARE user_id_local INT; 

	SELECT count(ur.id) INTO role_exists_local FROM user_roles ur INNER JOIN users u ON ur.user_id = u.id
	WHERE u.email = email_in AND ur.role = role_in AND u.stamp = stamp_in;

	IF role_exists_local = 0 THEN
		SELECT u.id INTO user_id_local FROM users u WHERE u.email = email_in AND u.stamp = stamp_in LIMIT 1;
		IF user_id_local < 1 OR user_id_local IS NULL THEN
			SELECT u.id INTO user_id_local FROM users u WHERE u.email = email_in;
			IF user_id_local < 1 OR user_id_local IS NULL	THEN		
				RETURN 1;
			ELSE
				RETURN 2;
			END IF;
		ELSE
			INSERT INTO user_roles (user_id, role) VALUES (user_id_local, role_in);
			UPDATE users SET stamp = CURRENT_TIMESTAMP() WHERE id = user_id_local; -- just updating stamp
			RETURN 0;
		END IF;
	END IF;
	RETURN 0;
END //
DELIMITER ; 


DELIMITER //
CREATE FUNCTION delete_role(email_in VARCHAR(100), role_in VARCHAR(45), stamp_in TIMESTAMP)
RETURNS INT
BEGIN
	DECLARE role_exists_local INT;
	DECLARE user_id_local INT; 

	SELECT count(ur.id) INTO role_exists_local FROM user_roles ur INNER JOIN users u ON ur.user_id = u.id
	WHERE u.email = email_in AND ur.role = role_in  AND u.stamp = stamp_in;

	IF role_exists_local = 0 THEN
		SELECT u.id INTO user_id_local FROM users u WHERE u.email = email_in AND u.stamp = stamp_in LIMIT 1;
		IF user_id_local < 1 OR user_id_local IS NULL THEN
			SELECT u.id INTO user_id_local FROM users u WHERE u.email = email_in;
			IF user_id_local < 1 OR user_id_local IS NULL THEN  
				RETURN 1;
			ELSE
				RETURN 2;
			END IF;
		ELSE
			RETURN 0;
		END IF;
	END IF;
    
	DELETE FROM user_roles USING user_roles,users  WHERE user_roles.role = role_in AND users.email = email_in AND users.stamp = stamp_in;
	RETURN 0;
END //
DELIMITER ;


DELIMITER //
CREATE FUNCTION update_roles(email_in VARCHAR(100), roles_ins VARCHAR(300), roles_del VARCHAR(300), stamp_in TIMESTAMP)
RETURNS INT
BEGIN 
	DECLARE i INT ;
	DECLARE user_id_local INT;
	DECLARE count_char_insert INT;
	DECLARE count_char_delete INT;
	DECLARE status INT;

	SET count_char_insert = CHARACTER_LENGTH(roles_ins);
   
	WHILE 1<count_char_insert AND count_char_insert IS NOT NULL DO
		SET status = create_role(email_in, TRIM(SUBSTRING_INDEX(roles_ins , ',', 1)) , stamp_in);  
			IF(status<>0) THEN    
				RETURN status;
			END IF;
		SET i = CHARACTER_LENGTH( SUBSTRING_INDEX(roles_ins ,',', 1) );  
		SET roles_ins = SUBSTRING(roles_ins, i+2);  
		SET count_char_insert = CHARACTER_LENGTH(roles_ins);     
	END WHILE;   
  
	SET count_char_delete = CHARACTER_LENGTH(roles_del);

	WHILE 1<count_char_delete AND count_char_delete IS NOT NULL DO
		SET status = delete_role (email_in, TRIM(SUBSTRING_INDEX(roles_del , ',', 1)) , stamp_in);  
			IF(status<>0) THEN    
				RETURN status;
			END IF;
		SET i = CHARACTER_LENGTH( SUBSTRING_INDEX(roles_del , ',', 1) );  
		SET roles_del = SUBSTRING(roles_del, i+2);  
		SET count_char_delete = CHARACTER_LENGTH(roles_del);   
	END WHILE;
	RETURN 0;
END //
DELIMITER ;



DELIMITER //
CREATE FUNCTION init_plugin_status(classpath_in VARCHAR(100), status_in INT)
RETURNS INT
BEGIN	
	DECLARE storage_id_local INT; 

	SELECT id INTO storage_id_local FROM storages WHERE classpath = classpath_in;

	IF storage_id_local IS NOT NULL THEN
		UPDATE storages SET status = status_in WHERE id = storage_id_local;
		RETURN storage_id_local;	
	END IF;

	
	INSERT INTO storages (classpath, status) VALUES (classpath_in, status_in);
	RETURN LAST_INSERT_ID();	
END //
DELIMITER ; 