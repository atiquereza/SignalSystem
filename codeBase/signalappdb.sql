-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.16 - MySQL Community Server (GPL)
-- Server OS:                    Win32
-- HeidiSQL Version:             9.1.0.4867
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping database structure for signalappdb
DROP DATABASE IF EXISTS `signalappdb`;
CREATE DATABASE IF NOT EXISTS `signalappdb` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `signalappdb`;


-- Dumping structure for table signalappdb.appmenuitems
DROP TABLE IF EXISTS `appmenuitems`;
CREATE TABLE IF NOT EXISTS `appmenuitems` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `MenuName` varchar(50) DEFAULT '0',
  `MenuParentName` varchar(50) NOT NULL DEFAULT '0',
  `AppViewId` int(11) NOT NULL DEFAULT '0',
  `MenuOrder` int(11) NOT NULL DEFAULT '0',
  `SubMenuOrder` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `appView` (`AppViewId`),
  CONSTRAINT `appView` FOREIGN KEY (`AppViewId`) REFERENCES `appviews` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.appmenuitems: ~1 rows (approximately)
DELETE FROM `appmenuitems`;
/*!40000 ALTER TABLE `appmenuitems` DISABLE KEYS */;
INSERT INTO `appmenuitems` (`id`, `MenuName`, `MenuParentName`, `AppViewId`, `MenuOrder`, `SubMenuOrder`) VALUES
	(1, 'Home', 'Home', 1, 1, 1);
/*!40000 ALTER TABLE `appmenuitems` ENABLE KEYS */;


-- Dumping structure for table signalappdb.appviews
DROP TABLE IF EXISTS `appviews`;
CREATE TABLE IF NOT EXISTS `appviews` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Controller` varchar(100) NOT NULL DEFAULT '0',
  `ControllerType` varchar(100) NOT NULL DEFAULT '0',
  `Action` varchar(100) NOT NULL DEFAULT '0',
  `Argument` int(1) NOT NULL DEFAULT '0',
  `SuperAdmin` int(11) NOT NULL DEFAULT '0',
  `Admin` int(11) NOT NULL DEFAULT '0',
  `Reporter` int(11) NOT NULL DEFAULT '0',
  `Resolver` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.appviews: ~1 rows (approximately)
DELETE FROM `appviews`;
/*!40000 ALTER TABLE `appviews` DISABLE KEYS */;
INSERT INTO `appviews` (`ID`, `Controller`, `ControllerType`, `Action`, `Argument`, `SuperAdmin`, `Admin`, `Reporter`, `Resolver`) VALUES
	(1, 'Home', 'mvc', 'Index', 0, 1, 1, 1, 1);
/*!40000 ALTER TABLE `appviews` ENABLE KEYS */;


-- Dumping structure for table signalappdb.menuitems
DROP TABLE IF EXISTS `menuitems`;
CREATE TABLE IF NOT EXISTS `menuitems` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `MenuName` varchar(50) NOT NULL,
  `ParentMenuName` varchar(50) NOT NULL,
  `AppViewId` int(11) NOT NULL,
  `MenuOrder` int(11) NOT NULL,
  `ParentMenuOrder` int(11) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `AppViewId` (`AppViewId`),
  CONSTRAINT `AppViewId` FOREIGN KEY (`AppViewId`) REFERENCES `appviews` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.menuitems: ~0 rows (approximately)
DELETE FROM `menuitems`;
/*!40000 ALTER TABLE `menuitems` DISABLE KEYS */;
/*!40000 ALTER TABLE `menuitems` ENABLE KEYS */;


-- Dumping structure for table signalappdb.roles
DROP TABLE IF EXISTS `roles`;
CREATE TABLE IF NOT EXISTS `roles` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(200) NOT NULL,
  `ParentRoleName` varchar(200) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.roles: ~4 rows (approximately)
DELETE FROM `roles`;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` (`ID`, `RoleName`, `ParentRoleName`) VALUES
	(1, 'SuperAdmin', 'SuperAdmin'),
	(2, 'Admin', 'SuperAdmin'),
	(9, 'Reporter', 'Admin'),
	(10, 'Resolver', 'Admin');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;


-- Dumping structure for table signalappdb.userinfo
DROP TABLE IF EXISTS `userinfo`;
CREATE TABLE IF NOT EXISTS `userinfo` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `UserName` varchar(50) NOT NULL,
  `FullName` text NOT NULL,
  `FatherName` text NOT NULL,
  `MotherName` text NOT NULL,
  `CellNumber` text NOT NULL,
  `BirthDay` date NOT NULL DEFAULT '1956-06-11',
  `CurrentAddress` text NOT NULL,
  `PermanentAddress` text NOT NULL,
  `BirthCertificateID` text NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `UserID` (`UserID`),
  CONSTRAINT `UserID` FOREIGN KEY (`UserID`) REFERENCES `users` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=103 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.userinfo: ~2 rows (approximately)
DELETE FROM `userinfo`;
/*!40000 ALTER TABLE `userinfo` DISABLE KEYS */;
INSERT INTO `userinfo` (`ID`, `UserID`, `UserName`, `FullName`, `FatherName`, `MotherName`, `CellNumber`, `BirthDay`, `CurrentAddress`, `PermanentAddress`, `BirthCertificateID`) VALUES
	(1, 4, 'jubayer', 'All Jubayer Mohammad Mahamudunnabi.', 'Md. Fazlur Rahman.', 'Momotaz Begum.', '01926662274', '1984-08-01', 'Dhaka.', 'Dinajpur', '22222222'),
	(2, 10, 'arosh', 'arosh', 'Habibunnobi Md Arifur Rahman', 'Arobi', '01926662227', '2015-03-01', 'Dhaka', 'Dinajpur', '222222222222222');
/*!40000 ALTER TABLE `userinfo` ENABLE KEYS */;


-- Dumping structure for table signalappdb.users
DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(50) NOT NULL,
  `UserCredential` varchar(50) NOT NULL,
  `UserRoleId` int(11) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `UserRoleId` (`UserRoleId`),
  CONSTRAINT `UserRoleId` FOREIGN KEY (`UserRoleId`) REFERENCES `roles` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=116 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.users: ~4 rows (approximately)
DELETE FROM `users`;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`ID`, `UserName`, `UserCredential`, `UserRoleId`) VALUES
	(4, 'jubayer', '01926662274', 2),
	(5, 'atique', '01926662227', 2),
	(10, 'habib', '01926662227', 9),
	(115, 'sakib', '01926662227', 10);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
