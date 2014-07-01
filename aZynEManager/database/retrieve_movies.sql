-- --------------------------------------------------------------------------------
-- Routine DDL
-- Note: comments before and after the routine body will not be stored by the server
-- --------------------------------------------------------------------------------
DELIMITER $$

CREATE PROCEDURE `retrieve_movies`(_movie_date DATETIME, _cinema_id INT)
BEGIN
	SELECT a.id, code, title FROM movies a, movies_schedule b WHERE a.id = b.movie_id AND movie_date = _movie_date AND cinema_id = _cinema_id;
END