CREATE DATABASE  IF NOT EXISTS `supermarket` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `supermarket`;
-- MySQL dump 10.13  Distrib 5.6.12, for Win64 (x86_64)
--
-- Host: localhost    Database: supermarket
-- ------------------------------------------------------
-- Server version	5.6.12

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
-- Table structure for table `measures`
--

DROP TABLE IF EXISTS `measures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `measures` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `MeasureName` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=103 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `measures`
--

LOCK TABLES `measures` WRITE;
/*!40000 ALTER TABLE `measures` DISABLE KEYS */;
INSERT INTO `measures` VALUES (100,'liters'),(101,'pieces'),(102,'kg');
/*!40000 ALTER TABLE `measures` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `products` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `VendorID` int(11) DEFAULT NULL,
  `ProductName` varchar(45) NOT NULL,
  `MeasureID` int(11) DEFAULT NULL,
  `BasePrice` double DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `Product_Measurment_Id_idx` (`MeasureID`),
  KEY `Product_Vendor_Id_idx` (`VendorID`),
  CONSTRAINT `Product_Vendor_Id` FOREIGN KEY (`VendorID`) REFERENCES `vendors` (`idvendorsID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `Product_Measurment_Id` FOREIGN KEY (`MeasureID`) REFERENCES `measures` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,3,'Beer \"Zagorka\"',100,0.86),(2,4,'Vodka “Targovishte”',100,7.53),(3,3,'Beer “Beck’s”',100,1.03),(4,1,'Chocolate “Milka”',101,1.45),(5,5,'Beer “Kamenica”',100,0.85),(6,12,'Milk \"Vereya\"',102,1.98),(7,6,'Cheese \"Rodopeya\"',102,8.56),(8,13,'Sausage \"Orehite\"',102,26.89),(9,15,'Mineral water \"Devin\"',100,0.66),(10,7,'Wiener \"Leki\"',102,3.26),(11,12,'Yogurt \"Vereya\"',101,0.89),(12,5,'Beer \"Astika\"',100,0.79),(13,8,'Wiener \"Tandem\"',102,3.78),(14,9,'Mastika \"Peshtera\"',100,7.45),(15,17,'\"Coca-cola\"',100,1.25),(16,16,'Mineral water \"Bankq\"',100,0.67),(17,11,'Bear \"Sumensko\"',100,0.87),(18,8,'Sausage \"Tandem\"',102,11.26),(19,17,'\"Fanta\"',100,1.25),(20,6,'Milk \"Rodopeya\"',100,1.94),(21,15,'Spring water \"Devin\"',100,0.68),(22,12,'Cheese \"Vereya\"',102,8.23),(23,9,'Rakiq \"Peshtera\"',100,8.65),(24,10,'Chocolate \"Svoge\"',101,1.21),(25,1,'Icecream \"Boss\"',101,1.68),(26,13,'Fillet Elena \"Orehite\"',102,28.66),(27,17,'\"Sprite\"',100,1.2),(28,14,'Peanuts \"Kronos\"',101,0.65),(29,14,'Almonds \"Kronos\"',101,2.45),(30,6,'Yogurt \"Rododpeya\"',101,0.88),(31,5,'Bear \"Burgasko\"',100,0.75),(32,10,'Candy  \"Svoge\"',101,2.35),(33,4,'Gin \"Tyrgovishte\"',100,8.24),(34,1,'Candy \"Nestle\"',101,3.48),(35,14,'Hezulnuts \"Kronos\"',101,3.21);
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vendors`
--

DROP TABLE IF EXISTS `vendors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `vendors` (
  `idvendorsID` int(11) NOT NULL AUTO_INCREMENT,
  `VendorName` varchar(45) NOT NULL,
  PRIMARY KEY (`idvendorsID`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vendors`
--

LOCK TABLES `vendors` WRITE;
/*!40000 ALTER TABLE `vendors` DISABLE KEYS */;
INSERT INTO `vendors` VALUES (1,'Nestle Sofia Corp.'),(3,'Zagorka Corp.'),(4,'Targovishte Bottling Company Ltd.'),(5,'Kamenica Corp.'),(6,'Rodopeya Ltd.'),(7,'Leki Ltd.'),(8,'Tandem Ltd.'),(9,'Vinprom Peshtera Ltd.'),(10,'Svoge Ltd.'),(11,'Shumensko pivo Corp.'),(12,'Vereya Ltd.'),(13,'Orehite Ltd.'),(14,'Kronos Ltd.'),(15,'Devin Ltd.'),(16,'Bankq Ltd.'),(17,'Coca-Cola Corp.');
/*!40000 ALTER TABLE `vendors` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2013-07-22 13:40:51
