/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `reports_cinema_sales_ticket_type_sold_per_movie_title`;
DELIMITER //
CREATE PROCEDURE `reports_cinema_sales_ticket_type_sold_per_movie_title`(_start_date DATETIME, _end_date DATETIME)
BEGIN
DECLARE _report_name VARCHAR(60);
DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);
SELECT `name` INTO _report_name FROM report WHERE id = 10;
SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');
SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
'' AS establishmentname,
IFNULL(_report_name, '') AS reportname, 
 CONCAT('From ', DATE_FORMAT(_start_date, '%m/%d/%Y'), ' to ', DATE_FORMAT(_end_date, '%m/%d/%Y')) AS daterange
FROM dual;
SELECT f.name,  g.title, d.code, d.name, a.price, COUNT(a.cinema_seat_id), SUM(a.price) FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, 
movies_schedule c, patrons d, cinema f, movies g
WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id AND a.patron_id = d.id AND cinema_id = f.id
AND c.movie_id = g.id AND c.movie_date BETWEEN _start_date AND _end_date GROUP BY cinema_id, g.id, a.patron_id, a.price 
ORDER BY f.in_order, g.title, d.name, a.price;
END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
