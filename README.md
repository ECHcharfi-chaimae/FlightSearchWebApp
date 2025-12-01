# FlightSearchWebApp

FlightSearchWebApp is a web application built with **ASP.NET MVC** that allows users to search for flights based on multiple criteria. It integrates with the **Amadeus API** to retrieve flight offers in real-time.

## Features

- Search flights by:
  - Departure city
  - Arrival city
  - Departure date
  - Travel class (Economy, Premium Economy, Business, First)
  - Number of passengers
- View flight results with details including:
  - Flight segments and stopovers
  - Departure and arrival times
  - Flight price and currency
- Responsive and user-friendly interface

## Technology Stack

- **ASP.NET MVC** (C#)
- **Amadeus API** for flight data
- **HttpClient** for API requests
- **JSON** serialization/deserialization
- **Razor Views** for front-end

## Architecture

The project follows the **Model-View-Controller (MVC)** pattern:

- **Models:** Represent flight data and search criteria (`Flight.cs`)
- **Views:** Display search forms and results (`Index.cshtml`, `FlightResults.cshtml`)
- **Controllers:** Handle user actions, call the API, and render views (`HomeController.cs`, `FlightController.cs`)

## Setup and Configuration

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/FlightSearchWebApp.git
