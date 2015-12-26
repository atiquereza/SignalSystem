-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.16 - MySQL Community Server (GPL)
-- Server OS:                    Win32
-- HeidiSQL Version:             9.3.0.5005
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping database structure for signalappdb
DROP DATABASE IF EXISTS `signalappdb`;
CREATE DATABASE IF NOT EXISTS `signalappdb` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `signalappdb`;


-- Dumping structure for table signalappdb.allactivephoneinfo
DROP TABLE IF EXISTS `allactivephoneinfo`;
CREATE TABLE IF NOT EXISTS `allactivephoneinfo` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `PhoneUserPersonalInfoId` bigint(20) NOT NULL,
  `AllPhoneInfoID` int(11) NOT NULL,
  `PhoneUsedFor` varchar(50) NOT NULL,
  `HomeAddress` text NOT NULL,
  `OfficeAddress` text NOT NULL,
  `RequestDate` datetime NOT NULL,
  `LetterNo` text NOT NULL,
  `ConnectDate` datetime NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `PersonInfo` (`PhoneUserPersonalInfoId`),
  KEY `PhoneInfo` (`AllPhoneInfoID`),
  CONSTRAINT `PersonInfo` FOREIGN KEY (`PhoneUserPersonalInfoId`) REFERENCES `phoneuserpersonalinfo` (`ID`),
  CONSTRAINT `PhoneInfo` FOREIGN KEY (`AllPhoneInfoID`) REFERENCES `allphoneinfo` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.allactivephoneinfo: ~15 rows (approximately)
DELETE FROM `allactivephoneinfo`;
/*!40000 ALTER TABLE `allactivephoneinfo` DISABLE KEYS */;
INSERT INTO `allactivephoneinfo` (`ID`, `PhoneUserPersonalInfoId`, `AllPhoneInfoID`, `PhoneUsedFor`, `HomeAddress`, `OfficeAddress`, `RequestDate`, `LetterNo`, `ConnectDate`) VALUES
	(8, 4, 3, 'Office', 'Dhaka', 'Mirpur_12', '2015-12-13 20:26:57', 'BA0001_Letter_Army_Office_1', '2015-12-13 20:27:45'),
	(9, 3, 8, 'Home', 'Ibrahim pur', 'Mohakhali', '2015-12-13 22:43:59', 'New Connection 6002', '2015-12-13 23:18:22'),
	(10, 7, 7, 'Office', 'Ibrahim pur', 'Mohakhali', '2015-12-10 22:43:10', 'New Connection 6001', '2015-12-13 23:18:27'),
	(11, 7, 4, 'Home', 'Ibrahim pur', 'Mohakhali', '2015-12-12 22:43:40', 'New Connection 5002', '2015-12-13 23:18:32'),
	(12, 5, 2, 'Office', 'Ibrahim pur', 'Mohakhali', '2015-12-11 22:42:36', 'New Connection 5000', '2015-12-13 23:18:38'),
	(14, 5, 6, 'Office', 'Nkhal Para', 'Gulshan', '2015-12-22 07:01:00', '', '2015-12-22 07:01:00'),
	(15, 5, 6, '', '', '', '2015-12-21 07:01:00', '', '2015-12-21 07:01:00'),
	(16, 5, 6, '', '', '', '2015-12-18 07:01:00', '', '2015-10-22 07:01:00'),
	(17, 5, 6, '', '', '', '2015-12-22 17:01:00', '', '2014-12-22 07:01:00'),
	(18, 5, 6, '', '', '', '2015-11-12 07:01:00', '', '2015-01-05 07:01:00'),
	(19, 5, 6, '', '', '', '2014-12-22 07:01:00', '', '2015-12-12 15:01:00'),
	(20, 5, 6, '', '', '', '0000-00-00 00:00:00', '', '2015-11-24 07:01:00'),
	(21, 5, 6, '', '', '', '0000-00-00 00:00:00', '', '2013-12-22 07:01:00'),
	(22, 5, 6, '', '', '', '0000-00-00 00:00:00', '', '2015-09-11 07:01:00'),
	(23, 5, 6, '', '', '', '0000-00-00 00:00:00', '', '2012-11-11 11:11:11');
/*!40000 ALTER TABLE `allactivephoneinfo` ENABLE KEYS */;


