-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

DROP PROCEDURE IF EXISTS `reports_daily_sales_distributor`;

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


END