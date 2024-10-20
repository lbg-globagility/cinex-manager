/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `reports_daily_sales_distributor`;
DELIMITER //
CREATE PROCEDURE `reports_daily_sales_distributor`(_distributorid INT, _start_date DATETIME)
BEGIN
DECLARE _distributorname VARCHAR(100);
DECLARE _report_name VARCHAR(60);
DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);
SELECT `name` INTO _distributorname FROM distributor WHERE id = _distributorid;
SELECT `name` INTO _report_name FROM report WHERE id = 1;
SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');
SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
'' AS establishmentname,
IFNULL(_report_name, '') AS reportname, IFNULL(_distributorname, '') AS distributorname, 
 CONCAT('For ', DATE_FORMAT(_start_date, '%m/%d/%Y')) AS fordate FROM dual;
SELECT e.code, e.title, COUNT(a.cinema_seat_id), SUM( a.price) FROM movies_schedule_list_reserved_seat a, ticket b, 
movies_schedule_list c, movies_schedule d, movies e
 WHERE a.ticket_id = b.id AND a.movies_schedule_list_id = c.id AND c.movies_schedule_id = d.movie_id
 AND d.movie_date = _start_date
 AND d.movie_id = e.id AND e.dist_id = _distributorid GROUP BY e.code, e.title ORDER BY e.code;
END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
