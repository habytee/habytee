# habytee

habytee is a habit-tracking application designed to help users establish and maintain new habits. The application leverages modern web technologies to provide a seamless user experience, allowing users to create, track, and manage their habits effectively.

## Features

- **Habit Creation**: Users can create new habits with customizable settings, including reasons, days of the week, and optional alarms.
- **Task Management**: View and manage tasks for yesterday, today, and tomorrow, with the ability to mark tasks as completed.
- **Statistics**: Visualize habit completion statistics over the past two weeks to track progress and identify areas for improvement.
- **Secure Authentication**: Utilizes GitHub OAuth for secure user authentication, ensuring user data privacy and security.

## Project Structure

The project is organized into several key components:

### I. [Infrastructure (as Code)](./habytee.Infrastructure/README.md)

The infrastructure is defined using Terraform, enabling easy deployment and management of the application's backend services. It includes configurations for load balancing, DNS management, and Kubernetes cluster setup.

### II. [Landing Page](./habytee.LandingPage/README.md)

The landing page serves as the entry point for users, providing a simple and intuitive interface for logging in and accessing the application.

### III. [Interconnection](./habytee.Interconnection/README.md)

Shared models and validators between the client and server, ensuring consistent data handling and validation across the application.

### IV. [Client](./habytee.Client/README.md)

The client-side application is built using Blazor, providing a rich, interactive user interface for managing habits and viewing statistics.

### V. [Server](./habytee.Server/README.md)

The server-side application handles API requests, manages database interactions, and provides the necessary backend logic to support the client application.

## Getting Started

To get started with habytee, follow the setup instructions in the [Infrastructure README](./habytee.Infrastructure/README.md) to deploy the application. Once deployed, users can access the application via the provided domain and begin tracking their habits.

## Contributing

Contributions to habytee are welcome! Please see the [contributing guidelines](./CONTRIBUTING.md) for more information on how to get involved.

## License

habytee is licensed under the MIT License. See the [LICENSE](./LICENSE) file for more information.
