-- phpMyAdmin SQL Dump
-- version 5.2.3
-- https://www.phpmyadmin.net/
--
-- Hôte : db:3306
-- Généré le : mar. 04 nov. 2025 à 17:11
-- Version du serveur : 8.1.0
-- Version de PHP : 8.3.26

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `gsbMonolith`
--
CREATE DATABASE IF NOT EXISTS `gsbMonolith` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE `gsbMonolith`;

-- --------------------------------------------------------

--
-- Structure de la table `Appartient`
--

CREATE TABLE `Appartient` (
  `id_prescription` int DEFAULT NULL,
  `id_medicine` int DEFAULT NULL,
  `quantity` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `Appartient`
--

INSERT INTO `Appartient` (`id_prescription`, `id_medicine`, `quantity`) VALUES
(1, 1, 1),
(1, 2, 1),
(2, 3, 5),
(3, 4, 6),
(4, 5, 4),
(5, 1, 1),
(6, 2, 3),
(7, 6, 2),
(8, 3, 1),
(9, 7, 7),
(10, 1, 2),
(11, 4, 3),
(12, 8, 2),
(13, 9, 3),
(14, 2, 1),
(15, 10, 3),
(16, 5, 8),
(17, 3, 1),
(18, 1, 6),
(19, 7, 6),
(20, 6, 2),
(21, 2, 1),
(21, 7, 1);

-- --------------------------------------------------------

--
-- Structure de la table `Medicine`
--

CREATE TABLE `Medicine` (
  `id_medicine` int NOT NULL,
  `id_user` int DEFAULT NULL,
  `dosage` int DEFAULT NULL,
  `name` varchar(100) DEFAULT NULL,
  `description` text,
  `molecule` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `Medicine`
--

INSERT INTO `Medicine` (`id_medicine`, `id_user`, `dosage`, `name`, `description`, `molecule`) VALUES
(1, 3, 500, 'Doliprane', 'Antidouleur et antipyrétique', 'Paracétamol'),
(2, 3, 20, 'Ibuprofène', 'Anti-inflammatoire', 'Ibuprofen'),
(3, 3, 5, 'Amlor', 'Traitement de l’hypertension', 'Amlodipine'),
(4, 3, 500, 'Amoxicilline', 'Antibiotique à large spectre', 'Amoxicillin'),
(5, 3, 10, 'Lexomil', 'Anxiolytique léger', 'Bromazépam'),
(6, 3, 50, 'Seroplex', 'Antidépresseur ISRS', 'Escitalopram'),
(7, 3, 100, 'Levothyrox', 'Substitut hormonal thyroïdien', 'L-thyroxine'),
(8, 3, 500, 'Augmentin', 'Antibiotique', 'Amoxicilline/Clavulanate'),
(9, 3, 75, 'Plavix', 'Antiagrégant plaquettaire', 'Clopidogrel'),
(10, 3, 10, 'Zyrtec', 'Antihistaminique', 'Cetirizine');

-- --------------------------------------------------------

--
-- Structure de la table `Patients`
--

CREATE TABLE `Patients` (
  `id_patient` int NOT NULL,
  `id_user` int DEFAULT NULL,
  `name` varchar(50) DEFAULT NULL,
  `age` int DEFAULT NULL,
  `firstname` varchar(50) DEFAULT NULL,
  `gender` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `Patients`
--

INSERT INTO `Patients` (`id_patient`, `id_user`, `name`, `age`, `firstname`, `gender`) VALUES
(1, 1, 'Dupont', 34, 'Marie', 'F'),
(2, 1, 'Bernard', 45, 'Luc', 'M'),
(3, 2, 'Moreau', 52, 'Sophie', 'F'),
(4, 1, 'Roux', 29, 'Julien', 'M'),
(5, 4, 'Fournier', 60, 'Chantal', 'F'),
(6, 2, 'Girard', 38, 'Nicolas', 'M'),
(7, 1, 'Chevalier', 42, 'Laura', 'F'),
(8, 2, 'Blanc', 31, 'Alexandre', 'M'),
(9, 4, 'Faure', 47, 'Isabelle', 'F'),
(10, 1, 'Garnier', 28, 'Thomas', 'M'),
(11, 4, 'Renaud', 36, 'Caroline', 'F'),
(12, 2, 'Henry', 50, 'David', 'M'),
(13, 1, 'Masson', 27, 'Emma', 'F'),
(14, 4, 'Marchand', 40, 'Vincent', 'M'),
(15, 1, 'Lefevre', 33, 'Julie', 'F'),
(16, 2, 'Carpentier', 58, 'Alain', 'M'),
(17, 1, 'Perrot', 46, 'Nathalie', 'F'),
(18, 2, 'Michel', 37, 'Paul', 'M'),
(19, 4, 'Barbier', 41, 'Camille', 'F'),
(20, 1, 'Giraud', 35, 'Antoine', 'M'),
(22, 1, 'pablo', 78, 'jesapellegrout', 'H'),
(23, 1, 'Fernandes', 69, 'Dylen', 'H');

-- --------------------------------------------------------

--
-- Structure de la table `Prescription`
--

CREATE TABLE `Prescription` (
  `id_prescription` int NOT NULL,
  `id_user` int DEFAULT NULL,
  `id_patient` int DEFAULT NULL,
  `validity` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `Prescription`
--

INSERT INTO `Prescription` (`id_prescription`, `id_user`, `id_patient`, `validity`) VALUES
(1, 1, 1, '2025-11-30'),
(2, 2, 2, '2025-12-15'),
(3, 1, 3, '2025-11-05'),
(4, 4, 4, '2025-12-01'),
(5, 1, 5, '2025-12-20'),
(6, 2, 6, '2025-11-15'),
(7, 1, 7, '2025-11-10'),
(8, 4, 8, '2025-12-05'),
(9, 2, 9, '2025-11-25'),
(10, 1, 10, '2025-12-30'),
(11, 4, 11, '2025-11-18'),
(12, 1, 12, '2025-12-22'),
(13, 2, 13, '2025-11-29'),
(14, 1, 14, '2025-11-17'),
(15, 2, 15, '2025-12-12'),
(16, 4, 16, '2025-12-01'),
(17, 1, 17, '2025-11-30'),
(18, 2, 18, '2025-11-25'),
(19, 1, 19, '2025-12-15'),
(20, 4, 20, '2025-11-28'),
(21, 1, 19, '2025-11-03');

-- --------------------------------------------------------

--
-- Structure de la table `Users`
--

CREATE TABLE `Users` (
  `id_user` int NOT NULL,
  `firstname` varchar(50) DEFAULT NULL,
  `role` tinyint(1) DEFAULT NULL,
  `name` varchar(50) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `password` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `Users`
--

INSERT INTO `Users` (`id_user`, `firstname`, `role`, `name`, `email`, `password`) VALUES
(1, 'Alice', 0, 'Martin', 'alice.martin@clinic.fr', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8'),
(2, 'Hugo', 1, 'Durand', 'hugo.durand@clinic.fr', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8'),
(3, 'Léa', 0, 'Petit', 'lea.petit@pharma.fr', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8'),
(4, 'Thomas', 1, 'Robert', 'thomas.robert@clinic.fr', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8'),
(5, 'Sarah', 0, 'Lemoine', 'sarah.lemoine@admin.fr', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8');

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `Appartient`
--
ALTER TABLE `Appartient`
  ADD KEY `id_prescription` (`id_prescription`),
  ADD KEY `id_medicine` (`id_medicine`);

--
-- Index pour la table `Medicine`
--
ALTER TABLE `Medicine`
  ADD PRIMARY KEY (`id_medicine`),
  ADD KEY `id_user` (`id_user`);

--
-- Index pour la table `Patients`
--
ALTER TABLE `Patients`
  ADD PRIMARY KEY (`id_patient`),
  ADD KEY `id_user` (`id_user`);

--
-- Index pour la table `Prescription`
--
ALTER TABLE `Prescription`
  ADD PRIMARY KEY (`id_prescription`),
  ADD KEY `id_user` (`id_user`),
  ADD KEY `Prescription_ibfk_2` (`id_patient`);

--
-- Index pour la table `Users`
--
ALTER TABLE `Users`
  ADD PRIMARY KEY (`id_user`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `Medicine`
--
ALTER TABLE `Medicine`
  MODIFY `id_medicine` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT pour la table `Patients`
--
ALTER TABLE `Patients`
  MODIFY `id_patient` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT pour la table `Prescription`
--
ALTER TABLE `Prescription`
  MODIFY `id_prescription` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT pour la table `Users`
--
ALTER TABLE `Users`
  MODIFY `id_user` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `Appartient`
--
ALTER TABLE `Appartient`
  ADD CONSTRAINT `Appartient_ibfk_1` FOREIGN KEY (`id_prescription`) REFERENCES `Prescription` (`id_prescription`) ON DELETE CASCADE,
  ADD CONSTRAINT `Appartient_ibfk_2` FOREIGN KEY (`id_medicine`) REFERENCES `Medicine` (`id_medicine`) ON DELETE CASCADE;

--
-- Contraintes pour la table `Medicine`
--
ALTER TABLE `Medicine`
  ADD CONSTRAINT `Medicine_ibfk_1` FOREIGN KEY (`id_user`) REFERENCES `Users` (`id_user`) ON DELETE CASCADE;

--
-- Contraintes pour la table `Patients`
--
ALTER TABLE `Patients`
  ADD CONSTRAINT `Patients_ibfk_1` FOREIGN KEY (`id_user`) REFERENCES `Users` (`id_user`) ON DELETE CASCADE;

--
-- Contraintes pour la table `Prescription`
--
ALTER TABLE `Prescription`
  ADD CONSTRAINT `Prescription_ibfk_1` FOREIGN KEY (`id_user`) REFERENCES `Users` (`id_user`) ON DELETE CASCADE,
  ADD CONSTRAINT `Prescription_ibfk_2` FOREIGN KEY (`id_patient`) REFERENCES `Patients` (`id_patient`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
