-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE PROCEDURE `retrieve_distributors`()
BEGIN
	SELECT id, code, name FROM distributor ORDER BY code;
END