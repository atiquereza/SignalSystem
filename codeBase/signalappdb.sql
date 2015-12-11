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
  `PhoneUsedFor` int(11) NOT NULL,
  `HomeAddress` text NOT NULL,
  `OfficeAddress` text NOT NULL,
  `RequestDate` date NOT NULL,
  `LetterNo` date NOT NULL,
  `ConnectDate` date NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `PersonInfo` (`PhoneUserPersonalInfoId`),
  KEY `PhoneInfo` (`AllPhoneInfoID`),
  CONSTRAINT `PersonInfo` FOREIGN KEY (`PhoneUserPersonalInfoId`) REFERENCES `phoneuserpersonalinfo` (`ID`),
  CONSTRAINT `PhoneInfo` FOREIGN KEY (`AllPhoneInfoID`) REFERENCES `allphoneinfo` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.allactivephoneinfo: ~0 rows (approximately)
DELETE FROM `allactivephoneinfo`;
/*!40000 ALTER TABLE `allactivephoneinfo` DISABLE KEYS */;
/*!40000 ALTER TABLE `allactivephoneinfo` ENABLE KEYS */;


-- Dumping structure for table signalappdb.allhistoryphoneinfo
DROP TABLE IF EXISTS `allhistoryphoneinfo`;
CREATE TABLE IF NOT EXISTS `allhistoryphoneinfo` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `FullName` text NOT NULL,
  `BANumber` varchar(100) NOT NULL,
  `Rank` varchar(200) NOT NULL,
  `Appointment` varchar(100) NOT NULL,
  `Address` text NOT NULL,
  `PhoneNumber` varchar(100) NOT NULL,
  `ConnectionLetterNo` varchar(200) NOT NULL,
  `TerminationLetterNo` varchar(200) NOT NULL,
  `HomeAddress` text NOT NULL,
  `OfficeAddress` text NOT NULL,
  `ConnectionDate` date NOT NULL,
  `TerminationDate` date NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.allhistoryphoneinfo: ~0 rows (approximately)
