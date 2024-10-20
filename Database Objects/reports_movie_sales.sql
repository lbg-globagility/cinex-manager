/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `reports_movie_sales`;
DELIMITER //
CREATE PROCEDURE `reports_movie_sales`(_start_date DATETIME, _end_date DATETIME)
BEGIN
DECLARE _report_name VARCHAR(60);
DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);
SELECT `name` INTO _report_name FROM report WHERE id = 2;
SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');
SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
'' AS establishmentname,
IFNULL(_report_name, '') AS reportname, 
 CONCAT('From ', DATE_FORMAT(_start_date, '%m/%d/%Y'), ' to ', DATE_FORMAT(_end_date, '%m/%d/%Y')) AS daterange
FROM dual;
SELECT e.code, e.title, e.no_of_days, f.no_of_screenings, total_seats_taken, total_available_seats,  total_ticket_sales,
(total_seats_taken/total_available_seats) * 100 util
   FROM (
SELECT a.id, code, title, rating_id, COUNT(movie_date) no_of_days FROM movies a, movies_schedule b 
WHERE a.id = b.movie_id
AND b.movie_date BETWEEN _start_date AND _end_date GROUP BY a.id ) e,
(
SELECT movie_id, COUNT(c.id) no_of_screenings FROM movies_schedule_list c, movies_schedule d WHERE c.movies_schedule_id =d.id AND
 d.movie_date BETWEEN  _start_date AND _end_date GROUP BY movie_id ) f,
(
SELECT j.movie_id, COUNT(h.cinema_seat_id) total_seats_taken, SUM(h.price) total_ticket_sales FROM 
movies_schedule_list_reserved_seat h, movies_schedule_list i, movies_schedule j 
WHERE h.movies_schedule_list_id = i.id AND i.movies_schedule_id = j.id AND
 j.movie_date BETWEEN  _start_date AND _end_date  GROUP BY movie_id
) g,
(
	SELECT movie_id, SUM(available_seats) total_available_seats FROM (
	SELECT movie_id, n.capacity * COUNT(l.id) available_seats FROM movies_schedule_list l, movies_schedule m, cinema n
	WHERE l.movies_schedule_id =m.id AND m.cinema_id = n.id AND
	m.movie_date BETWEEN  _start_date AND _end_date GROUP BY movie_id, cinema_id) o GROUP BY movie_id
) k
WHERE e.id = f.movie_id AND e.id = g.movie_id AND e.id = k.movie_id;
END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
