<div align="center">
  <h1="center">TCP/IP Status Monitoring System</h1>
</div>

### The TCP Status Server is a C#/.NET–based TCP/IP server application designed to manage and monitor multiple client connections through a custom command–response protocol. It supports configurable polling, concurrent client handling, and real-time message processing using asynchronous socket communication.

### The server architecture emphasizes reliability and scalability, featuring per-client connection management, protocol-driven command dispatching, and robust error handling. It provides a production-ready foundation for device status monitoring systems, demonstrating clean separation of concerns, extensible protocol design, and efficient multi-client concurrency.

## Built With

- [![C#][CSharp]][CSharp-url]
- [![.NET][DotNet]][DotNet-url]

## Getting started

### Prerequisites

- DotNET: [DotNET download page](https://dotnet.microsoft.com/en-us/download/dotnet)

### Installation

1. Clone the repo
   ```bash
   git clone https://github.com/CharakaJith/tcp-status-server.git
   ```
2. Step into the project
   ```bash
   cd tcp-status-server
   ```

### Setup configurations

1. Create a `appsettings.json` in root folder
   ```bash
   echo. > appsettings.json
   ```
2. Open the `appsettings.json` and paste below
   ```json
   {
     "Server": {
       "Port": 5000
     }
   }
   ```

### Run the project

1. Build the solution
   ```bash
   dotnet build tcp-status-server.sln
   ```
2. Start server
   ```bash
   dotnet run
   ```

### Declaration

- This project, including all source code and documentation, was developed by me as part of the Aeontrix AI Backend Developer technical assessment.
- Product descriptions and documentation were reviewed and refined using ChatGPT to ensure proper grammar, clarity, and professional English.
- Guidance from ChatGPT was consulted for project organization and coding best practices. All server logic, protocol design, and implementation were completed independently by the author.

## Contact

Email: [gunasinghe.info@gmail.com](mailto:gunasinghe.info@gmail.com) | LinkedIn: [Charaka Jith Gunasinghe](https://www.linkedin.com/in/charaka-gunasinghe/)

### References

- [.NET TCP/IP Networking Concepts](https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/tcp-classes)
- [Building a High-Performance TCP Server](https://medium.com/@Alikhalili/building-a-high-performance-tcp-server-from-scratch-a8ede35c4cc2)

<!-- MARKDOWN LINKS & IMAGES -->

[CSharp]: https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white
[CSharp-url]: https://learn.microsoft.com/en-us/dotnet/csharp/
[DotNet]: https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[DotNet-url]: https://dotnet.microsoft.com/
