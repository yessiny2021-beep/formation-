# 🎓 Système de Gestion de Formations

Une application web moderne ASP.NET Core MVC pour la gestion des formations d'entreprise, avec authentification, génération de certificats PDF et un design élégant.

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-purple)
![MySQL](https://img.shields.io/badge/MySQL-Database-orange)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-blueviolet)

## ✨ Fonctionnalités

### 👥 Gestion des Utilisateurs
- **Authentification sécurisée** avec ASP.NET Identity
- **Deux rôles** : Administrateur et Employé
- Inscription et connexion des utilisateurs
- Profils utilisateurs personnalisés

### 📚 Gestion des Formations
- ✅ Création, modification et suppression de formations
- 📅 Gestion des dates (début/fin) et durée
- 👤 Capacité maximale de participants
- 📊 Statuts : Active, Inactive, Terminée
- 🔍 Affichage détaillé des formations

### 👨‍💼 Gestion des Employés
- ✅ CRUD complet des employés
- 📧 Validation d'unicité des emails
- 📅 Date d'embauche
- 📊 Statistiques (embauches du mois, de l'année)
- 🔗 Liaison avec les comptes utilisateurs

### 📝 Gestion des Inscriptions
- ✅ Inscription des employés aux formations
- ⚠️ Vérification de disponibilité
- 🚫 Prévention des inscriptions multiples
- 📊 Suivi des inscriptions par formation
- 🎯 Validation de la capacité maximale

### 📜 Génération de Certificats
- 📄 Génération automatique de certificats PDF
- 🎨 Design professionnel avec QuestPDF
- ⬇️ Téléchargement direct
- 📋 Informations complètes (nom, formation, dates, durée)

### 📊 Tableaux de Bord
- **Dashboard Admin** : Vue d'ensemble complète
  - Statistiques globales
  - Calendrier des formations
  - Gestion complète
  
- **Dashboard Employé** : Vue personnalisée
  - Formations disponibles
  - Inscriptions en cours
  - Historique des formations

## 🎨 Design Moderne

### Interface Utilisateur
- 🌈 **Palette de couleurs** : Dégradés violets/bleus élégants
- ✨ **Animations fluides** : Transitions et effets de hover
- 💳 **Cards modernes** : Ombres douces, bordures arrondies
- 📊 **Tables élégantes** : Badges colorés, icônes contextuelles
- 📱 **Responsive** : Adaptation mobile optimisée
- 🔤 **Typographie** : Police Inter de Google Fonts
- 🎭 **Icônes** : Font Awesome 6.4.0

### Effets Visuels
- Glassmorphism sur la navigation
- Effet de brillance au survol des cartes
- Animations d'entrée en cascade
- Badges colorés par statut
- Dégradés animés

## 🛠️ Technologies Utilisées

### Backend
- **ASP.NET Core 8.0** - Framework web
- **Entity Framework Core 8.0.21** - ORM
- **ASP.NET Identity** - Authentification/Autorisation
- **Pomelo.EntityFrameworkCore.MySql** - Provider MySQL
- **QuestPDF 2024.12.0** - Génération PDF

### Frontend
- **Bootstrap 5** - Framework CSS
- **Font Awesome 6.4.0** - Icônes
- **Google Fonts (Inter)** - Typographie
- **CSS3** - Animations et effets modernes
- **JavaScript** - Interactivité

### Base de Données
- **MySQL** - Système de gestion de base de données

## 📋 Prérequis

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [MySQL Server](https://dev.mysql.com/downloads/)
- Un IDE : [Visual Studio](https://visualstudio.microsoft.com/), [VS Code](https://code.visualstudio.com/), ou [Rider](https://www.jetbrains.com/rider/)

## 🚀 Installation

### 1. Cloner le repository
```bash
git clone https://github.com/yessiny2021-beep/formation-.git
cd formation-
```

### 2. Configurer la base de données
Modifier le fichier `appsettings.json` avec vos paramètres MySQL :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=formationdb;user=root;password=votre_mot_de_passe"
  }
}
```

### 3. Appliquer les migrations
```bash
dotnet ef database update
```

### 4. Lancer l'application
```bash
dotnet run
```

L'application sera accessible sur `https://localhost:5178` ou `http://localhost:5178`

## 👤 Comptes de Test

### Administrateur
- **Email** : admin@test.com
- **Mot de passe** : Admin123!

### Employé
- **Email** : employe@test.com
- **Mot de passe** : Employe123!

> Note : Ces comptes sont créés automatiquement au démarrage si ils n'existent pas (via `SeedData.cs`)

## 📁 Structure du Projet

```
MvcMovie/
├── Areas/                      # Identity Scaffolding
│   └── Identity/
├── Controllers/                # Contrôleurs MVC
│   ├── HomeController.cs
│   ├── EmployeController.cs
│   ├── FormationController.cs
│   ├── InscriptionController.cs
│   ├── DashboardController.cs
│   └── EmployeDashboard.cs
├── Data/                       # Contexte EF Core
│   └── MvcMovieContext.cs
├── Documents/                  # Génération PDF
│   └── CertificateDocument.cs
├── Migrations/                 # Migrations EF Core
├── Models/                     # Modèles de données
│   ├── Formation.cs
│   ├── Employe.cs
│   ├── Inscription.cs
│   ├── ApplicationUser.cs
│   └── SeedData.cs
├── Views/                      # Vues Razor
│   ├── Shared/
│   ├── Home/
│   ├── Employe/
│   ├── Formation/
│   ├── Inscription/
│   ├── Dashboard/
│   └── EmployeDashboard/
├── wwwroot/                    # Fichiers statiques
│   ├── css/
│   │   ├── site.css
│   │   └── animations.css
│   ├── js/
│   └── lib/
├── appsettings.json
├── Program.cs
└── MvcMovie.csproj
```

## 🔒 Sécurité

- ✅ Authentification ASP.NET Identity
- ✅ Autorisation basée sur les rôles
- ✅ Protection CSRF
- ✅ Validation des données côté serveur
- ✅ Mots de passe hashés
- ✅ Protection contre les injections SQL (EF Core)

## 📊 Modèle de Données

### Formation
- Id, Titre, Description
- DateDebut, DateFin, Duree
- Capacite, Statut
- Relations : Liste d'inscriptions

### Employe
- Id, Nom, Prenom, Email
- DateEmbauche
- Relations : Inscriptions, ApplicationUser

### Inscription
- Id, DateInscription
- Relations : Formation, Employe

### ApplicationUser (Identity)
- Hérite de IdentityUser
- FullName, EmployeId
- Relations : Employe

## 🎯 Fonctionnalités Futures

- [ ] Notifications par email
- [ ] Export Excel des données
- [ ] Calendrier interactif
- [ ] Système de notes/évaluations
- [ ] Statistiques avancées avec graphiques
- [ ] Mode sombre
- [ ] Multi-langue (i18n)
- [ ] API REST
- [ ] Tests unitaires
- [ ] Intégration CI/CD

## 📝 License

Ce projet est sous licence MIT. Voir le fichier `LICENSE` pour plus de détails.

## 👨‍💻 Auteur

**Yessin**
- GitHub: [@yessiny2021-beep](https://github.com/yessiny2021-beep)

## 🙏 Remerciements

- ASP.NET Core Team
- Bootstrap Team
- Font Awesome
- QuestPDF
- Google Fonts

---

⭐ N'hésitez pas à star ce projet si vous le trouvez utile !
