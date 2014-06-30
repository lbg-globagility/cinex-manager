-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

DROP PROCEDURE IF EXISTS `reports_summary_daily_cinema_sales`;

CREATE PROCEDURE `reports_summary_daily_cinema_sales`(_start_date DATETIME)
BEGIN

DECLARE _report_name VARCHAR(60);
DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);

SELECT `name` INTO _report_name FROM report WHERE id = 3;
SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');

SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
'' AS establishmentname,
IFNULL(_report_name, '') AS reportname, 
 CONCAT('For ', DATE_FORMAT(_start_date, '%m/%d/%Y')) AS fordate
FROM dual;

SELECT f.in_order, f.name, e.code, e.title, COUNT(cinema_seat_id) quantity, SUM(price) sales FROM movies_schedule_list_reserved_seat a, ticket b, 
movies_schedule_list c, movies_schedule d, movies e, cinema f
 WHERE a.ticket_id = b.id
and a.status = 1 AND a.movies_schedule_list_id = c.id AND c.movies_schedule_id = d.id
AND d.movie_date = _start_date AND d.movie_id = e.id AND d.cinema_id = f.id
GROUP BY f.id, e.id ORDER BY f.in_order;

END