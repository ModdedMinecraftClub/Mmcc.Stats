<script lang="ts">
    import MenuItem from "./MenuItem.svelte";
    import Heart from "./buttonIcons/Heart.svelte";
    import Github from "./buttonIcons/Github.svelte";
    import Discord from "./buttonIcons/Discord.svelte";
    import type { IMenuButtonContent, IMenuItemContent } from "./models";
    import MenuButton from "./MenuButton.svelte";

    let menuOpened: boolean = false;
    let menuItems: IMenuItemContent[] = [
        {
            name: "MMCC Home",
            href: "https://www.moddedminecraft.club/",
        },
        {
            name: "Wiki",
            href: "https://wiki.moddedminecraft.club/",
        },
        {
            name: "Forum",
            href: "https://forum.moddedminecraft.club/",
        },
    ];
    let menuButtons: IMenuButtonContent[] = [
        {
            name: "Donate",
            href: "https://www.moddedminecraft.club/store.php",
            icon: Heart,
            colourName: "pink",
        },
        {
            name: "Discord",
            href: "https://discord.com/invite/8EgWdQC",
            icon: Discord,
            colourName: "blue",
        },
        {
            name: "GitHub",
            href: "https://github.com/ModdedMinecraftClub/Mmcc.Stats",
            icon: Github,
            colourName: "green",
        },
    ];
</script>

<nav
    class="flex items-center justify-between flex-wrap py-4 px-6 lg:px-10 bg-gray-800 text-gray-100"
>
    <a href="/" class="flex items-center flex-shrink-0">
        <svg
            class="w-8 h-8 mr-0.5"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
        >
            <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width={1.8}
                d="M16 8v8m-4-5v5m-4-2v2m-2 4h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"
            />
        </svg>
        <span class="font-semibold text-xl tracking-tight">MMCC Stats</span>
    </a>
    <div class="block lg:hidden">
        <button
            id="burger"
            on:click={() => (menuOpened = !menuOpened)}
            class="flex items-center px-3 py-2 rounded hover:text-gray-400 duration-300 outline-none focus:outline-none"
        >
            <svg
                class="w-5 h-5"
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 20 20"
                fill="currentColor"
            >
                <path
                    fill-rule="evenodd"
                    d="M3 5a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 10a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM9 15a1 1 0 011-1h6a1 1 0 110 2h-6a1 1 0 01-1-1z"
                    clip-rule="evenodd"
                />
            </svg>
        </button>
    </div>
    <div
        id="menu"
        class:hidden={!menuOpened}
        class="w-full lg:flex lg:items-center lg:w-auto"
    >
        <div class="lg:flex lg:items-center">
            {#each menuItems as menuItem}
                <MenuItem content={menuItem} />
            {/each}
            <div
                class="border-l border-gray-500 h-8 w-1 mr-4 hidden lg:inline-block"
            />
            <div class="flex mt-3 lg:mt-0">
                {#each menuButtons as menuButton}
                    <MenuButton content={menuButton} />
                {/each}
            </div>
        </div>
    </div>
</nav>
