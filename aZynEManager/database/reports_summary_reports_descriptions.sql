-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE PROCEDURE `reports_summary_reports_descriptions`()
BEGIN
	SELECT `key`, code, name, description FROM cinema.reports ORDER BY code;
END