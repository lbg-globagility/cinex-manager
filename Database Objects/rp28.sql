/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `rp28`;
DELIMITER //
CREATE PROCEDURE `rp28`(
	IN `datefrom` DATETIME,
	IN `dateto` DATETIME,
	IN `cinemaIds` TEXT,
	IN `usernames` TEXT,
	IN `terminals` TEXT,
	IN `patronIds` TEXT
)
BEGIN

SELECT

/*
teller name
pos #
date & time
patron
cinema #
title
qty
amount
*/

u.userid `teller`,
t.terminal `pos_num`,
t.ticket_datetime `datetime`,
mslrs.or_number `si_num`,
p.`name` `patron_name`,
REPLACE(c.`name`, 'Cinema_', '') `cinema_name`,
m.`code` `movie_title`,
1 `qty`,
mslrs.price `amount`
#mslrs.*, t.*, s.*

FROM movies_schedule_list_reserved_seat mslrs

INNER JOIN ticket t ON t.id=mslrs.ticket_id AND DATE(t.ticket_datetime) BETWEEN datefrom AND dateto
AND IF(IFNULL(terminals, '')='', TRUE, FIND_IN_SET(t.terminal, terminals) > 0)
INNER JOIN `session` s ON s.session_id=t.session_id

INNER JOIN movies_schedule_list msl ON msl.id=mslrs.movies_schedule_list_id
INNER JOIN movies_schedule ms ON ms.id=msl.movies_schedule_id
INNER JOIN cinema c ON c.id=ms.cinema_id AND IF(IFNULL(cinemaIds, '')='', TRUE, FIND_IN_SET(c.id, cinemaIds) > 0)

INNER JOIN movies m ON m.id=ms.movie_id

INNER JOIN users u ON u.id=t.user_id AND IF(IFNULL(usernames, '')='', TRUE, FIND_IN_SET(u.userid, usernames) > 0)

INNER JOIN movies_schedule_list_patron mslp ON mslp.movies_schedule_list_id=mslrs.movies_schedule_list_id AND mslp.id=mslrs.patron_id
AND IF(IFNULL(patronIds, '')='', TRUE, FIND_IN_SET(mslp.patron_id, patronIds) > 0)

INNER JOIN patrons p ON p.id=mslp.patron_id

WHERE mslrs.void_datetime IS NULL

ORDER BY t.ticket_datetime
;

END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
