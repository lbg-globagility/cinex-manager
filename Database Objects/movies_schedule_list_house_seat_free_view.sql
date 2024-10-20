/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP TABLE IF EXISTS `movies_schedule_list_house_seat_free_view`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `movies_schedule_list_house_seat_free_view` AS select `movies_schedule_list_house_seat`.`id` AS `id`,`movies_schedule_list_house_seat`.`movies_schedule_list_id` AS `movies_schedule_list_id`,`movies_schedule_list_house_seat`.`cinema_seat_id` AS `cinema_seat_id`,`movies_schedule_list_house_seat`.`full_name` AS `full_name`,`movies_schedule_list_house_seat`.`notes` AS `notes`,`movies_schedule_list_house_seat`.`reserved_date` AS `reserved_date`,`movies_schedule_list_house_seat`.`session_id` AS `session_id`,`movies_schedule_list_house_seat`.`movies_schedule_list_patron_id` AS `movies_schedule_list_patron_id` from `movies_schedule_list_house_seat` where ((time_to_sec(timediff(now(),ifnull(`movies_schedule_list_house_seat`.`reserved_date`,now()))) < 600) and `movies_schedule_list_house_seat`.`movies_schedule_list_id` in (select `movies_schedule_list`.`id` from `movies_schedule_list` where (`movies_schedule_list`.`seat_type` <> 1))) ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
