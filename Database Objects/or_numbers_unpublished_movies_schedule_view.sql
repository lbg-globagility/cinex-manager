/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP TABLE IF EXISTS `or_numbers_unpublished_movies_schedule_view`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `or_numbers_unpublished_movies_schedule_view` AS select `movies_schedule_list_reserved_seat`.`id` AS `id`,`movies_schedule_list_reserved_seat`.`or_number` AS `or_number` from `movies_schedule_list_reserved_seat` where (`movies_schedule_list_reserved_seat`.`movies_schedule_list_id` in (select `movies_schedule_list`.`id` from `movies_schedule_list` where ((`movies_schedule_list`.`status` = 0) and (cast(`movies_schedule_list`.`start_time` as date) = cast(now() as date)))) and isnull(`movies_schedule_list_reserved_seat`.`void_datetime`)) group by `movies_schedule_list_reserved_seat`.`or_number` ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
