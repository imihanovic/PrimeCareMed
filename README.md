# BookIt

Web application for making a table reservation in the restaurant. :plate_with_cutlery:

## Table of Contents

* [General Info](#general-info)
* [Features](#features)
* [Technologies](#technologies)
* [Database Model](#database-model)
* [Authors](#authors)

## General Info

Restaurant reservations is a web application that enables creating reservations in the restaurant.

> Project created as a college seminar for *Programming in C#*
> *University of Split - University Department of Professional Studies*

## Features

### User Management

- Roles (base for authorization):
    - Manager
    - Admin
    - Guest
- Available operations:
    - CRUD
- Authentication (login/logout)
- Show all users (with search and pagination)
- Show guest details (guests can see their own details, manager and waiter can se all)

#### Table

- Available operations:
    - CRUD
- View all tables
- Filter by day

#### Reservation

- Available operations:
    - Create
    - Edit and update 
- View all reservations (pagination, by table number can see details of reservation)
- Users can reserve a table (bot)


### Mail Notification

- Send "Welcome" mail to guest when his account is created
- Send "Reservation confirmed" mail to guest when manager confirms reservation

## Technologies

- C#
- .NET
- Visual Studio

## Database Model

![restoran drawio](https://user-images.githubusercontent.com/92686358/223742557-4d7ef91c-eb03-4b0a-be23-7a3ca3bacbfb.png)

## Authors:
✍️ 

* [Petar Vidović](https://github.com/Petar1107)
* [Ivan Komadina](https://github.com/IvanKomadina)
* [Ivana Mihanović](https://github.com/imihanovic)
