/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `rp27`;
DELIMITER //
CREATE PROCEDURE `rp27`(
	IN `_date` DATE,
	IN `_cinemaId` INT,
	IN `_movieId` INT,
	IN `_isWithUtilization` TINYINT,
	IN `_isExcelFormat` TINYINT
)
BEGIN

DROP TEMPORARY TABLE IF EXISTS `tbl_rp27`;
CREATE TEMPORARY TABLE IF NOT EXISTS `tbl_rp27`
SELECT

u.id `user`,
p.`code`,
p.`name` `PATRON`,
c.`name` `CINEMA`,
mslrs.base_price `PRICE`,
mslrs.cinema_seat_id `QTY`,
mslrs.base_price `TOTALSALES`,
mslrs.ordinance_price `TOTALORDINANCE`,
#IFNULL(ps.PremiumSeatTotal, 0) + IFNULL(ps.SurchargeTotal, 0) `TOTALSURCHARGE`,
IFNULL(ps.GrandTotalSurcharge, 0) - IFNULL(ps.FoodBundleTotal, 0) `TOTALSURCHARGE`,
(mslrs.base_price + mslrs.ordinance_price + mslrs.surcharge_price) `GRANDTOTAL`,
ms.movie_date,
t.ticket_datetime,
u.userid,
ct.system_value,
r.`name` `reportname`,
IFNULL(ps.FoodBundleTotal, 0) `FOOD`,
p.id `PatronId`,
c.in_order `CinemaOrdinal`,
mslrs.surcharge_price,
m.title `MovieTitle`,
msl.start_time,
msl.end_time,
ms.cinema_id

FROM movies_schedule_list_reserved_seat mslrs
INNER JOIN movies_schedule_list msl ON msl.id=mslrs.movies_schedule_list_id
INNER JOIN movies_schedule ms ON ms.id=msl.movies_schedule_id #AND DATE(ms.movie_date)=_date #STR_TO_DATE('11/20/2024', '%c/%e/%Y')
INNER JOIN movies m ON m.id=ms.movie_id
INNER JOIN ticket t ON t.id=mslrs.ticket_id AND DATE(t.ticket_datetime)=_date AND DATE(msl.start_time) > DATE(t.ticket_datetime)
INNER JOIN users u ON u.id=t.user_id #AND u.userid IN (_userId)

INNER JOIN cinema_seat cs ON cs.id=mslrs.cinema_seat_id
INNER JOIN cinema c ON c.id=cs.cinema_id

LEFT JOIN movies_schedule_list_patron mslp ON mslp.id=mslrs.patron_id
LEFT JOIN patrons p ON p.id=mslp.patron_id
LEFT JOIN (SELECT
	ps.*,
	SUM(sc.amount_val) `GrandTotalSurcharge`,
	SUM(IFNULL(food.amount_val, 0)) `FoodBundleTotal`,
	SUM(IFNULL(prem.amount_val, 0)) `PremiumSeatTotal`,
	SUM(IFNULL(sur.amount_val, 0)) `SurchargeTotal`
	FROM patrons_surcharge ps
	INNER JOIN surcharge_tbl sc ON sc.id=ps.surcharge_id
	LEFT JOIN surcharge_tbl food ON food.id=ps.surcharge_id AND food.details LIKE '%FOOD%'
	LEFT JOIN surcharge_tbl prem ON prem.id=ps.surcharge_id AND prem.details LIKE '%PREMIUM SEAT%'
	LEFT JOIN surcharge_tbl sur ON sur.id=ps.surcharge_id AND sur.details='SURCHARGE' AND sur.`code`='SCHARGE'
	GROUP BY ps.patron_id) ps ON ps.patron_id=p.id

#LEFT JOIN surcharge_tbl sur ON sur.details='SURCHARGE' AND sur.`code`='SCHARGE' AND p.with_surcharge=TRUE #AND IFNULL(ps.`SurchargeTotal`, 0) > 0

INNER JOIN config_table ct ON ct.system_code='001'
INNER JOIN report r ON r.id=27

WHERE mslrs.`status`=1
;

IF _isExcelFormat THEN
		
	SELECT
	`PATRON` `Patron`,
	COUNT(i.`QTY`) `Qty`,
	`PRICE` `Price`,
	SUM(i.`TOTALSALES`) `Sales`,
	`start_time` `StartTime`,
	`end_time` `EndTime`,
	`system_value` `SystemVal`,
	`reportname` `ReportName`,
	`CINEMA` `CinemaName`,
	`MovieTitle` `Title`
	FROM `tbl_rp27` i
	GROUP BY DATE(i.`start_time`), i.`cinema_id`, i.`PatronId`
	ORDER BY i.CinemaOrdinal, i.`code`
	;

ELSE
		
	SELECT
	`PATRON`,
	COUNT(i.`QTY`) `QTY`,
	`PRICE`,
	SUM(i.`TOTALSALES`) `SALES`,
	`start_time` START_TIME,
	`end_time` END_TIME,
	`system_value` `SYSTEM_VAL`,
	`reportname` `REPORT_NAME`,
	`CINEMA` `CINEMA_NAME`,
	`MovieTitle` `TITLE`,
	`ticket_datetime` `Date(j.ticket_datetime)`
	FROM `tbl_rp27` i
	GROUP BY DATE(i.`start_time`), i.`cinema_id`, i.`PatronId`
	ORDER BY i.CinemaOrdinal, i.`code`
	;

END IF;

END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
