/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `reports_daily_box_office`;
DELIMITER //
CREATE PROCEDURE `reports_daily_box_office`(_start_date DATETIME, _cinema_id INT, _movie_id INT)
BEGIN
DECLARE _cinema_name VARCHAR(100);
DECLARE _movie_name VARCHAR(100);
DECLARE _report_name VARCHAR(60);
DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);
SELECT `name` INTO _cinema_name FROM cinema WHERE id = _cinema_id;
SELECT title INTO _movie_name FROM movies WHERE id = _movie_id;
SELECT `name` INTO _report_name FROM report WHERE id = 8;
SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');
SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
'' AS establishmentname,
IFNULL(_report_name, '') AS reportname, IFNULL(_cinema_name, '') AS cinemaname, IFNULL(_movie_name, '') AS moviename,
DATE_FORMAT(_start_date, '%m/%d/%Y') AS startdate, 'Manager :      ___________________' as manager FROM dual;
SELECT COUNT(cinema_seat_id) totalquantity, SUM(price) totalprice, MIN(b.start_time) start_time, MAX(b.end_time) end_time FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c, patrons d
WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id AND a.patron_id = d.id
AND c.movie_date = _start_date AND cinema_id = _cinema_id AND movie_id = _movie_id;
SELECT d.name, IFNULL(COUNT(cinema_seat_id), 0), a.price, IFNULL(SUM(price), 0) FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c, patrons d
WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id AND a.patron_id = d.id
AND c.movie_date = _start_date AND cinema_id = _cinema_id AND movie_id = _movie_id
GROUP BY a.patron_id, a.price ORDER BY d.name;
END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
