-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

DROP PROCEDURE IF EXISTS `reports_teller_daily_sales`;

CREATE PROCEDURE `reports_teller_daily_sales`(_userid INT, _start_date DATETIME)
BEGIN

DECLARE _username VARCHAR(100);
DECLARE _report_name VARCHAR(60);

DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);

SELECT userid INTO _username FROM users WHERE id = _userid;
SELECT `name` INTO _report_name FROM report WHERE id = 1;

SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');

SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
'' AS establishmentname,
IFNULL(_report_name, '') AS reportname, IFNULL(_username, '') AS username, 
 CONCAT('Sales Date: ', DATE_FORMAT(_start_date, '%m/%d/%Y')) AS salesdate,
'Ticket Seller : ___________________' as ticketseller, 'Manager :      ___________________' as manager,  
'Checked By : ___________________' as checkedby FROM dual;

SELECT c.movie_date, d.code, d.name, f.name, COUNT(a.cinema_seat_id), a.price, SUM(a.price), 0 FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c, patrons d, 
ticket e, cinema f
WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id 
AND a.patron_id = d.id AND a.ticket_id = e.id
AND c.cinema_id = f.id
AND DATEDIFF(_start_date, ticket_datetime) = 0
AND e.user_id = _userid
AND a.status = 1
AND c.movie_date = _start_date
GROUP BY c.movie_date, d.code, d.name, f.in_order, f.name, a.price
ORDER BY c.movie_date,  f.in_order, d.code;


END