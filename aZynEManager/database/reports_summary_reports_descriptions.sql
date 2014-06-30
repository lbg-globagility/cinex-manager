-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE PROCEDURE `reports_summary_reports_descriptions`()
BEGIN
	SELECT id, code, name, description FROM report ORDER BY code;
END