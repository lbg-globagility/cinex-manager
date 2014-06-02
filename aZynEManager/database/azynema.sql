-- phpMyAdmin SQL Dump
-- version 3.5.1
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Jun 02, 2014 at 02:15 AM
-- Server version: 5.5.24-log
-- PHP Version: 5.3.13

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `azynema`
--

-- --------------------------------------------------------

--
-- Table structure for table `classification`
--

CREATE TABLE IF NOT EXISTS `classification` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `description` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=18 ;

--
-- Dumping data for table `classification`
--

INSERT INTO `classification` (`id`, `name`, `description`) VALUES
(2, 'COM', 'Comedy'),
(3, 'DRAMA', 'Drama'),
(4, 'ACT', 'Action'),
(5, 'ANIM', 'Animation'),
(6, 'TRL', 'Thriller'),
(7, 'SUS', 'Suspense'),
(8, 'ADV', 'Adventure'),
(9, 'SCIFI', 'Science Fiction'),
(10, 'FAN', 'Fantasy'),
(11, 'ROM', 'Romance'),
(12, 'HOR', 'Horror'),
(13, 'SPRT', 'Sports'),
(14, 'FAM', 'Family'),
(15, 'SUPER', 'Superhero'),
(16, 'SEXY COM', 'Sexy Comedy'),
(17, 'MUSIC', 'Musical');

-- --------------------------------------------------------

--
-- Table structure for table `distributor`
--

CREATE TABLE IF NOT EXISTS `distributor` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `code` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `share_perc` float NOT NULL,
  `contact_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=29 ;

--
-- Dumping data for table `distributor`
--

INSERT INTO `distributor` (`id`, `code`, `name`, `share_perc`, `contact_id`) VALUES
(2, 'WARNER', 'Warner Bros.', 50, NULL),
(3, 'COLUMBIA', 'Columbia Pictures', 50, NULL),
(4, 'STAR', 'Star Cinema', 50, NULL),
(5, 'VIVA', 'Viva Films', 50, NULL),
(6, 'SOLAR', 'Solar Films', 50, NULL),
(7, 'CINESTAR', 'Cinestar', 50, NULL),
(8, 'REGAL', 'Regal Films', 50, NULL),
(9, 'PIONEER', 'Pioneer Films', 50, NULL),
(10, 'SKY', 'Sky Films', 50, NULL),
(11, 'UIP', 'United International Pictures', 50, NULL),
(12, 'SEIKO', 'Seiko Films', 50, NULL),
(13, 'APT', 'APT Films', 50, NULL),
(14, 'KYE', 'Kye Films', 50, NULL),
(15, 'GMA', 'GMA Films', 50, NULL),
(16, 'OCTO', 'Octo Arts Films', 50, NULL),
(17, 'UNITEL', 'Unitel Pictures', 50, NULL),
(18, 'CM', 'CM FILMS', 50, NULL),
(19, 'VIOLETT', 'VIOLETT FILMS INC.', 50, NULL),
(20, 'DAVEN', 'DAVEN Prods.', 50, NULL),
(21, 'IMUS', 'IMUS PRODUCTION', 50, NULL),
(22, 'MAVERICK', 'MAVERICK', 50, NULL),
(23, 'COMGUILD', 'COMGUILD', 50, NULL),
(24, 'MEGA', 'MEGAVISION', 50, NULL),
(25, 'CANARY', 'CANARY FILMS', 50, NULL),
(26, 'WORLD ASIA', 'World Asia', 50, NULL),
(27, 'SPRING', 'SPRING FILM', 50, NULL),
(28, 'XXX', 'XXXX PRODCUTION', 50, 3);

-- --------------------------------------------------------

--
-- Table structure for table `movies`
--

CREATE TABLE IF NOT EXISTS `movies` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `code` varchar(100) NOT NULL,
  `title` varchar(255) NOT NULL,
  `dist_id` int(10) NOT NULL,
  `share_perc` float NOT NULL,
  `rating_id` int(10) NOT NULL,
  `duration` int(10) NOT NULL,
  `status` int(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `movies_class`
--

CREATE TABLE IF NOT EXISTS `movies_class` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `movie_id` int(10) NOT NULL,
  `class_id` int(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `mtrcb`
--

CREATE TABLE IF NOT EXISTS `mtrcb` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `description` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=7 ;

--
-- Dumping data for table `mtrcb`
--

INSERT INTO `mtrcb` (`id`, `name`, `description`) VALUES
(2, 'PG-13', 'Parental Guidance 13+'),
(3, 'GP', 'General Patronage'),
(4, 'R-13', 'Restricted 13+'),
(5, 'R-18', 'Restricted 18+'),
(6, 'SPG', 'STRICT PARENTAL GUIDANCE');

-- --------------------------------------------------------

--
-- Table structure for table `people`
--

CREATE TABLE IF NOT EXISTS `people` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `lname` varchar(255) NOT NULL,
  `fname` varchar(255) NOT NULL,
  `mname` varchar(255) NOT NULL,
  `position` varchar(255) NOT NULL,
  `contact_no` varchar(255) NOT NULL,
  `email_addr` varchar(255) NOT NULL,
  `address` varchar(255) NOT NULL,
  `city` varchar(255) NOT NULL,
  `country` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4 ;

--
-- Dumping data for table `people`
--

INSERT INTO `people` (`id`, `name`, `lname`, `fname`, `mname`, `position`, `contact_no`, `email_addr`, `address`, `city`, `country`) VALUES
(1, 'a a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a'),
(2, 'b b', 'b', 'b', 'b', 'b', 'b', 'b', 'b', 'b', 'b'),
(3, 'cac', 'c', 'ca', '', 'cg', 'ce', 'cf', 'cb', 'cc', 'cd');

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
