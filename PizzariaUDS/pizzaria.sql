-- MySQL dump 10.13  Distrib 8.0.13, for Win64 (x86_64)
--
-- Host: localhost    Database: pizzaria
-- ------------------------------------------------------
-- Server version	8.0.13

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `adicional`
--

DROP TABLE IF EXISTS `adicional`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `adicional` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `descricao` varchar(45) NOT NULL,
  `tempoPreparo` smallint(4) DEFAULT NULL,
  `valor` decimal(9,2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `descricao_UNIQUE` (`descricao`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pedido`
--

DROP TABLE IF EXISTS `pedido`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `pedido` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `pizzaId` int(10) unsigned NOT NULL,
  `tempoPreparoTotal` smallint(4) unsigned NOT NULL,
  `valorTotal` decimal(10,2) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_PEDIDO_PIZZA_idx` (`pizzaId`),
  CONSTRAINT `FK_PEDIDO_PIZZA` FOREIGN KEY (`pizzaId`) REFERENCES `pizza` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pizza`
--

DROP TABLE IF EXISTS `pizza`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `pizza` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `saborId` smallint(4) unsigned NOT NULL,
  `tamanhoId` smallint(4) unsigned NOT NULL,
  `valor` decimal(9,2) unsigned NOT NULL,
  `tempoPreparo` smallint(4) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_PIZZA_SABOR_idx` (`saborId`),
  KEY `FK_PIZZA_TAMANHO_idx` (`tamanhoId`),
  CONSTRAINT `FK_PIZZA_SABOR` FOREIGN KEY (`saborId`) REFERENCES `sabor` (`id`),
  CONSTRAINT `FK_PIZZA_TAMANHO` FOREIGN KEY (`tamanhoId`) REFERENCES `tamanho` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pizza_adicional`
--

DROP TABLE IF EXISTS `pizza_adicional`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `pizza_adicional` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `pizzaId` int(10) unsigned NOT NULL,
  `adicionalId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `FK_PIZZA_ADICIONAL_ADCIONAL_PIZZA` (`pizzaId`,`adicionalId`),
  KEY `FK_PIZZA_ADICIONAL_ADICIONAL_idx` (`adicionalId`),
  KEY `FK_PIZZA_ADICIONAL_PIZZA_idx` (`pizzaId`),
  CONSTRAINT `FK_PIZZAS_ADICIONAIS_ADICIONAL` FOREIGN KEY (`adicionalId`) REFERENCES `adicional` (`id`),
  CONSTRAINT `FK_PIZZAS_ADICIONAIS_PIZZA` FOREIGN KEY (`pizzaId`) REFERENCES `pizza` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sabor`
--

DROP TABLE IF EXISTS `sabor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `sabor` (
  `id` smallint(4) unsigned NOT NULL AUTO_INCREMENT,
  `descricao` varchar(45) NOT NULL,
  `tempoAdicional` smallint(4) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `descricao_UNIQUE` (`descricao`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tamanho`
--

DROP TABLE IF EXISTS `tamanho`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tamanho` (
  `id` smallint(4) unsigned NOT NULL AUTO_INCREMENT,
  `descricao` varchar(45) NOT NULL,
  `valor` decimal(9,2) unsigned NOT NULL,
  `tempoPreparo` smallint(4) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `descricao_UNIQUE` (`descricao`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-01-19 16:12:32
