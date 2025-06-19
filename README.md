# 🎵 playlist-pilot

**Playlist-Pilot** is a beginner-friendly ASP.NET Core Web API project that allows users to manage **Artists**, **Songs**, and **Playlists**. It demonstrates real-world features like:

- 🎤 1 to many relationship (Artist → Songs)
- 🎶 Many to many relationship (Songs ↔ Playlists)
- ✅ Full CRUD API endpoints using DTOs
- 🔄 Code-First EF Core with migrations
- 🔎 LINQ filtering and query logic
- 🧪 Swagger/OpenAPI support

---

## 🧱 Tech Stack

- ASP.NET Core 6 / 8
- Entity Framework Core (Code-First)
- SQL Server (LocalDB)
- Swagger (Swashbuckle)
- LINQ
- Visual Studio 2022+

---

## 📐 Data Model Overview

```plaintext
Artist (1) ────< Song (Many)
Song   (Many) ────< PlaylistSong >──── Playlist (Many)

---
## 🚀 Getting Started
### 1️⃣ Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/playlist-pilot.git
cd playlist-pilot
### 2️⃣ Setup Connection String
### 3️⃣ Run Migrations
### 4️⃣ Launch the App

---
## Learning Objectives
- ✅ Code-First database modeling with EF Core
- 📐 How to define 1:M and M:N relationships
- 🧩 Clean DTO usage (inline record types)
- 🔧 CRUD Web API development
- 🌐 Swagger-based API testing

