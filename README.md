# Project Name
AuthServerwithJWT

## About the Project
This project aims to obtain tokens from an API using JSON Web Token (JWT) and addresses the configuration of these tokens. The project's purpose is to provide a general example of JWT usage.

## Project Structure
The project utilizes a layered architecture and includes the following main components:

- **Shared Library**: This folder contains components that are shared among all projects within the solution. These components include `Response`, `ErrorDto`, and extension methods.

- **MiniApp.API, MiniApp2.API, MiniApp3.API**: These projects are used during the testing phase of the generated JSON Web Token. The "audiences" feature within JWTs is tested in these projects.
