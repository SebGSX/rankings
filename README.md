# Rankings

[![Rankings (CI)](https://github.com/SebGSX/rankings/actions/workflows/continuous-integration.yml/badge.svg)](https://github.com/SebGSX/rankings/actions/workflows/continuous-integration.yml)
[![GitHub tag](https://img.shields.io/github/tag/SebGSX/rankings?include_prereleases=&sort=semver&color=blue)](https://github.com/SebGSX/rankings/releases/)
[![License](https://img.shields.io/badge/License-MIT-blue)](#license)
[![issues - rankings](https://img.shields.io/github/issues/SebGSX/rankings)](https://github.com/SebGSX/rankings/issues)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SebGSX_rankings&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SebGSX_rankings)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=SebGSX_rankings&metric=bugs)](https://sonarcloud.io/summary/new_code?id=SebGSX_rankings)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=SebGSX_rankings&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=SebGSX_rankings)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=SebGSX_rankings&metric=coverage)](https://sonarcloud.io/summary/new_code?id=SebGSX_rankings)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=SebGSX_rankings&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=SebGSX_rankings)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=SebGSX_rankings&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=SebGSX_rankings)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=SebGSX_rankings&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=SebGSX_rankings)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=SebGSX_rankings&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=SebGSX_rankings)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=SebGSX_rankings&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=SebGSX_rankings)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=SebGSX_rankings&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=SebGSX_rankings)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=SebGSX_rankings&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=SebGSX_rankings)

## Overview

The `rankings` command-line app is a small, self-contained rankings app that runs on Windows, Linux, and macOS within 
their respective terminals.

## Getting Started

To run the app, please ensure that [.NET 9.0 SDK](https://dotnet.microsoft.com/download) is installed.

### GitHub Project

The project is hosted on GitHub within the [Rankings Project](https://github.com/users/SebGSX/projects/10).

> *Note:* The project uses a simple Kanban board with user and enabler stories to track progress.

### Project Structure
The project is structured as follows:

```
rankings/
├── .github/
│   ├── ISSUE_TEMPLATE/          # GitHub issue templates
│   ├── workflows/               # GitHub Actions workflows
├── src/
│   ├── Rankings/                # Main application source code
│   │   ├── Extensions/          # Extension methods used to configure the application
│   │   ├── Parsers/             # Parsers for command-line arguments as well as file content
│   │   ├── Resources/           # Embedded resources such as localized, reusable strings
│   │   ├── Services/            # Services used by the application
│   │   ├── Storage/             # Storage implementations (file-based)
│   │   ├── Validators/          # Validators for command-line arguments
├── test/
│   ├── Rankings.UnitTests/      # Unit tests for the application
├── .gitignore                   # Git ignore file
├── CODE_OF_CONDUCT.md           # Code of conduct
├── CONTRIBUTING.md              # Contribution guidelines
├── LICENSE                      # License file
├── Rankings.sln                 # Solution file
├── README.md                    # This readme file
├── SECRET.md                    # Security policy
```

## Developing

### AI Usage

This project has been developed with the assistance of artificial intelligence (AI) tools, per the following:
- **GitHub Copilot:** Used to assist with code generation and suggestions within the IDE, commit messages, pull request
  descriptions, pull request reviews, and documentation.
- **Grok:** Used to assist with research and code review using the [Tom Prompt](https://github.com/SebGSX/Prompt-Engineering/blob/main/prompt-engineering/pull-request-review.md).

### Sample Data

Sample data files should be created for local testing purposes including the following:
- `empty-file.txt`: An empty file.
- `invalid-data.txt`: A file containing invalid data.
- `sample-data.txt`: A file containing valid data.

> *Note:* Sample data is not provided in the repo and the `.gitignore` file excludes `[Ss]ampleData/`.

### Code Quality and Security

Code quality is supported using a variety of tools, including:
- **SonarQube Cloud:** Used to perform static code analysis and provide code quality and security reports.
- **GitHub Security Tools:** See the [Security Tab](https://github.com/SebGSX/rankings/security) within the repo. 
  Please note that CodeQL and Dependabot are enabled as is the secret scanning feature. Pull request reviews are
  handled by the author and supported by GitHub Copilot. Branch protection rules require pull requests for merges to
  the `main` branch, require checks to pass, and automatically request a review from GitHub Copilot.
- **ReSharper:** Used to perform static code analysis and provide code quality suggestions within the IDE.
- **Unit Tests:** The project is covered by unit tests using xUnit and Moq. Code coverage is reported to SonarQube
  Cloud.

### Building

Provided that the .NET 9.0 SDK is installed, the project can be built from within an appropriate IDE such as Rider,
VS Code, or Visual Studio, or from the command line at the project's root directory using:

```shell
dotnet build
```

### Running

The project can be run from within an appropriate IDE or from the command line at `src/Rankings/bin/Debug/net9.0/`
using the following command for Linux or macOS:

```bash
./rankings
```

Or the following command for Windows:

```shell
.\rankings
```

### Testing

The unit tests can be run from within an appropriate IDE or from the command line at the project's root directory using:

```shell
dotnet test
```

## Contributing

Please see [Contributing](https://github.com/SebGSX/rankings/blob/main/CONTRIBUTING.md) for details.

## License

Released under [MIT](/LICENSE) by [@SebGSX](https://github.com/SebGSX) (Seb Garrioch).
