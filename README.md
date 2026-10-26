# GalaxyQuest

GalaxyQuest is a small ASP.NET Core REST API that simulates a space exploration and resource-management game. The project generates a procedural galaxy map, exposes star systems and planets through HTTP endpoints, and manages users and units that can move across the galaxy.

## Objectif du projet

Le but de ce projet est de fournir une API backend simple pour un jeu spatial de type stratégie. Les composants principaux permettent de :

- générer un univers stellaire aléatoire à partir d'un seed,
- récupérer les systèmes et les planètes du jeu,
- créer des utilisateurs avec des unités associées,
- déplacer des unités entre systèmes et planètes,
- préparer les fondations pour la construction d'édifices comme les mines.

## Stack technique

- Langage : C#
- Framework : ASP.NET Core 7
- API : REST JSON
- Documentation : Swagger / OpenAPI
- Tests d'intégration : xUnit + Microsoft.AspNetCore.Mvc.Testing
- Bibliothèque partagée : `Shard.Shared.Core`

## Structure du dépôt

```text
.
├── GalaxyQuest/                  # Application ASP.NET Core principale
│   ├── Controllers/              # Points d'entrée HTTP
│   ├── Entities/                 # Modèles de domaine
│   ├── EntitiesDTO/              # DTOs exposés par l'API
│   ├── Services/                 # Repository et logique de génération/map
│   ├── Utilities/                # Helpers de réponse
│   ├── Program.cs                # Bootstrap de l'application
│   ├── appsettings.json          # Configuration principale
│   └── GalaxyQuest.csproj        # Projet API
├── GalaxyQuest.IntegrationTests/ # Tests d'intégration
├── Shard.Shared.Core/            # Génération de galaxie / logique commune
├── tests/                        # Fichiers de test / ressources
├── shard.sln                    # Solution .NET
├── coverlet.runsettings         # Paramétrage de couverture de tests
├── extractTestResults.sh         # Script d'extraction des résultats de tests
├── .gitlab-ci.yml               # Pipeline CI GitLab
├── README.md                    # Ce document
└── .gitignore
```

## Composants clés

### 1. Application web
Le cœur du projet est le projet `GalaxyQuest`, un service HTTP ASP.NET Core. Le bootstrap se fait dans `Program.cs` et ajoute :

- le stockage des services `MapFactory`, `MapRepository`, `UserRepository`,
- les contrôleurs API,
- Swagger en environnement de développement.

### 2. Génération de la galaxie
La logique de génération et de spécification du monde est dans `Shard.Shared.Core`.

Les classes importantes incluent :

- `MapGenerator`
- `MapGeneratorOptions`
- `SystemSpecification`
- `PlanetSpecification`
- `SectorSpecification`
- `RandomShareComputer`

Cette couche génère les systèmes, les planètes et leurs ressources de manière procédurale.

### 3. Repositories et services
Les services du projet centralisent l'état applicatif :

- `MapFactory`: transforme les données générées en DTOs exposables,
- `MapRepository`: stocke la carte, fournit les systèmes et planètes,
- `UserRepository`: garde en mémoire les utilisateurs et leurs unités.

### 4. Contrôleurs API
Les endpoints exposés par la configuration actuelle sont les suivants :

#### Systèmes et planètes
- `GET /Systems`
- `GET /Systems/{systemName}`
- `GET /Systems/{systemName}/planets`
- `GET /Systems/{systemName}/planets/{planetName}`

#### Utilisateurs
- `PUT /Users/{id}` : création d'un utilisateur
- `GET /Users/{id}` : récupération d'un utilisateur

#### Unités
- `GET /users/{userId}/Units`
- `GET /users/{userId}/Units/{unitId}`
- `PUT /users/{userId}/Units/{unitId}` : déplacement d'une unité
- `GET /users/{userId}/Units/{unitId}/location`

#### Bâtiments
- `POST /users/{userId}/Buildings` : tentative de création d'un bâtiment, notamment un `mine`

## Modèle de domaine

Le projet utilise des entités et DTOs séparés :

- `User` / `UserDTO`
- `Unit`
- `Planet` / `PlanetDTO`
- `StarSystem` / `StarSystemDTO`
- `Location`
- `Building`

Les DTOs sont utilisés pour exposer les données de l'API, tandis que les entités internes gardent la logique du jeu.

## Démarrage rapide

Prérequis :

- .NET SDK 7+
- un terminal compatible

### Installation et lancement

```bash
git clone https://github.com/JeremyRbs/GalaxyQuest.git
cd GalaxyQuest
dotnet restore
dotnet build
dotnet run --project GalaxyQuest
```

En environnement de développement, Swagger est activé. Vous pouvez généralement l'ouvrir via l'URL affichée dans le terminal, par exemple :

```text
https://localhost:<port>/swagger
```

## Tests

Le dépôt contient un projet de tests d'intégration : `GalaxyQuest.IntegrationTests`.

Pour lancer les tests :

```bash
dotnet test
```

## Exemple de flux de jeu

Un flux typique ressemble à ceci :

1. création d'un utilisateur via `PUT /Users/{id}` ;
2. récupération de la liste des systèmes via `GET /Systems` ;
3. sélection d'un système et d'une planète ;
4. déplacement d'une unité via `PUT /users/{userId}/Units/{unitId}` ;
5. consultation de la localisation et des ressources de la planète ;
6. éventuellement construction d'un bâtiment.

## Points d'attention

Le projet est encore une base fonctionnelle de démonstration plutôt qu'un jeu complet. Le code contient notamment :

- des structures en mémoire pour les repositories,
- des mécanismes de déplacement d'unités avec temps estimé d'arrivée,
- une logique de génération de map et de ressources partagée,
- des endpoints utiles pour un prototype/serveur backend.

## Licence et statut

Le projet est un prototype technique d'API de jeu spatial. Il est conçu comme une base de travail pour un backend de jeu ou un service de gestion de galaxie.

## Contributions

Les contributions sont bienvenues sous forme de pull requests. Les améliorations naturelles du projet pourraient inclure :

- persistance des utilisateurs et des systèmes,
- gestion réelle des ressources et de la production,
- construction et amélioration de bâtiments,
- combat ou exploration sur les planètes,
- sécurisation et validation des requêtes API.

---

Ce README a été rédigé à partir de l'analyse du code source présent dans le dépôt et de la structure des projets C# du repository.
