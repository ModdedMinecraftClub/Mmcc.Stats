# Mmcc.Stats

[![License](https://img.shields.io/github/license/ModdedMinecraftClub/Mmcc.Stats?color=blue)](https://github.com/ModdedMinecraftClub/Mmcc.Stats/blob/master/LICENSE) [![Donate](https://img.shields.io/badge/donate-PayPal-ff69b4)](https://www.moddedminecraft.club/store.php) [![Chat on Discord](https://discordapp.com/api/guilds/251491739322286081/widget.png)](https://discord.com/invite/8EgWdQC)

MMCC Statistics (player count & TPS) - available at [poller.moddedminecraft.club](https://poller.moddedminecraft.club/).

## OpenAPI

- API documentation: <https://poller.moddedminecraft.club/docs/>
- JSON OpenAPI specification: <https://poller.moddedminecraft.club/openapi/v1/openapi.json>

## Build Release version

**Before starting install the following dependencies:**

*All of the dependecies are cross-platform and can be installed on Windows, Linux and macOS.*

- [PowerShell (Core) >= 7.0.3](https://github.com/PowerShell/PowerShell)
- [.NET >= 5.0](https://dotnet.microsoft.com/download)
- [Node.js >= 12.18.3](https://nodejs.org/en/)

**1.** Clone this repository. From now on we will be referring to the root directory of the cloned repository as `./`.

**2.** Rename `./src/Mmcc.Stats/appsettings.default.json` to `./src/Mmcc.Stats/appsettings.json` and fill it in.

**3.** Run the `./publish.ps1` build script.

**4.** You will find the compiled version of the app in `./out`.