-- Dumping structure for table signalappdb.allphoneinfo
DROP TABLE IF EXISTS `allphoneinfo`;
CREATE TABLE IF NOT EXISTS `allphoneinfo` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `PhoneNumber` text NOT NULL,
  `ServiceStatus` varchar(50) NOT NULL,
  `ConnectionTypeID` int(11) NOT NULL,
  `Remarks` text NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_allphoneinfo_menuconnectiontype` (`ConnectionTypeID`),
  CONSTRAINT `FK_allphoneinfo_menuconnectiontype` FOREIGN KEY (`ConnectionTypeID`) REFERENCES `menuconnectiontype` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.allphoneinfo: ~21 rows (approximately)
DELETE FROM `allphoneinfo`;
/*!40000 ALTER TABLE `allphoneinfo` DISABLE KEYS */;
INSERT INTO `allphoneinfo` (`ID`, `PhoneNumber`, `ServiceStatus`, `ConnectionTypeID`, `Remarks`) VALUES
	(2, '5000', 'Active', 1, 'New Phone'),
	(3, '5001', 'Active', 1, 'New Phone'),
	(4, '5002', 'Active', 1, 'New Phone'),
	(5, '5003', 'Active', 1, 'New Phone'),
	(6, '5004', 'Terminated', 1, 'New Phone'),
	(7, '6001', 'Active', 2, 'New Phone'),
	(8, '6002', 'Active', 2, 'New Phone'),
	(9, '6003', 'Active', 2, 'New Phone'),
	(10, '5005', 'Terminated', 1, 'New Phone'),
	(11, '5006', 'Terminated', 1, 'New Phone'),
	(12, '5007', 'Terminated', 1, 'New Phone'),
	(13, '5008', 'Terminated', 1, 'New Phone'),
	(14, '5009', 'Terminated', 1, 'New Phone'),
	(15, '5010', 'Terminated', 1, 'New Phone'),
	(16, '6004', 'Terminated', 2, 'New Phone'),
	(17, '6005', 'Terminated', 2, 'New Phone'),
	(18, '6006', 'Terminated', 2, 'New Phone'),
	(19, '6007', 'Terminated', 2, 'New Phone'),
	(20, '6008', 'Terminated', 2, 'New Phone'),
	(21, '6009', 'Terminated', 2, 'New Phone'),
	(22, '6010', 'Terminated', 2, 'New Phone');
/*!40000 ALTER TABLE `allphoneinfo` ENABLE KEYS */;


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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.appviews: ~1 rows (approximately)
DELETE FROM `appviews`;
/*!40000 ALTER TABLE `appviews` DISABLE KEYS */;
INSERT INTO `appviews` (`ID`, `Controller`, `ControllerType`, `Action`, `Argument`, `SuperAdmin`, `Admin`, `Reporter`, `Resolver`) VALUES
	(1, 'Home', 'mvc', 'Index', 0, 1, 1, 1, 1);
/*!40000 ALTER TABLE `appviews` ENABLE KEYS */;


-- Dumping structure for table signalappdb.complains
DROP TABLE IF EXISTS `complains`;
CREATE TABLE IF NOT EXISTS `complains` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Description` text NOT NULL,
  `Status` varchar(50) NOT NULL,
  `MenuComplainTypeId` int(11) NOT NULL,
  `TelephoneUserId` bigint(20) NOT NULL,
  `ComplainDate` datetime NOT NULL,
  `ResolvedDate` datetime NOT NULL,
  `Remarks` text NOT NULL,
  `ActionTaken` text NOT NULL,
  `ResolvedBy` text NOT NULL,
  `AllPhoneInfoID` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `MenuComplainTypeId` (`MenuComplainTypeId`),
  KEY `TelephoneUserId` (`TelephoneUserId`),
  KEY `FK_complains_allphoneinfo` (`AllPhoneInfoID`),
  CONSTRAINT `FK_complains_allphoneinfo` FOREIGN KEY (`AllPhoneInfoID`) REFERENCES `allphoneinfo` (`ID`),
  CONSTRAINT `FK_complains_menucomplaintype` FOREIGN KEY (`MenuComplainTypeId`) REFERENCES `menucomplaintype` (`Id`),
  CONSTRAINT `FK_complains_telephoneusers` FOREIGN KEY (`TelephoneUserId`) REFERENCES `phoneuserpersonalinfo` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=121 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.complains: ~34 rows (approximately)
