CREATE SCHEMA `cloudbin` DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci ;

USE `cloudbin`;

CREATE TABLE `storage_plugin_descriptors` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `classpath` VARCHAR(100) NOT NULL ,
  `status` INT NOT NULL DEFAULT 0 ,  
  PRIMARY KEY (`id`) ,
  UNIQUE INDEX `un_storage_plugin_descriptors_classpath` (`classpath` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;

CREATE TABLE `users` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `name` VARCHAR(100) NOT NULL ,
  `password` VARCHAR(200) NOT NULL ,  
  `locale` VARCHAR(10) NOT NULL DEFAULT 'en-US',
  `timezone` INT NOT NULL DEFAULT 0 ,
  `version` BIGINT NOT NULL DEFAULT 0 ,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;

CREATE TABLE `users_emails` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `email` VARCHAR(100) NOT NULL,
  `user_id` INT NOT NULL ,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `un_users_emails_email` (`email` ASC) ,
  CONSTRAINT `fk_users_emails_users`
    FOREIGN KEY (`user_id` )
    REFERENCES `users` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;

CREATE  TABLE `user_sessions` (
  `id` BIGINT NOT NULL AUTO_INCREMENT ,
  `user_id` INT NOT NULL ,
  `session_token` VARCHAR(32) NOT NULL ,
  `is_persistent` TINYINT(1) NOT NULL,  
  `user_agent` VARCHAR(200) NOT NULL ,  
  `ip_address` VARCHAR(100) NOT NULL ,  
  `expires` BIGINT NOT NULL ,
  `signed_in` BIGINT NOT NULL ,
  PRIMARY KEY (`id`) ,
  INDEX `fk_sessions_users` (`user_id` ASC) ,
  UNIQUE INDEX `un_user_sessions_session_token` (`session_token` ASC) ,
  CONSTRAINT `fk_user_sessions_users`
    FOREIGN KEY (`user_id` )
    REFERENCES `users` (`id` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;