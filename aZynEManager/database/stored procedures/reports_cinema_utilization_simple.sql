-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

DROP PROCEDURE IF EXISTS `reports_cinema_utilization_simple`;

CREATE PROCEDURE `reports_cinema_utilization_simple`(_start_date DATETIME, _end_date DATETIME)
BEGIN

DECLARE _report_name VARCHAR(60);

DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);

SELECT `name` INTO _report_name FROM report WHERE id = 17;

SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');

SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
'' AS establishmentname,
IFNULL(_report_name, '') AS reportname, 
 CONCAT('From ', DATE_FORMAT(_start_date, '%m/%d/%Y'), ' to ', DATE_FORMAT(_end_date, '%m/%d/%Y')) AS daterange
FROM dual;

SELECT d.in_order, d.name,  COUNT(DISTINCT b.id)* capacity, COUNT(a.cinema_seat_id), SUM(a.price) ,
COUNT(a.cinema_seat_id)/ (COUNT(DISTINCT b.id)* capacity) * 100  
FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c,
cinema d 
WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id  
AND c.cinema_id = d.id
AND (c.movie_date BETWEEN _start_date AND _end_date ) AND a.status = 1
GROUP BY d.id
ORDER BY d.in_order;


END