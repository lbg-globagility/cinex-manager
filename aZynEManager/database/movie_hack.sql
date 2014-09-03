UPDATE movies_schedule SET movie_date = '2014-09-03' WHERE movie_date = '2014-08-14';
UPDATE movies_schedule_list SET start_time = CONCAT('2014-09-03 ',TIME(start_time)), end_time = CONCAT('2014-09-03 ',TIME(end_time))  WHERE start_time LIKE '2014-08-14%';

