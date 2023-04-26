# BookIt

Web application for table reservation in the restaurant. :plate_with_cutlery:

## Table of Contents

* [General Info](#general-info)
* [Features](#features)
* [Technologies](#technologies)
* [Database Model](#database-model)
* [Authors](#authors)

## General Info

BookIt is a web application that enables table booking in the restaurant.

- Project created as a college seminar for *Programming in C#*
- *University of Split - University Department of Professional Studies*

## Features

### User Management

- Roles:
    - Manager
    - User (guest)
- Available operations:
    - CRUD
- Authentication (login/logout)
- Show all users (with search and pagination)
- Show details (guests can see their own details, manager can see all)

#### Table

- Available operations:
    - CRUD
- View all tables

#### Reservation

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

![restoran2 drawio (2)](https://user-images.githubusercontent.com/92686358/225091311-68a71f9c-9ce1-4ef8-8d50-fbf923cf0984.png)


Option 2

![restoranVerzija2 drawio](https://user-images.githubusercontent.com/92686358/234578643-efdc6286-5d5a-41bb-9cf9-79b5c5dc09c9.png)


## Authors:
✍️ 

* [Petar Vidović](https://github.com/Petar1107)
* [Ivan Komadina](https://github.com/IvanKomadina)
* [Ivana Mihanović](https://github.com/imihanovic)
