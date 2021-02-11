<script lang="ts">
    import Discord from "./navigation/buttonIcons/Discord.svelte";
    import Github from "./navigation/buttonIcons/Github.svelte";
    import type { IMenuItemContent } from "./navigation/models";

    interface ISocialItemContent extends IMenuItemContent {
        icon: any;
    }

    const items: IMenuItemContent[] = [
        {
            name: "API docs",
            href: "/docs",
        },
        {
            name: "OpenAPI",
            href: "/openapi/v2/openapi.json",
        },
        {
            name: "Donate",
            href: "https://www.moddedminecraft.club/store.php",
        },
        {
            name: "Forums",
            href: "https://forum.moddedminecraft.club/",
        },
        {
            name: "Wiki",
            href: "https://wiki.moddedminecraft.club/",
        },
    ];

    const socials: ISocialItemContent[] = [
        {
            name: "GitHub",
            href: "https://github.com/moddedminecraftclub",
            icon: Github,
        },
        {
            name: "Discord",
            href: "https://discord.gg/8EgWdQC",
            icon: Discord,
        },
    ];
</script>

<footer
    class="px-6 lg:px-10 py-10 flex flex-col mt-auto items-center text-gray-300 bg-gray-800"
>
    <div class="flex gap-x-8 flex-wrap">
        {#each items as item}
            <a class="text-lg hover:text-gray-400 duration-300" href={item.href}>{item.name}</a>
        {/each}
    </div>
    <div class="flex gap-x-8 mt-6 flex-wrap">
        {#each socials as social}
            <a href={social.href} class="h-6 w-6 hover:text-gray-400 duration-300">
                <svelte:component this={social.icon} />
            </a>
        {/each}
    </div>
    <div class="mt-6 font-light text-center text-gray-400 text-xs">
        <div>
            MMCC Stats by
            <span
                ><a
                    class="mr-0 pr-0 underline"
                    href="https://github.com/ModdedMinecraftClub/Mmcc.Stats/graphs/contributors"
                >
                    TraceLD, ModdedMinecraftClub and contributors
                </a></span
            >
        </div>
        <div class="mt-2">
            Source code licensed under
            <span
                ><a
                    class="mr-0 pr-0 underline"
                    href="https://github.com/ModdedMinecraftClub/Mmcc.Stats/blob/master/LICENSE"
                >
                    AGPL-3.0
                </a></span
            >
        </div>
        <div class="mt-2">
            Icons from HeroIcons
            <span
                ><a class="mr-0 pr-0 underline" href="https://heroicons.com/">
                    (MIT)
                </a></span
            >
            and FontAwesome
            <span
                ><a
                    class="mr-0 pr-0 underline"
                    href="https://creativecommons.org/licenses/by/4.0/"
                >
                    (CC-BY 4.0)
                </a></span
            >
        </div>
    </div>
</footer>
