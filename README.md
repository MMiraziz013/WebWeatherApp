# Weather Application

## Overview

This Weather Application is a responsive and feature-rich web application that provides users with detailed weather forecasts for a specified location.
Users can view the current weather conditions, tomorrow's weather information, a 2-day forecast, along with the additional details such as sunrise and sunset times, humidity, wind speed, and visibility. 
The application also features dynamic updates, smooth animations, and an intuitive user interface.

## Features

#### Dynamic Weather Data:

* Works with any location. It can be set in the initial welcome page.

* Users can change the location dynamically using the provided form.

* Weather data includes temperature, condition, wind speed, visibility, and more.

#### Responsive Design:

* Optimized for both desktop and mobile devices.

* Utilizes Bootswatch themes for consistent and attractive styling.

#### Smooth Animations:

* Forms and components feature animations for an engaging user experience.

#### Error Handling:

* Displays appropriate error messages if weather data cannot be fetched for a given location.

#### Scalable Architecture:

* Designed to allow for future extensions, such as integration with additional APIs or features.

## Technologies Used

### Frontend:

* HTML5, CSS3, JavaScript

* Bootswatch for styled components

### Backend:

* ASP.NET Core

* .NET 8 Framework

### API:

* Weather data fetched from a third-party weather API (weatherapi.com)

### Database:

* No database is used in the current version, as it relies entirely on live API calls.

### Hosting:

* Ready for deployment on platforms like AWS EC2.

## How It Works

* On initial page load, the application requests the user to enter the location to check the weather.

* Users can submit a new location through the location form, and the application updates the weather data dynamically.

**A responsive weather box displays detailed weather information, including:**

* Current weather conditions.

* Forecast for the next 2 days (can be extended to 7 with the paid API subscription).

* Additional details like sunrise, sunset, wind speed, and more.

## Future Ideas

* Arcade Game Integration: Add a space shooter-style mini-game with weather-themed elements, accessible through a button in the application.

* Improved Data Visualization: Use charts to display temperature trends and wind patterns.

* Enhanced Error Handling: Implement geolocation support for users who do not provide a location.

## Installation and Setup

### 1. Clone the Repository:

```bash
git clone https://github.com/MMiraziz013/WebWeatherApp.git]
cd weather-application
```

### 2. Set Up API Key:

* Obtain an API key from the weather service provider (WeatherApi).

* Add the API key to the application configuration.

### 3. Run the Application:

Build and run the project using Visual Studio or the .NET CLI.

```bash
dotnet run
```

### 4. Access the Application:

* Open a browser and navigate to http://localhost:5000.

## License

This project is open-source under the MIT license, and can be freely used and modified according to the license policies.