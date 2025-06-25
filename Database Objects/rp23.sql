/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `rp23`;
DELIMITER //
CREATE PROCEDURE `rp23`(
	IN `_date` DATE,
	IN `_userId` TEXT
)
BEGIN

DECLARE _mayaEwalletIds TEXT DEFAULT '';
DECLARE _gcashEwalletIds TEXT DEFAULT '';

SET _mayaEwalletIds=(SELECT GROUP_CONCAT(CONVERT(ew.id, CHAR)) FROM ewallets ew WHERE ew.`name` LIKE '%MAYA%');
SET _gcashEwalletIds=(SELECT GROUP_CONCAT(CONVERT(ew.id, CHAR)) FROM ewallets ew WHERE ew.`name` LIKE '%GCASH%');

SET @customDateFormat='%Y%m';
#SET @customDateFormat='%Y%m%d';

#SET @sessionId=NULL;
#SET @sessionId='20241101-122601-239';

SET @_systemValue=(SELECT c.system_value FROM config_table c WHERE c.system_code='001' LIMIT 1);

SET @_reportName=(SELECT r.`name` FROM report r WHERE r.id=23 LIMIT 1);

SELECT
NULL `id`,
DATE(t.ticket_datetime) `report_date`,
COUNT(s.session_id) + COUNT(IFNULL(cc.session_id, 0)) `total_cnt`,
SUM(IF(IFNULL(s.payment_mode, cc.payment_mode)=4, 0, mslrs.`base_price`)) `total_cash`,
SUM(IFNULL(s.gift_certificate_amount, 0)) `total_gc`,
SUM(IF(IFNULL(s.payment_mode, cc.payment_mode)=4, mslrs.`base_price`, 0)) `total_cc`, #SUM(mslrs.base_price)
_userId `userid`,
@_systemValue `system_value`,
@_reportName `report_name`,
SUM(IF(maya.id IS NULL, 0, mslrs.base_price)) `total_ewallet_maya`,
SUM(IF(gcash.id IS NULL, 0, mslrs.base_price)) `total_ewallet_gcash`

FROM movies_schedule_list_reserved_seat mslrs
INNER JOIN movies_schedule_list msl ON msl.id=mslrs.movies_schedule_list_id AND msl.`status`=1
INNER JOIN movies_schedule ms ON ms.id=msl.movies_schedule_id

INNER JOIN ticket t ON t.id=mslrs.ticket_id AND DATE_FORMAT(t.ticket_datetime, @customDateFormat)=DATE_FORMAT(STR_TO_DATE(_date, '%Y-%m-%d'), @customDateFormat)
LEFT JOIN `session` s ON s.session_id=t.session_id AND s.payment_mode IN (1,2,3)
LEFT JOIN `session` cc ON cc.session_id=t.session_id AND cc.payment_mode=4
#AND s.session_id=IFNULL(@sessionId, s.session_id)

LEFT JOIN session_ewallet maya ON maya.session_id=s.session_id AND FIND_IN_SET(maya.ewallet_id, _mayaEwalletIds) > 0

LEFT JOIN session_ewallet gcash ON gcash.session_id=s.session_id AND FIND_IN_SET(gcash.ewallet_id, _gcashEwalletIds) > 0

WHERE mslrs.`status`=1

GROUP BY DATE(t.ticket_datetime)
ORDER BY t.ticket_datetime
;

END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
