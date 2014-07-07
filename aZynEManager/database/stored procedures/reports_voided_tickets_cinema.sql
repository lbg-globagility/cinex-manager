-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

DROP PROCEDURE IF EXISTS `reports_voided_tickets_cinema`;

CREATE PROCEDURE `reports_voided_tickets_cinema`(_start_date DATETIME, _end_date DATETIME)
BEGIN

DECLARE _report_name VARCHAR(60);

DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);

SELECT `name` INTO _report_name FROM report WHERE id = 15;

SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');

SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
'' AS establishmentname,
IFNULL(_report_name, '') AS reportname, 
 CONCAT('From ', DATE_FORMAT(_start_date, '%m/%d/%Y'), ' to ', DATE_FORMAT(_end_date, '%m/%d/%Y')) AS daterange
FROM dual;

SELECT 
c.movie_date,
h.code,
h.name,
g.name,
f.code,
f.title,
e.ticket_datetime,
i.userid,
DATE_FORMAT(a.void_datetime, '%H:%i:%s'),
a.void_datetime,
j.userid,
a.or_number,
CONCAT(k.row_name, k.col_name),
1,
a.price
FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c, ticket e,
movies f, cinema g, patrons h, users i, users j, cinema_seat k
WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id  AND a.ticket_id = e.id
AND c.movie_id = f.id AND c.cinema_id = g.id AND a.patron_id = h.id AND e.user_id = i.id AND a.cinema_seat_id = k.id
AND a.void_user_id = j.id
AND (c.movie_date BETWEEN _start_date AND _end_date) AND a.status = 0;

END