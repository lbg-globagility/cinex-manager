-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

DROP PROCEDURE IF EXISTS `reports_teller_daily_sales_all`;

CREATE PROCEDURE `reports_teller_daily_sales_all`(_start_date DATETIME)
BEGIN

DECLARE _report_name VARCHAR(60);
DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);

SELECT `name` INTO _report_name FROM report WHERE id = 12;
SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');

SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
'' AS establishmentname,
IFNULL(_report_name, '') AS reportname, 
DATE_FORMAT(_start_date, '%m/%d/%Y') AS fordate
FROM dual;

SELECT h.userid, CONCAT(h.fname, ' ', h.mname, '.' , h.lname), d.code, d.name,  f.in_order, a.price,  COUNT(a.cinema_seat_id), SUM(a.price) FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, 
movies_schedule c, patrons d, cinema f, ticket g, users h
WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id AND a.patron_id = d.id AND cinema_id = f.id
AND a.ticket_id = g.id AND g.user_id = h.id
AND c.movie_date = _start_date GROUP BY h.id, a.patron_id, cinema_id,  a.price 
ORDER BY h.userid, d.name, f.in_order, a.price;
END