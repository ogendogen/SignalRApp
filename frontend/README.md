# SignalR Chat Application

This is a simple chat application built with Angular 19, Angular Material, and SignalR for real-time communication.

## Features

- Real-time messaging using SignalR
- Modern UI with Angular Material
- Responsive design

## Prerequisites

- Node.js (v20.14.0 or later)
- npm (v10.9.0 or later)

## Getting Started

1. Clone the repository
2. Install dependencies:
   ```
   npm install
   ```
3. Start the development server:
   ```
   ng serve
   ```
4. Open your browser and navigate to `http://localhost:4200`

## Backend Setup

This frontend application requires a SignalR backend to function properly. You'll need to create an ASP.NET Core application with SignalR Hub to handle the real-time communication.

The frontend is configured to connect to a SignalR hub at `http://localhost:5000/chathub`. Make sure your backend is running and accessible at this URL, or update the URL in the `chat.component.ts` file.

## Building for Production

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.

## Further Help

To get more help on the Angular CLI use `ng help` or check out the [Angular CLI Overview and Command Reference](https://angular.io/cli) page.