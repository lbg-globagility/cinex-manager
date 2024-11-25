/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `rp01`;
DELIMITER //
CREATE PROCEDURE `rp01`(
	IN `_date` DATE,
	IN `_userId` TEXT
)
BEGIN

DROP TEMPORARY TABLE IF EXISTS `tbl_rp01`;
CREATE TEMPORARY TABLE IF NOT EXISTS `tbl_rp01`
SELECT

#u.id user, p.code, p.name `PATRON`, c.name `CINEMA`, mslrs.base_price `PRICE`, COUNT(mslrs.cinema_seat_id) `QTY`, SUM(mslrs.base_price) `TOTALSALES`, SUM(mslrs.ordinance_price) `TOTALORDINANCE`, SUM(mslrs.surcharge_price) `TOTALSURCHARGE`, SUM(mslrs.base_price) + SUM(mslrs.ordinance_price) + SUM(mslrs.surcharge_price) `GRANDTOTAL`, ms.movie_date, t.ticket_datetime, u.userid, ct.system_value, r.name reportname, SUM(IFNULL(sur.amount_val, 0)) `FOOD`

u.id user, p.code, p.name `PATRON`, c.name `CINEMA`, mslrs.base_price `PRICE`, (mslrs.cinema_seat_id) `QTY`, (mslrs.base_price) `TOTALSALES`, (mslrs.ordinance_price) `TOTALORDINANCE`,
#(mslrs.surcharge_price) `TOTALSURCHARGE`,
(mslrs.surcharge_price - IFNULL(sur.amount_val, 0)) `TOTALSURCHARGE`,
#IFNULL(sur.amount_val, 0) `TOTALSURCHARGE`,
(mslrs.base_price) + (mslrs.ordinance_price) + (mslrs.surcharge_price) `GRANDTOTAL`, ms.movie_date, t.ticket_datetime, u.userid, ct.system_value, r.name `reportname`, (IFNULL(i.amount_val, 0)) `FOOD`, p.id `PatronId`, c.in_order `CinemaOrdinal`

#, mslrs.*

FROM movies_schedule_list_reserved_seat mslrs
INNER JOIN movies_schedule_list msl ON msl.id=mslrs.movies_schedule_list_id
INNER JOIN movies_schedule ms ON ms.id=msl.movies_schedule_id AND DATE(ms.movie_date)=_date #STR_TO_DATE('11/20/2024', '%c/%e/%Y')
INNER JOIN ticket t ON t.id=mslrs.ticket_id
INNER JOIN users u ON u.id=t.user_id AND u.userid IN (_userId)

INNER JOIN cinema_seat cs ON cs.id=mslrs.cinema_seat_id
INNER JOIN cinema c ON c.id=cs.cinema_id

LEFT JOIN movies_schedule_list_patron mslp ON mslp.id=mslrs.patron_id
LEFT JOIN patrons p ON p.id=mslp.patron_id
LEFT JOIN (SELECT
	ps.*,
	SUM(sur.amount_val) `amount_val`
	FROM patrons_surcharge ps
	INNER JOIN surcharge_tbl sur ON sur.id=ps.surcharge_id AND sur.details LIKE '%FOOD%'
	GROUP BY ps.patron_id) i ON i.patron_id=p.id

LEFT JOIN (SELECT
	ps.*,
	SUM(sur.amount_val) `amount_val`
	FROM patrons_surcharge ps
	INNER JOIN surcharge_tbl sur ON sur.id=ps.surcharge_id AND sur.details LIKE '%SURCHARGE%'
	GROUP BY ps.patron_id) sur ON sur.patron_id=p.id

INNER JOIN config_table ct ON ct.system_code='001'
INNER JOIN report r ON r.id=1

#WHERE mslrs.id IS NOT NULL

#GROUP BY p.id
#ORDER BY c.in_order, p.`code`
;

















SELECT

`user`, `code`, `PATRON`, `CINEMA`, `PRICE`, COUNT(i.`QTY`) `QTY`,
SUM(i.`TOTALSALES`) `TOTALSALES`,

SUM(i.`TOTALORDINANCE`) `TOTALORDINANCE`, COUNT(i.`QTY`) * SUM(i.`TOTALSURCHARGE`) `TOTALSURCHARGE`, SUM(i.`GRANDTOTAL`) `GRANDTOTAL`, `movie_date`, `ticket_datetime`, `userid`, `system_value`, `reportname`, (COUNT(i.`QTY`) * `FOOD`) `FOOD`, `PatronId`, `CinemaOrdinal`




FROM `tbl_rp01` i
GROUP BY i.PatronId
ORDER BY i.CinemaOrdinal, i.`code`
;

END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
