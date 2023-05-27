# BookIt

Web application for table reservation in the restaurant. :plate_with_cutlery:

## Table of Contents

* [General Info](#general-info)
* [Features](#features)
* [Technologies](#technologies)
* [Database Model](#database-model)
* [Authors](#authors)

## General Info

BookIt is a web application that enables table booking in restaurants.

- Project created as a college seminar for *Programming in C#*
- *University of Split - University Department of Professional Studies*

## Features

### User Management 🧑 👩

- Roles:
    - Admin
    - Manager - can only manage one restaurant
    - Customer
- Available operations:
    - CRUD
- Authentication (login/logout)
- Show all users (with search and pagination for admin)
- Show details (customers and managers can see their own details, admin can see all)

#### Table 

- Available operations:
    - CRUD
- View tables for admin - Admin can see all tables for all restaurants
- View tables for manager - Manager can see all tables in the restaurant they manage

#### Restaurant 🍽️

- Available operations:
    - CRUD
- Only admin can create restaurants and set a manager
- Manager can edit Restaurant

#### Dish 🍷🍔

- Available operations:
    - CRUD
- Admin and Manager can create new Dish
- Admin can add any dish to any restaurant
- Manager can only add dish to the restaurant they manage

#### Reservation 📖✏️

- Available operations:
    - Create
    - Edit and update 
- Show all reservations (pagination)
- Filter reservations by day
- Reservation can have multiple tables
- Table in period between startTime and endTime will be unavailable to book

- Users can reserve a table (via bot?)

## Technologies

![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)  
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)  
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)  
![Postgres](https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white)  


## Database Model

![restoranVerzija2 drawio (1)](https://github.com/imihanovic/Sudoku/assets/92686358/763d83c5-e441-4f6d-b692-94765af417c2)


## Authors:
✍️ 

* [Petar Vidović](https://github.com/Petar1107)
* [Ivan Komadina](https://github.com/IvanKomadina)
* [Ivana Mihanović](https://github.com/imihanovic)
