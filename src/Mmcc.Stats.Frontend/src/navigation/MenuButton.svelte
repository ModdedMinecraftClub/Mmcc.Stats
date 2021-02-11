<script lang="ts">
    import type { IMenuButtonContent } from "./models";

    export let content: IMenuButtonContent;
</script>

<a class={`bg-${content.colourName}-500 mr-2 p-2 w-auto flex items-center rounded text-gray-100 focus:outline-none focus:ring focus:ring-${content.colourName}-300`} href={content.href}>
    <span class="w-4 h-4 mr-2"><svelte:component this={content.icon} /></span>
    {content.name}
</a>