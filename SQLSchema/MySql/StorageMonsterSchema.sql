CREATE SCHEMA `storage_services` DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci ;

USE `storage_services`;

CREATE  TABLE `storage_plugins` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `classpath` VARCHAR(100) NOT NULL ,
  `status` INT NOT NULL DEFAULT 0,
  `stamp`  TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP  ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) ,
  UNIQUE INDEX `un_classpath` (`classpath` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;

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
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;

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
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;

CREATE  TABLE `sessions` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `user_id` INT NOT NULL ,
  `session_token` VARCHAR(32) NOT NULL ,  
  `expiration_date` DATETIME NULL ,
  PRIMARY KEY (`id`) ,
  INDEX `fk_sessions_users` (`user_id` ASC) ,
  UNIQUE INDEX `un_session_token` (`session_token` ASC) ,
  CONSTRAINT `fk_sessions_users`
    FOREIGN KEY (`user_id` )
    REFERENCES `users` (`id` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;

CREATE  TABLE `storage_accounts` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `user_id` INT NOT NULL ,
  `storage_plugin_id` INT NOT NULL ,
  `account_name` VARCHAR(100) NOT NULL , 
  `stamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP  ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`id`) ,
  INDEX `fk_saccounts_splugins` (`storage_plugin_id` ASC) ,
  UNIQUE INDEX `un_saccountlogin_spluginid` (`storage_plugin_id` ASC, `account_name` ASC, `user_id` ASC) ,
  CONSTRAINT `fk_saccounts_splugins`
    FOREIGN KEY (`storage_plugin_id` )
    REFERENCES `storage_plugins` (`id` )
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `fk_saccounts_users`
    FOREIGN KEY (`user_id` )
    REFERENCES `users` (`id` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;


CREATE  TABLE `storage_accounts_settings` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `storage_account_id` INT NOT NULL ,
  `setting_name` VARCHAR(45) NOT NULL ,
  `setting_value` VARCHAR(300) NOT NULL ,
  `stamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP  ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`id`) ,
  INDEX `fk_saccounts_saccountssettings` (`storage_account_id` ASC) ,
  UNIQUE INDEX `un_saccountid_settingname` (`storage_account_id` ASC, `setting_name` ASC) ,
  CONSTRAINT `fk_saccounts_saccountssettings`
    FOREIGN KEY (`storage_account_id` )
    REFERENCES `storage_accounts` (`id` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;

CREATE  TABLE `reset_password_requests` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `user_id` INT NOT NULL ,
  `token` VARCHAR(100) NOT NULL ,
  `expiration_date` DATETIME NOT NULL ,
  PRIMARY KEY (`id`) ,
  INDEX `fk_resetpasswd_users` (`user_id` ASC) ,
  CONSTRAINT `fk_resetpasswd_users`
    FOREIGN KEY (`user_id` )
    REFERENCES `users` (`id` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;