/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP TABLE IF EXISTS `movies_schedule_list_patron_ordinance_surcharge_view`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `movies_schedule_list_patron_ordinance_surcharge_view` AS select `patrons_ordinance`.`patron_id` AS `patron_id`,`ordinance_tbl`.`amount_val` AS `amount_val`,`ordinance_tbl`.`in_pesovalue` AS `in_pesovalue`,`ordinance_tbl`.`with_enddate` AS `with_enddate`,`ordinance_tbl`.`effective_date` AS `effective_date`,`ordinance_tbl`.`end_date` AS `end_date`,1 AS `isordinance` from (`patrons_ordinance` join `ordinance_tbl`) where (`patrons_ordinance`.`ordinance_id` = `ordinance_tbl`.`id`) union select `patrons_surcharge`.`patron_id` AS `patron_id`,`surcharge_tbl`.`amount_val` AS `amount_val`,`surcharge_tbl`.`in_pesovalue` AS `in_pesovalue`,`surcharge_tbl`.`with_enddate` AS `with_enddate`,`surcharge_tbl`.`effective_date` AS `effective_date`,`surcharge_tbl`.`end_date` AS `end_date`,0 AS `0` from (`patrons_surcharge` join `surcharge_tbl`) where (`patrons_surcharge`.`surcharge_id` = `surcharge_tbl`.`id`) ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
