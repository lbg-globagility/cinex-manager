-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

DROP PROCEDURE IF EXISTS `reports_pos`;

CREATE PROCEDURE `reports_pos`(_start_date DATETIME)
BEGIN

DECLARE _report_name VARCHAR(60);
DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);

SELECT `name` INTO _report_name FROM report WHERE id = 19;
SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');

SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
'' AS establishmentname,
IFNULL(_report_name, '') AS reportname, 
DATE_FORMAT(_start_date, '%m/%d/%Y') AS fordate, 

IFNULL(SUM(a.price), 0) AS voidamount
FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c, ticket d
WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id 
AND a.ticket_id = d.id
AND c.movie_date = _start_date AND a.status = 0;


SELECT IFNULL(COUNT(a.id), 0), IFNULL(SUM(a.price), 0) 
FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c, ticket e 
WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id  AND a.ticket_id = e.id
AND c.movie_date = _start_date AND a.status = 1;


END