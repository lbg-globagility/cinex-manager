/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `retrieve_movies_schedule_list_patron_mslid_first`;
DELIMITER //
CREATE PROCEDURE `retrieve_movies_schedule_list_patron_mslid_first`(mlsid INT, movie_date DATETIME)
BEGIN
SELECT f.*, g.code patron_code, g.name patron_name, g.seat_color patron_seat_color  FROM (
SELECT id, movies_schedule_list_id, patron_id, price base_price, is_default, price + SUM(ordinance_val) + SUM(surcharge_val) price,
SUM(ordinance_val) ordinance_price, SUM(surcharge_val) surcharge_price FROM (
SELECT a.*, IF(d.patron_id IS NULL, 0, IF (isordinance = 0, 0, IF(in_pesovalue, amount_val, a.price * amount_val))) ordinance_val,
IF(d.patron_id IS NULL, 0, IF (isordinance = 1, 0, IF(in_pesovalue, amount_val, a.price * amount_val))) surcharge_val FROM movies_schedule_list_patron a
LEFT OUTER JOIN 
(SELECT patron_id, amount_val, in_pesovalue, isordinance FROM movies_schedule_list_patron_ordinance_surcharge_view WHERE ( (with_enddate = 0 && movie_date >= effective_date) || (with_enddate = 1 && movie_date >= effective_date && movie_date <= end_date))
) d ON a.patron_id = d.patron_id WHERE a.movies_schedule_list_id = mlsid) e
GROUP BY id) f, patrons g WHERE f.patron_id = g.id LIMIT 1;
END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
