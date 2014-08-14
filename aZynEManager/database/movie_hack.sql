UPDATE movies_schedule SET movie_date = '2014-08-12' WHERE movie_date = '2007-01-06';
UPDATE movies_schedule_list SET start_time = CONCAT('2014-08-12 ',TIME(start_time)), end_time = CONCAT('2014-08-12 ',TIME(end_time))  WHERE start_time LIKE '2007-01-06%';

