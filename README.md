BDD Style API Automation Framework for Pet Store API


Overview

This project is a BDD-style API automation framework built with .NET 8.0, Reqnroll, RestSharp, NUnit, Fluent Assertions, Log4Net, Allure Reporting, and written in C#.
It is designed to test the functionality of the Pet Store API https://petstore.swagger.io/#/ by automating API test scenarios using behavior-driven development principles.


Features

>Uses the latest Reqnroll version considering the decommissioning of SpecFLow from .net 8.0.
>API Testing is achieved with RestSharp librabry for sending HTTP requests and handling responses, which is much faster and has built-in serialization.
>Assertions using Fluent Assertions for improved test readability and maintainability.
>Advanced OOP concepts such as objects, constructors, method overloading, static, getters/setters etc. have been used and demonstrated.
>Useful techniques such as JsonSerializar, Reqnroll Data Table helpers, Reqnroll Hooks, exception handling etc. have been adapted.
>Coding is performed with the high sense of code reusability, easy maintenance and easy to understand.
>Logging with Log4Net for enhanced debugging and traceability.
>NUnit is used as the backbone framework to run the tests. 
>Allure Reporting for structured test results. Allure reports get generated as part of the Hooks after every test run and gets stored DATETIME-wise.
>Supports Parallel Execution to improve test performance.


Note

The project does not ensure 100% test coverage. Just 1 or 2 tests for the main endpoints are showcased to demonstrate different ways of handling different types of requests and responses.
It needs to be improved by adding more functional, negative and validation tests for each endpoint.


Prerequisites

Ensure you have the following installed:
.NET 8.0 SDK
Visual Studio 2022 or JetBrains Rider
Allure Command-Line Tool (Scoop)


Installation

Clone the repository:
git clone https://github.com/sbhat6/SubuBhat-CBA-Test
cd SubuBhat-CBA-Test


Running Tests

Execute tests using Visual Studio Test Explorer
Or run the command from the Terminal dotnet test


Logging & Reports

Logs are captured using Log4Net at info, debug, error, fetal levels.
