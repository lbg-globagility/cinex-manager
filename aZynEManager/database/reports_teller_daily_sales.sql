-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

DROP PROCEDURE IF EXISTS `reports_teller_daily_sales`;

CREATE PROCEDURE `reports_teller_daily_sales`(_username VARCHAR(12), _start_date DATETIME)
BEGIN

DECLARE _report_name VARCHAR(60);

DECLARE _dt DATETIME;
DECLARE _date VARCHAR(20);
DECLARE _time VARCHAR(20);

DROP TEMPORARY TABLE IF EXISTS _Result;
DROP TEMPORARY TABLE IF EXISTS _Result2;

SELECT `name` INTO _report_name FROM reports WHERE `key` = 1;

CREATE TEMPORARY TABLE _Result
(
`key` INT NOT NULL AUTO_INCREMENT,
`screening_date` DATETIME,
patron_code VARCHAR(12),
patron_name VARCHAR(60),
cinema_code VARCHAR(12),
cinema_name VARCHAR(60),
quantity INT,
price DECIMAL(19, 4),
sales DECIMAL(19, 4),
`type` SMALLINT NOT NULL,
PRIMARY KEY (`key`)
);

INSERT INTO _Result(screening_date,patron_code,patron_name,cinema_code,cinema_name,quantity,price,sales,`type`)
SELECT
mc.screening_start_date, p.code, p.name, c.code, c.name, COUNT(mctrs.cinema_seat_key), mctrs.price price, SUM(mctrs.price), 0
FROM
movie_calendar mc
INNER JOIN
cinemas c
ON
mc.cinema_key = c.key
INNER JOIN
movie_calendar_times mct
ON
mct.movie_calendar_key = mc.key
INNER JOIN
movie_calendar_time_reserved_seats mctrs
ON
mctrs.movie_calendar_time_key = mct.key
INNER JOIN
ticket_transactions tt
ON
(mctrs.ticket_transaction_key = tt.key) AND (tt.username = _username)

INNER JOIN
patrons p
ON
mctrs.patron_key = p.key
WHERE
datediff(_start_date, tt.datetime) = 0 AND p.display_in_reports = 1
 AND mctrs.status = 1
GROUP BY
mc.screening_start_date, p.code, p.name, c.code, c.display_order, c.name, mctrs.price
ORDER BY
mc.screening_start_date, c.display_order, p.code;

CREATE TEMPORARY TABLE _Result2 AS
SELECT `type`, screening_date,patron_code, patron_name, cinema_code, cinema_name, quantity, price, sales FROM _Result ORDER BY screening_date,type, cinema_code,patron_code;

UPDATE _Result2 SET screening_date = NULL, patron_code = NULL, patron_name = NULL, cinema_code = NULL, cinema_name = NULL, price = NULL WHERE type = 1;
UPDATE _Result2 SET screening_date = NULL, patron_code = NULL, patron_name = NULL, cinema_code = NULL, cinema_name = NULL, price = NULL WHERE type = 10;

SET _dt = NOW();
SET _date = DATE_FORMAT(_dt, '%m/%d/%Y');
SET _time = DATE_FORMAT(_dt, '%h:%i %p');

SELECT CONCAT('Date:  ', _date) as currentdate, CONCAT('Time:  ',_time) as currenttime, 
RetrieveEstablishmentName() AS establishmentname,
IFNULL(_report_name, '') AS reportname, IFNULL(_username, '') AS username, 
 CONCAT('Sales Date: ', DATE_FORMAT(_start_date, '%m/%d/%Y')) AS salesdate,
'Ticket Seller : ___________________' as ticketseller, 'Manager :      ___________________' as manager,  
'Checked By : ___________________' as checkedby FROM dual;

SELECT screening_date, patron_code, patron_name, cinema_code, quantity, price, sales FROM _Result2;

DROP TEMPORARY TABLE _Result;
DROP TEMPORARY TABLE _Result2;

END