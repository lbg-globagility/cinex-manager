-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE PROCEDURE `retrieve_tellers_ticket`(_ticket_date DATETIME)
BEGIN
	SELECT a.id, userid, CONCAT(fname, ' ', mname, '. ',  lname), true FROM users a, ticket b WHERE a.id = user_id AND DATE(ticket_datetime) = _ticket_date
	AND a.system_code = 2 GROUP BY a.id ORDER BY userid;
END