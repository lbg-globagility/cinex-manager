-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE PROCEDURE `retrieve_tellers`()
BEGIN
	SELECT uname, IFNULL(full_name,  CONCAT(uname, ' (Inactive)')) FROM 
	(select username uname from ticket_transactions 
	GROUP BY username) d
	LEFT OUTER JOIN
	( 
	SELECT username, full_name FROM users a, access_groups b WHERE a.access_group_key = b.key
	AND code = 'TELLER') c
	ON uname = username
	ORDER BY uname;
END