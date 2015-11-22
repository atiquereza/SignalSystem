-- --------------------------------------------------------
-- Host:                         localhost
-- Server version:               5.5.16 - MySQL Community Server (GPL)
-- Server OS:                    Win32
-- HeidiSQL Version:             9.2.0.4947
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping database structure for signalappdb
CREATE DATABASE IF NOT EXISTS `signalappdb` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `signalappdb`;


-- Dumping structure for table signalappdb.appmenuitems
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
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.complains: ~30 rows (approximately)
DELETE FROM `complains`;
/*!40000 ALTER TABLE `complains` DISABLE KEYS */;
INSERT INTO `complains` (`Id`, `Description`, `Status`, `MenuComplainTypeId`, `TelephoneUserId`, `ComplainDate`, `ResolvedDate`, `Remarks`, `ActionTaken`, `ResolvedBy`) VALUES
	(5, 'Unclear sound, bad voice quality, Silent Call, noise', 'Resolved', 3, 2, '2015-10-17 17:17:04', '2015-11-21 13:50:00', 'Some remarks', '', 'Habib'),
	(8, 'asdfasdf', 'Pending', 2, 3, '2015-11-07 14:33:27', '0000-00-00 00:00:00', '', '', ''),
	(9, 'Bad kotha', 'Resolve', 3, 4, '2015-11-07 14:35:37', '2015-11-08 14:35:37', '', 'Changed Line', 'Habib'),
	(10, 'bnbnbmbn', 'Pending', 3, 4, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(11, 'hhjjklll', 'Pending', 1, 3, '2015-09-17 17:17:04', '0000-00-00 00:00:00', '', '', ''),
	(12, 'bnbnbmbn', 'Pending', 3, 4, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(13, 'bnbnbmbn', 'Pending', 4, 2, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(14, 'bnbnbmbn', 'Pending', 4, 2, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(15, 'bnbnbmbn', 'Pending', 4, 2, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(16, 'bnbnbmbn', 'Pending', 4, 2, '2015-11-13 05:47:53', '2015-11-13 05:47:56', '', '', ''),
	(17, 'bnbnbmbn', 'Resolved', 4, 2, '2015-11-13 05:47:53', '2015-11-21 03:46:00', '', 'jhhjh', 'Habib'),
	(18, 'asdfasdf', 'Pending', 2, 3, '2015-11-07 14:33:27', '0000-00-00 00:00:00', '', '', ''),
	(20, 'Unclear sound, bad voice quality, Silent Call, noise', 'Resolved', 3, 2, '2015-10-17 17:17:04', '2015-11-20 22:19:00', 'Some remarks', 'Phone Ste Changed', 'Habib'),
	(21, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(22, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(23, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(24, 'Unclear sound, bad voice quality, Silent Call, noise', 'Resolved', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(25, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(26, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(27, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(28, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(29, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(30, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(31, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(32, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(33, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(34, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(35, 'Unclear sound, bad voice quality, Silent Call, noise', 'Pending', 3, 2, '2015-10-17 17:17:04', '2015-10-17 23:17:04', 'Some remarks', ' Some ActionTaken', 'Habib'),
	(36, 'bnbnbmbn', 'Resolved', 4, 5, '2015-11-13 05:47:53', '2015-11-21 16:16:00', '', 'Wire changed', 'Habib'),
	(38, 'cannot hear', 'Resolved', 2, 2, '2015-11-16 05:20:01', '0000-00-00 00:00:00', '', 'can hear now', 'Habib');
/*!40000 ALTER TABLE `complains` ENABLE KEYS */;


-- Dumping structure for table signalappdb.equipemntdescription
CREATE TABLE IF NOT EXISTS `equipemntdescription` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TypeId` int(11) NOT NULL DEFAULT '0',
  `Amount` int(11) NOT NULL DEFAULT '0',
  `Description` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `FK__equipmnttype` (`TypeId`),
  CONSTRAINT `FK__equipmnttype` FOREIGN KEY (`TypeId`) REFERENCES `equipmnttype` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.equipemntdescription: ~2 rows (approximately)
DELETE FROM `equipemntdescription`;
/*!40000 ALTER TABLE `equipemntdescription` DISABLE KEYS */;
INSERT INTO `equipemntdescription` (`Id`, `TypeId`, `Amount`, `Description`) VALUES
	(1, 1, 50, 'good'),
	(3, 4, 900, 'very good tooo'),
	(4, 4, 60, 'Quality Product'),
	(5, 1, 10, '10 feet'),
	(6, 7, 30, 'Ericsson');
/*!40000 ALTER TABLE `equipemntdescription` ENABLE KEYS */;


-- Dumping structure for table signalappdb.equipmnttype
CREATE TABLE IF NOT EXISTS `equipmnttype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TypeName` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.equipmnttype: ~4 rows (approximately)
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


-- Dumping structure for table signalappdb.menucomplaintype
CREATE TABLE IF NOT EXISTS `menucomplaintype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Value` text NOT NULL,
  `Name` text NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.menucomplaintype: ~4 rows (approximately)
DELETE FROM `menucomplaintype`;
/*!40000 ALTER TABLE `menucomplaintype` DISABLE KEYS */;
INSERT INTO `menucomplaintype` (`Id`, `Value`, `Name`) VALUES
	(1, 'Disconnected', 'Complain'),
	(2, 'Unclear Sound', 'Complain'),
	(3, 'Phone Faulty', 'Complain'),
	(4, 'Other', 'Complain');
/*!40000 ALTER TABLE `menucomplaintype` ENABLE KEYS */;


-- Dumping structure for table signalappdb.menuitems
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


-- Dumping structure for table signalappdb.menusrank
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


-- Dumping structure for table signalappdb.roles
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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.telephoneusers: ~4 rows (approximately)
DELETE FROM `telephoneusers`;
/*!40000 ALTER TABLE `telephoneusers` DISABLE KEYS */;
INSERT INTO `telephoneusers` (`Id`, `BANumber`, `Name`, `RankId`, `Status`, `NewPhoneNumber`, `Address`, `Gender`, `ConnectedDate`, `DisconnectedDate`) VALUES
	(2, 'BA123555', 'Khairul', 5, 'Connected', '123456', 'Cox\'s Bazar', 'male', '2015-10-17', '0000-00-00'),
	(3, 'BA123456', 'Test User', 1, 'Connected', '987654', 'Dhaka', 'male', '2015-11-07', '0000-00-00'),
	(4, 'BA123456', 'Test User1', 1, 'Connected', '9876547', 'Dhaka', 'male', '2015-11-07', '0000-00-00'),
	(5, 'BA123457\r\n', 'Test User1hhkk jjhdfdfdsfsdfl fdfdfdfd dfdfdf', 1, 'Connected', '98765478889999999', 'Dhaka', 'male', '2015-11-07', '0000-00-00');
/*!40000 ALTER TABLE `telephoneusers` ENABLE KEYS */;


-- Dumping structure for table signalappdb.userinfo
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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.userinfo: ~6 rows (approximately)
DELETE FROM `userinfo`;
/*!40000 ALTER TABLE `userinfo` DISABLE KEYS */;
INSERT INTO `userinfo` (`ID`, `UserID`, `UserName`, `FullName`, `RankId`, `Email`, `Gender`, `BANumber`) VALUES
	(1, 4, 'jubayer', 'All Jubayer Mohammad Mahamudunnabi.', 2, 'aljubayer@gmail.com', 'Male', 'BA123551'),
	(2, 5, 'atique', 'atique', 3, 'atique@gmail.com', 'Male', 'BA123552'),
	(3, 10, 'habib', 'habib', 2, 'habib@gmail.com', 'Male', 'BA123553'),
	(4, 115, 'sakib', 'sakib', 5, 'sakib@gmail.com', 'Male', 'BA123554'),
	(10, 128, 'Khairul', 'Khairul', 1, 'khairul@gmail.com', 'female', 'BA123555'),
	(11, 131, 'Razib', 'Razib', 4, 'asdf@gamil.com', 'male', 'BA123556');
/*!40000 ALTER TABLE `userinfo` ENABLE KEYS */;


-- Dumping structure for table signalappdb.users
CREATE TABLE IF NOT EXISTS `users` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(50) NOT NULL,
  `UserCredential` varchar(50) NOT NULL,
  `UserRoleId` int(11) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `UserRoleId` (`UserRoleId`),
  CONSTRAINT `UserRoleId` FOREIGN KEY (`UserRoleId`) REFERENCES `roles` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=132 DEFAULT CHARSET=latin1;

-- Dumping data for table signalappdb.users: ~6 rows (approximately)
DELETE FROM `users`;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`ID`, `UserName`, `UserCredential`, `UserRoleId`) VALUES
	(4, 'jubayer', '01926662274', 2),
	(5, 'atique', '01926662227', 2),
	(10, 'habib', '01926662227', 9),
	(115, 'sakib', '01926662227', 10),
	(128, 'Khairul', '123456', 2),
	(131, 'Razib', '123456', 9);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