DELETE FROM `complains`;
/*!40000 ALTER TABLE `complains` DISABLE KEYS */;
INSERT INTO `complains` (`Id`, `Description`, `Status`, `MenuComplainTypeId`, `TelephoneUserId`, `ComplainDate`, `ResolvedDate`, `Remarks`, `ActionTaken`, `ResolvedBy`, `AllPhoneInfoID`) VALUES
	(83, 'h1', 'Pending', 6, 7, '2015-12-19 16:53:31', '0000-00-00 00:00:00', '', '', '', 6),
	(84, 'h2', 'Pending', 8, 5, '9999-01-01 23:59:59', '0000-00-00 00:00:00', '', '', '', 5),
	(85, 'h3', 'Pending', 3, 6, '2015-12-20 07:00:14', '0000-00-00 00:00:00', '', '', '', 3),
	(86, 'h8', 'Pending', 1, 5, '2015-12-20 07:04:41', '0000-00-00 00:00:00', '', '', '', 2),
	(87, 'h5', 'Pending', 4, 5, '2015-12-20 07:07:13', '0000-00-00 00:00:00', '', '', '', 6),
	(89, 'H7', 'Pending', 6, 8, '2015-12-20 07:09:38', '0000-00-00 00:00:00', '', '', '', 3),
	(91, 'h4', 'Pending', 1, 5, '2015-12-20 07:11:15', '0000-00-00 00:00:00', '', '', '', 12),
	(92, 'h6', 'Pending', 7, 5, '2015-12-20 07:11:30', '0000-00-00 00:00:00', '', '', '', 5),
	(94, 'h10', 'Pending', 3, 8, '2015-12-20 07:12:09', '0000-00-00 00:00:00', '', '', '', 10),
	(96, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(97, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(98, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(99, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(100, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(101, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(102, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(103, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(104, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(105, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(106, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(107, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(108, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(109, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(110, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(111, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(112, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(113, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(114, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(115, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 2),
	(116, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 6),
	(117, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 6),
	(118, 'h3', 'Pending', 5, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 6),
	(119, 'h3', 'Pending', 4, 6, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 6),
	(120, 'h3', 'Pending', 4, 7, '2015-12-22 01:24:03', '0000-00-00 00:00:00', '', '', '', 6);
/*!40000 ALTER TABLE `complains` ENABLE KEYS */;


-- Dumping structure for table signalappdb.deletedtelephoneusers
DROP TABLE IF EXISTS `deletedtelephoneusers`;
CREATE TABLE IF NOT EXISTS `deletedtelephoneusers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BANumber` text NOT NULL,
  `Name` text NOT NULL,
  `RankId` int(11) NOT NULL,
  `Status` varchar(50) NOT NULL,
  `NewPhoneNumber` text NOT NULL,
  `Address` text NOT NULL,
  `Gender` varchar(50) NOT NULL,
  `ConnectedDate` date NOT NULL,
  `DisconnectedDate` date NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Rank` (`RankId`),
  CONSTRAINT `deletedtelephoneusers_ibfk_1` FOREIGN KEY (`RankId`) REFERENCES `menusrank` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- Dumping data for table signalappdb.deletedtelephoneusers: ~3 rows (approximately)
DELETE FROM `deletedtelephoneusers`;
/*!40000 ALTER TABLE `deletedtelephoneusers` DISABLE KEYS */;
INSERT INTO `deletedtelephoneusers` (`Id`, `BANumber`, `Name`, `RankId`, `Status`, `NewPhoneNumber`, `Address`, `Gender`, `ConnectedDate`, `DisconnectedDate`) VALUES
	(8, 'BA558300', 'Reza', 3, 'Connected', '123456', 'Rangpur', 'male', '2015-11-22', '0000-00-00'),
	(9, 'BA123555', 'Khairul', 5, 'Connected', '123456', 'Cox\'s Bazar', 'male', '2015-10-17', '2015-11-23'),
	(10, 'BA123555', 'Khairul', 5, 'Connected', '123456', 'Cox\'s Bazar', 'male', '2015-10-17', '2015-11-23');
/*!40000 ALTER TABLE `deletedtelephoneusers` ENABLE KEYS */;


-- Dumping structure for table signalappdb.equipemntdescription
DROP TABLE IF EXISTS `equipemntdescription`;
CREATE TABLE IF NOT EXISTS `equipemntdescription` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TypeId` int(11) NOT NULL DEFAULT '0',
  `Amount` int(11) NOT NULL DEFAULT '0',
  `Description` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `FK__equipmnttype` (`TypeId`),
  CONSTRAINT `FK__equipmnttype` FOREIGN KEY (`TypeId`) REFERENCES `equipmnttype` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.equipemntdescription: ~6 rows (approximately)
DELETE FROM `equipemntdescription`;
/*!40000 ALTER TABLE `equipemntdescription` DISABLE KEYS */;
INSERT INTO `equipemntdescription` (`Id`, `TypeId`, `Amount`, `Description`) VALUES
	(1, 1, 50, 'good'),
	(3, 4, 900, 'very good tooo'),
	(4, 4, 60, 'Quality Product'),
	(5, 1, 10, '10 feet'),
	(6, 7, 30, 'Ericsson'),
	(7, 3, 40, 'test');
/*!40000 ALTER TABLE `equipemntdescription` ENABLE KEYS */;


-- Dumping structure for table signalappdb.equipmnttype
DROP TABLE IF EXISTS `equipmnttype`;
CREATE TABLE IF NOT EXISTS `equipmnttype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TypeName` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.equipmnttype: ~0 rows (approximately)
DELETE FROM `equipmnttype`;
/*!40000 ALTER TABLE `equipmnttype` DISABLE KEYS */;
/*!40000 ALTER TABLE `equipmnttype` ENABLE KEYS */;


-- Dumping structure for table signalappdb.maildata
DROP TABLE IF EXISTS `maildata`;
CREATE TABLE IF NOT EXISTS `maildata` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `MailID` text NOT NULL,
  `MailDescription` text NOT NULL,
  `MailFromID` int(11) NOT NULL,
  `MailToID` int(11) NOT NULL,
  `DateArrival` date NOT NULL,
  `DateDeparture` date NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_maildata_menustations` (`MailFromID`),
  KEY `FK_maildata_menustations_2` (`MailToID`),
  CONSTRAINT `FK_maildata_menustations` FOREIGN KEY (`MailFromID`) REFERENCES `menustations` (`ID`),
  CONSTRAINT `FK_maildata_menustations_2` FOREIGN KEY (`MailToID`) REFERENCES `menustations` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.maildata: ~12 rows (approximately)
DELETE FROM `maildata`;
/*!40000 ALTER TABLE `maildata` DISABLE KEYS */;
INSERT INTO `maildata` (`ID`, `MailID`, `MailDescription`, `MailFromID`, `MailToID`, `DateArrival`, `DateDeparture`) VALUES
	(12, 'MID00001', 'some description test', 1, 5, '2015-11-24', '2015-12-25'),
	(13, 'MID00002', 'ctg to bog', 2, 3, '2015-12-25', '2015-12-25'),
	(14, 'MID00003', 'some desc', 3, 4, '2015-12-25', '2015-12-25'),
	(15, 'MID00004', 'some desc', 3, 4, '2015-12-25', '2015-12-25'),
	(16, 'MID00005', 'some desc', 1, 5, '2015-12-25', '2015-12-25'),
	(17, 'MID00006', 'some desc', 2, 3, '2015-12-25', '2015-12-25'),
	(18, 'MID00007', 'some desc', 4, 4, '2015-12-25', '2015-12-25'),
	(19, 'MID00008', 'some desc', 3, 4, '2015-12-25', '2015-12-25'),
	(20, 'MID00009', 'some desc', 4, 3, '2015-12-25', '2015-12-25'),
	(21, 'MID00010', 'some desc', 3, 1, '2015-12-25', '2015-12-25'),
	(22, 'MID00011', 'some desc', 4, 4, '2015-12-25', '2015-12-25'),
	(23, 'MID00012', ' test1', 1, 2, '2015-12-25', '2015-12-25');
/*!40000 ALTER TABLE `maildata` ENABLE KEYS */;


-- Dumping structure for table signalappdb.menucomplaintype
DROP TABLE IF EXISTS `menucomplaintype`;
CREATE TABLE IF NOT EXISTS `menucomplaintype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Value` text NOT NULL,
  `Name` text NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.menucomplaintype: ~8 rows (approximately)
DELETE FROM `menucomplaintype`;
/*!40000 ALTER TABLE `menucomplaintype` DISABLE KEYS */;
INSERT INTO `menucomplaintype` (`Id`, `Value`, `Name`) VALUES
	(1, 'Disconnected', 'Complain'),
	(2, 'Unclear Sound', 'Complain'),
	(3, 'Phone Faulty', 'Complain'),
	(4, 'Other', 'Complain'),
	(5, 'NewConnection', 'Complain'),
	(6, 'DiscardConnection', 'Complain'),
	(7, 'Cable Stolen', 'Complain'),
	(8, 'Test', 'Complain');
/*!40000 ALTER TABLE `menucomplaintype` ENABLE KEYS */;


-- Dumping structure for table signalappdb.menuconnectiontype
DROP TABLE IF EXISTS `menuconnectiontype`;
CREATE TABLE IF NOT EXISTS `menuconnectiontype` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Value` varchar(50) NOT NULL,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.menuconnectiontype: ~2 rows (approximately)
DELETE FROM `menuconnectiontype`;
/*!40000 ALTER TABLE `menuconnectiontype` DISABLE KEYS */;
INSERT INTO `menuconnectiontype` (`ID`, `Value`, `Name`) VALUES
	(1, 'Army', 'Connection Type'),
	(2, 'BTCL', 'Connection Type');
/*!40000 ALTER TABLE `menuconnectiontype` ENABLE KEYS */;


-- Dumping structure for table signalappdb.menurequesttype
DROP TABLE IF EXISTS `menurequesttype`;
CREATE TABLE IF NOT EXISTS `menurequesttype` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Value` varchar(50) DEFAULT NULL,
  `Name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.menurequesttype: ~3 rows (approximately)
DELETE FROM `menurequesttype`;
/*!40000 ALTER TABLE `menurequesttype` DISABLE KEYS */;
INSERT INTO `menurequesttype` (`ID`, `Value`, `Name`) VALUES
	(1, 'New Connection', 'Request'),
	(2, 'Shifting', 'Request'),
	(3, 'Termination', 'Request');
/*!40000 ALTER TABLE `menurequesttype` ENABLE KEYS */;


-- Dumping structure for table signalappdb.menusrank
DROP TABLE IF EXISTS `menusrank`;
CREATE TABLE IF NOT EXISTS `menusrank` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `Value` text NOT NULL,
  `Name` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.menusrank: ~9 rows (approximately)
DELETE FROM `menusrank`;
/*!40000 ALTER TABLE `menusrank` DISABLE KEYS */;
INSERT INTO `menusrank` (`id`, `Value`, `Name`) VALUES
	(1, 'General', 'rank'),
	(2, 'Lieutenant General', 'rank'),
	(3, 'Major General', 'rank'),
	(4, 'Colonel-Brigadier General', 'rank'),
	(5, 'Lieutenant colonel', 'rank'),
	(6, 'Major', 'rank'),
	(7, 'Captain', 'rank'),
	(8, 'Lieutenant', 'rank'),
	(9, 'Second Lieutenant', 'rank');
/*!40000 ALTER TABLE `menusrank` ENABLE KEYS */;


-- Dumping structure for table signalappdb.menustations
DROP TABLE IF EXISTS `menustations`;
CREATE TABLE IF NOT EXISTS `menustations` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Value` varchar(50) NOT NULL,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.menustations: ~5 rows (approximately)
DELETE FROM `menustations`;
/*!40000 ALTER TABLE `menustations` DISABLE KEYS */;
INSERT INTO `menustations` (`ID`, `Value`, `Name`) VALUES
	(1, 'Dhaka Cant.', 'Dhaka Cant.'),
	(2, 'CTG Cant.', 'CTG Cant.'),
	(3, 'BOG Cant.', 'BOG Cant.'),
	(4, 'RAJ Cant.', 'RAJ Cant.'),
	(5, 'RANG Cant.', 'RANG Cant.');
/*!40000 ALTER TABLE `menustations` ENABLE KEYS */;


-- Dumping structure for table signalappdb.pendingrequest
DROP TABLE IF EXISTS `pendingrequest`;
CREATE TABLE IF NOT EXISTS `pendingrequest` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `MenuRequestTypeID` int(11) NOT NULL,
  `PhoneUserPersonalInfoID` bigint(20) NOT NULL,
  `AllPhoneInfoID` int(11) NOT NULL,
  `LetterNo` text NOT NULL,
  `AddressType` varchar(50) NOT NULL,
  `Comment` varchar(50) NOT NULL,
  `RequestDate` datetime NOT NULL,
  `NewAddressForShifting` text,
  PRIMARY KEY (`ID`),
  KEY `Request` (`MenuRequestTypeID`),
  KEY `PersonalInfo_PhoneUserPersonalInfo` (`PhoneUserPersonalInfoID`),
  KEY `PhoneNumber_AllPhones` (`AllPhoneInfoID`),
  CONSTRAINT `PersonalInfo_PhoneUserPersonalInfo` FOREIGN KEY (`PhoneUserPersonalInfoID`) REFERENCES `phoneuserpersonalinfo` (`ID`),
  CONSTRAINT `PhoneNumber_AllPhones` FOREIGN KEY (`AllPhoneInfoID`) REFERENCES `allphoneinfo` (`ID`),
  CONSTRAINT `Request` FOREIGN KEY (`MenuRequestTypeID`) REFERENCES `menurequesttype` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.pendingrequest: ~3 rows (approximately)
DELETE FROM `pendingrequest`;
/*!40000 ALTER TABLE `pendingrequest` DISABLE KEYS */;
INSERT INTO `pendingrequest` (`ID`, `MenuRequestTypeID`, `PhoneUserPersonalInfoID`, `AllPhoneInfoID`, `LetterNo`, `AddressType`, `Comment`, `RequestDate`, `NewAddressForShifting`) VALUES
	(5, 2, 3, 7, 'Shifting Letter _4', 'Office', '', '2015-12-13 00:29:57', 'Mohakhali To DOHS'),
	(6, 1, 5, 5, '77777', 'Office', '', '2015-12-14 00:30:37', NULL),
	(7, 1, 5, 9, 'New Connection 6003', 'Office', '', '2015-12-01 00:31:23', NULL);
/*!40000 ALTER TABLE `pendingrequest` ENABLE KEYS */;


-- Dumping structure for table signalappdb.phoneuserpersonalinfo
DROP TABLE IF EXISTS `phoneuserpersonalinfo`;
CREATE TABLE IF NOT EXISTS `phoneuserpersonalinfo` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `BANumber` varchar(100) NOT NULL,
  `FullName` varchar(100) NOT NULL,
  `Appointment` varchar(100) NOT NULL,
  `PlateName` varchar(100) NOT NULL,
  `ServiceStatus` varchar(100) NOT NULL,
  `RankId` int(11) NOT NULL,
  `Sex` varchar(100) NOT NULL,
  `Unit` varchar(100) NOT NULL,
  `JoiningDate` date NOT NULL,
  `LPRDate` date NOT NULL,
  `MaritalStatus` varchar(50) NOT NULL,
  `PresentAddress` text NOT NULL,
  `PermanentAddress` text NOT NULL,
  `OfficeAddress` text NOT NULL,
  `PersonalPhoneNumber` text NOT NULL,
  `EmailAddress` text NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `RankId` (`RankId`),
  CONSTRAINT `RankId` FOREIGN KEY (`RankId`) REFERENCES `menusrank` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.phoneuserpersonalinfo: ~6 rows (approximately)
DELETE FROM `phoneuserpersonalinfo`;
/*!40000 ALTER TABLE `phoneuserpersonalinfo` DISABLE KEYS */;
INSERT INTO `phoneuserpersonalinfo` (`ID`, `BANumber`, `FullName`, `Appointment`, `PlateName`, `ServiceStatus`, `RankId`, `Sex`, `Unit`, `JoiningDate`, `LPRDate`, `MaritalStatus`, `PresentAddress`, `PermanentAddress`, `OfficeAddress`, `PersonalPhoneNumber`, `EmailAddress`) VALUES
	(3, 'BA123456', 'Atique Reza Chowdhury', 'Appointment', 'Atique', 'On Service', 3, 'male', 'NSN', '2015-02-12', '0001-01-01', 'unmarried', 'Ibrahim pur', 'Kaharol', 'Mohakhali', '01926662260', 'Email.Address@email.com'),
	(4, 'BA0001', 'HabibUllah Hem', 'SIGNAL', 'Habib', 'On Service', 1, 'male', 'AO', '2015-01-12', '0001-01-01', 'unmarried', 'Dhaka', 'Dhaka', 'Mirpur_12', '01926662260', 'Email.Address@email.com'),
	(5, 'BA0002', 'Anwarul Karim', 'SIGNAL', 'Anwar', 'On Service', 5, 'male', 'SS', '2015-02-12', '0001-01-01', 'unmarried', 'Dhaka', 'Dhaka', 'Dhaka', '01926662260', 'Email.Address@email.com'),
	(6, 'BA0003', 'Anamul Hakim', 'SIGNAL', 'Anwar', 'On Service', 5, 'male', 'SS', '2015-02-12', '0001-01-01', 'unmarried', 'Dhaka', 'Dhaka', 'Dhaka', '01926662260', 'Email.Address@email.com'),
	(7, 'BA0004', 'Jamil Uddin Khan', 'SIGNAL', 'Anwar', 'On Service', 5, 'male', 'SS', '2015-02-12', '0001-01-01', 'unmarried', 'Dhaka', 'Dhaka', 'Dhaka', '01926662260', 'Email.Address@email.com'),
	(8, 'BA0005', 'Shaheb Patwari', 'SIGNAL', 'Anwar', 'On Service', 5, 'male', 'SS', '2015-02-12', '0001-01-01', 'unmarried', 'Dhaka', 'Dhaka', 'Dhaka', '01926662260', 'Email.Address@email.com');
/*!40000 ALTER TABLE `phoneuserpersonalinfo` ENABLE KEYS */;


-- Dumping structure for table signalappdb.resolvedrequest
DROP TABLE IF EXISTS `resolvedrequest`;
CREATE TABLE IF NOT EXISTS `resolvedrequest` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FullName` text,
  `BANumber` text,
  `Address` text,
  `AddressType` text,
  `Rank` varchar(50) DEFAULT NULL,
  `ConnectionType` varchar(50) DEFAULT NULL,
  `RequestType` varchar(100) NOT NULL,
  `LetterNo` varchar(200) NOT NULL,
  `RequestDate` datetime NOT NULL,
  `ResolveDate` datetime NOT NULL,
  `PhoneNumber` varchar(50) NOT NULL,
  `ResolveBy` varchar(50) DEFAULT NULL,
  `ActionTaken` text,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.resolvedrequest: ~10 rows (approximately)
DELETE FROM `resolvedrequest`;
/*!40000 ALTER TABLE `resolvedrequest` DISABLE KEYS */;
INSERT INTO `resolvedrequest` (`ID`, `FullName`, `BANumber`, `Address`, `AddressType`, `Rank`, `ConnectionType`, `RequestType`, `LetterNo`, `RequestDate`, `ResolveDate`, `PhoneNumber`, `ResolveBy`, `ActionTaken`) VALUES
	(12, 'HabibUllah Hem', 'BA0001', 'Dhaka', 'Office', 'General', 'Army', 'New Connection', 'BA0001_Letter_Army_Office_1', '2015-12-11 20:26:57', '2015-12-13 20:27:45', '5001', 'Ilias Jamil', 'Done'),
	(13, 'Atique Reza Chowdhury', 'BA123456', 'Mohakhali', 'Office', 'Major General', 'Army', 'New Connection', 'BA123456_Letter_Army_Office_1', '2015-12-13 20:26:26', '2015-12-13 20:28:22', '5000', 'Azizul Haque', 'done'),
	(14, 'Atique Reza Chowdhury', 'BA123456', 'Mohakhali', 'Office', 'Major General', 'Army', 'Termination', 'BA123456_Letter_Army_Office_1_Terminate', '2015-12-13 20:29:44', '2015-12-13 20:32:14', '5000', 'Azizul Haque', 'ffff'),
	(15, 'HabibUllah Hem', 'BA0001', 'Dhaka', 'Office', 'General', 'Army', 'Shifting', 'BA0001_Letter_Army_Office_1_Shifting', '2015-12-12 20:34:10', '2015-12-13 20:34:25', '5001', 'Azizul Haque', 'ffff'),
	(16, 'HabibUllah Hem', 'BA0001', 'Dhaka', 'Office', 'General', 'Army', 'Shifting', 'Shifting Letter _1', '2015-12-12 20:39:14', '2015-12-13 20:42:03', '5001', 'Azizul Haque', 'fffff'),
	(17, 'HabibUllah Hem', 'BA0001', 'Mirpur_10', 'Office', 'General', 'Army', 'Shifting', 'Shifting Letter _2', '2015-12-13 20:48:41', '2015-12-13 20:49:06', '5001', 'Ilias Jamil', 'Done'),
	(18, 'Atique Reza Chowdhury', 'BA123456', 'Ibrahim pur', 'Home', 'Major General', 'BTCL', 'New Connection', 'New Connection 6002', '2015-12-13 22:43:59', '2015-12-13 23:18:22', '6002', 'Ilias Jamil', 'line man'),
	(19, 'Atique Reza Chowdhury', 'BA123456', 'Mohakhali', 'Office', 'Major General', 'BTCL', 'New Connection', 'New Connection 6001', '2015-12-13 22:43:10', '2015-12-13 23:18:27', '6001', 'Ilias Jamil', 'line man'),
	(20, 'Atique Reza Chowdhury', 'BA123456', 'Ibrahim pur', 'Home', 'Major General', 'Army', 'New Connection', 'New Connection 5002', '2015-12-10 22:43:40', '2015-12-13 23:18:32', '5002', 'Ilias Jamil', 'line man'),
	(21, 'Atique Reza Chowdhury', 'BA123456', 'Mohakhali', 'Office', 'Major General', 'Army', 'New Connection', 'New Connection 5000', '2015-12-13 22:42:36', '2015-12-13 23:18:38', '5000', 'Ilias Jamil', 'line man');
/*!40000 ALTER TABLE `resolvedrequest` ENABLE KEYS */;


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


-- Dumping structure for table signalappdb.telephoneusers
DROP TABLE IF EXISTS `telephoneusers`;
CREATE TABLE IF NOT EXISTS `telephoneusers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BANumber` text NOT NULL,
  `Name` text NOT NULL,
  `RankId` int(11) NOT NULL,
  `Status` varchar(50) NOT NULL,
  `NewPhoneNumber` text NOT NULL,
  `Address` text NOT NULL,
  `Gender` varchar(50) NOT NULL,
  `ConnectedDate` date NOT NULL,
  `DisconnectedDate` date NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Rank` (`RankId`),
  CONSTRAINT `FK__menus` FOREIGN KEY (`RankId`) REFERENCES `menusrank` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.telephoneusers: ~8 rows (approximately)
DELETE FROM `telephoneusers`;
/*!40000 ALTER TABLE `telephoneusers` DISABLE KEYS */;
INSERT INTO `telephoneusers` (`Id`, `BANumber`, `Name`, `RankId`, `Status`, `NewPhoneNumber`, `Address`, `Gender`, `ConnectedDate`, `DisconnectedDate`) VALUES
	(3, 'BA123456', 'Test User', 1, 'Disconnected', '987654', 'Dhaka', 'male', '2015-11-07', '0000-00-00'),
	(4, 'BA123456', 'Test User1', 1, 'Connected', '9876547', 'Dhaka', 'male', '2015-11-07', '0000-00-00'),
	(5, 'BA123457\r\n', 'Test User1hhkk jjhdfdfdsfsdfl fdfdfdfd dfdfdf', 1, 'Connected', '98765478889999999', 'Dhaka', 'male', '2015-11-07', '0000-00-00'),
	(6, 'BA558300', 'Reza', 3, 'Connected', '12345689', 'Rangpur', 'male', '2015-11-22', '0000-00-00'),
	(9, 'BA112345', 'User101', 7, 'Disconnected', '1125568', 'Comilla', 'male', '0000-00-00', '0000-00-00'),
	(10, 'BA445789', 'Kazi ', 6, 'Connected', '99807', 'Comilla', 'male', '2015-11-24', '0000-00-00'),
	(12, 'BA445789', 'Akkasur', 8, 'Connected', '995527', 'Rangpur', 'male', '2015-11-23', '0000-00-00'),
	(13, 'BA123555', 'Khairul', 5, 'Connected', '123456', 'Cox\'s Bazar', 'male', '2015-10-17', '0000-00-00');
/*!40000 ALTER TABLE `telephoneusers` ENABLE KEYS */;


-- Dumping structure for table signalappdb.userinfo
DROP TABLE IF EXISTS `userinfo`;
CREATE TABLE IF NOT EXISTS `userinfo` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `UserName` varchar(50) NOT NULL,
  `FullName` text NOT NULL,
  `RankId` int(11) NOT NULL,
  `Email` text NOT NULL,
  `Gender` text NOT NULL,
  `BANumber` text,
  PRIMARY KEY (`ID`),
  KEY `UserID` (`UserID`),
  CONSTRAINT `UserID` FOREIGN KEY (`UserID`) REFERENCES `users` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.userinfo: ~7 rows (approximately)
DELETE FROM `userinfo`;
/*!40000 ALTER TABLE `userinfo` DISABLE KEYS */;
INSERT INTO `userinfo` (`ID`, `UserID`, `UserName`, `FullName`, `RankId`, `Email`, `Gender`, `BANumber`) VALUES
	(1, 4, 'jubayer', 'All Jubayer Mohammad Mahamudunnabi.', 2, 'aljubayer@gmail.com', 'Male', 'BA123551'),
	(2, 5, 'atique', 'atique', 3, 'atique@gmail.com', 'Male', 'BA123552'),
	(3, 10, 'habib', 'habib', 2, 'habib@gmail.com', 'Male', 'BA123553'),
	(4, 115, 'sakib', 'sakib', 5, 'sakib@gmail.com', 'Male', 'BA123554'),
	(10, 128, 'Khairul', 'Khairul', 1, 'khairul@gmail.com', 'female', 'BA123555'),
	(11, 131, 'Razib', 'Razib', 4, 'asdf@gamil.com', 'male', 'BA123556'),
	(12, 132, 'Zahurul', 'Zahurul', 1, 'zahurul@gmail.com', 'male', 'BA291488');
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
) ENGINE=InnoDB AUTO_INCREMENT=133 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.users: ~7 rows (approximately)
DELETE FROM `users`;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`ID`, `UserName`, `UserCredential`, `UserRoleId`) VALUES
	(4, 'jubayer', '01926662274', 2),
	(5, 'atique', '01926662227', 2),
	(10, 'habib', '01926662227', 9),
	(115, 'sakib', '01926662227', 10),
	(128, 'Khairul', '123456', 2),
	(131, 'Razib', '123456', 9),
	(132, 'Zahurul', '01926662274', 2);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
