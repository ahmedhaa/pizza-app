Projet de Gestion de Pizzas

Description

Ce projet est une application web permettant la gestion des pizzas, des commandes et des utilisateurs. Il est divisé en deux parties :

Backend (API REST) : Développé avec .NET Core 6, il fournit des endpoints pour gérer les utilisateurs, les pizzas et les commandes.

Frontend (Angular 14) : Interface utilisateur développée avec Angular, intégrant un système d'authentification, une interface d'administration et une interface utilisateur.

Compte administrateur de test:

Email : ahmed@test.com
Mot de passe : Password123!

1. Backend - API REST (.NET Core 6)

Technologies utilisées

.NET Core 6

Entity Framework Core (SQLite)

JWT pour l'authentification

Swagger pour la documentation API

ASP.NET Core Identity

Serilog pour la gestion des logs

Fonctionnalités développées en 2 jours

✅ Création des modèles et du contexte de base de données
✅ Mise en place de l'authentification avec JWT
✅ Implémentation des endpoints API :

Gestion des utilisateurs (CRUD)

Gestion des pizzas (CRUD)

Gestion des commandes (Création, récupération des commandes d'un utilisateur....)
✅ Ajout de la documentation Swagger
✅ Gestion des erreurs centralisée

Structure de l'API
/pizza-api
│
├── Controllers/               # Contrôleurs de l'API
│   ├── AuthController.cs      # Authentification
│   ├── PizzasController.cs    # Gestion des pizzas
│   ├── CommandesController.cs # Gestion des commandes
│   ├── UserController.cs      # Gestion des utilisateurs
│
├── Data/                      # Base de données SQLite & contexte
├── Services/                  # Logique métier
├── Interfaces/                # Interfaces des services
├── Entities/                  # Modèles de données
├── Migrations/                # Migrations de la base de données
├── appsettings.json           # Configuration
└── Program.cs                 # Démarrage de l'application

2. Frontend - Application Angular 14

Technologies utilisées

Angular 14

Angular Material (Design UI)

MatSnackBar (Notifications utilisateur)

JWT Interceptor (Gestion des tokens JWT)

AuthGuard (Protection des routes)

Fonctionnalités développées en 4 jours

✅ Création de l'interface administrateur :

Gestion des utilisateurs (CRUD)

Gestion des pizzas (CRUD)

Gestion des commandes

Gestion des commandes
✅ Mise en place de l'authentification (Login/Logout) avec JWT
✅ Ajout d'un interceptor HTTP pour attacher le token JWT aux requêtes API
✅ Mise en place d'AuthGuard pour restreindre l'accès aux pages
✅ Développement du début de l'interface utilisateur : affichage des pizzas
✅ Intégration de MatSnackBar pour afficher des messages utilisateur
⏳ Reste à faire : gestion des commandes côté utilisateur

Conclusion

En 4 jours, les bases de l'application ont été mises en place avec :

Une API REST fonctionnelle et sécurisée

Une interface administrateur complète

Un début d'interface utilisateur

Une architecture modulaire et maintenable

🎯 Prochaines étapes : Finalisation de la gestion des commandes côté utilisateur et amélioration de l'expérience UI/UX.
