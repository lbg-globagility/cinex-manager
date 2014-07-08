-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

DROP PROCEDURE IF EXISTS `reports_cinema_utilization_complex`;

CREATE PROCEDURE `reports_cinema_utilization_complex`(_start_date DATETIME, _end_date DATETIME)
BEGIN

DECLARE _report_name VARCHAR(60);

DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);

SELECT `name` INTO _report_name FROM report WHERE id = 18;

SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');

SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
'' AS establishmentname,
IFNULL(_report_name, '') AS reportname, 
 CONCAT('From ', DATE_FORMAT(_start_date, '%m/%d/%Y'), ' to ', DATE_FORMAT(_end_date, '%m/%d/%Y')) AS daterange
FROM dual;

SELECT cinema_name, cinema_capacity, cinema_util, movie_title, movie_capacity, start_date, end_date, movie_util, screens, patron_code, patron_name, patron_price, (movie_seats_taken/movie_capacity*100)  patron_util, movie_seats_taken, ticket_sales FROM 
(
SELECT aa.in_order, bb.cinema_id, bb.movie_id, aa.name cinema_name, aa.cinema_capacity, aa.cinema_util, bb.movie_title, bb.movie_capacity, bb.start_date, bb.end_date, bb.movie_util,
bb.screens
FROM
(SELECT d.id, d.in_order, d.name,  COUNT(DISTINCT b.id)* capacity as cinema_capacity, 
COUNT(a.cinema_seat_id)/ (COUNT(DISTINCT b.id)* capacity) * 100 cinema_util
FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c,
cinema d 
WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id  
AND c.cinema_id = d.id
AND (c.movie_date BETWEEN _start_date AND _end_date ) AND a.status = 1
GROUP BY d.id
ORDER BY d.in_order) aa
LEFT OUTER JOIN 
(
SELECT d.id cinema_id, e.id movie_id, e.title movie_title,  COUNT(DISTINCT b.id) screens, COUNT(DISTINCT b.id) * capacity as movie_capacity, 
COUNT(a.cinema_seat_id) / (COUNT(DISTINCT b.id) * capacity) * 100 movie_util,
MIN(c.movie_date) start_date, MAX(c.movie_date) end_date 
FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c,
cinema d, movies e
WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id  
AND c.cinema_id = d.id AND c.movie_id = e.id 
AND (c.movie_date BETWEEN _start_date AND _end_date ) AND a.status = 1
GROUP BY d.id, e.id) bb
ON aa.id = bb.cinema_id
) cc
LEFT OUTER JOIN
(
SELECT d.id c_id, e.id m_id, f.code patron_code, f.name patron_name, a.price patron_price, COUNT(a.cinema_seat_id) movie_seats_taken, SUM(a.price) ticket_sales
FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c,
cinema d, movies e, patrons f
WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id  
AND c.cinema_id = d.id AND c.movie_id = e.id   AND a.patron_id = f.id 
AND (c.movie_date BETWEEN _start_date AND _end_date ) AND a.status = 1
GROUP BY d.id, e.id, f.id, a.price
) dd
ON cc.cinema_id = dd.c_id AND cc.movie_id = dd.m_id
ORDER BY in_order, movie_title, patron_name;


END