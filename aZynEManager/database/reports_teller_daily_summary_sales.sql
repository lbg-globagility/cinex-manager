-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

DROP PROCEDURE IF EXISTS `reports_teller_daily_summary_sales`;

CREATE PROCEDURE `reports_teller_daily_summary_sales`(_start_date DATETIME)
BEGIN

DECLARE _report_name VARCHAR(60);
DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);

SELECT `name` INTO _report_name FROM report WHERE id = 4;
SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');

SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
'' AS establishmentname,
IFNULL(_report_name, '') AS reportname, 
CONCAT('For Sales Date ', DATE_FORMAT(_start_date, '%m/%d/%Y')) AS forsalesdate,
'Manager :      ___________________' as manager,  
'Checked By : ___________________' as checkedby 
FROM dual;

SELECT e.userid, CONCAT(e.fname, ' ', e.mname, '. ', e.lname), d.movie_date, COUNT(a.cinema_seat_id), SUM(a.price)
FROM movies_schedule_list_reserved_seat a, ticket b, 
movies_schedule_list c, movies_schedule d, users e
 WHERE a.ticket_id = b.id
and a.status = 1 AND a.movies_schedule_list_id = c.id AND c.movies_schedule_id = d.id
AND b.user_id = e.id
AND DATEDIFF(b.ticket_datetime, _start_date) = 0 
GROUP BY b.user_id, d.movie_date;


END