-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE PROCEDURE `retrieve_tellers`()
BEGIN
SELECT id, userid, full_name, user_id IS NOT NULL FROM 
	(SELECT a.id, userid, CONCAT(fname, ' ', mname, '. ',  lname) full_name FROM users a, user_level b WHERE  
	a.user_level_id = b.id AND a.system_code = 2 AND b.system_code  = 2) c 
	LEFT OUTER JOIN
	(SELECT user_id FROM ticket GROUP BY user_id) d 
	ON c.id = user_id ORDER BY userid;
END