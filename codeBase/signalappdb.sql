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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- Data exporting was unselected.


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table signalappdb.equipmnttype
DROP TABLE IF EXISTS `equipmnttype`;
CREATE TABLE IF NOT EXISTS `equipmnttype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TypeName` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table signalappdb.menucomplaintype
DROP TABLE IF EXISTS `menucomplaintype`;
CREATE TABLE IF NOT EXISTS `menucomplaintype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Value` text NOT NULL,
  `Name` text NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table signalappdb.menuconnectiontype
DROP TABLE IF EXISTS `menuconnectiontype`;
CREATE TABLE IF NOT EXISTS `menuconnectiontype` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Value` varchar(50) NOT NULL,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table signalappdb.menurequesttype
DROP TABLE IF EXISTS `menurequesttype`;
CREATE TABLE IF NOT EXISTS `menurequesttype` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Value` varchar(50) DEFAULT NULL,
  `Name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table signalappdb.menusrank
DROP TABLE IF EXISTS `menusrank`;
CREATE TABLE IF NOT EXISTS `menusrank` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `Value` text NOT NULL,
  `Name` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table signalappdb.menustations
DROP TABLE IF EXISTS `menustations`;
CREATE TABLE IF NOT EXISTS `menustations` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Value` varchar(50) NOT NULL,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table signalappdb.resolvedcomplains
DROP TABLE IF EXISTS `resolvedcomplains`;
CREATE TABLE IF NOT EXISTS `resolvedcomplains` (
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
  CONSTRAINT `resolvedcomplains_ibfk_1` FOREIGN KEY (`AllPhoneInfoID`) REFERENCES `allphoneinfo` (`ID`),
  CONSTRAINT `resolvedcomplains_ibfk_2` FOREIGN KEY (`MenuComplainTypeId`) REFERENCES `menucomplaintype` (`Id`),
  CONSTRAINT `resolvedcomplains_ibfk_3` FOREIGN KEY (`TelephoneUserId`) REFERENCES `phoneuserpersonalinfo` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- Data exporting was unselected.


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table signalappdb.roles
DROP TABLE IF EXISTS `roles`;
CREATE TABLE IF NOT EXISTS `roles` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(200) NOT NULL,
  `ParentRoleName` varchar(200) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table signalappdb.technicaldescription
DROP TABLE IF EXISTS `technicaldescription`;
CREATE TABLE IF NOT EXISTS `technicaldescription` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TypeId` int(11) NOT NULL DEFAULT '0',
  `Amount` int(11) NOT NULL DEFAULT '0',
  `OnAirDate` date DEFAULT NULL,
  `Description` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `FK__equipmnttype` (`TypeId`),
  CONSTRAINT `technicaldescription_ibfk_1` FOREIGN KEY (`TypeId`) REFERENCES `equipmnttype` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- Data exporting was unselected.


-- Dumping structure for table signalappdb.technicaltype
DROP TABLE IF EXISTS `technicaltype`;
CREATE TABLE IF NOT EXISTS `technicaltype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TypeName` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- Data exporting was unselected.


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
