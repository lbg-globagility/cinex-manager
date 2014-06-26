/*sound_system*/
INSERT INTO azynema.sound_system SELECT 0, sound_system FROM cinema.cinemas GROUP BY sound_system;

/*cinema*/
INSERT INTO azynema.cinema SELECT `key`, name, azynema.sound_system.id, capacity, display_order FROM cinema.cinemas, azynema.sound_system WHERE sound_system.sound_system_type = cinema.cinemas.sound_system;

/*patrons*/
INSERT INTO azynema.patrons SELECT `key`, code, `name`, price, promo, taxable, apply_cultural_tax, apply_lgu_tax, !disregard_gross_margin, !disregard_producer_share, color, position, lgu_tax_amount  FROM cinema.patrons;

/*cinema_patron*/
INSERT INTO azynema.cinema_patron SELECT * FROM cinema.cinema_patrons;

/*classification*/
INSERT INTO azynema.classification SELECT `key`, `code`, `name` FROM cinema.movie_classifications;

/*distributor*/
INSERT INTO azynema.distributor SELECT `key`, `code`, `name`, share_rate, NULL FROM cinema.distributors;

/*people*/
INSERT INTO azynema.people SELECT 0, contact_person, '', '', '', '', phone_no, email, address, '', '' FROM cinema.distributors GROUP BY contact_person;
UPDATE azynema.distributor, azynema.people, cinema.distributors SET azynema.distributor.contact_id = people.id WHERE azynema.distributor.id = cinema.distributors.key AND cinema.distributors.contact_person = azynema.people.name; 

/*mtrcb*/
INSERT INTO azynema.mtrcb SELECT `key`, `code`, `name` FROM cinema.mtrcb_ratings;

/*movies*/
INSERT INTO azynema.movies SELECT `key`, code, name, distributor_key, distributor_share_rate, mtrcb_rating_key, running_time, active FROM cinema.movie_database;

/*movies_class*/
INSERT INTO azynema.movies_class SELECT `key`, movie_database_key, movie_classification_key FROM cinema.movie_database_movie_classifications;

/*movies_distributor*/
INSERT INTO azynema.movies_distributor SELECT `key`, movie_database_key, share_rate, effective_date FROM cinema.movie_database_distributor_share;

/*movies_schedule*/
INSERT INTO azynema.movies_schedule SELECT `key`, cinema_key, movie_key, screening_start_date FROM cinema.movie_calendar;

/*movies_schedule_list*/
INSERT INTO azynema.movies_schedule_list SELECT `key`, movie_calendar_key, start_time, end_time, screening_type  FROM cinema.movie_calendar_times;

/*movies_schedule_list_patrons*/
SET foreign_key_checks = 0;
INSERT INTO azynema.movies_schedule_list_patron SELECT `key`, movie_calendar_time_key, patron_key, price FROM cinema.movie_calendar_time_patrons;*/

/*movies_schedule_list_house_seat*/
INSERT INTO  azynema.movies_schedule_list_house_seat SELECT 0, movie_calendar_time_key, cinema_seat_key, full_name, notes FROM cinema.movie_calendar_time_house_seats;

/*movies_schedule_list_reserved_seat*/
INSERT INTO  azynema.movies_schedule_list_reserved_seat SELECT `key`, movie_calendar_time_key, cinema_seat_key, ticket_transaction_key, patron_key, price, `status`, amusement_tax_amount, cultural_tax_amount, VAT_amount, or_number, void_user, void_datetime  FROM cinema.movie_calendar_time_reserved_seats;

/*ticket*/
INSERT INTO azynema.ticket SELECT `key`, movie_calendar_times_key, username, terminal, `datetime`, sessionid, `status` FROM cinema.ticket_transactions;
SET foreign_key_checks = 1;

/*cinema_seat*/
INSERT INTO azynema.cinema_seat SELECT `key`, cinema_key, p1x, p1y, p3x, p3y, originx, originy, '', '', object_type,  handicapped from cinema.cinema_seats WHERE object_type = 2;
INSERT INTO azynema.cinema_seat SELECT `key`, cinema_key, p1x, p1y, p2x, p2y, originx, originy, `row`, `column`, object_type,  handicapped from cinema.cinema_seats WHERE object_type = 1;
