/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP TABLE IF EXISTS `movies_schedule_list_patron_ordinance_view`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `movies_schedule_list_patron_ordinance_view` AS select `a`.`id` AS `id`,`a`.`movies_schedule_list_id` AS `movies_schedule_list_id`,`a`.`patron_id` AS `patron_id`,`a`.`price` AS `price`,`a`.`is_default` AS `is_default`,if(isnull(`d`.`patron_id`),0,if(`d`.`in_pesovalue`,`d`.`amount_val`,(`a`.`price` * `d`.`amount_val`))) AS `ordinance_val` from (`movies_schedule_list_patron` `a` left join `patrons_ordinance_view` `d` on((`a`.`patron_id` = `d`.`patron_id`))) ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
