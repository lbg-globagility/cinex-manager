ALTER TABLE `movies_schedule_list_house_seat` ADD COLUMN `reserved_date` DATETIME NULL AFTER `notes`;

CREATE VIEW `movies_schedule_list_house_seat_view` AS
SELECT * FROM movies_schedule_list_house_seat WHERE 
TIME_TO_SEC(TIMEDIFF(NOW(), IFNULL(reserved_date, NOW())))  < 600
AND (movies_schedule_list_id, cinema_seat_id) NOT IN (SELECT movies_schedule_list_id, cinema_seat_id FROM movies_schedule_list_reserved_seat);

-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE DEFINER=`root`@`localhost` FUNCTION `CurrentDateTime`() RETURNS datetime
    NO SQL
BEGIN

RETURN NOW();
END

ALTER TABLE `movies_schedule_list` 
ADD COLUMN `laytime` INT NOT NULL DEFAULT 0 AFTER `seat_type`;

ALTER TABLE `movies_schedule_list_patron` 
ADD COLUMN `is_default` INT(1) NULL DEFAULT 0 AFTER `price`;

--set first entry as default 
UPDATE movies_schedule_list_patron a, 
(SELECT id FROM azynema.movies_schedule_list_patron GROUP BY movies_schedule_list_id) b
SET a.is_default = 1 
WHERE a.id = b.id;

ALTER TABLE `movies_schedule_list_house_seat` 
ADD COLUMN `session_id` VARCHAR(45) NULL AFTER `reserved_date`;

UPDATE cinema_seat SET y2= 105 WHERE object_type <> 1;
UPDATE cinema_seat SET y2 = y1+36 WHERE object_type = 1;

--hack
UPDATE  movies_schedule_list_reserved_seat SET cinema_seat_id = 1058 + (id - 22589) WHERE movies_schedule_list_id = 4349;

		
		
ALTER TABLE `movies_schedule_list_house_seat` 
ADD COLUMN `movies_schedule_list_patron_id` INT NOT NULL DEFAULT 0 AFTER `session_id`;


CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `movies_schedule_list_house_seat_view` AS
    select 
        `movies_schedule_list_house_seat`.`id` AS `id`,
        `movies_schedule_list_house_seat`.`movies_schedule_list_id` AS `movies_schedule_list_id`,
        `movies_schedule_list_house_seat`.`cinema_seat_id` AS `cinema_seat_id`,
        `movies_schedule_list_house_seat`.`full_name` AS `full_name`,
        `movies_schedule_list_house_seat`.`notes` AS `notes`,
        `movies_schedule_list_house_seat`.`reserved_date` AS `reserved_date`,
        `movies_schedule_list_house_seat`.`session_id` AS `session_id`,
		`movies_schedule_list_house_seat`.`movies_schedule_list_patron_id` AS `movies_schedule_list_patron_id`
    from
        `movies_schedule_list_house_seat`
    where
        ((time_to_sec(timediff(now(),
                        ifnull(`movies_schedule_list_house_seat`.`reserved_date`,
                                now()))) < 600)
            and (not ((`movies_schedule_list_house_seat`.`movies_schedule_list_id` , `movies_schedule_list_house_seat`.`cinema_seat_id`) in (select 
                `movies_schedule_list_reserved_seat`.`movies_schedule_list_id`,
                    `movies_schedule_list_reserved_seat`.`cinema_seat_id`
            from
                `movies_schedule_list_reserved_seat`))));

INSERT INTO cinema_seat SELECT 0, cinema_id, 0, 0, 0, 0, 0, 0, '', '', 3, 0 FROM cinema_seat GROUP BY cinema_id;
		
		
CREATE VIEW `movies_schedule_list_house_seat_free_view` AS
 select 
        `movies_schedule_list_house_seat`.`id` AS `id`,
        `movies_schedule_list_house_seat`.`movies_schedule_list_id` AS `movies_schedule_list_id`,
        `movies_schedule_list_house_seat`.`cinema_seat_id` AS `cinema_seat_id`,
        `movies_schedule_list_house_seat`.`full_name` AS `full_name`,
        `movies_schedule_list_house_seat`.`notes` AS `notes`,
        `movies_schedule_list_house_seat`.`reserved_date` AS `reserved_date`,
        `movies_schedule_list_house_seat`.`session_id` AS `session_id`,
        `movies_schedule_list_house_seat`.`movies_schedule_list_patron_id` AS `movies_schedule_list_patron_id`
    from
        `movies_schedule_list_house_seat`
    where
        ((time_to_sec(timediff(now(),
                        ifnull(`movies_schedule_list_house_seat`.`reserved_date`,
                                now()))) < 600)
AND movies_schedule_list_id IN (SELECT id FROM movies_schedule_list WHERE seat_type <> 1)
		);
