-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE PROCEDURE `retrieve_cinemas`()
BEGIN
	SELECT id, name FROM cinema ORDER BY in_order;
END