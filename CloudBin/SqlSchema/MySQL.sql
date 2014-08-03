CREATE SCHEMA `cloudbin` DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci ;

USE `cloudbin`;

CREATE  TABLE `storage_plugin_descriptors` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `classpath` VARCHAR(100) NOT NULL ,
  `status` INT NOT NULL DEFAULT 0 ,  
  PRIMARY KEY (`id`) ,
  UNIQUE INDEX `un_storage_plugin_descriptors_classpath` (`classpath` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;
