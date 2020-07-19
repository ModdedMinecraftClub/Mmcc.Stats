# Mmcc.Stats

[![License](https://img.shields.io/github/license/ModdedMinecraftClub/Mmcc.Stats?color=blue)](https://github.com/ModdedMinecraftClub/Mmcc.Stats/blob/master/LICENSE) [![Actions Status](https://github.com/ModdedMinecraftClub/Mmcc.Stats/workflows/build/badge.svg)](https://github.com/ModdedMinecraftClub/Mmcc.Stats/actions) [![Donate](https://img.shields.io/badge/donate-PayPal-ff69b4)](https://www.moddedminecraft.club/store.php) [![Chat on Discord](https://discordapp.com/api/guilds/251491739322286081/widget.png)](https://discord.com/invite/8EgWdQC)

MMCC Playerbase Statistics - available at [poller.moddedminecraft.club](https://poller.moddedminecraft.club/).

![screenshot](./screenshots/web_app.png)

## Build Release version

**1.** Clone this repository. From now on we'll refer to the root directory of the cloned repository as `./`

**2.** Rename `./src/Mmcc.Stats/appsettings.default.json` to `./src/Mmcc.Stats/appsettings.json` and fill it in.

**3.** In the root directory of the cloned repository run the following command: `dotnet publish ./src/Mmcc.Stats -c Release --output ./out`.

**4.** You will find the compiled version of the app in `./out`.
