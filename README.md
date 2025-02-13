# Weather Report Console Application
Prepared for Axis Communications by Vladimir Grigoriev

Version 1.0.0 (22/01/2025)

# Application Structure

Based on the requirement that the entire code base should be placed in one application, all code was placed in one console application type project.

Among other things, it was required that the project be coded as if it were a part of a larger application. This is why the project groups different parts of functionality into directories.

The application directories overview:

1. **Clients/** - The directory contains the definition for the clients to interact with a third-party Meteorology REST API. This directory can be isolated in a separate class library if necessary, for example **Axis.WeatherReport.Providers.SMHI**
2. **Configurations/** - The directory contains settings for REST API clients and also can be isolated in a separate class library if necessary together with clients
3. **Extensions/** - The directory contains extensions that allow to separate the logic of declaring project dependencies into a separate entity
4. **Models/** - The directory contains the definition of all data models used in the project. If necessary, some models can be moved to a separate project together with clients. The remaining models can be moved to a separate project along with business logic, or even to a separate class library for contracts.
5. **Services/** - The directory contains the definition of all business logic services.
6. **Workers/** - The directory contains the definition of all workers that are responsible for displaying the user interface part of the application.

The project also uses a central dependency management mechanism, which will allow, if necessary, to split the project into subprojects, while managing dependencies for all subprojects in one place.

The task did not mention unit testing, but in the solution you can find a separate project with a small example of unit tests just as an example: **Axis.WeatherReport.ConsoleApp.UnitTests**