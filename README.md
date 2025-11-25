# 💊 GSB Monolith — Application Desktop de Gestion Médicale

## 🧩 Description du projet

**GSB Monolith** est une application **desktop C# (.NET Windows Forms)** développée pour le groupe **GSB (Galaxy Swiss Bourdin)**.
Ce logiciel a pour objectif de **centraliser et gérer les prescriptions médicales**, les **patients**, les **médicaments** et les **utilisateurs (médecins, pharmaciens, administrateurs)** dans un **système monolithique connecté à une base de données MySQL**.

---

## ⚙️ Fonctionnalités principales

### 👤 Gestion des utilisateurs

* Connexion sécurisée (mot de passe haché SHA256)
* Rôles utilisateurs :

  * **Médecins** : création et gestion des prescriptions
  * **Administrateurs** : supervision générale

### 🧍 Gestion des patients

* Création, consultation, modification et suppression de patients
* Informations stockées : nom, prénom, âge, genre, utilisateur associé (médecin référent)

### 💊 Gestion des médicaments

* Référencement des médicaments avec :

  * Nom
  * Description
  * Dosage
  * Molécule
  * Pharmacien référent

### 📜 Gestion des prescriptions

* Création de prescriptions liées à un patient et un médecin
* Ajout automatique des médicaments via la table **Appartient**
* Date de validité, quantité prescrite
* Transaction SQL pour garantir la cohérence entre les tables `Prescription` et `Appartient`

---

## 🗃️ Structure de la base de données (MySQL)

| Table            | Description                                          | Colonnes principales                                                   |
| ---------------- | ---------------------------------------------------- | ---------------------------------------------------------------------- |
| **Users**        | Contient les comptes utilisateurs                    | `id_user`, `firstname`, `role`, `name`, `email`, `password`           |
| **Patients**     | Gère les informations patient                        | `id_patient`, `id_user`, `name`, `firstname`, `age`, `gender`        |
| **Medicine**     | Liste les médicaments disponibles                    | `id_medicine`, `id_user`, `name`, `description`, `dosage`, `molecule` |
| **Prescription** | Enregistre les prescriptions faites par les médecins | `id_prescription`, `id_user`, `id_patient`, `quantity`, `validity`   |
| **Appartient**   | Table de liaison entre `Prescription` et `Medicine`  | `id_prescription`, `id_medicine`                                       |

---

## 🧱 Architecture du projet

```
gsbMonolith/
│
├── DAO                    → Accès aux données (Data Access Object)
│   ├── AppartientDAO.cs
│   ├── Database.cs        → Connexion à MySQL
│   ├── MedicineDAO.cs
│   ├── PatientDAO.cs
│   ├── PrescriptionDAO.cs
│   └── UserDAO.cs
├── Forms                  → Interfaces Windows Forms
│   ├── MainForm.Designer.cs
│   ├── MainForm.cs
│   ├── MainForm.resx
│   ├── MedicinesForm.Designer.cs
│   ├── MedicinesForm.cs
│   ├── MedicinesForm.resx
│   ├── PatientsForm.Designer.cs
│   ├── PatientsForm.cs
│   ├── PatientsForm.resx
│   ├── PrescriptionsForm.Designer.cs
│   ├── PrescriptionsForm.cs
│   ├── PrescriptionsForm.resx
│   ├── UserForm.Designer.cs
│   ├── UserForm.cs
│   └── UserForm.resx
├── Models                 → Classes modèles (entités)
│   ├── Appartient.cs
│   ├── Medicine.cs
│   ├── Patient.cs
│   ├── Prescription.cs
│   └── User.cs
|
├── docker-compose.yml
├── gsbMonolith.csproj
├── gsbMonolith.csproj.user
├── gsbMonolith.sln
├── init.sql
├── Program.cs              → Point d’entrée principal
└── README.md               → Documentation du projet
```

---

## 💾 Installation

### 1️⃣ Prérequis

* **Visual Studio 2022+**
* **.NET Framework 6.0 ou supérieur**
* **Docker** (pour MySQL + phpMyAdmin)
* *(Optionnel)* **MySQL Workbench** ou **phpMyAdmin**