DELETE FROM `allhistoryphoneinfo`;
/*!40000 ALTER TABLE `allhistoryphoneinfo` DISABLE KEYS */;
/*!40000 ALTER TABLE `allhistoryphoneinfo` ENABLE KEYS */;


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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.allphoneinfo: ~0 rows (approximately)
DELETE FROM `allphoneinfo`;
/*!40000 ALTER TABLE `allphoneinfo` DISABLE KEYS */;
INSERT INTO `allphoneinfo` (`ID`, `PhoneNumber`, `ServiceStatus`, `ConnectionTypeID`, `Remarks`) VALUES
	(2, '12345678', 'Terminated', 1, 'New Phone'),
	(3, '123456789', 'Terminated', 2, 'New Phone');
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
  `TelephoneUserId` int(11) NOT NULL,
  `ComplainDate` datetime NOT NULL,
  `ResolvedDate` datetime NOT NULL,
  `Remarks` text NOT NULL,
  `ActionTaken` text NOT NULL,
  `ResolvedBy` text NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `MenuComplainTypeId` (`MenuComplainTypeId`),
  KEY `TelephoneUserId` (`TelephoneUserId`),
  CONSTRAINT `FK_complains_menucomplaintype` FOREIGN KEY (`MenuComplainTypeId`) REFERENCES `menucomplaintype` (`Id`),
  CONSTRAINT `FK_complains_telephoneusers` FOREIGN KEY (`TelephoneUserId`) REFERENCES `telephoneusers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=75 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.complains: ~43 rows (approximately)
DELETE FROM `complains`;
/*!40000 ALTER TABLE `complains` DISABLE KEYS */;
INSERT INTO `complains` (`Id`, `Description`, `Status`, `MenuComplainTypeId`, `TelephoneUserId`, `ComplainDate`, `ResolvedDate`, `Remarks`, `ActionTaken`, `ResolvedBy`) VALUES
	(8, 'asdfasdf', 'Resolved', 2, 3, '2015-11-07 14:33:27', '2015-11-23 11:43:00', 'N/A', 'Cable changed', 'jubayer'),
	(9, 'Bad kotha', 'Resolve', 3, 4, '2015-11-07 14:35:37', '2015-11-08 14:35:37', '', 'Changed Line', 'Habib'),
	(10, 'bnbnbmbn', 'Pending', 3, 4, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(11, 'hhjjklllfgfgfg helloooo', 'Resolved', 3, 3, '2015-09-17 17:17:04', '2015-11-23 11:25:00', '', 'gghhuujj', 'Habib'),
	(12, 'bnbnbmbn', 'Pending', 3, 4, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(18, 'asdfasdf', 'Resolved', 3, 3, '2015-11-07 14:33:27', '2015-11-23 12:11:00', 'No Issue', 'Solved it', 'jubayer'),
	(36, 'bnbnbmbn', 'Resolved', 4, 5, '2015-11-13 05:47:53', '2015-11-21 16:16:00', '', 'Wire changed', 'Habib'),
	(39, 'New Connection Activation', 'Resolved', 5, 9, '2015-11-23 16:57:02', '2015-11-23 16:57:07', 'Addition Remark', 'Connection Provided', 'Atique'),
	(40, 'New Connection Activation', 'Resolved', 5, 12, '0000-00-00 00:00:00', '0000-00-00 00:00:00', 'Checking Done', 'Connection Provided', 'jubayer'),
	(41, 'Unclear sound, bad voice quality, Silent Call, noise', 'Resolved', 3, 13, '2015-10-17 17:17:04', '2015-11-21 13:50:00', 'Some remarks', '', 'Habib'),
	(42, 'asdfasdf', 'Pending', 2, 3, '2015-11-07 14:33:27', '0000-00-00 00:00:00', '', '', ''),
	(43, 'Bad kotha', 'Resolve', 3, 4, '2015-11-07 14:35:37', '2015-11-08 14:35:37', '', 'Changed Line', 'Habib'),
	(44, 'bnbnbmbn', 'Pending', 3, 4, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(45, 'hhjjklll', 'Resolved', 1, 3, '2015-09-17 17:17:04', '2015-11-24 20:28:00', 'asdf', 'fasdf', 'jubayer'),
	(46, 'bnbnbmbn', 'Pending', 3, 4, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(47, 'bnbnbmbn', 'Pending', 4, 13, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(48, 'bnbnbmbn', 'Pending', 4, 13, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(49, 'bnbnbmbn', 'Pending', 4, 13, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(50, 'bnbnbmbn', 'Pending', 4, 13, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(51, 'bnbnbmbn', 'Resolved', 4, 13, '2015-11-13 05:47:53', '2015-11-21 03:46:00', '', 'jhhjh', 'Habib'),
	(52, 'asdfasdf', 'Pending', 2, 3, '2015-11-07 14:33:27', '0000-00-00 00:00:00', '', '', ''),
	(53, 'Unclear sound, bad voice quality, Silent Call, noise', 'Resolved', 3, 13, '2015-10-17 17:17:04', '2015-11-20 22:19:00', 'Some remarks', 'Phone Ste Changed', 'Habib'),
	(54, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(55, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(56, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(57, 'Unclear sound, bad voice quality, Silent Call, noise', 'Resolved', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(58, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(59, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(60, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(61, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(62, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(63, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(64, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(65, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(66, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(67, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(68, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 13, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(69, 'bnbnbmbn', 'Resolved', 4, 5, '2015-11-13 05:47:53', '2015-11-21 16:16:00', '', 'Wire changed', 'Habib'),
	(70, 'cannot hear', 'Resolved', 2, 13, '2015-11-16 05:20:01', '0000-00-00 00:00:00', '', 'can hear now', 'Habib'),
	(71, 'Stolen by Zahurul', 'Pending', 7, 13, '2015-11-24 16:26:53', '0000-00-00 00:00:00', '', '', ''),
	(72, 'Disconnected', 'Pending', 1, 13, '2015-11-24 16:30:38', '0000-00-00 00:00:00', '', '', ''),
	(73, 'New Connection Activation', 'Resolved', 5, 10, '0000-00-00 00:00:00', '0000-00-00 00:00:00', 'temp', 'Connection Provided', 'jubayer'),
	(74, 'tesw', 'Pending', 2, 13, '2015-11-24 20:28:01', '0000-00-00 00:00:00', '', '', '');
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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.equipmnttype: ~7 rows (approximately)
DELETE FROM `equipmnttype`;
/*!40000 ALTER TABLE `equipmnttype` DISABLE KEYS */;
INSERT INTO `equipmnttype` (`Id`, `TypeName`) VALUES
	(1, 'E1 Link'),
	(2, 'Voice Musk'),
	(3, 'ONUs'),
	(4, 'MicroWave'),
	(5, 'PDH'),
	(6, 'onsu'),
	(7, 'SDH');
/*!40000 ALTER TABLE `equipmnttype` ENABLE KEYS */;


-- Dumping structure for table signalappdb.maildata
DROP TABLE IF EXISTS `maildata`;
CREATE TABLE IF NOT EXISTS `maildata` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `MailID` text NOT NULL,
  `MailDescription` text NOT NULL,
  `Date` date NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.maildata: ~11 rows (approximately)
DELETE FROM `maildata`;
/*!40000 ALTER TABLE `maildata` DISABLE KEYS */;
INSERT INTO `maildata` (`ID`, `MailID`, `MailDescription`, `Date`) VALUES
	(1, 'MID0001', 'DHK to CTG', '2015-11-24'),
	(2, 'MID0002', 'CTG to DHK', '2015-11-24'),
	(3, 'MID0003', 'CTG to BOG', '2015-11-24'),
	(4, 'MID0004', 'DHK to DHK', '2015-11-24'),
	(5, 'MID0005', 'BOG to DHK', '2015-11-24'),
	(6, 'MID0006', 'Jess to DHK', '2015-11-24'),
	(7, 'MID0007', 'BOG to DHK', '2015-11-24'),
	(8, 'MID0007', 'BOG to RAJ', '2015-11-24'),
	(9, 'MID0008', 'DHK to Saidpur', '2015-11-24'),
	(10, 'MID0009', 'Saidpur to Rangpur', '2015-11-24'),
	(11, 'MID00010', 'RAJ to CTG', '2015-11-24');
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
	(1, 'Army Phone', 'Connection Type'),
	(2, 'BTCL Phone', 'Connection Type');
/*!40000 ALTER TABLE `menuconnectiontype` ENABLE KEYS */;


-- Dumping structure for table signalappdb.menurequesttype
DROP TABLE IF EXISTS `menurequesttype`;
CREATE TABLE IF NOT EXISTS `menurequesttype` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `Value` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.menurequesttype: ~3 rows (approximately)
DELETE FROM `menurequesttype`;
/*!40000 ALTER TABLE `menurequesttype` DISABLE KEYS */;
INSERT INTO `menurequesttype` (`ID`, `Name`, `Value`) VALUES
	(1, 'Request', 'New Connection'),
	(2, 'Request', 'Shifting'),
	(3, 'Request', 'Termination');
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


-- Dumping structure for table signalappdb.pendingrequest
DROP TABLE IF EXISTS `pendingrequest`;
CREATE TABLE IF NOT EXISTS `pendingrequest` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `MenuRequestTypeID` int(11) NOT NULL,
  `PhoneUserPersonalInfoID` bigint(20) NOT NULL,
  `PhoneNumber` int(11) NOT NULL,
  `LetterNo` text NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `Request` (`MenuRequestTypeID`),
  KEY `PhoneNumber_AllPhones` (`PhoneNumber`),
  KEY `PersonalInfo_PhoneUserPersonalInfo` (`PhoneUserPersonalInfoID`),
  CONSTRAINT `PersonalInfo_PhoneUserPersonalInfo` FOREIGN KEY (`PhoneUserPersonalInfoID`) REFERENCES `phoneuserpersonalinfo` (`ID`),
  CONSTRAINT `PhoneNumber_AllPhones` FOREIGN KEY (`PhoneNumber`) REFERENCES `allphoneinfo` (`ID`),
  CONSTRAINT `Request` FOREIGN KEY (`MenuRequestTypeID`) REFERENCES `menurequesttype` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.pendingrequest: ~0 rows (approximately)
DELETE FROM `pendingrequest`;
/*!40000 ALTER TABLE `pendingrequest` DISABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.phoneuserpersonalinfo: ~1 rows (approximately)
DELETE FROM `phoneuserpersonalinfo`;
/*!40000 ALTER TABLE `phoneuserpersonalinfo` DISABLE KEYS */;
INSERT INTO `phoneuserpersonalinfo` (`ID`, `BANumber`, `FullName`, `Appointment`, `PlateName`, `ServiceStatus`, `RankId`, `Sex`, `Unit`, `JoiningDate`, `LPRDate`, `MaritalStatus`, `PresentAddress`, `PermanentAddress`, `OfficeAddress`, `PersonalPhoneNumber`, `EmailAddress`) VALUES
	(3, 'BA123456', 'Atique Reza Chowdhury', 'Appointment', 'Atique', 'On Service', 3, 'male', 'NSN', '2015-02-12', '0001-01-01', 'unmarried', 'Ibrahim pur', 'Kaharol', 'Mohakhali', '01926662260', 'Email.Address@email.com');
/*!40000 ALTER TABLE `phoneuserpersonalinfo` ENABLE KEYS */;


-- Dumping structure for table signalappdb.resolvedrequest
DROP TABLE IF EXISTS `resolvedrequest`;
CREATE TABLE IF NOT EXISTS `resolvedrequest` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `AllPhoneInfoId` int(11) NOT NULL,
  `RequestType` varchar(100) NOT NULL,
  `LetterNo` varchar(200) NOT NULL,
  `RequestDate` date NOT NULL,
  `ResolveDate` date NOT NULL,
  `PhoneNumber` varchar(50) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `phone number` (`AllPhoneInfoId`),
  CONSTRAINT `phone number` FOREIGN KEY (`AllPhoneInfoId`) REFERENCES `allphoneinfo` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.resolvedrequest: ~0 rows (approximately)
DELETE FROM `resolvedrequest`;
/*!40000 ALTER TABLE `resolvedrequest` DISABLE KEYS */;
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
