-- MySQL dump 10.13  Distrib 5.6.13, for Win32 (x86)
--
-- Host: localhost    Database: azynema
-- ------------------------------------------------------
-- Server version	5.5.24-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `a_trail`
--

DROP TABLE IF EXISTS `a_trail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `a_trail` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` varchar(255) NOT NULL,
  `tr_date` datetime NOT NULL,
  `module_code` int(11) NOT NULL,
  `aff_table_layer` varchar(255) NOT NULL,
  `computer_name` varchar(255) NOT NULL,
  `tr_details` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `a_trail`
--

LOCK TABLES `a_trail` WRITE;
/*!40000 ALTER TABLE `a_trail` DISABLE KEYS */;
INSERT INTO `a_trail` VALUES (1,'ADMIN','2014-10-10 10:10:10',1,'USERS, USERS_RIGHTS','ACER-01','ADDED A NEW USER: LILOY'),(2,'ADMIN','2014-10-11 00:00:00',1,'USERS, USERS_RIGHTS','ACER-01','ADDED A NEW USER: LILOY'),(3,'ADMIN','2014-10-10 00:00:00',1,'USERS, USERS_RIGHTS','ACER-01','ADDED A NEW USER: LILOY');
/*!40000 ALTER TABLE `a_trail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `application`
--

DROP TABLE IF EXISTS `application`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `application` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `system_code` int(1) NOT NULL,
  `system_name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `application`
--

LOCK TABLES `application` WRITE;
/*!40000 ALTER TABLE `application` DISABLE KEYS */;
INSERT INTO `application` VALUES (1,1,'CINEMA MANAGER');
/*!40000 ALTER TABLE `application` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cinema`
--

DROP TABLE IF EXISTS `cinema`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cinema` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(225) NOT NULL,
  `sound_id` int(1) NOT NULL,
  `capacity` int(11) DEFAULT NULL,
  `in_order` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cinema`
--

LOCK TABLES `cinema` WRITE;
/*!40000 ALTER TABLE `cinema` DISABLE KEYS */;
INSERT INTO `cinema` VALUES (2,'Cinema 1',1,262,1),(3,'Cinema 2',1,262,2),(4,'Cinema 3',1,259,3),(5,'Cinema 4',1,262,4);
/*!40000 ALTER TABLE `cinema` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cinema_patron`
--

DROP TABLE IF EXISTS `cinema_patron`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cinema_patron` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cinema_id` int(11) NOT NULL,
  `patron_id` int(11) NOT NULL,
  `price` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=128 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cinema_patron`
--

LOCK TABLES `cinema_patron` WRITE;
/*!40000 ALTER TABLE `cinema_patron` DISABLE KEYS */;
INSERT INTO `cinema_patron` VALUES (53,1,7,160),(54,1,8,0),(55,1,9,88),(56,1,10,5),(57,1,11,0),(58,5,27,5),(59,5,4,5),(60,5,12,0),(61,5,5,0),(62,5,28,5),(63,5,41,160),(64,5,13,140),(65,5,34,150),(66,5,42,128),(67,5,30,112),(68,5,14,112),(69,5,35,120),(70,5,2,0),(71,5,32,130),(72,5,33,104),(73,2,27,5),(74,2,4,5),(75,2,8,0),(76,2,12,0),(77,2,5,0),(78,2,43,160),(79,2,39,600),(80,2,28,5),(81,2,41,160),(82,2,13,140),(83,2,34,150),(84,2,42,128),(85,2,14,112),(86,2,35,120),(87,2,2,0),(88,2,22,600),(89,2,23,480),(90,2,32,130),(91,2,33,104),(92,2,40,480),(93,4,27,5),(94,4,4,5),(95,4,12,0),(96,4,5,0),(97,4,39,600),(98,4,28,5),(99,4,29,140),(100,4,41,160),(101,4,13,140),(102,4,34,150),(103,4,42,128),(104,4,14,112),(105,4,35,120),(106,4,2,0),(107,4,32,130),(108,4,33,104),(109,3,27,5),(110,3,4,5),(111,3,8,0),(112,3,12,0),(113,3,5,0),(114,3,39,600),(115,3,28,5),(116,3,41,160),(117,3,13,140),(118,3,34,150),(119,3,42,128),(120,3,14,112),(121,3,35,120),(122,3,2,0),(123,3,22,600),(124,3,23,480),(125,3,32,130),(126,3,33,104),(127,3,40,480);
/*!40000 ALTER TABLE `cinema_patron` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `classification`
--

DROP TABLE IF EXISTS `classification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `classification` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `description` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `classification`
--

LOCK TABLES `classification` WRITE;
/*!40000 ALTER TABLE `classification` DISABLE KEYS */;
INSERT INTO `classification` VALUES (2,'COM','Comedy'),(3,'DRAMA','Drama'),(4,'ACT','Action'),(5,'ANIM','Animation'),(6,'TRL','Thriller'),(7,'SUS','Suspense'),(8,'ADV','Adventure'),(9,'SCIFI','Science Fiction'),(10,'FAN','Fantasy'),(11,'ROM','Romance'),(12,'HOR','Horror'),(13,'SPRT','Sports'),(14,'FAM','Family'),(15,'SUPER','Superhero'),(16,'SEXY COM','Sexy Comedy'),(17,'MUSIC','Musical');
/*!40000 ALTER TABLE `classification` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `distributor`
--

DROP TABLE IF EXISTS `distributor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `distributor` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `code` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `share_perc` float NOT NULL,
  `contact_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `distributor`
--

LOCK TABLES `distributor` WRITE;
/*!40000 ALTER TABLE `distributor` DISABLE KEYS */;
INSERT INTO `distributor` VALUES (2,'WARNER','Warner Bros.',50,NULL),(3,'COLUMBIA','Columbia Pictures',50,NULL),(4,'STAR','Star Cinema',50,NULL),(5,'VIVA','Viva Films',50,NULL),(6,'SOLAR','Solar Films',50,NULL),(8,'REGAL','Regal Films',50,NULL),(9,'PIONEER','Pioneer Films',50,NULL),(10,'SKY','Sky Films',50,NULL),(11,'UIP','United International Pictures',50,NULL),(12,'SEIKO','Seiko Films',50,NULL),(13,'APT','APT Films',50,NULL),(14,'KYE','Kye Films',50,NULL),(15,'GMA','GMA Films',50,NULL),(16,'OCTO','Octo Arts Films',50,NULL),(17,'UNITEL','Unitel Pictures',50,NULL),(18,'CM','CM FILMS',50,NULL),(19,'VIOLETT','VIOLETT FILMS INC.',50,NULL),(20,'DAVEN','DAVEN Prods.',50,NULL),(21,'IMUS','IMUS PRODUCTION',50,NULL),(22,'MAVERICK','MAVERICK',50,NULL),(23,'COMGUILD','COMGUILD',50,NULL),(24,'MEGA','MEGAVISION',50,NULL),(25,'CANARY','CANARY FILMS',50,NULL),(26,'WORLD ASIA','World Asia',50,3),(27,'SPRING','SPRING FILM',50,NULL),(28,'XXX','XXXX PRODCUTION',50,3),(30,'a','a',20,5);
/*!40000 ALTER TABLE `distributor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movies`
--

DROP TABLE IF EXISTS `movies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `movies` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `code` varchar(100) NOT NULL,
  `title` varchar(255) NOT NULL,
  `dist_id` int(10) NOT NULL,
  `share_perc` float NOT NULL,
  `rating_id` int(10) NOT NULL,
  `duration` int(10) NOT NULL,
  `status` int(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movies`
--

LOCK TABLES `movies` WRITE;
/*!40000 ALTER TABLE `movies` DISABLE KEYS */;
INSERT INTO `movies` VALUES (1,'A','title cinema 1',2,50,2,125,0),(3,'C','C',27,40,5,180,0),(4,'D','D',23,50,6,120,0),(5,'DFASG','ASGASD',21,46,3,120,0);
/*!40000 ALTER TABLE `movies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movies_class`
--

DROP TABLE IF EXISTS `movies_class`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `movies_class` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `movie_id` int(10) NOT NULL,
  `class_id` int(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movies_class`
--

LOCK TABLES `movies_class` WRITE;
/*!40000 ALTER TABLE `movies_class` DISABLE KEYS */;
INSERT INTO `movies_class` VALUES (1,3,15),(2,3,7),(3,3,6),(4,4,15),(5,4,7),(6,1,4),(7,1,8),(8,1,5),(29,5,4);
/*!40000 ALTER TABLE `movies_class` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movies_distributor`
--

DROP TABLE IF EXISTS `movies_distributor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `movies_distributor` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `movie_id` int(11) NOT NULL,
  `share_perc` float NOT NULL,
  `effective_date` date NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movies_distributor`
--

LOCK TABLES `movies_distributor` WRITE;
/*!40000 ALTER TABLE `movies_distributor` DISABLE KEYS */;
INSERT INTO `movies_distributor` VALUES (1,1,92,'2010-05-10'),(2,5,82,'2014-05-01'),(3,5,82,'2014-06-04'),(4,3,20,'2014-06-04'),(6,3,50,'2014-10-30'),(7,5,444,'2014-06-04');
/*!40000 ALTER TABLE `movies_distributor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movies_schedule`
--

DROP TABLE IF EXISTS `movies_schedule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `movies_schedule` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cinema_id` int(11) NOT NULL,
  `movie_id` int(11) NOT NULL,
  `movie_date` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movies_schedule`
--

LOCK TABLES `movies_schedule` WRITE;
/*!40000 ALTER TABLE `movies_schedule` DISABLE KEYS */;
INSERT INTO `movies_schedule` VALUES (27,2,1,'2014-06-08 00:00:00'),(28,2,1,'2014-06-09 00:00:00'),(29,2,1,'2014-06-10 00:00:00'),(30,2,1,'2014-06-11 00:00:00'),(31,2,1,'2014-06-12 00:00:00'),(32,2,1,'2014-06-13 00:00:00'),(33,2,1,'2014-06-14 00:00:00'),(34,2,5,'2014-06-08 00:00:00'),(35,2,5,'2014-06-09 00:00:00'),(36,2,5,'2014-06-10 00:00:00'),(37,2,5,'2014-06-11 00:00:00'),(38,2,5,'2014-06-12 00:00:00'),(39,2,5,'2014-06-13 00:00:00'),(40,2,5,'2014-06-14 00:00:00'),(41,2,3,'2014-06-09 00:00:00'),(42,2,3,'2014-06-08 00:00:00'),(43,5,4,'2014-07-01 00:00:00');
/*!40000 ALTER TABLE `movies_schedule` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movies_schedule_list`
--

DROP TABLE IF EXISTS `movies_schedule_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `movies_schedule_list` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `movies_schedule_id` int(11) NOT NULL,
  `start_time` datetime NOT NULL,
  `end_time` datetime NOT NULL,
  `seat_type` int(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movies_schedule_list`
--

LOCK TABLES `movies_schedule_list` WRITE;
/*!40000 ALTER TABLE `movies_schedule_list` DISABLE KEYS */;
INSERT INTO `movies_schedule_list` VALUES (42,27,'2014-06-08 10:00:00','2014-06-08 12:05:00',2),(44,29,'2014-06-10 10:00:00','2014-06-10 12:05:00',2),(45,30,'2014-06-11 10:00:00','2014-06-11 12:05:00',2),(46,31,'2014-06-12 10:00:00','2014-06-12 12:05:00',2),(47,32,'2014-06-13 10:00:00','2014-06-13 12:05:00',2),(48,33,'2014-06-14 10:00:00','2014-06-14 12:05:00',2),(50,35,'2014-06-09 00:00:00','2014-06-09 02:00:00',2),(51,36,'2014-06-10 00:00:00','2014-06-10 02:00:00',2),(52,37,'2014-06-11 00:00:00','2014-06-11 02:00:00',2),(53,38,'2014-06-12 00:00:00','2014-06-12 02:00:00',2),(54,39,'2014-06-13 00:00:00','2014-06-13 02:00:00',2),(55,40,'2014-06-14 00:00:00','2014-06-14 02:00:00',2),(57,35,'2014-06-09 02:30:00','2014-06-09 04:30:00',1),(58,36,'2014-06-10 02:30:00','2014-06-10 04:30:00',1),(59,37,'2014-06-11 02:30:00','2014-06-11 04:30:00',1),(60,38,'2014-06-12 02:30:00','2014-06-12 04:30:00',1),(61,39,'2014-06-13 02:30:00','2014-06-13 04:30:00',1),(62,40,'2014-06-14 02:30:00','2014-06-14 04:30:00',1),(64,28,'2014-06-09 21:30:00','2014-06-09 23:30:00',3),(65,29,'2014-06-10 21:30:00','2014-06-10 23:30:00',3),(66,30,'2014-06-11 21:30:00','2014-06-11 23:30:00',3),(67,31,'2014-06-12 21:30:00','2014-06-12 23:30:00',3),(68,32,'2014-06-13 21:30:00','2014-06-13 23:30:00',3),(69,33,'2014-06-14 21:30:00','2014-06-14 23:30:00',3);
/*!40000 ALTER TABLE `movies_schedule_list` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movies_schedule_list_patron`
--

DROP TABLE IF EXISTS `movies_schedule_list_patron`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `movies_schedule_list_patron` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `movies_schedule_list_id` int(11) NOT NULL,
  `patron_id` int(11) NOT NULL,
  `price` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=227 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movies_schedule_list_patron`
--

LOCK TABLES `movies_schedule_list_patron` WRITE;
/*!40000 ALTER TABLE `movies_schedule_list_patron` DISABLE KEYS */;
INSERT INTO `movies_schedule_list_patron` VALUES (118,42,8,0),(119,42,41,160),(120,42,13,140),(121,42,34,150),(122,43,8,0),(123,43,41,160),(124,43,13,140),(125,43,34,150),(126,44,8,0),(127,44,41,160),(128,44,13,140),(129,44,34,150),(130,45,8,0),(131,45,41,160),(132,45,13,140),(133,45,34,150),(134,46,8,0),(135,46,41,160),(136,46,13,140),(137,46,34,150),(138,47,8,0),(139,47,41,160),(140,47,13,140),(141,47,34,150),(142,48,8,0),(143,48,41,160),(144,48,13,140),(145,48,34,150),(146,49,8,0),(147,49,43,160),(148,49,39,600),(149,49,28,5),(150,50,8,0),(151,50,43,160),(152,50,39,600),(153,50,28,5),(154,51,8,0),(155,51,43,160),(156,51,39,600),(157,51,28,5),(158,52,8,0),(159,52,43,160),(160,52,39,600),(161,52,28,5),(162,53,8,0),(163,53,43,160),(164,53,39,600),(165,53,28,5),(166,54,8,0),(167,54,43,160),(168,54,39,600),(169,54,28,5),(170,55,8,0),(171,55,43,160),(172,55,39,600),(173,55,28,5),(178,57,43,160),(179,57,28,5),(180,57,41,160),(181,57,13,140),(182,58,43,160),(183,58,28,5),(184,58,41,160),(185,58,13,140),(186,59,43,160),(187,59,28,5),(188,59,41,160),(189,59,13,140),(190,60,43,160),(191,60,28,5),(192,60,41,160),(193,60,13,140),(194,61,43,160),(195,61,28,5),(196,61,41,160),(197,61,13,140),(198,62,43,160),(199,62,28,5),(200,62,41,160),(201,62,13,140),(202,63,8,0),(203,63,39,600),(204,63,41,160),(205,64,8,0),(206,64,39,600),(207,64,41,160),(208,65,8,0),(209,65,39,600),(210,65,41,160),(211,66,8,0),(212,66,39,600),(213,66,41,160),(214,67,8,0),(215,67,39,600),(216,67,41,160),(217,68,8,0),(218,68,39,600),(219,68,41,160),(220,69,8,0),(221,69,39,600),(222,69,41,160),(223,0,43,160),(224,0,28,5),(225,70,32,130),(226,70,13,140);
/*!40000 ALTER TABLE `movies_schedule_list_patron` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movies_status`
--

DROP TABLE IF EXISTS `movies_status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `movies_status` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `status_id` int(11) NOT NULL,
  `status_desc` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movies_status`
--

LOCK TABLES `movies_status` WRITE;
/*!40000 ALTER TABLE `movies_status` DISABLE KEYS */;
INSERT INTO `movies_status` VALUES (1,0,'NEW'),(2,1,'ACTIVE'),(3,2,'CANCELLED');
/*!40000 ALTER TABLE `movies_status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mtrcb`
--

DROP TABLE IF EXISTS `mtrcb`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mtrcb` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `description` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mtrcb`
--

LOCK TABLES `mtrcb` WRITE;
/*!40000 ALTER TABLE `mtrcb` DISABLE KEYS */;
INSERT INTO `mtrcb` VALUES (2,'PG-13','Parental Guidance 13+'),(3,'GP','General Patronage'),(4,'R-13','Restricted 13+'),(5,'R-18','Restricted 18+'),(6,'SPG','STRICT PARENTAL GUIDANCE'),(10,'X','XXX'),(11,'yy','yyyyyy');
/*!40000 ALTER TABLE `mtrcb` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `patrons`
--

DROP TABLE IF EXISTS `patrons`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `patrons` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `unit_price` float NOT NULL,
  `with_promo` int(1) DEFAULT NULL,
  `with_amusement` int(1) DEFAULT NULL,
  `with_cultural` int(1) DEFAULT NULL,
  `with_citytax` int(1) DEFAULT NULL,
  `with_gross_margin` int(1) DEFAULT NULL,
  `with_prod_share` int(1) DEFAULT NULL,
  `seat_color` int(32) DEFAULT NULL,
  `seat_position` int(11) DEFAULT NULL,
  `lgutax` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `patrons`
--

LOCK TABLES `patrons` WRITE;
/*!40000 ALTER TABLE `patrons` DISABLE KEYS */;
INSERT INTO `patrons` VALUES (7,'REG','Regular - 160',160,0,1,1,0,0,0,65408,1,0),(8,'SC-MKTI','Senior Citizen Makati - 0',0,0,0,0,0,0,0,33023,16,0),(9,'SC-REG','Senior Citizen Regular - 88',88,0,1,1,0,0,0,65535,2,0),(10,'C2K PASS','COMPANY PASS - 5',5,0,0,1,0,0,1,255,12,0),(11,'MTRCB','MTRCB Deputies - 0',0,0,0,0,0,0,0,16711680,14,0),(12,'SP-REG','Special Regular - 500',500,0,1,1,0,0,0,65408,16,0),(13,'SP-SC','Special Senior - 400',400,0,1,1,0,0,0,33023,17,0),(14,'COMPLI','Complimentary Pass - 0',0,0,0,0,0,0,1,16744576,28,0),(15,'MMFF PASS','MMFF PASS  - 10',10,0,0,1,0,0,1,16744576,15,0),(16,'SPE-REG','Spec Regular-122',122,0,1,1,0,0,0,65280,3,0),(17,'SPE-SC REG','Spec Senior Regular-96',96,0,1,1,0,0,0,65535,4,0),(18,'DIS','Disabled- 0',0,0,0,0,0,0,0,4194432,17,0),(19,'REG-FREE','Regular Free Seating - 140',140,0,1,1,0,0,0,65280,7,0),(20,'SC REG-FREE','Senior Regular Free Seats - 112',112,0,1,1,0,0,0,65535,8,0),(21,'TEST','TESTING',0,0,0,0,0,0,0,12632256,18,0),(22,'SPEC-REG','Special Regular-121',121,0,1,1,0,0,0,65280,5,0),(23,'SPEC SC REG','Special SC Regular-97',97,0,0,0,0,0,0,65535,6,0),(24,'REG RES','Regular Reserved-141',141,0,0,0,0,0,0,65280,9,0),(25,'SC REG RES','Senior Regular Reserved-113',113,0,0,0,0,0,0,65535,10,0),(26,'EP- FS REG','Regular (Early Patron)-450',450,0,1,1,0,0,0,8388863,20,0),(27,'EP-FS SR REG','SR Regular (Early Patron)-350',350,0,1,1,0,0,0,16777088,21,0),(28,'SP-RES','Special Reserved - 600',600,0,1,1,0,0,0,65280,22,0),(29,'SP-RES SR','Special Reserved Senior - 480',480,0,1,1,0,0,0,65535,23,0),(30,'EP RES REG','Reserved (Early Patron)-550',550,0,1,1,0,0,0,8388863,24,0),(31,'EP RES SR','Reserved Senior (Early Patron)-430',430,0,1,1,0,0,0,16777088,25,0),(32,'VIP/SPECIAL','VIP/SPECIAL PASS - 0',5,1,0,1,0,0,1,32896,36,0),(33,'AD PASS','COM\'L AD PASS - 5',0,0,0,1,0,0,1,16744703,27,0),(34,'PROD.PASS','PRODUCTION PASS - 5',5,0,0,1,0,0,1,32896,28,0),(35,'REG - MMFF','MMFF REG - 140',140,0,1,1,0,0,0,65280,13,0),(36,'SC REG -MMFF','MMFF SC REG - 112',112,0,1,1,0,0,0,65535,14,0),(37,'PASS','EMERGENCY PASS',0,0,0,0,0,0,0,-16777201,31,0),(38,'SPEC REG','SPEC REG - 130',130,0,1,1,0,0,0,65280,8,0),(39,'SPEC SR REG','SPEC SR REG - 104',104,0,1,1,0,0,0,65535,9,0),(40,'REG-RES','Regular Reserved Seating - 150',150,0,1,1,0,0,0,65280,5,0),(41,'SC REG-RES','SC Regular Reserved - 120',120,0,1,1,0,0,0,65535,6,0),(42,'REG-DIS','Reg Discounted-500',500,1,1,1,0,0,0,4194432,36,0),(43,'PP','PROMO PASS - 0',0,0,0,1,0,0,1,8421440,27,0),(44,'NO SEAT#','SPECIAL RESRVED',600,0,1,1,0,0,0,-16777201,38,0),(45,'SR NO SEAT#','SPECIAL RESRVED SENIOR',480,0,1,1,0,0,0,-16777201,39,0),(46,'REG - RES','Regular Reserved Seating - 160',160,0,1,1,0,0,0,65280,40,0),(47,'SC REG - RES','Senior Regular Reserved - 128',128,0,1,1,0,0,0,65535,41,0),(48,'NO SEAT 160','No Seat Regular - 160',160,0,1,1,0,0,0,65408,43,0);
/*!40000 ALTER TABLE `patrons` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `people`
--

DROP TABLE IF EXISTS `people`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `people` (
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `people`
--

LOCK TABLES `people` WRITE;
/*!40000 ALTER TABLE `people` DISABLE KEYS */;
INSERT INTO `people` VALUES (1,'aaaa aaaa','aaaa','aaaa','a','','aaaaaa','aaaaaaaa','aaaaa','aaaaa','aaaaa'),(2,'b b','b','b','b','b','b','b','b','b','b'),(3,'ca c','c','ca','','','ce','cf','cb','cc','cd'),(4,'e e','e','e','','e','e','e','e','e','e'),(5,'azzz azz','azz','azzz','','','azzzzz','azzzzzz','azzzz','azzz','azzzzz'),(6,'w w','w','w','','w','w','w','w','w','w');
/*!40000 ALTER TABLE `people` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sound_system`
--

DROP TABLE IF EXISTS `sound_system`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sound_system` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sound_system_type` varchar(225) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sound_system`
--

LOCK TABLES `sound_system` WRITE;
/*!40000 ALTER TABLE `sound_system` DISABLE KEYS */;
INSERT INTO `sound_system` VALUES (1,'Dolby Digital'),(2,'Dolby Atmos'),(3,'Dolby 3D'),(4,'Dolby Surround');
/*!40000 ALTER TABLE `sound_system` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `system_module`
--

DROP TABLE IF EXISTS `system_module`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `system_module` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `module_code` varchar(255) NOT NULL,
  `module_desc` varchar(225) NOT NULL,
  `module_group` varchar(255) NOT NULL,
  `system_code` int(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `system_module`
--

LOCK TABLES `system_module` WRITE;
/*!40000 ALTER TABLE `system_module` DISABLE KEYS */;
INSERT INTO `system_module` VALUES (1,'USER_ADD','ADD NEW SYSTEM USER','CONFIG',1),(2,'USER_EDIT','EDIT/UPDATE EXISTING SYSTEM USER','CONFIG',1),(3,'USER_DELETE','REMOVE EXISTING SYSTEM USER','CONFIG',1),(4,'USER_ADD','REPORT','REPORT',1),(5,'USER_EDIT','UTILITY','UTILITY',1),(6,'USER_DELETE','CINEMA','CINEMA',1);
/*!40000 ALTER TABLE `system_module` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_level`
--

DROP TABLE IF EXISTS `user_level`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_level` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `level_desc` varchar(255) NOT NULL,
  `system_code` int(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_level`
--

LOCK TABLES `user_level` WRITE;
/*!40000 ALTER TABLE `user_level` DISABLE KEYS */;
INSERT INTO `user_level` VALUES (10,'ADMINISTRATOR',1),(12,'AD',1),(13,'ADS',1);
/*!40000 ALTER TABLE `user_level` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_level_rights`
--

DROP TABLE IF EXISTS `user_level_rights`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_level_rights` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_level` int(11) NOT NULL,
  `module_id` int(11) NOT NULL,
  `system_code` int(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=60 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_level_rights`
--

LOCK TABLES `user_level_rights` WRITE;
/*!40000 ALTER TABLE `user_level_rights` DISABLE KEYS */;
INSERT INTO `user_level_rights` VALUES (37,10,6,1),(38,10,4,1),(39,10,5,1),(40,10,1,1),(41,10,2,1),(42,10,3,1),(54,13,6,1),(55,13,4,1),(56,13,5,1),(57,13,1,1),(58,13,2,1),(59,13,3,1);
/*!40000 ALTER TABLE `user_level_rights` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_logs`
--

DROP TABLE IF EXISTS `user_logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_logs` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_name` varchar(255) NOT NULL,
  `user_name` varchar(255) NOT NULL,
  `user_authority` varchar(255) NOT NULL,
  `time_in` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_logs`
--

LOCK TABLES `user_logs` WRITE;
/*!40000 ALTER TABLE `user_logs` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_logs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_logs_temp`
--

DROP TABLE IF EXISTS `user_logs_temp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_logs_temp` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_name` varchar(255) NOT NULL,
  `user_name` varchar(255) NOT NULL,
  `user_authority` varchar(255) NOT NULL,
  `time_in` varchar(255) NOT NULL,
  `time_out` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_logs_temp`
--

LOCK TABLES `user_logs_temp` WRITE;
/*!40000 ALTER TABLE `user_logs_temp` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_logs_temp` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_rights`
--

DROP TABLE IF EXISTS `user_rights`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_rights` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `module_id` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_rights`
--

LOCK TABLES `user_rights` WRITE;
/*!40000 ALTER TABLE `user_rights` DISABLE KEYS */;
INSERT INTO `user_rights` VALUES (25,2,6),(26,2,4),(27,2,5),(28,2,1),(29,2,2),(30,2,3);
/*!40000 ALTER TABLE `user_rights` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` varchar(255) NOT NULL,
  `user_password` varchar(255) NOT NULL,
  `designation` varchar(255) NOT NULL,
  `user_level_id` int(11) NOT NULL,
  `lname` varchar(255) NOT NULL,
  `fname` varchar(255) NOT NULL,
  `mname` varchar(45) NOT NULL,
  `system_code` int(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (2,'ADMIN','f3997c6f5a299b9b','GIS DEVELOPER',10,'BETITO','ROGELIO','M',1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2014-06-25 11:35:48
