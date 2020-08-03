# Mmcc.Stats

[![License](https://img.shields.io/github/license/ModdedMinecraftClub/Mmcc.Stats?color=blue)](https://github.com/ModdedMinecraftClub/Mmcc.Stats/blob/master/LICENSE) [![Donate](https://img.shields.io/badge/donate-PayPal-ff69b4)](https://www.moddedminecraft.club/store.php) [![Chat on Discord](https://discordapp.com/api/guilds/251491739322286081/widget.png)](https://discord.com/invite/8EgWdQC)

MMCC Statistics (player count & TPS) - available at [poller.moddedminecraft.club](https://poller.moddedminecraft.club/).

![screenshot](./screenshots/web_app.png)


## Build Release version

**Before starting install the following dependencies:**

- .NET Core >= 3.1
- Node.js >= 12.18.3

**1.** Clone this repository. From now on we'll refer to the root directory of the cloned repository as `./`

**2.** Rename `./src/Mmcc.Stats/appsettings.default.json` to `./src/Mmcc.Stats/appsettings.json` and fill it in.

**3.** Go to `./src/Mmcc.Stats.Frontend` and run `npm install`.

**3.** In the root directory of the cloned repository run the following command: `dotnet publish ./src/Mmcc.Stats -c Release --output ./out`.

**4.** You will find the compiled version of the app in `./out`.
