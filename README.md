# DOCUMENTATION TECHNIQUE COMPLÈTE – PROJET GSB MONOLITH

![C#](https://img.shields.io/badge/C%23-11.0-blue?logo=c-sharp)
![.NET Framework](https://img.shields.io/badge/.NET-6.0-blueviolet?logo=.net)
![Windows Forms](https://img.shields.io/badge/Windows_Forms-WinForms-blue)
![MySQL](https://img.shields.io/badge/MySQL-8.1-orange?logo=mysql)
![Docker](https://img.shields.io/badge/Docker-Powered-blue?logo=docker)
![Licence](https://img.shields.io/badge/Licence-MIT-green)

---

## 1. CONTEXTE ET OBJECTIFS DU PROJET

### 1.1. Présentation du Mandat GSB

Le laboratoire **Galaxy Swiss Bourdin (GSB)**, acteur majeur de l'industrie pharmaceutique, a mandaté la réalisation d'une application de bureau dans le cadre de la modernisation de son système d'information. Ce projet, réalisé dans le contexte de l'épreuve E4 "Support et mise à disposition de services informatiques" du **BTS SIO (Services Informatiques aux Organisations)**, vise à remplacer les processus manuels et fragmentés (fichiers Excel, documents papier) par une solution centralisée, robuste et sécurisée.

Le système actuel présentait des risques majeurs :
- **Hétérogénéité des données** : Pas de format standardisé pour les informations sur les patients et les prescriptions.
- **Manque de traçabilité** : Difficulté à suivre l'historique des modifications et à identifier les responsables.
- **Risques de sécurité** : Informations sensibles stockées sans contrôle d'accès adéquat.
- **Perte de temps et d'efficacité** : Processus de saisie et de recherche lents et redondants.

### 1.2. Cahier des Charges Fonctionnel

L'application **GSB Monolith** a été conçue pour répondre aux exigences suivantes :
- **Centralisation** : Fournir un point d'accès unique pour la gestion des données métiers.
- **Sécurité** : Garantir la confidentialité et l'intégrité des données par un système d'authentification et de gestion des droits.
- **Traçabilité** : Journaliser les opérations critiques pour un audit ultérieur.
- **Ergonomie** : Proposer une interface utilisateur intuitive et adaptée aux différents corps de métier (médecins, pharmaciens, administrateurs).
- **Maintenabilité** : Adopter une architecture logicielle claire et documentée pour faciliter les évolutions futures.

---

## 2. PÉRIMÈTRE FONCTIONNEL ET USER STORIES

### 2.1. Module d'Authentification et Gestion des Utilisateurs

- **User Story 1 (Connexion)** : En tant qu'utilisateur (médecin, pharmacien, admin), je veux me connecter à l'application avec mon email et un mot de passe pour accéder à mes fonctionnalités dédiées.
- **User Story 2 (Droits d'Accès)** : En tant qu'utilisateur, je ne veux voir que les menus et les options correspondant à mon rôle pour éviter les erreurs et protéger les données.
- **User Story 3 (Gestion des Comptes)** : En tant qu'administrateur, je veux pouvoir créer, modifier, et désactiver des comptes utilisateurs pour gérer les effectifs.

### 2.2. Module de Gestion des Patients

- **User Story 4 (Création Patient)** : En tant que médecin, je veux pouvoir créer une nouvelle fiche patient avec ses informations personnelles (nom, prénom, âge, genre).
- **User Story 5 (Consultation Patients)** : En tant que médecin, je veux pouvoir consulter la liste de tous mes patients et filtrer la liste par nom pour retrouver rapidement un dossier.
- **User Story 6 (Mise à Jour Patient)** : En tant que médecin, je veux pouvoir modifier les informations d'un patient existant.

### 2.3. Module de Gestion des Médicaments

- **User Story 7 (Catalogue Médicaments)** : En tant que pharmacien, je veux pouvoir consulter la liste de tous les médicaments disponibles dans le système.
- **User Story 8 (Ajout Médicament)** : En tant que pharmacien, je veux pouvoir ajouter un nouveau médicament au catalogue en spécifiant son nom, sa description, son dosage et la molécule principale.
- **User Story 9 (Modification Médicament)** : En tant que pharmacien, je veux pouvoir mettre à jour les informations d'un médicament ou le retirer de la vente.

### 2.4. Module de Gestion des Prescriptions

- **User Story 10 (Création Prescription)** : En tant que médecin, je veux pouvoir créer une nouvelle prescription pour un patient donné, en y ajoutant un ou plusieurs médicaments avec une quantité spécifique.
- **User Story 11 (Validation Atomique)** : Le système doit garantir que la création d'une prescription et l'ajout des médicaments associés sont une opération atomique (tout ou rien) pour éviter les prescriptions incomplètes.
- **User Story 12 (Consultation Prescription)** : En tant que médecin, je veux pouvoir consulter l'historique des prescriptions d'un patient.
- **User Story 13 (Export PDF)** : En tant que médecin, je veux pouvoir exporter une prescription au format PDF pour l'imprimer ou l'envoyer.

---

## 3. ARCHITECTURE LOGICIELLE DÉTAILLÉE

L'application est construite sur une architecture en couches (N-Tier) afin de découpler la logique de présentation, la logique métier et l'accès aux données.

```
+-------------------------------------------------+
|                couche PRÉSENTATION               |
|  (Namespace: gsbMonolith.Forms)                 |
|  WinForms, Contrôles UI, Gestion des événements |
+-------------------------------------------------+
                        |
+-------------------------------------------------+
|                  couche MÉTIER                  |
|  (Namespace: gsbMonolith.Models)                |
|  Classes: User, Patient, Medicine, etc.         |
+-------------------------------------------------+
                        |
+-------------------------------------------------+
|            couche D'ACCÈS AUX DONNÉES           |
|  (Namespace: gsbMonolith.DAO)                   |
|  Classes DAO: UserDAO, PatientDAO, etc.         |
+-------------------------------------------------+
```

### 3.1. Couche d'Accès aux Données (DAO)

- **Rôle** : Isoler le reste de l'application des détails d'implémentation de la base de données. Toute requête SQL est confinée à cette couche.
- **`Database.cs`** : Classe centrale qui agit comme une fabrique de connexions (`MySqlConnection`). Elle lit la configuration et fournit des objets de connexion prêts à l'emploi aux classes DAO, évitant la duplication de code de connexion.
- **`UserDAO.cs`** : Gère toutes les opérations CRUD pour l'entité `User`, y compris la vérification du mot de passe lors de l'authentification.
- **`PatientDAO.cs`**, **`MedicineDAO.cs`** : Gèrent les opérations CRUD pour leurs entités respectives.
- **`PrescriptionDAO.cs`** : Contient la logique la plus complexe, notamment la méthode `CreatePrescriptionWithMedicines` qui utilise une `MySqlTransaction` pour garantir l'atomicité.

### 3.2. Couche Métier (Models)

- **Rôle** : Représenter les entités du domaine de l'application sous forme d'objets C#. Ces classes sont des "POCO" (Plain Old C# Object) sans logique complexe.
- **`User.cs`** : Propriétés `Id`, `FirstName`, `LastName`, `Email`, `Role`, etc.
- **`Patient.cs`** : Propriétés `Id`, `Name`, `FirstName`, `Age`, et une référence au `User` qui est son médecin.
- Ces objets sont utilisés pour transférer les données entre la couche de présentation et la couche d'accès aux données.

### 3.3. Couche de Présentation (Forms)

- **Rôle** : Gérer l'interface utilisateur et l'interaction avec l'utilisateur.
- **`MainForm.cs`** : Formulaire principal qui sert de conteneur MDI (Multiple Document Interface) ou de point de départ pour ouvrir les autres formulaires.
- **`LoginForm.cs`** : Premier formulaire affiché, gère la saisie des identifiants.
- **`PatientsForm.cs`**, **`MedicinesForm.cs`**, etc. : Formulaires dédiés à la gestion d'une entité spécifique. Ils contiennent des contrôles comme des `DataGridView` pour lister les données et des `TextBox` pour l'édition. La logique de ces formulaires se limite à la validation des entrées et à l'appel des méthodes DAO appropriées.

---

## 4. MODÈLE PHYSIQUE DES DONNÉES (MYSQL)

### 4.1. Schéma Détaillé

| Table          | Colonne             | Type          | Contraintes / Notes                                |
|----------------|---------------------|---------------|----------------------------------------------------|
| **User**       | `id_user`           | `INT`         | `PRIMARY KEY`, `AUTO_INCREMENT`                    |
|                | `firstname`         | `VARCHAR(50)` |                                                    |
|                | `lastname`          | `VARCHAR(50)` |                                                    |
|                | `email`             | `VARCHAR(100)`| `UNIQUE`                                           |
|                | `password_hash`     | `VARCHAR(256)`| Stocke le hash SHA256 du mot de passe              |
|                | `role`              | `VARCHAR(20)` | Ex: 'Médecin', 'Pharmacien', 'Admin'               |
|                |                     |               |                                                    |
| **Patient**    | `id_patient`        | `INT`         | `PRIMARY KEY`, `AUTO_INCREMENT`                    |
|                | `id_user`           | `INT`         | `FOREIGN KEY` vers `User(id_user)` (Médecin réf.)  |
|                | `name`              | `VARCHAR(50)` |                                                    |
|                | `firstname`         | `VARCHAR(50)` |                                                    |
|                | `age`               | `INT`         |                                                    |
|                | `gender`            | `VARCHAR(10)` | Ex: 'Homme', 'Femme'                               |
|                |                     |               |                                                    |
| **Medicine**   | `id_medicine`       | `INT`         | `PRIMARY KEY`, `AUTO_INCREMENT`                    |
|                | `id_user`           | `INT`         | `FOREIGN KEY` vers `User(id_user)` (Pharmacien réf.)|
|                | `name`              | `VARCHAR(100)`|                                                    |
|                | `description`       | `TEXT`        |                                                    |
|                | `dosage`            | `VARCHAR(50)` |                                                    |
|                | `molecule`          | `VARCHAR(100)`|                                                    |
|                |                     |               |                                                    |
| **Prescription** | `id_prescription`   | `INT`         | `PRIMARY KEY`, `AUTO_INCREMENT`                    |
|                | `id_user`           | `INT`         | `FOREIGN KEY` vers `User(id_user)` (Médecin)       |
|                | `id_patient`        | `INT`         | `FOREIGN KEY` vers `Patient(id_patient)`           |
|                | `validity`          | `DATE`        | Date de fin de validité de l'ordonnance            |
|                |                     |               |                                                    |
| **Appartient** | `id_prescription`   | `INT`         | `PRIMARY KEY`, `FOREIGN KEY` vers `Prescription`   |
|                | `id_medicine`       | `INT`         | `PRIMARY KEY`, `FOREIGN KEY` vers `Medicine`       |
|                | `quantity`          | `INT`         | Quantité du médicament prescrit                    |

### 4.2. Script d'Initialisation (`docker/init.sql`)

Le script SQL complet est exécuté automatiquement par Docker au premier lancement. Il crée la base de données, les tables, et insère un jeu de données de test.

<details>
  <summary>Cliquer pour voir le contenu de init.sql</summary>

```sql
-- Exemple de contenu possible pour init.sql
CREATE DATABASE IF NOT EXISTS gsbMonolith;
USE gsbMonolith;

-- (Contenu complet des CREATE TABLE et INSERT INTO...)
-- ...
```
</details>

---

## 5. GUIDE D'INSTALLATION VERBEUX

### 5.1. Prérequis Logiciels
- **Visual Studio 2022** : Assurez-vous d'avoir installé le workload **"Développement desktop .NET"**.
- **Docker Desktop** : Doit être en cours d'exécution sur votre machine.

### 5.2. Procédure de Lancement
1.  **Clonage du Dépôt** :
    ```bash
    git clone <URL_DU_DEPOT>
    cd gsbMonolith
    ```
2.  **Démarrage de l'Environnement Docker** :
    Cette étape est cruciale. Ouvrez un terminal (PowerShell, CMD, etc.) à la racine du projet.
    ```bash
    docker-compose up -d
    ```
    Le flag `-d` (detached) lance les conteneurs en arrière-plan.

3.  **Vérification des Conteneurs** :
    Vous pouvez vérifier que les conteneurs tournent correctement :
    ```bash
    docker ps
    ```
    Vous devriez voir deux conteneurs : `gsb_mysql_db` et `gsb_phpmyadmin`.

4.  **Vérification de la Base de Données (Optionnel mais recommandé)** :
    - Ouvrez votre navigateur et allez sur `http://localhost:8080`.
    - Connectez-vous à phpMyAdmin :
        - **Serveur** : `db` (laisser tel quel)
        - **Utilisateur** : `root`
        - **Mot de passe** : (laisser vide)
    - Une fois connecté, vous devriez voir la base de données `gsbMonolith` dans le menu de gauche, avec toutes ses tables.

5.  **Lancement de l'Application .NET** :
    - Ouvrez `gsbMonolith.sln` dans Visual Studio 2022.
    - Le projet devrait se charger. Attendez que Visual Studio finisse de restaurer les dépendances NuGet.
    - Assurez-vous que le profil de build en haut est bien `Debug`.
    - Appuyez sur `F5` pour lancer le débogage. Le formulaire de connexion doit apparaître.

### 5.3. Dépannage (Troubleshooting)
- **Erreur `Cannot connect to any of the specified MySQL hosts`** :
    - Vérifiez que vos conteneurs Docker sont bien démarrés (`docker ps`).
    - Assurez-vous que le port `3306` n'est pas déjà utilisé par une autre instance de MySQL sur votre machine.
- **Erreur Docker `port is already allocated`** :
    - Un autre service utilise le port `3306` ou `8080`. Arrêtez ce service ou modifiez les ports dans le fichier `docker-compose.yml`.
- **L'application se lance mais les données n'apparaissent pas** :
    - Vérifiez dans phpMyAdmin que le script `init.sql` a bien inséré les données de test. Si les tables sont vides, vous pouvez forcer la réinitialisation de la base de données avec `docker-compose down -v` puis `docker-compose up -d`.

---

## 6. STRATÉGIES DE SÉCURITÉ ET DE QUALITÉ

### 6.1. Mesures de Sécurité
- **Hachage des mots de passe** : L'algorithme SHA256 est utilisé. Un "sel" (salt) n'est pas implémenté dans cette version mais constitue une piste d'amélioration cruciale.
- **Prévention des Injections SQL** : Toutes les requêtes SQL sont construites avec des paramètres (`MySqlParameter`), jamais par concaténation de chaînes.
- **Gestion des Erreurs** : Les blocs `try-catch` sont utilisés pour intercepter les exceptions SQL et éviter de divulguer des informations techniques à l'utilisateur final.

### 6.2. Stratégie de Tests
- **Tests Unitaires (DAO)** : L'architecture se prête à l'écriture de tests unitaires pour la couche DAO. Il serait pertinent d'utiliser un framework comme MSTest ou NUnit pour créer des tests qui valident chaque méthode DAO (ex: `Test_CreateUser_Returns_NewId`). Cela nécessiterait une base de données de test dédiée.
- **Tests d'Intégration** : Scénarios de tests qui valident un workflow complet (ex: connexion, création d'un patient, ajout d'une prescription).
- **Tests Manuels (UI)** : Une série de cas de tests manuels doit être suivie pour valider l'ergonomie et le comportement visuel de chaque formulaire.

---

## 7. GESTION DES DÉPENDANCES ET DOCUMENTATION

### 7.1. Dépendances NuGet
- **`MySql.Data`** : Connecteur officiel de MySQL pour .NET, indispensable pour communiquer avec la base de données.
- **Autres dépendances** : Le projet repose principalement sur les librairies standards du .NET Framework 6.

### 7.2. Génération de la Documentation Technique
Le projet inclut une configuration pour **DocFX**, un générateur de documentation pour .NET.
Pour générer la documentation :
1. Installez DocFX en suivant les instructions officielles.
2. Exécutez la commande suivante à la racine du projet :
   ```bash
   docfx docfx/docfx.json --serve
   ```
3. La documentation sera générée dans le dossier `_site` et accessible sur `http://localhost:8081`.

---

## 8. ÉVOLUTIONS ET PISTES D'AMÉLIORATION

- **Passage au Web** : Migrer l'application vers une technologie web comme ASP.NET Core MVC / Blazor.
- **API REST** : Exposer la logique métier via une API REST pour permettre la consommation par d'autres clients (application mobile, etc.).
- **Améliorer la Sécurité** : Ajouter un "sel" au hachage des mots de passe.
- **Déploiement Continu** : Mettre en place une pipeline CI/CD (avec GitHub Actions, par exemple) pour automatiser les builds, les tests et les déploiements en mode `Release`.
- **Reporting** : Développer un module de reporting plus avancé (statistiques sur les prescriptions, etc.).

---

## 9. AUTEURS

- **Morgan Bourré**
- Projet réalisé dans le cadre du **BTS SIO — GSB**
- Année : **2025**

---

## 10. LICENCE

Ce projet est distribué sous la **licence MIT**.