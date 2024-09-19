# BlockbusterPOS
This repository contains a .NET Core API built as part of a coding assignment for a temporary cash register system for a video rental store. The goal of the application is to manage rentals of DVDs and Blu-ray discs, calculate total rental costs based on customer type (club member or non-member), and handle different pricing rules depending on the number of items rented.

Architecturally, I chose to use the Controller-Service-Repository pattern due to its clear separation of concerns, maintainability, and scalability. This approach has facilitated integration testing with mocking, which I have also implemented. This has provided a controlled environment to observe how different parts of the application interact with each other. This method has improved testability and allowed me to verify the requirements and the system's behavior under various scenarios.

Here are some reasons why I opted for this method instead of a console application or MVC application:

**Flexibility and Scalability:** ASP.NET Core API offers greater flexibility and scalability, making it easier to build a solution that is easy to extend.

**Separation of Concerns:** Clear division between frontend and backend, which facilitates focus on logic and data processing.

**Easy Integration:** API-based solutions are better suited for integration with third-party services and external systems if needed.

**Testing and Mocking:** The API architecture facilitates testing and mocking, allowing for effective isolation and testing of specific parts of the application.

**Easier Maintenance:** Clear interfaces and separate layers in an API-based architecture make it easier to maintain, develop, and add new functionality.

## Assignment Overview
Objective
The goal of the assignment is to demonstrate how I approach developing a .NET application, including how I structure code and utilize various tools and features that a programmer has at their disposal. The focus is on writing clean, maintainable, and testable code, while also showcasing my C# and .NET skills.

## Requirements
The application should handle two types of media:

* DVD (rental cost: 29kr)
* Blu-ray (rental cost: 39kr)
  
The system must calculate the total rental price for customers, who are categorized into two types:

**Non-members:** Pay full price for all rentals.

**Club members:**
* Get a 10% discount on DVD rentals and a 15% discount on Blu-ray rentals.
* Can rent 4 movies for a total of 100kr (any combination of media), with additional movies charged at the discounted price.

## Ambition Levels
**Basic Level:** Show the total price for two specific customers:

* One non-member renting 2 DVDs and 1 Blu-ray.
* One club member renting 2 Blu-rays and 3 DVDs.
The program outputs the total price directly to the screen.

**Advanced Level:** Allow input of customer details (whether they are a club member or not) and the number of films they wish to rent. The program then calculates and displays the total price based on the given input.

## Features
* **Rental Management:** Support for handling rentals of DVDs and Blu-rays, with pricing rules tailored to customer membership status.
* **Cost Calculation:** Automatically calculates total rental prices, including discounts and special pricing for club members. Extension methods are used for calculations to keep the code organized and reusable.
* **Controller-Service-Repository Pattern:** Ensures separation of concerns, making the system more maintainable and scalable. The Controller handles HTTP requests, the Service layer contains business logic, and the Repository manages data access.
* **Configuration Management:** Uses IOptions<T> to read configuration values from appsettings.json, allowing for flexible and centralized configuration management.
* **Customer Interaction:** Optionally allows input of customer data and rental details to calculate the price dynamically.
* **Ambition Levels:** Supports both basic and advanced levels of functionality based on user input.
* **Testing & Reliability:** The project includes integration tests to validate system functionality and ensure reliability.
Mock frameworks have been used to simulate external dependencies, ensuring a robust testing environment that accurately mimics real-world scenarios.