---

## 🐳 Déploiement rapide avec Docker (recommandé)

Tu peux lancer **MySQL** et **phpMyAdmin** sans rien installer manuellement grâce au fichier `docker-compose.yml` fourni.

### 🚀 Lancer la base de données

Dans le dossier racine du projet :

```bash
docker compose up -d
```

Cela va :

* démarrer un conteneur MySQL (`gsb_mysql_db`)
* démarrer phpMyAdmin sur le port **8080**
* créer automatiquement la base et exécuter le fichier `init.sql` (s’il existe)
* autoriser la connexion root **sans mot de passe**

### 🔍 Accéder à phpMyAdmin

Une fois les conteneurs lancés :
👉 [http://localhost:8080](http://localhost:8080)

Connecte-toi avec :

```
Utilisateur : root
Mot de passe : (laisser vide)
```

### 🧠 Informations de connexion MySQL

| Élément                      | Valeur       |
| ---------------------------- | ------------ |
| Hôte (depuis C# dans Docker) | `db`         |
| Hôte (depuis ta machine)     | `127.0.0.1`  |
| Port                         | `3306`       |
| Utilisateur                  | `root`       |
| Mot de passe                 | *(vide)*     |
| Base de données par défaut   | `gsbMonolith` |

> ⚠️ Si ton application C# tourne **dans le même `docker-compose.yml`**, utilise
> `server=db;database=gsbMonolith;user=root;password=;`
>
> Si elle tourne **en local sur ta machine**, utilise
> `server=127.0.0.1;port=3306;database=gsbMonolith;user=root;password=;`.

### 🧹 Réinitialiser la base de données

Si tu veux tout nettoyer et repartir à zéro :

```bash
docker compose down -v
docker compose up -d
```

> Cela supprime le volume MySQL et recharge `init.sql` automatiquement.

---

## ⚙️ Installation manuelle (optionnelle)

Si tu préfères installer MySQL à la main :

1. Crée une base :

   ```sql
   CREATE DATABASE gsbMonolith;
   ```
2. Importe le fichier SQL fourni (`init.sql`).
3. Modifie la chaîne de connexion dans `Database.cs` :

   ```csharp
   private string connectionString = "server=localhost;user=root;password=;database=gsbMonolith;";
   ```

---

## 🧠 DAO disponibles

| Classe              | Description                           | Méthodes clés                                                                        |
| ------------------- | ------------------------------------- | ------------------------------------------------------------------------------------ |
| **UserDAO**         | Gestion des utilisateurs              | `GetUserByEmail()`, `AuthenticateUser()`                                             |
| **PatientDAO**      | Gestion des patients                  | `GetPatientById()`, `CreatePatient()`, `GetAllPatients()`                            |
| **MedicineDAO**     | Gestion des médicaments               | `GetMedicineById()`, `GetAllMedicines()`                                             |
| **PrescriptionDAO** | Gestion des prescriptions et liaisons | `GetPrescriptionById()`, `CreatePrescription()`, `CreatePrescriptionWithMedicines()` |

---

## 💡 Exemple d’utilisation

```csharp
PrescriptionDAO dao = new PrescriptionDAO();
Prescription p = new Prescription(0, 1, 5, 2, "2025-12-01");
List<int> meds = new List<int> { 1, 3, 5 };

bool ok = dao.CreatePrescriptionWithMedicines(p, meds);
if (ok)
    MessageBox.Show("Prescription créée avec succès !");
else
    MessageBox.Show("Erreur lors de la création.");
```

---

## 🔐 Sécurité

* Les mots de passe utilisateurs sont **hachés en SHA256**
* Les interactions SQL utilisent des **requêtes paramétrées**
* Gestion des transactions pour les opérations critiques (`PrescriptionDAO`)

---

## 🧑‍💻 Auteurs

* **[Morgan Bourré]**
* Projet réalisé dans le cadre de **GSB - BTS SIO**
* Année : **2025**

---

## 🧾 Licence

Ce projet est distribué sous licence **MIT**.
Vous êtes libre de le réutiliser, modifier et distribuer à condition de conserver la mention d’auteur.