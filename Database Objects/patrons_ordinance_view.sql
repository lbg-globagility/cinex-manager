/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP TABLE IF EXISTS `patrons_ordinance_view`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `patrons_ordinance_view` AS select `b`.`patron_id` AS `patron_id`,`c`.`amount_val` AS `amount_val`,`c`.`in_pesovalue` AS `in_pesovalue` from (`patrons_ordinance` `b` join `ordinance_tbl` `c`) where ((`b`.`ordinance_id` = `c`.`id`) and (((`c`.`with_enddate` = 0) and (now() >= `c`.`effective_date`)) or ((`c`.`with_enddate` = 1) and (now() >= `c`.`effective_date`) and (now() <= `c`.`end_date`)))) ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
